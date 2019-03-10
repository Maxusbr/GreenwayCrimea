; (function (ng) {
    'use strict';

    var CardsRemoveCtrl = function (cardsService) {
        var ctrl = this;

        ctrl.remove = function (type) {

            var request;

            switch (type) {
                case 'coupon':
                    request = cardsService.deleteCoupon();
                    break;
                case 'certificate':
                    request = cardsService.deleteCertificate();
                    break;
                default:
                    throw Error('Not found type for remove discount card');
            }

            request.then(function () {
                ctrl.applyFn();
            });
        };
    };

    ng.module('cards')
      .controller('CardsRemoveCtrl', CardsRemoveCtrl);

    CardsRemoveCtrl.$inject = ['cardsService'];

})(window.angular);