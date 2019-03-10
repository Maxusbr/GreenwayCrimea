; (function (ng) {
    'use strict';

    var modulesService = function ($http) {
        var service = this;

        service.getLocalModules = function (params) {
            return $http.get('modules/getlocalmodules', { params: params }).then(function (response) {
                return response.data;
            });
        };

        service.getMarketModules = function (params) {
            return $http.get('modules/getmarketmodules', { params: params }).then(function (response) {
                return response.data;
            });
        };


    };

    modulesService.$inject = ['$http'];

    ng.module('modules')
        .service('modulesService', modulesService)
  

})(window.angular);