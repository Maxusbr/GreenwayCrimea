; (function (ng) {
    'use strict';

    var ProductPropertiesCtrl = function ($http, $q, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            getProperties().then(ctrl.selectProperty);
        }

        ctrl.propertyValueTransform = function(newTag) {
            return { Value: newTag };
        };

        ctrl.propertyTransform = function (newTag) {
            return { Name: newTag };
        };
        
        ctrl.addPropertyValue = function (propertyId, item, model) {

            ctrl.showNewPropertyValue = false;
            ctrl.newPropertyValue = null;

            var params = {
                ProductId: ctrl.productId,
                PropertyId: propertyId,
                PropertyValueId: item.PropertyValueId,
                Value: item.Value,
                IsNew: isNaN(parseFloat(item.PropertyValueId)) || parseFloat(item.PropertyValueId) < 1 ? true : false
            }
            $http.post('product/addPropertyValue', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения успешно сохранены');
                    item.PropertyValueId = data.propertyValueId;
                    model.PropertyValueId = data.propertyValueId;
                }
            });
        }

        ctrl.removePropertyValue = function (propertyId, item, model, groupId) {

            var params = {
                ProductId: ctrl.productId,
                PropertyValueId: item.PropertyValueId,
            }
            $http.post('product/deletePropertyValue', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения успешно сохранены');
                    getProperties().then(ctrl.selectProperty);
                }
            });
        }

        ctrl.selectProperty = function () {

            ctrl.selectedPropertyValue = null;
            ctrl.newProperty = null;

            if (ctrl.selectedProperty.value == null)
                return;
            
            $http.get('product/getPropertyValues', { params: { propertyId: ctrl.selectedProperty.value } }).then(function (response) {
                ctrl.PropertyValues = response.data;
                if (ctrl.PropertyValues != null && ctrl.PropertyValues.length > 0) {
                    ctrl.selectedPropertyValue = ctrl.PropertyValues[0];
                }
            });
        }

        ctrl.selectPropertyValue = function () {
            ctrl.newPropertyValue = null;
        }

        ctrl.addNewProperty = function () {

            if (!ctrl.showNewProperty) {
                ctrl.showNewProperty = true;
                ctrl.showNewPropertyValue = true;
            } else {
                ctrl.showNewProperty = false;
                ctrl.showNewPropertyValue = false;
            }

            ctrl.newProperty = null;
        }

        ctrl.addPropertyWithValue = function () {

            var params = { ProductId: ctrl.productId };

            if (ctrl.newProperty == null || ctrl.newProperty == '') {
                params.PropertyId = ctrl.selectedProperty.value;
            } else {
                params.PropertyName = ctrl.newProperty;
            }

            if (ctrl.newPropertyValue == null || ctrl.newPropertyValue == '') {
                params.PropertyValueId = ctrl.selectedPropertyValue.value;
            } else {
                params.PropertyValue = ctrl.newPropertyValue;
            }


            $http.post('product/AddPropertyWithValue', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    getProperties().then(ctrl.selectProperty);

                    ctrl.showNewProperty = false;
                    ctrl.showNewPropertyValue = false;
                    ctrl.newProperty = null;
                    ctrl.newPropertyValue = null;

                    toaster.pop('success', '', 'Изменения успешно сохранены');
                } else {
                    toaster.pop('error', '', 'Ошибка при добавлении свойства');
                }
            });
        }

        function getProperties() {
            return $http.get('product/getProperties', { params: { productId: ctrl.productId } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.categoryName = data.CategoryName;
                    ctrl.groups = data.Groups;
                    ctrl.properties = data.Properties;
                    if (ctrl.properties != null && ctrl.properties.length > 0) {
                        ctrl.selectedProperty = ctrl.properties[0];
                    }
                }
            });
        }
    };

    ProductPropertiesCtrl.$inject = ['$http', '$q', 'toaster'];


    ng.module('productProperties', ['ui.select'])
        .controller('ProductPropertiesCtrl', ProductPropertiesCtrl)
        .component('productProperties', {
            templateUrl: '../areas/admin/content/src/product/components/productProperties/productProperties.html',
            controller: 'ProductPropertiesCtrl',
            bindings: {
                productId: '@',
            }
      });

})(window.angular);