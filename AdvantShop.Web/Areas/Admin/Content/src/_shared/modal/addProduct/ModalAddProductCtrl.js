; (function (ng) {
    'use strict';

    var ModalAddProductCtrl = function ($uibModalInstance, $http, $window, urlHelper, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var categoryId = urlHelper.getUrlParamByName("categoryid");
            if (categoryId != null) {
                $http.get('catalog/getCategoryName?categoryId=' + categoryId).then(function(response) {
                    ctrl.categoryId = categoryId;
                    ctrl.categoryName = response.data.name;
                });
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.changeCategory = function (result) {
            ctrl.categoryId = result.categoryId;
            ctrl.categoryName = result.categoryName;
        }

        ctrl.addProduct = function () {

            if (ctrl.name == "" || ctrl.btnLoading === true) {
                return;
            }

            if (ctrl.categoryId == null) {
                toaster.pop('error', '', 'Выберите категорию');
                return;
            }
            
            ctrl.btnLoading = true;

            $http.post('product/add', { name: ctrl.name, categoryId: ctrl.categoryId }).then(function (response) {
                if (response.data != null && response.data.result) {
                    $window.location.assign('product/edit/' + response.data.obj);
                }
                else {
                    if (response.data.errors.length) {
                        toaster.pop('error', '', response.data.errors[0]);
                    }                    
                }
            }).finally(function () {
                ctrl.btnLoading = false;
            });
        };
    };

    ModalAddProductCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'urlHelper', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddProductCtrl', ModalAddProductCtrl);

})(window.angular);