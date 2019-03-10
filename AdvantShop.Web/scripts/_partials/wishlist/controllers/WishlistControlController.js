; (function (ng) {
    'use strict';

    var WishlistControlCtrl = function (wishlistService) {
        var ctrl = this;

        ctrl.dirty = false;

        ctrl.add = function (offerId) {
            return wishlistService.add(offerId);
        };

        ctrl.remove = function (offerId) {
            return wishlistService.remove(offerId);
        };

        ctrl.change = function (offerId) {

            ctrl.dirty = true;

            if (ctrl.isAdded) {
                ctrl.add(offerId);
            } else {
                ctrl.remove(offerId);
            }
        };

        ctrl.checkStatus = function (offerId) {
            wishlistService.getStatus(offerId).then(function (isAdded) {
                ctrl.isAdded = isAdded;
            });
        };
    };


    ng.module('wishlist')
      .controller('WishlistControlCtrl', WishlistControlCtrl);

    WishlistControlCtrl.$inject = ['wishlistService'];

})(window.angular);