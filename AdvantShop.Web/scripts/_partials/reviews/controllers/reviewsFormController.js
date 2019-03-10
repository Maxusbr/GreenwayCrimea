; (function (ng) {

    'use strict';

    var ReviewsFormCtrl = function ($timeout, toaster, $translate) {
        var ctrl = this;

        ctrl.nameFocus = ctrl.emailFocus = ctrl.textFocus = false;

        ctrl.submit = function () {
            if (ctrl.isShowUserAgreementText && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                return;
            }
            ctrl.submitFn({ form: ctrl });

            if (ctrl.moderate == null || ctrl.moderate == false) {
                $translate(['Js.Reviews.SuccessTitle', 'Js.Reviews.SuccessMessage']).then(function (translations) {
                    toaster.success(translations['Js.Reviews.SuccessTitle'], translations['Js.Reviews.SuccessMessage']);
                });
            }
        };

        ctrl.reset = function () {
            //formScope.name = '';
            //formScope.email = '';
            ctrl.text = '';
            ctrl.image = null;
            ctrl.agreement = false;
            ctrl.form.$setPristine();
        };

        ctrl.setAutofocus = function () {
            ctrl.nameFocus = ctrl.emailFocus = ctrl.textFocus = false;

            $timeout(function () {
                if (ctrl.name == null || ctrl.name.length === 0) {
                    ctrl.nameFocus = true;
                } else if (ctrl.email == null || ctrl.email.length === 0) {
                    ctrl.emailFocus = true;
                }
                else if (ctrl.text == null || ctrl.text.length === 0) {
                    ctrl.textFocus = true;
                }
            }, 0);
        };
    };

    ng.module('reviews')
      .controller('ReviewsFormCtrl', ReviewsFormCtrl);

    ReviewsFormCtrl.$inject = ['$timeout', 'toaster', '$translate'];

})(window.angular);