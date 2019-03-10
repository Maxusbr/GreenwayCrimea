; (function (ng) {
    'use strict';

    var ModalAddEditLocalizationCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.ResourceKey = params.ResourceKey != undefined && params.ResourceKey.value != null ? params.ResourceKey.value : null;
            ctrl.LanguageId = params.LanguageId != null && params.LanguageId != undefined? params.LanguageId : null;
            ctrl.mode = ctrl.LanguageId != null && ctrl.ResourceKey != null ? "edit" : "add";

            if (ctrl.mode == "edit") {
                ctrl.getLocalize(ctrl.ResourceKey,ctrl.LanguageId);
            }
            else {
                ctrl.id = 0;
            }
            $http.get('Localization/GetLanguage', { params: { lang: ctrl.LanguageId, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.Langs = data;
                    var mass = data.filter(function (item) {
                        return item.Selected === true;
                    });
                    ctrl.langSelected = mass.length > 0 ? mass[0] : null;
                }
            });
        };

        ctrl.getLocalize = function (key, id) {
            $http.get('Localization/GetLocalizeItem', { params: { LanguageId: id, ResourceKey: key, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null && data.length > 0) {
                    ctrl.ResourceKey = data[0].ResourceKey;
                    ctrl.ResourceValue = data[0].ResourceValue == null ? "" : data[0].ResourceValue;
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.saveLocalization = function () {

            ctrl.btnSleep = true;

            var params = {
                ResourceKey: ctrl.ResourceKey,
                ResourceValue: ctrl.ResourceValue,
                LanguageId: ctrl.langSelected.Value,
                rnd: Math.random()
            };

            var url = 'Localization/AddEditLocalization';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close('saveLocalization');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании локализации");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditLocalizationCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditLocalizationCtrl', ModalAddEditLocalizationCtrl);

})(window.angular);