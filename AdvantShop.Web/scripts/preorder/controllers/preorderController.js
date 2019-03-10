; (function (ng) {

    'use strict';

    var PreorderCtrl = function () {
        var ctrl = this;

        ctrl.validateInput = function () {

            var result = true;

            if (typeof (ctrl.agreement) != "undefined" && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                result = false;
            }

            return result;
        }

    };

    ng.module('preorder')
      .controller('PreorderCtrl', PreorderCtrl);

    PreorderCtrl.$inject = ['toaster', '$translate'];

})(window.angular);

