; (function (ng) {
    'use strict';

    var TelephonyTriggerCtrl = function (telephonyService) {
        var ctrl = this;

        ctrl.dialogOpen = function (code) {
            telephonyService.dialogOpen(ctrl);
        };

        ctrl.modalCallbackClose = function () {
            ctrl.telephonyForm.reset();
            telephonyService.dialogFormReset();
        };
    };

    ng.module('telephony')
      .controller('TelephonyTriggerCtrl', TelephonyTriggerCtrl);

    TelephonyTriggerCtrl.$inject = ['telephonyService'];

})(window.angular);