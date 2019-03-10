; (function (ng) {
    'use strict';

    var SettingsTelephonyCtrl = function ($http, $q, $window, toaster) {

        var ctrl = this;

        ctrl.getTelphinExtensions = function () {
            if (!ctrl.telphinAppKey || !ctrl.telphinAppSecret) {
                $window.document.getElementById(ctrl.telphinAppKey ? 'TelphinAppSecret' : 'TelphinAppKey').focus();
                toaster.error('Укажите данные приложения');
                return;
            }
            $http.post('settingsTelephony/getTelphinExtensions').then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    ctrl.telphinExtensions = data.obj;
                    toaster.success('Данные обновлены');
                } else {
                    toaster.error('Не удалось получить данные');
                }
            });
        }

        ctrl.addTelphinEvents = function (ext) {
            $http.post('settingsTelephony/addTelphinEvents', { extensionId: ext.id }).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    ctrl.telphinExtensions = data.obj;
                    toaster.success('Оповещения о событиях для добавочного номера ' + ext.extension + ' установлены');
                } else {
                    data.errors.forEach(function (error) {
                        toaster.error('Ошибка', error);
                    });
                }
            });
        }

        ctrl.deleteTelphinEvents = function (ext, notify) {
            return $http.post('settingsTelephony/deleteTelphinEvents', { extensionId: ext.id }).then(function (response) {
                if (notify === false) {
                    return;
                }
                var data = response.data;
                if (data.result == true) {
                    ctrl.telphinExtensions = data.obj;
                    toaster.success('Оповещения о событиях для добавочного номера ' + ext.extension + ' удалены');
                } else {
                    data.errors.forEach(function (error) {
                        toaster.error('Ошибка', error);
                    });
                }
            });
        }

        ctrl.setTelphinEvents = function (ext) {
            ctrl.deleteTelphinEvents(ext, false).then(function () {
                ctrl.addTelphinEvents(ext);
            });
        }
    };

    SettingsTelephonyCtrl.$inject = ['$http', '$q', '$window', 'toaster'];

    ng.module('settingsTelephony', [])
      .controller('SettingsTelephonyCtrl', SettingsTelephonyCtrl);

})(window.angular);