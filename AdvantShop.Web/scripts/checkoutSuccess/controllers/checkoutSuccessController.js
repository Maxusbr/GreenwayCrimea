; (function (ng) {

    'use strict';

    var windowIsLoaded = false;

    window.addEventListener('load', function load() {
        window.removeEventListener('load', load);
        windowIsLoaded = true;
    });

    var CheckOutSuccessCtrl = function ($http, $window, $sce, windowService) {
        var ctrl = this;

        ctrl.payment = {};

        ctrl.submitNow = function () {
            setTimeout(function () {
                var form = document.querySelector('.js-checkout-success form');
                if (form != null) {
                    form.submit();
                } else {
                    var link = document.querySelector('.js-checkout-success a');
                    if (link != null) {
                        link.click();
                    }
                }
            }, 100);
        };

        ctrl.getHtml = function (orderid) {
            $http.get('checkout/getpaymentbutton', { params: { orderid: orderid } }).then(function (response) {
                ng.extend(ctrl.payment, response.data);
                ctrl.payment.script = $sce.trustAsHtml(ctrl.payment.script);

                if (ctrl.payment.proceedToPayment === true) {
                    if (windowIsLoaded === false) {

                        $window.addEventListener('load', function load() {

                            $window.removeEventListener('load', load);

                            ctrl.submitNow();
                        });
                    } else {
                        ctrl.submitNow();
                    }
                }
            });
        };

        ctrl.print = function (url) {
            windowService.print(url, 'checkoutSuccess');
        };
    };

    ng.module('checkoutSuccess')
      .controller('CheckOutSuccessCtrl', CheckOutSuccessCtrl);

    CheckOutSuccessCtrl.$inject = ['$http', '$window', '$sce', 'windowService'];

})(window.angular);

