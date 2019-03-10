; (function (ng) {
    'use strict';

    var roistatSettingsCtrl = function ($http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            ctrl.getSettings();
        };

        ctrl.getSettings = function () {
            $http.get('../roistatSettings/getSettings').then(function (response) {
                ctrl.settings = response.data;
                ctrl.haveAccount =
                    (ctrl.settings.RoistatLogin != null ||
                        ctrl.settings.RoistatPassword != null ||
                        ctrl.settings.RoistatScript != null)
                    ? 'true'
                    : 'false';
            });
        }

        ctrl.register = function() {
            ctrl.haveAccount = 'true';
            ctrl.saveSettings();
        }

        ctrl.saveSettings = function () {
            $http.post('../roistatSettings/saveSettings', ctrl.settings).then(function (response) {
                if (response.data.result === true) {
                   toaster.pop('success', '', 'Настройки сохранены');
               } else {
                   toaster.pop('error', '', 'Ошибка при сохранении настроек');
               }
            });
        }
    };

    roistatSettingsCtrl.$inject = ['$http', 'toaster'];

    ng.module('roistatSettings', [])
        .controller('roistatSettingsCtrl', roistatSettingsCtrl)
        .component('roistatSettings', {
            templateUrl: '../modules/roistat/content/scripts/admin/roistatSettings.html',
            controller: 'roistatSettingsCtrl'
        });


})(window.angular);