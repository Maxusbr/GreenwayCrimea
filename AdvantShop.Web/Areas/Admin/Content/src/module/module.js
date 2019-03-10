; (function (ng) {
    'use strict';
    window.addEventListener('load', function () {
        var portBtns = document.querySelectorAll('.trigger-port');
        if (portBtns.length != 0) {
            for (var i = 0; i < portBtns.length; i++) {
                portBtns[i].data = i;
            }
        }
    });

    var ModuleCtrl = function ($http, toaster) {

        var ctrl = this;
            ctrl.tab = 0;

        ctrl.onInit = function () {
            ctrl.activeImport = false;
        };
        
        ctrl.changeEnabled = function () {
            $http.post('modules/changeEnabled', { stringId: ctrl.stringId, enabled: ctrl.enabled }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', ctrl.enabled ? 'Модуль активирован' : 'Модуль не активен');
                } else {
                    ctrl.enabled = false;
                    toaster.pop('error', '', 'Ошибка при сохранении');
                }
            });
        }

        ctrl.setTab = function(newTab) {
            ctrl.tab = newTab;
        }

        ctrl.isSet = function (tabNum) {
            return ctrl.tab === tabNum;
        };
    }


    ModuleCtrl.$inject = ['$http', 'toaster'];

    ng.module('module', [])
      .controller('ModuleCtrl', ModuleCtrl);

})(window.angular);