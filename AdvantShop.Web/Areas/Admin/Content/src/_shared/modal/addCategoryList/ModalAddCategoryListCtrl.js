; (function (ng) {
    'use strict';

    var ModalAddCategoryListCtrl = function ($uibModalInstance, $http, $window, toaster) {
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

        ctrl.save = function () {

            if (ctrl.categoryId == null) {
                toaster.pop('error', '', 'Выберите категорию');
                return;
            }

            if (ctrl.categories == null || ctrl.categories === "" || ctrl.inProgress === true) {
                return;
            }

            var categories = ctrl.categories.split('\n').filter(function (x) { return x.trim() !== "" });

            ctrl.inProgress = true;

            $http.post('category/addCategoryList', { categoryId: ctrl.categoryId, categories: categories })
                .then(function (response) {
                    var data = response.data;
                    if (data.result === true) {
                        $window.location.assign('catalog?categoryId=' + ctrl.categoryId);
                    }
                })
                .catch(function () {
                    ctrl.inProgress = false;
                });
        }
    };

    ModalAddCategoryListCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddCategoryListCtrl', ModalAddCategoryListCtrl);

})(window.angular);