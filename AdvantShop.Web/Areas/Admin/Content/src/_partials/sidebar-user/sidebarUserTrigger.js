; (function (ng) {
    'use strict';

    var SidebarUserTriggerCtrl = function (sidebarUserService) {
        var ctrl = this;

        ctrl.open = function (customerId) {
            sidebarUserService.getUser(customerId).then(function (user) {
                if (user.HeadCustomerId) {
                    return sidebarUserService.getUser(user.HeadCustomerId).then(function (headUser) {
                        user.HeadCustomer = headUser;
                        return user;
                    });
                }else{
                    return user;
                }
            }).then(sidebarUserService.addUser);
        };

    };

    SidebarUserTriggerCtrl.$inject = ['sidebarUserService'];

    ng.module('sidebarUser')
        .controller('SidebarUserTriggerCtrl', SidebarUserTriggerCtrl);

})(window.angular);