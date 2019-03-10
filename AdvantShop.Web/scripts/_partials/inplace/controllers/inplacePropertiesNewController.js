; (function (ng) {
    'use strict';

    var InplacePropertiesNewCtrl = function ($compile, $http, $scope, $timeout, toaster, inplaceService, $translate) {
        var container,
            isRemoveStaticContainer = false,
            ctrl = this;

        ctrl.$onInit = function () {
            ctrl.autocompleteValueParams = {
                productId: ctrl.productId
            };

            ctrl.inplaceParams = {
                productId: ctrl.productId
            };
        };

        ctrl.save = function () {
            $timeout(function () {
                return inplaceService.save('inplaceeditor/propertyadd', ng.extend(ctrl.inplaceParams, { name: ctrl.name, value: ctrl.value }))
                    .then(function (response) {
                        if (response.data != null) {
                            ctrl.name = '';
                            ctrl.value = '';
                            ctrl.form.$setPristine();
                            ctrl.inplaceParams.propertyId = null;
                            ctrl.inplaceParams.propertyValueId = null;
                            toaster.pop('success', $translate.instant('Js.Inplace.PropertyHasBeenAdded'));
                            ctrl.htmlUpdate = true;

                            ctrl.getPropertiesHtml(ctrl.productId).then(function (properties) {
                                ctrl.generate(properties);
                            });

                        } else {
                            toaster.pop('error', $translate.instant('Js.Inplace.ErrorPropertyAdding'));
                        }
                    });
            }, 100);
        };

        ctrl.autocompleteNameApply = function (value, obj) {

            if (obj != null) {
                ctrl.autocompleteValueParams.propertyId = obj.Key;
                ctrl.inplaceParams.propertyId = obj.Key;
            } else {
                ctrl.autocompleteValueParams.propertyId = null;
                ctrl.inplaceParams.propertyId = null;
            }

            ctrl.inplaceParams.name = value;
        };

        ctrl.autocompleteValueApply = function (value, obj) {

            if (obj != null) {
                ctrl.inplaceParams.propertyValueId = obj.Key;
            } else {
                ctrl.inplaceParams.propertyValueId = null;
            }

            ctrl.inplaceParams.value = value;
        };

        ctrl.getPropertiesHtml = function (productId) {
            return $http.get('/product/productproperties', { params: { productId: productId, renderInplaceBlock: false, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        ctrl.generate = function (html) {
            var containerStatic;

            if (isRemoveStaticContainer === false) {
                //remove container which rendered on page load 
                containerStatic = document.getElementById('properties');

                if (containerStatic != null) {
                    containerStatic.parentNode.removeChild(containerStatic);
                }

                isRemoveStaticContainer = true;
            }

            container = container || document.getElementById('inplacePropertiesNewContainer');
            container.innerHTML = html;
            $compile(container)($scope);
        };
    };

    ng.module('inplace')
      .controller('InplacePropertiesNewCtrl', InplacePropertiesNewCtrl);

    InplacePropertiesNewCtrl.$inject = ['$compile', '$http', '$scope', '$timeout', 'toaster', 'inplaceService', '$translate'];

})(window.angular);