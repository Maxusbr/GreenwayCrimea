; (function (ng) {
    'use strict';

    var SettingsCtrl = function (Upload, $http, toaster, $q, $location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, SweetAlert) {

        var ctrl = this;

        //ctrl.hasRegions = true;

        ctrl.init = function (langs) {
            ctrl.Langs = langs;
            var mass = langs.filter(function (item) {
                return item.Selected === true;
            });

            ctrl.langLocalization = mass.length > 0 ? mass[0] : null;
        }

        //start APi

        ctrl.generateApiKey = function () {
            $http.get('settingsApi/generate').then(function (response) {
                ctrl.Key = response.data;
            });
        }

        //end API


        ctrl.uploadLogo = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Upload.upload({
                    url: '/settings/uploadlogo',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.result === true) {
                        ctrl.logoSrc = data.file;
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке логотипа', data.error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке логотипа', 'Файл не соответствует требованиям');
            }
        };

        ctrl.uploadFavicon = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Upload.upload({
                    url: '/settings/uploadfavicon',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.result === true) {
                        ctrl.faviconSrc = data.file;
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке favicon', data.error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке favicon', 'Файл не соответствует требованиям');
            }
        };

        ctrl.uploadStamp = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Upload.upload({
                    url: '/settings/uploadbankstamp',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.result === true) {
                        ctrl.stampSrc = data.file;
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке штампа', data.error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке штампа', 'Файл не соответствует требованиям');
            }
        };

        ctrl.deleteLogo = function () {
            $http.post('/settings/deletelogo').then(function (response) {
                ctrl.logoSrc = response.data.file;
            });
        };

        ctrl.deleteFavicon = function () {
            $http.post('/settings/deletefavicon').then(function (response) {
                ctrl.faviconSrc = response.data.file;
            });
        };

        ctrl.deleteStamp = function () {
            $http.post('/settings/deletebankstamp').then(function (response) {
                ctrl.stampSrc = response.data.file;
            });
        };

        ctrl.loadRegions = function (currentRegion) {
            ctrl.hasRegions = true;
            $http.post('/settings/GetRegions', { 'countryId': ctrl.countryId }).then(function (response) {
                ctrl.regions = response.data.obj;
                if (response.data.obj.length) {
                    if (currentRegion == '') {
                        ctrl.regionId = response.data.obj[0].Value;
                    }
                    else {
                        ctrl.regionId = currentRegion;
                    }
                    ctrl.hasRegions = true;
                }
                else {
                    ctrl.hasRegions = false;
                }
            });
        };
    };

    SettingsCtrl.$inject = ['Upload', '$http', 'toaster', '$q', '$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'SweetAlert'];

    ng.module('settings', ['ngFileUpload', 'toaster', 'as.sortable', 'paymentMethodsList'])
      .controller('SettingsCtrl', SettingsCtrl);

})(window.angular);