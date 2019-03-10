; (function (ng) {
    'use strict';

    var BonusAuthCtrl = function (toaster, bonusService, $translate) {
        var ctrl = this;

        ctrl.isCheckout = ctrl.page === 'checkout';

        ctrl.autorize = function () {

            if (ctrl.inProgress === true) {
                return;
            }

            ctrl.inProgress = true;

            bonusService.autorize(ctrl.numberCard, ctrl.phone).then(function (response) {
                if (response.error != null && response.error.length > 0) {
                    toaster.pop('error', $translate.instant('Js.Bonus.AuthCartError'), response.error);
                } else {
                    bonusService.showModalCode(ctrl.check);
                }

                ctrl.inProgress = false;
            });
        };

        ctrl.check = function (code) {
            bonusService.checkCode(code, ctrl.isCheckout).then(function (bonus) {
                if (bonus.error != null && bonus.error.length > 0) {
                    toaster.pop('error', $translate.instant('Js.Bonus.SmsConfirmError'), bonus.error);
                } else {
                    bonusService.successModal();
                    ctrl.callbackSuccess({ bonus: bonus });
                }
            });
        }
    };


    ng.module('bonus')
      .controller('BonusAuthCtrl', BonusAuthCtrl);

    BonusAuthCtrl.$inject = ['toaster', 'bonusService', '$translate'];

})(window.angular);