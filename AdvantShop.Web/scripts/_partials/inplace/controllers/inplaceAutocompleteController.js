; (function (ng) {
    'use strict';

    var InplaceAutocompleteCtrl = function ($compile, $http, $scope, domService, inplaceService, toaster, $translate) {
        var ctrl = this,
            container,
            isRemoveStaticContainer = false;

        ctrl.$onInit = function () {
            ctrl.autocompleteParams = ctrl.autocompleteParams();
            ctrl.inplaceParams = ctrl.inplaceParams();
        };

        ctrl.active = function () {
            ctrl.isShow = true;
        };

        ctrl.save = function () {
            if (ctrl.startContent !== ctrl.value) {

                inplaceService.save('inplaceeditor/propertyupdate', ng.extend(ctrl.inplaceParams, { content: ctrl.value })).then(function (response) {

                    var data = response.data;

                    if (data.result === true) {
                        toaster.pop('success', $translate.instant('Js.Inplace.PropertyHasBeenUpdate'));

                        if (data.obj != null) {
                            ng.extend(ctrl.inplaceParams, data.obj);
                        }

                        ctrl.startContent = ctrl.value;
                    } else {
                        toaster.pop('error', $translate.instant('Js.Inplace.ErrorPropertyUpdate'));
                    }
                })
                .catch(function () {
                    toaster.pop('error', $translate.instant('Js.Inplace.ErrorPropertyUpdate'));
                })
                .finally(function () {
                    ctrl.isShow = false;
                });
            }
        };

        ctrl.cancel = function () {
            ctrl.isShow = false;
            ctrl.value = ctrl.startContent;
        };

        ctrl.autocompleteApply = function (value, obj) {
            ctrl.inplaceParams.propertyValueId = obj != null ? obj.Key : null;
        };

        ctrl.delete = function (event) {

            var rowDelete;

            if (ctrl.inplaceAutocompleteSelectorBlock != null) {
                rowDelete = domService.closest(event.target, ctrl.inplaceAutocompleteSelectorBlock);
            }

            if (rowDelete != null) {
                rowDelete.parentNode.removeChild(rowDelete);
            }

            ctrl.value = null;

            inplaceService.save('inplaceeditor/propertydelete', ng.extend(ctrl.inplaceParams, { content: ctrl.value })).finally(function () {
                ctrl.isShow = false;
               toaster.pop('success', $translate.instant('Js.Inplace.PropertyHasBeenDelete'));

                ctrl.getPropertiesHtml(ctrl.inplaceParams.productId).then(function (properties) {
                    ctrl.generate(properties);
                });
            });
        };

        ctrl.getPropertiesHtml = function (productId) {
            return $http.get('/product/productproperties', { params: { productId: productId,  renderInplaceBlock: false, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        ctrl.generate = function (html) {
            var containerStatic;

            if (isRemoveStaticContainer === false) {
                //remove container which rendered on page load 
                containerStatic = document.getElementById('properties');
                containerStatic.parentNode.removeChild(containerStatic);
                isRemoveStaticContainer = true;
            }

            container = container || document.getElementById('inplacePropertiesNewContainer');
            container.innerHTML = html;
            $compile(container)($scope);
        };
    };

    ng.module('inplace')
      .controller('InplaceAutocompleteCtrl', InplaceAutocompleteCtrl);

    InplaceAutocompleteCtrl.$inject = ['$compile', '$http', '$scope', 'domService', 'inplaceService', 'toaster', '$translate'];

})(window.angular);