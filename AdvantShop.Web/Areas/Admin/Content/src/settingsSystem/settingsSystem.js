; (function (ng) {
    'use strict';

    var SettingsSystemCtrl = function (Upload, $http, toaster, $q, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, SweetAlert) {

        var ctrl = this;

        ctrl.$onInit = function () {
        }

        ctrl.checkLicense = function () {
            return SweetAlert.confirm("Здесь вы можете проверить и указать лицензионный ключ. Обратите внимание, что смена ключа повлечёт смену настроек, не меняйте ключ без надобности.", { title: "Проверка лицензии" }).then(function (result) {
                return result === true
                    ? $http.post('settingsSystem/checkLicense', { 'licKey': ctrl.LicKey }).then(function (response) {
                        if (response.data.result === true) {
                            ctrl.ActiveLic = true;
                            toaster.pop('success', 'Статус лицензии - активна', "");
                        } else {
                            ctrl.ActiveLic = false;
                            toaster.pop({
                                type: 'error',
                                title: 'Статус лицензии - не активна',
                                timeout: 0
                            });
                        }
                    })
                    : $q.reject('sweetAlertCancel');
            });
        };

        ctrl.updateSiteMaps = function () {
            $http.post('settingsSystem/updateSiteMaps').then(function (response) {
                if (response.data.result === true) {

                    ctrl.SiteMapFileHtmlDate = response.data.obj.htmlLastWriteTime;
                    ctrl.SiteMapFileXmlDate = response.data.obj.xmlLastWriteTime;

                    ctrl.SiteMapFileHtmlLink = response.data.obj.SiteMapFileHtmlLink;
                    ctrl.SiteMapFileXmlLink = response.data.obj.SiteMapFileXmlLink;

                    toaster.pop('success', '', 'Карты сайта обновлены');
                }
                else {
                    toaster.pop('error', 'Ошибка при обновлении карт сайта', "");
                }
            });
        };

        ctrl.fileStorageRecalc = function () {
            $http.post('settingsSystem/fileStorageRecalc').then(function (response) {
                toaster.pop('success', '', 'Перерасчет запущен. Ограничение 1 перерасчет в час.');
                ctrl.showFileStorageRecalc = false;
            });
        };

        //#region Localization

        ctrl.AllLocalization = true;

        ctrl.changeSelectLanguage = function () {
            ctrl.gridLocalization.setParams({ 'Value': ctrl.langLocalization, 'ChangeAll': ctrl.AllLocalization });
            ctrl.gridLocalization.fetchData();
        }

        ctrl.startExportlocalization = function () {
            if (ctrl.langLocalization == null) {
                toaster.pop("error", "Ошибка", "Выберите язык для выгрузки");
            }
            else {
                $window.location.assign('localization/export?lang=' + ctrl.langLocalization);
            }
        }

        var columnDefsLocalization = [
                {
                    name: 'ResourceKey',
                    displayName: 'Ключ',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Ключ',
                        type: uiGridConstants.filter.INPUT,
                        name: 'ResourceKey'
                    }
                },
                {
                    name: 'ResourceValue',
                    displayName: 'Значение',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Значение',
                        name: 'ResourceValue',
                        type: uiGridConstants.filter.INPUT
                    }
                }
        ];

        ctrl.gridOptionsLocalization = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsLocalization,
            uiGridCustom: {
                rowUrl: ''
            }
        });

        ctrl.gridLocalizationOnInit = function (gridLocalization) {
            ctrl.gridLocalization = gridLocalization;
        };

        //#endregion
    };

    SettingsSystemCtrl.$inject = ['Upload', '$http', 'toaster', '$q', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'SweetAlert'];

    ng.module('settingsSystem', ['ngFileUpload', 'toaster', 'as.sortable', 'paymentMethodsList'])
      .controller('SettingsSystemCtrl', SettingsSystemCtrl);

})(window.angular);