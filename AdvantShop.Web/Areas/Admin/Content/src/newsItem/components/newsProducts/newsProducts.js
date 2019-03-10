; (function (ng) {
    'use strict';

    var NewsProductsCtrl = function ($http, SweetAlert, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            getNewsProducts();
        }
        
        ctrl.addProductsModal = function(result) {
            if (result == null || result.ids == null || result.ids.length === 0)
                return;
            $http.post('news/addNewsProduct', { newsId: ctrl.newsId, ids: result.ids }).then(function (response) {
                getNewsProducts();
                toaster.success('Изменения сохранены');
            });
        }

        ctrl.deleteNewsProduct = function (productId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('news/deleteNewsProduct', { newsId: ctrl.newsId, productId: productId }).then(function (response) {
                        getNewsProducts();
                        toaster.success('Изменения сохранены');
                    });
                }
            });
        }

        function getNewsProducts() {
            $http.get('news/getNewsProducts', { params: { newsId: ctrl.newsId } }).then(function (response) {
                ctrl.products = response.data;
            });
        }
    };

    NewsProductsCtrl.$inject = ['$http', 'SweetAlert', 'toaster'];

    ng.module('newsProducts', [])
        .controller('NewsProductsCtrl', NewsProductsCtrl)
        .component('newsProducts', {
            templateUrl: '../areas/admin/content/src/newsItem/components/newsProducts/newsProducts.html',
            controller: NewsProductsCtrl,
            bindings: {
                newsId: '='
            }
      });

})(window.angular);