; (function (ng) {
    'use strict';

    var WishlistCountCtrl = function (wishlistService) {
        var ctrl = this;

        ctrl.countObj = wishlistService.getCountObj();
    };


    ng.module('wishlist')
      .controller('WishlistCountCtrl', WishlistCountCtrl);

    WishlistCountCtrl.$inject = ['wishlistService'];

})(window.angular);