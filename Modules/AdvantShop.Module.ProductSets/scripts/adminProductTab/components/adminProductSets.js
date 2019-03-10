; (function (ng) {
    'use strict';

    var AdminProductSetsCtrl = function ($http, toaster, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.getLinkedOffers();
        };

        ctrl.getLinkedOffers = function() {
            $http.get('../adminProductSet/getLinkedOffers?productId=' + ctrl.productId)
                .then(function(response) {
                    ctrl.items = response.data;
                });
        }

        ctrl.addLinkedOffers = function (result) {

            if (result == null || result.ids == null || result.ids.length === 0)
                return;

            $http.post('../adminProductSet/addLinkedOffers', { productId: ctrl.productId, offerIds: result.ids })
                .then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения сохранены');
                    }
                })
                .then(ctrl.getLinkedOffers);
        }

        ctrl.deleteItem = function(offerId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {

                    $http.post('../adminProductSet/deleteLinkedOffer', { productId: ctrl.productId, offerId: offerId })
                        .then(function(response) {
                            if (response.data.result === true) {
                                toaster.pop('success', '', 'Изменения сохранены');
                            } else {
                                toaster.pop('error', '', 'Ошибка при удалении');
                            }
                        })
                        .then(ctrl.getLinkedOffers);
                }
            });
        }
    };

    AdminProductSetsCtrl.$inject = ['$http', 'toaster', 'SweetAlert'];

    ng.module('product')
        .controller('AdminProductSetsCtrl', AdminProductSetsCtrl)
        .component('adminProductSets', {
            templateUrl: '../modules/productSets/scripts/adminProductTab/components/templates/adminProductSets.html',
            controller: 'AdminProductSetsCtrl',
            bindings: {
                onInit: '&',
                productId: '@',
                title: '@',
            }
        });

})(window.angular);