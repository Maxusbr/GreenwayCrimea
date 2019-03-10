; (function (ng) {
    'use strict';

    var SidebarService = function () {
        var service = this;
        service.sidebarOptions = { visible : false };

        service.toggleSidebar = function () {
            service.sidebarOptions.visible = !service.sidebarOptions.visible;
        }
        
    };

    ng.module('sidebar')
      .service('sidebarService', SidebarService);

    

})(window.angular);