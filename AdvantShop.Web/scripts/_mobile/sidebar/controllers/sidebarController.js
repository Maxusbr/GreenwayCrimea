; (function (ng) {
    'use strict';

    var SidebarCtrl = function (sidebarService) {
        var ctrl = this;
            ctrl.sidebarVisible = sidebarService.sidebarOptions;

        ctrl.toggleSidebar = function () {
            sidebarService.toggleSidebar();
            
        };
    };

    ng.module('sidebar')
      .controller('sidebarCtrl', SidebarCtrl);

    SidebarCtrl.$inject = ['sidebarService'];


})(window.angular);