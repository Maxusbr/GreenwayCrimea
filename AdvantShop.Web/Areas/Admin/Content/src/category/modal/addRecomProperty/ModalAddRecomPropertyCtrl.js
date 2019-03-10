; (function (ng) {
    'use strict';

    var ModalAddRecomPropertyCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            ctrl.getProperties();
            ctrl.propType = "0";
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getProperties = function () {
            $http.get('category/getProperties').then(function (response) {
                ctrl.properties = response.data;
            });
        }

        ctrl.getPropertyValues = function () {
            if (ctrl.property == null)
                return;

            $http.get('category/getPropertyValues', { params: { propertyId: ctrl.property.PropertyId } }).then(function (response) {
                ctrl.propertyValues = response.data;
                if (ctrl.propertyValues != null && ctrl.propertyValues.length > 0) {
                    ctrl.propertyValue = ctrl.propertyValues[0];
                }
            });
        }

        ctrl.changeProperty = function() {
            ctrl.getPropertyValues();
        }

        ctrl.saveProperty = function() {
            $uibModalInstance.close({
                propertyId: ctrl.property.PropertyId, 
                propertyValueId: ctrl.propType == "0" && ctrl.propertyValue != null ? ctrl.propertyValue.PropertyValueId : null,
                type: ctrl.propType
            });
        }
    };

    ModalAddRecomPropertyCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddRecomPropertyCtrl', ModalAddRecomPropertyCtrl);

})(window.angular);