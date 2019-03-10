; (function (ng) {
    'use strict';

    var RelatedProductsCtrl = function ($http, SweetAlert, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            getRelatedProducts();
        }


        ctrl.addProductsModal = function(result) {
            if (result == null || result.ids == null || result.ids.length === 0)
                return;
            $http.post('product/addRelatedProduct', { productId: ctrl.productId, type: ctrl.type, ids: result.ids }).then(function (response) {
                getRelatedProducts();
                toaster.pop('success', '', 'Изменения сохранены');
            });
        }

        ctrl.deleteRelatedProduct = function (relatedProductId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('product/deleteRelatedProduct', { productId: ctrl.productId, type: ctrl.type, relatedProductId: relatedProductId }).then(function (response) {
                        getRelatedProducts();
                        toaster.pop('success', '', 'Изменения сохранены');
                    });
                }
            });
        }


        function getRelatedProducts() {
            $http.get('product/getRelatedProducts', { params: { productId: ctrl.productId, type: ctrl.type } }).then(function (response) {
                ctrl.products = response.data;
            });
        }
    };

    RelatedProductsCtrl.$inject = ['$http', 'SweetAlert', 'toaster'];

    ng.module('relatedProducts', [])
        .controller('RelatedProductsCtrl', RelatedProductsCtrl)
        .component('relatedProducts', {
            templateUrl: '../areas/admin/content/src/product/components/relatedProducts/relatedProducts.html',
            controller: RelatedProductsCtrl,
            bindings: {
                productId: '=',
                type: '@'
            }
      });

})(window.angular);