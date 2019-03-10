; (function (ng) {
    'use strict';

    var OrdersAnalysisCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {
            if (ctrl.onInit != null) {
                ctrl.onInit({ orders: ctrl });
            }
        }

        ctrl.recalc = function (dateFrom, dateTo, paid, orderStatus) {

            ctrl.fetchPayments(dateFrom, dateTo, paid, orderStatus);
            ctrl.fetchShippings(dateFrom, dateTo, paid, orderStatus);
            ctrl.fetchStatuses(dateFrom, dateTo, paid, orderStatus);
            ctrl.fetchSources(dateFrom, dateTo, paid, orderStatus);
            ctrl.fetchRepeatOrders(dateFrom, dateTo, paid, orderStatus);
        }

        ctrl.fetchPayments = function (dateFrom, dateTo, paid, orderStatus) {
            $http.get("analytics/getOrders", { params: { type: "payments", dateFrom: dateFrom, dateTo: dateTo, paid: paid, orderStatus: orderStatus } }).then(function (result) {
                ctrl.Payments = result.data;
            });
        }

        ctrl.fetchShippings = function (dateFrom, dateTo, paid, orderStatus) {
            $http.get("analytics/getOrders", { params: { type: "shippings", dateFrom: dateFrom, dateTo: dateTo, paid: paid, orderStatus: orderStatus } }).then(function (result) {
                ctrl.Shippings = result.data;
            });
        }

        ctrl.fetchStatuses = function (dateFrom, dateTo, paid, orderStatus) {
            $http.get("analytics/getOrders", { params: { type: "statuses", dateFrom: dateFrom, dateTo: dateTo, paid: paid, orderStatus: orderStatus } }).then(function (result) {
                ctrl.Statuses = result.data;
            });
        }

        ctrl.fetchSources = function (dateFrom, dateTo, paid, orderStatus) {
            $http.get("analytics/getOrders", { params: { type: "sources", dateFrom: dateFrom, dateTo: dateTo, paid: paid, orderStatus: orderStatus } }).then(function (result) {
                ctrl.Sources = result.data;
            });
        }

        ctrl.fetchRepeatOrders = function (dateFrom, dateTo, paid, orderStatus) {
            $http.get("analytics/getOrders", { params: { type: "repeatorders", dateFrom: dateFrom, dateTo: dateTo, paid: paid, orderStatus: orderStatus } }).then(function (result) {
                ctrl.RepeatOrders = result.data;
            });
        }
    };

    OrdersAnalysisCtrl.$inject = ['$http'];

    ng.module('analyticsReport')
        .controller('OrdersAnalysisCtrl', OrdersAnalysisCtrl)
        .component('ordersAnalysis', {
            templateUrl: '../areas/admin/content/src/analyticsReport/components/ordersAnalysis/ordersAnalysis.html',
            controller: OrdersAnalysisCtrl,
            bindings: {
                onInit: '&'
            }
      });

})(window.angular);