; (function (ng) {

    'use strict';

    var WishlistPageCtrl = function (wishlistService) {
        var ctrl = this;

        ctrl.countObj = wishlistService.getCountObj();
    };

    ng.module('wishlistPage')
      .controller('WishlistPageCtrl', WishlistPageCtrl);


    WishlistPageCtrl.$inject = ['wishlistService'];

})(window.angular);
