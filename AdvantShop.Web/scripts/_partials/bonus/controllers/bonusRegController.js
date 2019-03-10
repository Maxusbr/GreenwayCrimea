; (function (ng) {
    'use strict';

    var BonusRegCtrl = function (toaster, bonusService, modalService, $translate) {
        var ctrl = this;


        ctrl.cardAdded = false;
        ctrl.isCheckout = ctrl.page === 'checkout';
		
		bonusService.getDataAgreement().then(function (response) {
            ctrl.isShowUserAgreementText = response.IsShowUserAgreementText;
            ctrl.userAgreementText = response.UserAgreementText;
        });
		
        ctrl.register = function () {

            if (ctrl.inProgress === true) {
                return;
            }

            ctrl.inProgress = true;

            bonusService.register(ctrl.phone).then(function (response) {
                if (response.error != null && response.error.length > 0) {
                    ctrl.error = response.error;
                } else {
                    ctrl.error = null;
                    bonusService.showModalCode(ctrl.check);
                }

                ctrl.inProgress = false;
            });
        };

		ctrl.validate = function() {
            var result = true;

            if (typeof (ctrl.agreement) != "undefined" && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                result = false;
            }

            return result;
        }
		
        ctrl.check = function (code) {
            //bonusService.newCard(code, ctrl.isCheckout, ctrl.name, ctrl.surname, ctrl.patronymic || '', ctrl.sex || 0, ctrl.birthday, ctrl.phone, ctrl.email || '', ctrl.city || '')
            bonusService.newCard(code, ctrl.isCheckout)
                .then(function (bonus) {
                    if (bonus.error != null && bonus.error.length > 0) {
                        toaster.pop('error', $translate.instant('Js.Bonus.SmsConfirmError'), bonus.error);
                    } else {
                        bonusService.successModal();
                        ctrl.cardAdded = true;
                        ctrl.callbackSuccess({ bonus: bonus });
                    }
                });
        };
    };

    ng.module('bonus')
      .controller('BonusRegCtrl', BonusRegCtrl);

    BonusRegCtrl.$inject = ['toaster', 'bonusService', 'modalService', '$translate'];

})(window.angular);