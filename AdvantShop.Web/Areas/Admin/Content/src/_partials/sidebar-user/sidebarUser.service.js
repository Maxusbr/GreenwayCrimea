; (function (ng) {
    'use strict';

    var sidebarUserService = function ($http) {
        var service = this, _container;

        service.getUser = function (customerId) {
            return $http.get('account/getuserinfo', { params: { customerId: customerId } }).then(function (response) {
                return response.data.obj;
            });
        };

        service.initContainer = function (container) {
            _container = container;
        };

        service.addUser = function (user) {
            _container.addUser(user);
        };
    };

    sidebarUserService.$inject = ['$http'];

    ng.module('sidebarUser')
        .service('sidebarUserService', sidebarUserService);

})(window.angular);