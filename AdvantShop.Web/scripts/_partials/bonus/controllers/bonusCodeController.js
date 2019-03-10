; (function (ng) {
    'use strict';

    var BonusCodeCtrl = function ($q, bonusService) {
        var ctrl = this,
            callback;

        ctrl.state = 'form';

        ctrl.modalInit = function (modal) {
            ctrl.modal = modal;
        };

        ctrl.showModal = function () {
            ctrl.modal.open();
        };

        ctrl.successModal = function () {
            ctrl.state = 'success';
        };

        ctrl.closeModal = function () {
            ctrl.modal.close();
            ctrl.state = 'form';
        };

        ctrl.setCallback = function (fn) {
            callback = fn;
        };

        ctrl.confirm = function (code) {

            if (callback != null) {
                callback(code);
            }
        };

        bonusService.addModalCode(ctrl);
    };

    ng.module('bonus')
      .controller('BonusCodeCtrl', BonusCodeCtrl);

    BonusCodeCtrl.$inject = ['$q','bonusService'];

})(window.angular);