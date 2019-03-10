; (function (ng) {
    'use strict';

    /**
* @ngdoc service
* @name autocompleter.service:autocompleterService
* @function
*
* @description
* Test service
*/

    var autocompleterService = function ($http, $q, $cacheFactory) {
        var service = this,
            cache = $cacheFactory('autocompleter');

        service.getData = function (url, q, params) {
            return service.getFromServer(url, q, params); // service.getFromCache(url, q, params) ||
        };

        service.getFromCache = function (url, q, params) {

            var unique = service.getUnique(url, q, params),
                cacheItem = cache.get(unique);

            return ng.isDefined(cacheItem) ? $q.when(cacheItem) : undefined;
        };

        service.getFromServer = function (url, q, params) {
            return $http.get(url, { params: ng.extend(params || {}, { q: q }) }).then(function (response) {
                var unique = service.getUnique(url, q, params);

                cache.put(unique, response.data);

                return response.data;
            });
        };

        service.getUnique = function (url, q, params) {
            return [url, '?q=', q, '&' + service.objectToQuery(params)].join('');
        };

        service.objectToQuery = function (obj) {
            var result = [];

            if (obj != null) {
                for (var key in obj) {
                    if (obj.hasOwnProperty(key)) {
                        result.push(encodeURIComponent(key) + '=' + encodeURIComponent(obj[key]));
                    }
                }
            }

            return result.length > 0 ? result.join('&') : '';
        };
    };

    ng.module('autocompleter')
      .service('autocompleterService', autocompleterService);

    autocompleterService.$inject = ['$http', '$q', '$cacheFactory'];

})(window.angular);