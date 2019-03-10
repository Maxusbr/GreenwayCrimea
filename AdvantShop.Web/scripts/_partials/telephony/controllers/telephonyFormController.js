; (function (ng) {
    'use strict';

    var TelephonyFormCtrl = function ($cookies, telephonyService, toaster, $translate) {
        var ctrl = this;

        ctrl.submit = function (phone) {

            telephonyService.call(phone, true).then(function (result) {
                if (result.Success === true) {      // нерабочее время. Создан лид
                    ctrl.isCompact = true;
                    ctrl.reset();
                    $(document).trigger("callback_request");
                } else {    // рабочее время, повторный запрос с отправкой запроса на соединение с клиентом
                    telephonyService.call(phone, false).then(function (subResult) {
                        if (subResult.Success === false) {
                            // ошибке при отправке запроса
                            toaster.pop('error', $translate.instant('Js.Telephony.ErrorRequestCall'), subResult.Message);
                        } else {
                            // Внимание! код достигается либо после ответа клиента на звонок, либо по истечении таймаута!
                            ctrl.isCompact = true;
                            ctrl.reset();
                            $(document).trigger("callback");
                        }
                    });
                }

                telephonyService.dialogOpen($translate.instant('Js.Telephony.ThxForRequest'), result.Message);
            });
        };

        ctrl.reset = function () {
            ctrl.phone = '';
            ctrl.form.$setPristine();
        };

        ctrl.switchOnCompact = function () {
            ctrl.isCompact = true;
            ctrl.focus = false;
            $cookies.put('telephonyUserMode', 'Mini');
        };

        ctrl.switchOnFull = function () {
            ctrl.isCompact = false;
            ctrl.focus = true;
            $cookies.put('telephonyUserMode', 'Full');
        };
    };

    ng.module('telephony')
      .controller('TelephonyFormCtrl', TelephonyFormCtrl);

    TelephonyFormCtrl.$inject = ['$cookies', 'telephonyService', 'toaster', '$translate'];

})(window.angular);