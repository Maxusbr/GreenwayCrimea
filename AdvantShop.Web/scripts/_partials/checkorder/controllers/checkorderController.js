; (function (ng) {

    'use strict';

    var CheckOrderCtrl = function (checkOrderService, toaster) {

        var ctrl = this;

        ctrl.checkOrderSubmit = function (orderNumber) {
            checkOrderService.getStatus(orderNumber).then(function (status) {
                toaster.pop('info', status.StatusName, status.StatusComment);
            });
        };
    };

    ng.module('checkOrder')
      .controller('CheckOrderCtrl', CheckOrderCtrl);

    CheckOrderCtrl.$inject = ['checkOrderService', 'toaster'];

})(window.angular);