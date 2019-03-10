; (function (ng) {
    'use strict';

    var selectCatalogCtrl = function ($window) {

        var ctrl = this;

        ctrl.selectCategory = function (catUrl) {
            $window.location = catUrl;
        };

    };

    ng.module('selectCatalog')
      .controller('selectCatalogCtrl', selectCatalogCtrl);

    selectCatalogCtrl.$inject = ['$window'];

})(window.angular);