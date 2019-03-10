; (function (ng) {

    'use strict';

    var AuthCtrl = function ($window, toaster, authService) {
        var ctrl = this;

        ctrl.login = function (email, password, redirect) {
            authService.login(email, password).then(function (result) {
                if (result.error != null && result.error.length > 0) {
                    toaster.pop('error', result.error);
                } else {
                    if (redirect != null && redirect.length > 0) {
                        $window.location = redirect;
                    } else {
                        $window.location.reload();
                    }
                }
            });
        };

    };

    ng.module('auth')
      .controller('AuthCtrl', AuthCtrl);

    AuthCtrl.$inject = ['$window', 'toaster', 'authService'];

})(window.angular);