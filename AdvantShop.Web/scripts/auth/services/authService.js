; (function (ng) {

    'use strict';

    var authService = function ($http) {
        var service = this;

        service.login = function (email, password) {
            return $http.post('/user/loginjson', { email: email, password: password }).then(function (response) {
                return response.data;
            });
        }
    };

    ng.module('auth')
      .service('authService', authService);

    authService.$inject = ['$http'];

})(window.angular);