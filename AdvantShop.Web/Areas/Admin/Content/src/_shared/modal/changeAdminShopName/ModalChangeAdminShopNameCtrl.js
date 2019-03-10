; (function (ng) {
    'use strict';

    var ModalChangeAdminShopNameCtrl = function ($uibModalInstance, $http, $q, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            $q.when(ctrl.$resolve != null && ctrl.$resolve.data != null && ctrl.$resolve.data.name != null ? { data: { name: ctrl.$resolve.data.name } } : $http.get('home/getAdminShopName'))
                .then(function (result) {
                    ctrl.name = result.data.name;
                });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.save = function () {

            if (ctrl.name == "" || ctrl.btnLoading === true) {
                return;
            }
            
            ctrl.btnLoading = true;

            $http.post('home/saveAdminShopName', { name: ctrl.name}).then(function (response) {

                toaster.pop('success', '', 'Изменения успешно сохранены');
                $uibModalInstance.close({ name: response.data.name });

            }).finally(function () {
                ctrl.btnLoading = false;
            });
        };
    };

    ModalChangeAdminShopNameCtrl.$inject = ['$uibModalInstance', '$http', '$q', 'toaster'];

    ng.module('uiModal')
        .controller('ModalChangeAdminShopNameCtrl', ModalChangeAdminShopNameCtrl);

})(window.angular);