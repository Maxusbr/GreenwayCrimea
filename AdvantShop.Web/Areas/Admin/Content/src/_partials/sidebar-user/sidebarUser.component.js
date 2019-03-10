; (function (ng) {
    'use strict';


    ng.module('sidebarUser')
        .component('sidebarUserContainer', {
            templateUrl: '../areas/admin/content/src/_partials/sidebar-user/templates/sidebar-user-container.html',
            controller: 'SidebarUserContainerCtrl'
        })
        .component('sidebarUser', {
            templateUrl: '../areas/admin/content/src/_partials/sidebar-user/templates/sidebar-user.html',
            controller: 'SidebarUserCtrl',
            bindings: {
                user: '<?',
                onInit: '&',
                onOpen: '&',
                onClose: '&'
            }
        })

})(window.angular);