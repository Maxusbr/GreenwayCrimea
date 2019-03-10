; (function (ng) {
    'use strict';

    var SidebarUserContainerCtrl = function ($scope, sidebarUserService, domService) {
        var ctrl = this;

        ctrl.items = [];

        ctrl.$onInit = function () {
            sidebarUserService.initContainer(ctrl);
        };

        ctrl.initItem = function (sidebar) {
            sidebar.open();
        };

        ctrl.closeItem = function (item) {

            var index;

            for (var i = 0, len = ctrl.items.length; i < len; i++) {
                if (item === ctrl.items[i]) {
                    index = i;
                    break;
                }
            }

            if (index != null) {
                ctrl.items.splice(index, 1);
            }
        }

        ctrl.onCloseItem = function (sidebar) {
            ctrl.closeItem(sidebar.user);
        };

        ctrl.addUser = function (user) {
            ctrl.items.push(user);
        };

        ctrl.clickOut = function (event) {
            if (domService.closest(event.target, '.js-sidebar-user-container') == null) {
                ctrl.items.length = 0;
                $scope.$digest();
            }
        }
    };

    SidebarUserContainerCtrl.$inject = ['$scope','sidebarUserService', 'domService'];

    ng.module('sidebarUser')
        .controller('SidebarUserContainerCtrl', SidebarUserContainerCtrl);

})(window.angular);