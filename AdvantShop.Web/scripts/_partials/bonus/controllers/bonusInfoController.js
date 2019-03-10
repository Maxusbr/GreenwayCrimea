; (function (ng) {
    'use strict';

    var BonusInfoCtrl = function (toaster, bonusService, modalService, $translate) {
        var ctrl = this,
            isRenderModal = false;

        ctrl.bonusDataMaster = {};

        ctrl.save = function () {
            bonusService.updateCard(ctrl.bonusData).then(function (response) {
                if (response.error != null && response.error.length > 0) {
                    toaster.pop('error', $translate.instant('Js.Bonus.BonusCartTitle'), response.error);
                } else {
                    ctrl.modalDataSave = true;
                    ctrl.dialogClose();
                    toaster.pop('success', $translate.instant('Js.Bonus.BonusCartTitle'), $translate.instant('Js.Bonus.ChangesSaved'));
                }
            });
        };

        ctrl.dialogOpen = function () {

            ctrl.modalDataSave = false;

            ng.copy(ctrl.bonusData, ctrl.bonusDataMaster);

            if (isRenderModal === false) {
                modalService.renderModal(
                    'modalBonusInfo',
                    $translate.instant('Js.Bonus.BonusCartTitle'),
                    '<div data-ng-include="\'scripts/_partials/bonus/templates/modalEdit.html\'"></div>',
                    '<input data-ng-click="bonusInfo.save()" class="btn btn-middle btn-buy" type="button" value="' + $translate.instant('Js.Bonus.Save') + '">',
                    { 
                        isOpen: true, 
                        modalClass: 'modal-bonus-info',
                        callbackClose: 'bonusInfo.dialogReset'
                    },
                    { 
                        bonusInfo: ctrl 
                    });

                isRenderModal = true;

            } else {
                modalService.open('modalBonusInfo');
            }
        };

        ctrl.dialogClose = function () {
            modalService.close('modalBonusInfo');
        };

        ctrl.dialogReset = function () {
            if (ctrl.modalDataSave === false) {
                ng.copy(ctrl.bonusDataMaster, ctrl.bonusData);
            }
        };
};

ng.module('bonus')
  .controller('BonusInfoCtrl', BonusInfoCtrl);

BonusInfoCtrl.$inject = ['toaster', 'bonusService', 'modalService', '$translate'];

})(window.angular);