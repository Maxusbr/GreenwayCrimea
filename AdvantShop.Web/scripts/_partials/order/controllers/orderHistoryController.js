; (function (ng) {
    'use strict';

    var OrderHistoryCtrl = function ($sce, $window, orderService, windowService, toaster, $translate) {
        var ctrl = this;

        ctrl.changeModeAll = function () {
            ctrl.mode = 'all';
        };

        ctrl.changeModeDetails = function () {
            ctrl.mode = 'details';
        };

        ctrl.prepareDetails = function (order) {

            var paymentSelected;

            ctrl.orderDetails = order;

            if (ctrl.orderDetails.Payments != null && ctrl.orderDetails.Payments.length > 0) {

                for (var i = 0, l = ctrl.orderDetails.Payments.length; i < l; i++) {
                    if (ctrl.orderDetails.Payments[i].id == ctrl.orderDetails.PaymentMethodId) {
                        paymentSelected = ctrl.orderDetails.Payments[i];
                        break;
                    }
                }

                if (paymentSelected == null) {
                    paymentSelected = ctrl.orderDetails.Payments[0];
                }

                ctrl.orderDetails.paymentSelected = paymentSelected;
                ctrl.orderDetails.PaymentForm = $sce.trustAsHtml(ctrl.orderDetails.PaymentForm);
            }

            return ctrl.orderDetails;
        };

        ctrl.view = function (ordernumber) {
            return orderService.getOrderDetails(ordernumber).then(function (order) {
                ctrl.prepareDetails(order);
                ctrl.changeModeDetails();
            });
        };


        ctrl.cancelOrder = function (ordernumber) {
            return orderService.cancelOrder(ordernumber).then(function () {
                return orderService.getOrderDetails(ordernumber).then(function (order) {
                    return ctrl.orderDetails = order;
                });
            });
        };

        ctrl.print = function (ordernumber) {
           windowService.print('PrintOrder/' + ordernumber, 'printOrder', 'menubar=no,location=no,resizable=yes,scrollbars=yes');
        };

        ctrl.changePaymentMethod = function (ordernumber, paymentId) {
           return orderService.changePaymentMethod(ordernumber, paymentId).then(function (response) {
                if (response != null) {
                    return orderService.getOrderDetails(ordernumber).then(ctrl.prepareDetails);
                }
            });
        };

        ctrl.changeOrderComment = function (ordernumber, customercomment) {
            return orderService.changeOrderComment(ordernumber, customercomment).then(function (response) {
                if (response === true) {
                    toaster.pop('success','',$translate.instant('Js.Order.CommentSaved'));
                }
                else {
                    toaster.pop('error', '', $translate.instant('Js.Order.CommentNotSaved'));
                }
            });
        };

        orderService.getOrders().then(function (orders) {
            ctrl.items = orders;
        });
    };

    ng.module('order')
      .controller('OrderHistoryCtrl', OrderHistoryCtrl);

    OrderHistoryCtrl.$inject = ['$sce', '$window', 'orderService', 'windowService', 'toaster', '$translate'];

})(window.angular);