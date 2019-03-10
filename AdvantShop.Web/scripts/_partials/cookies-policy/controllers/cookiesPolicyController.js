; (function (ng) {
    'use strict';

    var CookiesPolicyCtrl = function ($cookies, $translate) {
        var ctrl = this;

        ctrl.onInited = function () {
            $cookies.put(ctrl.cookieName, 'true');
        }

        ctrl.close = function () {
            ctrl.accepted = true;
        }
    };

    ng.module('cookiesPolicy')
      .controller('CookiesPolicyCtrl', CookiesPolicyCtrl);

    CookiesPolicyCtrl.$inject = ['$cookies', '$translate'];

})(window.angular);