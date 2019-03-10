; (function (ng) {
    'use strict';

    var BuyOneClickFormCtrl = function ($sce, $timeout, $window, buyOneClickService, toaster) {

        var ctrl = this;


        ctrl.$onInit = function () {
            ctrl.success = false;
            ctrl.process = false;
            ctrl.showRedirectButton = false;


            buyOneClickService.getFieldsOptions().then(function (fields) {
                ctrl.fields = ng.extend(fields, ctrl.fieldsOptions);
                ctrl.fields.BuyInOneClickFirstText = $sce.trustAsHtml(ctrl.fields.BuyInOneClickFirstText);
                ctrl.fields.BuyInOneClickFinalText = $sce.trustAsHtml(ctrl.fields.BuyInOneClickFinalText);
            });


            buyOneClickService.getCustomerInfo().then(function (data) {
                ctrl.name = data.name;
                ctrl.email = data.email;
                ctrl.phone = data.phone;
            });

            if (ctrl.formInit != null) {
                ctrl.formInit({ form: ctrl });
            }
        }

        ctrl.reset = function () {
            ctrl.name = '';
            ctrl.email = '';
            ctrl.phone = '';
            ctrl.comment = '';

            ctrl.success = false;
            ctrl.showRedirectButton = false;
            ctrl.result = null;

            ctrl.form.$setPristine();
        }

        ctrl.send = function () {

            var isValid = ctrl.buyOneClickValid();

            if (isValid === true || isValid == null) {

                ctrl.process = true;

                buyOneClickService.checkout(ctrl.page, ctrl.orderType, ctrl.offerId, ctrl.productId, ctrl.amount, ctrl.attributesXml, ctrl.name, ctrl.email, ctrl.phone, ctrl.comment).then(function (result) {
                    if (result.error != null && result.error.length > 0) {
                        toaster.pop('error', null, result.error);
                    } else {
                        ctrl.result = result;
                        ctrl.success = true;
 
                        ctrl.successFn({ result: result });

                        if (ctrl.autoReset != null) {
                            $timeout(ctrl.reset, ctrl.autoReset);
                        }
                    }

                    ctrl.process = false;
                });
            }
        };
    };

    ng.module('buyOneClick')
      .controller('BuyOneClickFormCtrl', BuyOneClickFormCtrl);

    BuyOneClickFormCtrl.$inject = ['$sce', '$timeout', '$window', 'buyOneClickService', 'toaster'];

})(window.angular);