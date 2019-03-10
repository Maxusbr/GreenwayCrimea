; (function (ng) {
    'use strict';

    var OrderItemCtrl = function ($http, $timeout) {

        var ctrl = this;

        ctrl.status = {};
        ctrl.order = {};

        ctrl.Init = function(orderId, paid, statusId, statusName, statusColor) {
            ctrl.order.Id = orderId;
            ctrl.status.Id = statusId.toString(); //in angular  1.4 ng-option ng-model become with strict comparison '===' with option value. Option value is a string, but ng model is number. Therefore => .toString()
            ctrl.order.paid = paid.toString();
            ctrl.status.Name = statusName;
            ctrl.status.Color = '#' + statusColor;
        }

        ctrl.changeStatus = function () {
            $http.get("/adminmobile/changeorderstatus", { params: { 'orderId': ctrl.order.Id, 'statusId': ctrl.status.Id } }).then(function (response) {
                if (response.data != null && response.data.ResultCode === 0) {
                    ctrl.statusSaved = true;
                    ctrl.status.Name = response.data.StatusName;
                    ctrl.status.Color = '#' + response.data.Color;
                    alert('Статус заказа успешно изменен');
                }
            });
        }

        ctrl.setOrderPaid = function () {
            $http.get("/adminmobile/setorderpaid", { params: { 'orderId': ctrl.order.Id, 'paid': ctrl.order.paid } }).then(function (response) {
                if (response.data != null && response.data.ResultCode === 0) {
                    alert('Статус оплаты изменен');
                }
            });
        }
    };

    ng.module("orderItem")
        .controller("OrderItemController", OrderItemCtrl);

    OrderItemCtrl.$inject = ['$http', '$timeout'];

})(window.angular);
