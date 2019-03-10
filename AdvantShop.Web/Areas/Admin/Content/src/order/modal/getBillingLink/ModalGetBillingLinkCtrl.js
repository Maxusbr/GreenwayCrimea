; (function (ng) {
    'use strict';

    var ModalGetBillingLinkCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.orderId = params.orderId;

            $http.get('orders/getBillingLink', {params: { orderId: ctrl.orderId}}).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    ctrl.link = data.obj;
                    setTimeout(ctrl.select, 200);
                } else {
                    data.errors.forEach(function(error) {
                        ctrl.error = error;
                    });
                }
            });
        };
        
        ctrl.copyToClipboard = function () {
            var copyTextarea = document.querySelector('.js-copy');
            copyTextarea.select();

            try {
                var successful = document.execCommand('copy');
                if (successful)
                    toaster.pop('success', '', 'Ссылка скопирована!');

            } catch (err) {
                console.log('Oops, unable to copy');
            }
        }

        ctrl.select = function() {
            var copyTextarea = document.querySelector('.js-copy');
            copyTextarea.select();
        }

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalGetBillingLinkCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalGetBillingLinkCtrl', ModalGetBillingLinkCtrl);

})(window.angular);