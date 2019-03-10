; (function (ng) {
    'use strict';

    var CardsFormCtrl = function (cardsService, toaster, $translate) {
        var ctrl = this;

        ctrl.apply = function (code) {
            if (code != null && code.length > 0) {
                cardsService.apply(code).then(function (data) {
                    if (data.result === true && data.msg == null) {
                        ctrl.applyFn();
                    } else {
                        ctrl.applyFn();
                        toaster.pop('error', $translate.instant('Js.Cards.CantApplyCoupon'), data.msg);
                    }
                });
            }
        };
    };

    ng.module('cards')
      .controller('CardsFormCtrl', CardsFormCtrl);

    CardsFormCtrl.$inject = ['cardsService', 'toaster', '$translate'];

})(window.angular);