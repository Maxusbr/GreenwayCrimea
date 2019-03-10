; (function (ng) {
    'use strict';

    var ModalShippingsTimeCtrl = function ($uibModalInstance, $window, toaster, $q, $http) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve.obj;
            ctrl.id = params.id;
            ctrl.isLead = params.isLead || false;
            ctrl.urlPath = !ctrl.isLead ? 'orders' : 'leads';

            ctrl.getDeliveryTime();
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getDeliveryTime = function () {
            $http.get(ctrl.urlPath + '/getDeliveryTime', { params: { id: ctrl.id }}).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.deliveryDate = data.DeliveryDate;
                    ctrl.deliveryTime = data.DeliveryTime;
                }
            });
        }

        ctrl.save = function () {
            var params = {
                id: ctrl.id,
                deliveryDate: ctrl.deliveryDate,
                deliveryTime: ctrl.deliveryTime
            };

            $http.post(ctrl.urlPath + '/saveDeliveryTime', params).then(function () {
                $uibModalInstance.close();
            });
        }

    };

    ModalShippingsTimeCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', '$q', '$http'];

    ng.module('uiModal')
        .controller('ModalShippingsTimeCtrl', ModalShippingsTimeCtrl);

})(window.angular);