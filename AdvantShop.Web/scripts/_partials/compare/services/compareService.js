; (function (ng) {

    'use strict';

    var compareService = function ($http) {
        var service = this,
            countObj = {};

        service.add = function (offerId) {
            return $http.post('/compare/addtocomparison', { offerId: offerId, rnd: Math.random() }).then(function (response) {

                countObj.count = response.data.Count;
                $(document).trigger("compare.add");

                return response.data;
            });
        };

        service.remove = function (offerId) {
            return $http.get('/compare/removefromcompare', { params: { offerId: offerId, rnd: Math.random() } }).then(function (response) {

                countObj.count = response.data.Count;

                return response.data;
            });
        };

        service.getCountObj = function () {
            return countObj;
        };

        service.getStatus = function (offerId) {
            return $http.get('/compare/getstatus', { params: { offerId: offerId, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('compare')
      .service('compareService', compareService);

    compareService.$inject = ['$http'];

})(window.angular);