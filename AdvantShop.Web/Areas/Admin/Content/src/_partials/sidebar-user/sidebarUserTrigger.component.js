; (function (ng) {
    'use strict';


    ng.module('sidebarUser')
        .component('sidebarUserTrigger', {
            controller: 'SidebarUserTriggerCtrl',
            transclude: true,
            template: '<span class="sidebar-user-trigger" ng-click="$ctrl.open($ctrl.customerId)" ng-transclude></span>',
            bindings: {
                customerId: '<'
            }
        })

})(window.angular);