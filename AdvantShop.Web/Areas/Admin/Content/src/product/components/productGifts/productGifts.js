; (function (ng) {
    'use strict';

    var ProductGiftsCtrl = function ($http, SweetAlert, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            getGifts();
        }

        ctrl.addGifts = function(result) {
            if (result == null || result.ids == null || result.ids.length === 0)
                return;
            $http.post('product/addGifts', { productId: ctrl.productId, offerIds: result.ids }).then(function (response) {
                getGifts();
                toaster.pop('success', '', 'Изменения сохранены');
            });
        }

        ctrl.deleteGift = function (offerId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('product/deleteGift', { productId: ctrl.productId, offerId: offerId }).then(function (response) {
                        getGifts();
                        toaster.pop('success', '', 'Изменения сохранены');
                    });
                }
            });
        }

        function getGifts() {
            $http.get('product/getGifts', { params: { productId: ctrl.productId } }).then(function (response) {
                ctrl.products = response.data;
            });
        }
    };

    ProductGiftsCtrl.$inject = ['$http', 'SweetAlert', 'toaster'];

    ng.module('productGifts', ['offersSelectvizr'])
        .controller('ProductGiftsCtrl', ProductGiftsCtrl)
        .component('productGifts', {
            templateUrl: '../areas/admin/content/src/product/components/productGifts/productGifts.html',
            controller: ProductGiftsCtrl,
            bindings: {
                productId: '@'
            }
      });

})(window.angular);