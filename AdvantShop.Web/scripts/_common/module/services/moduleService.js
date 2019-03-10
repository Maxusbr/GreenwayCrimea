; (function (ng) {
    'use strict';

    var moduleService = function ($q, $http, $sce) {
        var service = this,
            modules = {};

        service.getModule = function (key) {
            return modules[key];
        };

        service.add = function (key, moduleScope) {
            modules[key] = modules[key] || [];
            modules[key].push(moduleScope);
        };

        service.update = function (keys) {

            var arrayDefer = [],
                requestPromise,
                moduleItems,
                keyItem;

            if (ng.isString(keys) === true) {
                keys = [keys];
            }

            for (var k = keys.length - 1; k >= 0; k--){
                
                keyItem = keys[k];

                moduleItems = modules[keyItem];

                if (moduleItems != null && moduleItems.length > 0) {
                    arrayDefer.push(service.fetch(keyItem, moduleItems));
                }
            }

            if (arrayDefer.length === 0) {
                requestPromise = $q.defer();
                arrayDefer.push(requestPromise.promise);
                requestPromise.resolve();
            }

            return $q.all(arrayDefer);
        };

        service.fetch = function (key, moduleItems) {
            return service.request(key).then(function (content) {

                for (var i = moduleItems.length - 1; i >= 0; i--) {
                    moduleItems[i].content = $sce.trustAsHtml(content);
                }

                return content;
            })
        };

        service.request = function (key) {
            return $http.get('Modules/RenderModules', { params: { rnd: Math.random(), key: key } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('module')
      .service('moduleService', moduleService);

    moduleService.$inject = ['$q', '$http', '$sce'];

})(window.angular);