; (function (ng) {

    'use strict';

    var GiftCertificateCtrl = function (giftcertificateService, $document) {
        var ctrl = this;
        
        ctrl.init = function(paymentType) {
            ctrl.visiblePhone = paymentType == "qiwi";
        }

		ctrl.send = function () {

            var result = true;

            if (typeof (ctrl.agreement) != "undefined" && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                result = false;
            }

            return result;
        }
		
        ctrl.paymentMethodChange = function (paymentType, id) {
            ctrl.visiblePhone = paymentType == "qiwi";
            var el = $document[0].getElementById("PaymentMethod");
            el.value = id;
        }

        ctrl.previewModal = giftcertificateService.dialogOpen;
    };

    ng.module('giftcertificate')
        .controller('GiftCertificateCtrl', GiftCertificateCtrl);

    GiftCertificateCtrl.$inject = ['giftcertificateService', '$document'];


})(window.angular);

