; (function (ng) {

    var cardsService = function ($http) {
        var service = this;

        service.apply = function (code) {
            return $http.post('coupon/couponpost', { code: code, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.deleteCoupon = function () {
            return $http.post('coupon/deletecoupon', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.deleteCertificate = function () {
            return $http.post('coupon/deletecertificate', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('cards')
      .service('cardsService', cardsService);

    cardsService.$inject = ['$http'];

})(window.angular);