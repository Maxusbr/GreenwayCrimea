; (function (ng) {
    'use strict';

    var SidebarUserCtrl = function ($uibModalStack, sidebarUserService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            if (ctrl.onInit != null) {
                ctrl.onInit({ sidebar: ctrl });
            }
        };

        ctrl.open = function () {
            ctrl.isShow = true;

            if (ctrl.onOpen != null) {
                ctrl.onOpen({ sidebar: ctrl });
            }
        };

        ctrl.close = function () {
            ctrl.isShow = false;

            if (ctrl.onClose != null) {
                ctrl.onClose({ sidebar: ctrl });
            }
        };
    };

    SidebarUserCtrl.$inject = ['$uibModalStack', 'sidebarUserService'];

    ng.module('sidebarUser', [])
        .controller('SidebarUserCtrl', SidebarUserCtrl);

})(window.angular);