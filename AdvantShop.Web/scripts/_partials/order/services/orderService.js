; (function (ng) {
    'use strict';

    var orderService = function ($http) {
        var service = this;

        service.getOrders = function () {
            return $http.get('/myaccount/GetCustomerOrderHistory', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });;
        };

        service.getOrderDetails = function (ordernumber) {
            return $http.get('/myaccount/GetOrderDetails', { params: { ordernumber: ordernumber, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });;
        };

        service.cancelOrder = function (ordernumber) {
            return $http.post('/myaccount/CancelOrder', { ordernumber: ordernumber, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.changePaymentMethod = function (ordernumber, paymentId) {
            return $http.post('/myaccount/ChangePaymentMethod', { ordernumber: ordernumber, paymentId: paymentId, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.changeOrderComment = function (ordernumber, customercomment) {
            return $http.post('/myaccount/ChangeOrderComment', { ordernumber: ordernumber, customercomment: customercomment, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

    };

    ng.module('order')
      .service('orderService', orderService);

    orderService.$inject = ['$http'];

})(window.angular);