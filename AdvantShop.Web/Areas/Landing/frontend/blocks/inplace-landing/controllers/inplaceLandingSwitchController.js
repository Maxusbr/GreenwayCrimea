; (function (ng) {
    'use strict';

    var InplaceLandingSwitchCtrl = function ($window) {
        var ctrl = this;

        ctrl.change = function (enabled) {
            $window.location.search = ctrl.updateQueryStringParameter($window.location.search, 'inplace', enabled);
        };

        ctrl.updateQueryStringParameter = function (uri, key, value) {

            var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
            var separator = uri.indexOf('?') !== -1 ? "&" : "?";

            if (uri.match(re)) {
                return uri.replace(re, '$1' + key + "=" + value + '$2');
            }
            else {
                return uri + separator + key + "=" + value;
            }
        }
    };

    ng.module('inplaceLanding')
      .controller('InplaceLandingSwitchCtrl', InplaceLandingSwitchCtrl);

    InplaceLandingSwitchCtrl.$inject = ['$window'];



})(window.angular);