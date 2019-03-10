; (function (ng) {
    'use strict';

    var CatalogLeftMenuCtrl = function (catalogService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            if(ctrl.onInit != null){
                ctrl.onInit({ catalogLeftMenu  : ctrl});
            }
        };

        ctrl.updateData = function (jstree) {
            catalogService.getDataProducts().then(function (data) {
                ctrl.data = data;
            });
        };
    };


    CatalogLeftMenuCtrl.$inject = ['catalogService'];

    ng.module('catalog')
        .controller('CatalogLeftMenuCtrl', CatalogLeftMenuCtrl);

})(window.angular);
