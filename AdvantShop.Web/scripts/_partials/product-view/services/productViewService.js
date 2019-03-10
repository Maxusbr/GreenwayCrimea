; (function (ng) {
    'use strict';

    var productViewService = function ($http, $q, $cookies, $window) {
        var service = this,
            productViewTransformers = {},
            queue = {},
            callbacks = {};

        service.getPhotos = function (productId) {
            return $http.get('product/getphotos', { params: { productId: productId, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.getView = function (name) {

            var defer = $q.defer();

            if (productViewTransformers[name] == null) {
                queue[name] = defer;
            } else {
                defer.resolve(productViewTransformers[name]);
            }

            return defer.promise.finally(function () {
                delete queue[name];
            });
        };

        service.addCallback = function (name, func) {
            callbacks[name] = callbacks[name] || [];
            callbacks[name].push(func);
        };

        service.pricessCallback = function (name, data) {
            if (callbacks[name] != null) {
                for (var i = 0, len = callbacks[name].length - 1; i <= len; i++) {
                    callbacks[name][i](data);
                }
            }
        };

        service.setView = function (name, view) {

            if ($window.location.pathname.indexOf('/search') != -1) {
                $cookies.put('search_viewmode', view);
            } else {
                $cookies.put('viewmode', view);
                $cookies.put('mobile_viewmode', view);
            }

            productViewTransformers[name] = productViewTransformers[name] || {};

            productViewTransformers[name].viewName = view;

            if (queue[name] != null) {
                queue[name].resolve(productViewTransformers[name]);
            }

            service.pricessCallback('setView', productViewTransformers[name]);

            return productViewTransformers[name];
        };
    };

    ng.module('productView')
      .service('productViewService', productViewService);

    productViewService.$inject = ['$http', '$q', '$cookies', '$window'];

})(window.angular);