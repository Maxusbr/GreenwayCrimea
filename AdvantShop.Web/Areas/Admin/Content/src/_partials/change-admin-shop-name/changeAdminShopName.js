; (function (ng) {
    'use strict';

    var ChangeAdminShopNameCtrl = function () {
        var ctrl = this;

        ctrl.save = function (result) {
            ctrl.shopname = result.name;
        };
    };


    ng.module('changeAdminShopName', [])
        .controller('ChangeAdminShopNameCtrl', ChangeAdminShopNameCtrl);

})(window.angular);