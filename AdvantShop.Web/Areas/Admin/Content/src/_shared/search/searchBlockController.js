; (function (ng) {

    'use strict';

    ng.module('search', ['ui.bootstrap']);

    var SearchBlockCtrl = function ($q, $http) {
        var ctrl = this;

        ctrl.shouldSelect = function () {
            return false;
        }

        ctrl.find = function (val) {
            return $http.get('search/autocomplete', { params: { q: val, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('search')
      .controller('SearchBlockCtrl', SearchBlockCtrl);

    SearchBlockCtrl.$inject = ['$q', '$http'];

})(window.angular);

