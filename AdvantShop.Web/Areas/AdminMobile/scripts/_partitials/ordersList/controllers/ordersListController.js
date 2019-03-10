; (function (ng) {
    'use strict';

    var OrdersController = function ($http) {

        var currentPage = 1,
            isEnd = false,
            isProcessing = false,
            ordCtrl = this;

        ordCtrl.orders = [];

        ordCtrl.getOrders = function () {


            if (!isEnd && !isProcessing && (getDocHeight() - 50 < getScrollXY()[1] + window.innerHeight)) {

                var urlParams = location.pathname.split('/');
                var statusParam = urlParams[urlParams.length - 1];
                var statusId = statusParam != "orders" ? statusParam : "";

                isProcessing = true;
                var page = currentPage;

                $http.get("/adminmobile/getlastorders", { params: { 'page': page, 'statusid': statusId, 'number': ordCtrl.number || '' } }).then(function (response) {
                    currentPage += 1;
                    ordCtrl.orders = ordCtrl.orders.concat(response.data);
                    isEnd = response.data == null || response.data.length == 0 || response.data.length < 10;
                    isProcessing = false;
                });
            }

        };

        ordCtrl.search = function (orderNumber) {
            
            if (ordCtrl.number) {
                if (orderNumber) {
                    $http.get("/adminmobile/getorders", { params: { number: orderNumber, rnd: Math.random() } }).then(function (response) {
                        ordCtrl.orders = [];
                        ordCtrl.orders = ordCtrl.orders.concat(response.data);
                        currentPage = 1;
                        isEnd = false;
                    });
                }
                else {
                    ordCtrl.reset();
                }
            }
        };

        ordCtrl.reset = function () {
            currentPage = 1;
            ordCtrl.orders = [];
            ordCtrl.number = "";
            isEnd = false;
            setTimeout(ordCtrl.getOrders, 0);
        };

        ordCtrl.getOrders();
    };

    ng.module("ordersList")
        .controller("OrdersListController", OrdersController);

    OrdersController.$inject = ['$http'];

})(window.angular);
