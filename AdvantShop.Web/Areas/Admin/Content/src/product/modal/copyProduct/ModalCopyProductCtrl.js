; (function (ng) {
    'use strict';

    var ModalCopyProductCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.product;
            ctrl.productId = params.productId != null ? params.productId : 0;
            ctrl.name = params.name + " - копия";
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.copyProduct = function () {
            $http.post('product/copyProduct', { productId: ctrl.productId, name: ctrl.name }).then(function (response) {
                if (response.data.result == true) {
                    $window.location.assign('product/edit/' + response.data.productId);
                } else {
                    toaster.pop("error", response.data.error);
                    ctrl.btnLoading = false;
                }
            });
        };
    };

    ModalCopyProductCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalCopyProductCtrl', ModalCopyProductCtrl);

})(window.angular);