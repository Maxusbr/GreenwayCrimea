; (function (ng) {
    'use strict';

    var CurrencyCtrl = function ($http) {
        var ctrl = this;

        ctrl.changeCurrency = function (currency) {
            $http.get('/Common/SetCurrency', { params: { currencyISO: currency, rnd: Math.random() } }).then(function (response) {
                window.location.reload();
            });
        };
    };

    ng.module('currency')
      .controller('currencyController', CurrencyCtrl);

    CurrencyCtrl.$inject = ['$http'];

})(window.angular);