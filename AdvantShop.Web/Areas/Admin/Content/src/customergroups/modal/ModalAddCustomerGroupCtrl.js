; (function (ng) {
    'use strict';

    var ModalAddCustomerGroupCtrl = function ($uibModalInstance, $http) {
        var ctrl = this;
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addCustomerGroup = function () {

            var params = { groupName: ctrl.name, groupDiscount: ctrl.discount, minimumOrderPrice: ctrl.minimumOrderPrice };

            $http.post('customergroups/addCustomerGroup', params).then(function (response) {
                $uibModalInstance.close('addCustomerGroup');
            });
        }
    };

    ModalAddCustomerGroupCtrl.$inject = ['$uibModalInstance', '$http'];

    ng.module('uiModal')
        .controller('ModalAddCustomerGroupCtrl', ModalAddCustomerGroupCtrl);

})(window.angular);