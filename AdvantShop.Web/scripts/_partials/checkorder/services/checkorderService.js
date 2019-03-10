; (function (ng) {

    'use strict';

    var checkOrderService = function ($http) {

        var service = this;

        service.getStatus = function (orderNumber) {
            return $http.get('checkout/checkorder', { params: { orderNumber: orderNumber, rnd: Math.random() }}).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('checkOrder')
      .service('checkOrderService', checkOrderService);

    checkOrderService.$inject = ['$http'];

})(window.angular);