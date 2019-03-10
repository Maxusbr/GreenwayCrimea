; (function (ng) {
    'use strict';

    var inputSearchCtrl = function ($window) {

        var ctrl = this;

        ctrl.search = function (searchQuery) {
            if (searchQuery) {
                //$window.location.replace($window.location.origin + "/search?q=" + searchQuery);
                $window.location.replace(document.getElementsByTagName('base')[0].href + "/search?q=" + searchQuery);
            }
        };

    };

    ng.module('inputSearch')
      .controller('inputSearchCtrl', inputSearchCtrl);

    inputSearchCtrl.$inject = ['$window'];

})(window.angular);