; (function (ng) {
    'use strict';

    var BuyOneClickTriggerCtrl = function ($window, toaster, buyOneClickService) {

        var ctrl = this;

        ctrl.formInit = function (form) {
            ctrl.form = form;
        };

        ctrl.modalCallbackClose = function (modalScope) {

            if (ctrl.form.result != null && ctrl.form.showRedirectButton === true) {
                window.location = ctrl.form.result.url;
            }

            if (ctrl.form.success === true) {
                ctrl.form.reset();
            }
        }

        ctrl.successFn = function (result) {
            if (result.url != null && result.doGo === true) {
                ctrl.form.success = false;
                window.location = result.url;
            } else {
                if (result.url != null) {
                    ctrl.form.showRedirectButton = true;
                } else {
                    buyOneClickService.modalFooterShow(ctrl.modalId, false);
                }
            }
        }
    };

    ng.module('buyOneClick')
      .controller('BuyOneClickTriggerCtrl', BuyOneClickTriggerCtrl);

    BuyOneClickTriggerCtrl.$inject = ['$window', 'toaster', 'buyOneClickService'];

})(window.angular);