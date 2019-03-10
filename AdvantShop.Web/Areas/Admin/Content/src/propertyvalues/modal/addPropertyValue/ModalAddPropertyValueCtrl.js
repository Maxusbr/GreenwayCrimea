; (function (ng) {
    'use strict';

    var ModalAddPropertyValueCtrl = function ($uibModalInstance, $http) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.propertyId = params.propertyId != null ? params.propertyId : 0;
            //ctrl.propertyValueId = params.propertyValueId != null ? params.propertyValueId : 0;
            //ctrl.mode = ctrl.groupId != 0 ? "edit" : "add";

            ctrl.sortOrder = 0;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addPropertyValue = function () {
            $http.post('propertyValues/addPropertyValue', { propertyId: ctrl.propertyId, value: ctrl.value, sortOrder: ctrl.sortOrder }).then(function(response) {
                $uibModalInstance.close('addpropertyValue');
            });
        };
    };

    ModalAddPropertyValueCtrl.$inject = ['$uibModalInstance', '$http'];

    ng.module('uiModal')
        .controller('ModalAddPropertyValueCtrl', ModalAddPropertyValueCtrl);

})(window.angular);