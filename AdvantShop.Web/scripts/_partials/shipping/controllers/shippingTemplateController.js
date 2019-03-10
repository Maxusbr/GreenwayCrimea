; (function (ng) {
    'use strict';

    var ShippingTemplateCtrl = function ($timeout, $ocLazyLoad, urlHelper) {
        var ctrl = this,
            timer;

        ctrl.prepereLazyLoadUrl = function (params) {
            for (var i = 0, len = params.length; i < len; i++) {
                params[i] = urlHelper.getAbsUrl(params[i], true);
            }
            return params;
        };

        ctrl.changePrepare = function (event, field, shipping) {

            if (field == null) {
                return;
            }

            ng.extend(ctrl.shipping, shipping);

            if (event != null && event.type == 'keyup') {

                if (timer != null) {
                    $timeout.cancel(timer);
                }

                timer = $timeout(function () {
                    ctrl.changeControl({ shipping: ctrl.shipping });
                }, 500);

            } else {
                ctrl.changeControl({ shipping: ctrl.shipping });
            }
        };

        ctrl.changeSpinbox = function (value, proxy) {
            ctrl.changeControl({ shipping: ctrl.shipping });
        };
    };

    ng.module('shipping')
      .controller('ShippingTemplateCtrl', ShippingTemplateCtrl);


    ShippingTemplateCtrl.$inject = ['$timeout', '$ocLazyLoad', 'urlHelper'];

})(window.angular);