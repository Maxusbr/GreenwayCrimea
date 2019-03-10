; (function (ng, $) {
    'use strict';

    var SubscribeCtrl = function ($http, toaster, $translate) {

        var ctrl = this;

        ctrl.subscribeSend = function () {
            $http.post('newssubscribe', { email: ctrl.subscribeEmail, agree: ctrl.agree, rnd: Math.random() }).then(function (response) {
                var status = response.data.status;
                if (status === 'success') {
                    toaster.pop('success', $translate.instant('Js.Subscribe.SuccessMsg'));
                    ctrl.agree = false;
                    $(document).trigger('subscribe.email', ctrl.subscribeEmail);
                    ctrl.subscribeEmail = '';
                    ctrl.form.$setPristine();
                } else if (response.data.agree != null && response.data.agree === "none") {
                    toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                } else {
                    toaster.pop('error', $translate.instant('Js.Subscribe.EmailAreadySubscribed'));
                }
            });
        };
    };

    ng.module('subscribe')
      .controller('SubscribeCtrl', SubscribeCtrl);

    SubscribeCtrl.$inject = ['$http', 'toaster', '$translate'];

})(window.angular, window.jQuery);