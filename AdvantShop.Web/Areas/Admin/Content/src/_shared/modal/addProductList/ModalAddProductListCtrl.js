; (function (ng) {
    'use strict';

    var ModalAddProductListCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;
        

        ctrl.$onInit = function () {
            ctrl.categoryId = ctrl.$resolve != null && ctrl.$resolve.data != null ? ctrl.$resolve.data.categoryId : null;
            ctrl.categoryName = ctrl.categoryId != null ? ctrl.$resolve.data.categoryName : null;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.changeCategory = function (result) {
            ctrl.categoryId = result.categoryId;
            ctrl.categoryName = result.categoryName;
        }

        ctrl.save = function() {
            if (ctrl.products == null || ctrl.products === "" || ctrl.inProgress === true)
                return;

            ctrl.inProgress = true;

            var products = ctrl.products.split('\n').filter(function (x) { return x.trim() !== "" });

            $http.post('product/addProductList', { categoryId: ctrl.categoryId, products: products }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    $window.location.assign('catalog?categoryId=' + ctrl.categoryId);
                }
                else
                {
                    toaster.pop('error', '', 'Ошибка добавления товара');
                }
            })
            .catch(function () {
                ctrl.inProgress = false;
            })
        }
    };

    ModalAddProductListCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddProductListCtrl', ModalAddProductListCtrl);

})(window.angular);