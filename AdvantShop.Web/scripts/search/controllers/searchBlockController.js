; (function (ng) {

    'use strict';

    var SearchBlockCtrl = function ($window) {
        var ctrl = this;

        ctrl.submit = function (value, redirectOnObj) {
            if (value != null && value.length > 0) {
                var resulUrl = redirectOnObj === true ? value : ctrl.url + '?q=' + encodeURIComponent(value);

                if (resulUrl != null && resulUrl.trim().length > 0) {
                    $window.location.assign(resulUrl);
                }
            }
        };

        ctrl.aSubmut = function (value, obj) {
            if (obj != null) {
                ctrl.submit(obj.Url, true);
            }
        };
    };

    ng.module('search')
      .controller('SearchBlockCtrl', SearchBlockCtrl);

    SearchBlockCtrl.$inject = ['$window']

})(window.angular);

