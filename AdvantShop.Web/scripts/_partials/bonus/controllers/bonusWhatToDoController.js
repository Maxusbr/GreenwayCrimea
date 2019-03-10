; (function (ng) {
    'use strict';

    var BonusWhatToDoCtrl = function (bonusService, toaster) {

        var ctrl = this;

        ctrl.bonusAvalable = 'not';
        ctrl.activeView = 'none';

        ctrl.isShowPatronymic = ctrl.isShowPatronymic();
        ctrl.isApply = ctrl.isApply();
        
        ctrl.init = function() {
            bonusService.getBonus().then(function (bonus) {

                ctrl.bonusData = bonus;

                if (ctrl.bonusData == null) {
                    ctrl.activeView = ctrl.page === 'myaccount' ? 'myaccount_newcart' : 'form';

                } else if (ctrl.bonusData != null && ctrl.bonusData.bonus != null && ctrl.bonusData.bonus.Blocked === true) {
                    ctrl.activeView = 'blocked';

                } else {
                    ctrl.activeView = ctrl.page === 'checkout' ? 'apply' : 'info';
                }
            });
        }

        ctrl.init();

        ctrl.signIn = function (bonusData) {
            ctrl.bonusData = bonusData;

            ctrl.activeView = (ctrl.page === 'checkout' ? 'apply' : 'info');

            ctrl.autorizeBonus({ cardNumber: bonusData.bonus.CardNumber });
        };

        ctrl.changeBonusInterface = function (isApply) {
            ctrl.changeBonus({ isApply: isApply });
        };

        ctrl.createBonusCard = function() {
            bonusService.createBonusCard().then(function(data) {
                if (data.result === true) {
                    toaster.pop('success', '', 'Бонусная карта создана');
                } else {
                    toaster.pop('error', '', data.error);
                }
                ctrl.init();
            });
        }
    };

    ng.module('bonus')
      .controller('BonusWhatToDoCtrl', BonusWhatToDoCtrl);

    BonusWhatToDoCtrl.$inject = ['bonusService', 'toaster'];

})(window.angular);