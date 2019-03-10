; (function (ng) {
    'use strict';

    var ModalAddEditVkCategoryCtrl = function ($uibModalInstance, $http, $filter, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.type = ctrl.id !== 0 ? "edit" : "add";
            ctrl.category = {};

            ctrl.getMarketCategories().then(function() {
                if (ctrl.id !== 0) {
                    ctrl.getCategory();
                } else {
                    ctrl.vkCategoryId = ctrl.marketCategories[0];
                    ctrl.category.SortOrder = 0;
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getMarketCategories = function () {
            return $http.get('../vkMarketSettings/getMarketCategories').then(function (response) {
                ctrl.marketCategories = response.data;
            });
        }


        ctrl.getCategory = function () {
            $http.get('../vkMarketSettings/getCategory', { params: { id: ctrl.id } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.category = data;
                    ctrl.vkCategoryId = ctrl.marketCategories.filter(function(x) { return ctrl.category.VkCategoryId === x.Id; })[0];
                }
            });
        }

        ctrl.selectCategories = function(result) {
            ctrl.category.CategoryIds = result.categoryIds;
            ctrl.category.Categories = null;
        }


        ctrl.save = function () {

            if (ctrl.category.CategoryIds == null || ctrl.category.CategoryIds.length === 0) {
                toaster.pop('error', '', 'Выберите категории магазина');
                return;
            }

            ctrl.category.VkCategoryId = ctrl.vkCategoryId.Id;

            var url = ctrl.type === "add" ? '../vkMarketSettings/addCategory' : '../vkMarketSettings/updateCategory';

            $http.post(url, { category: ctrl.category }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    $uibModalInstance.close();
                } else {
                    if (data.errors != null) {
                        data.errors.forEach(function(error) {
                            toaster.pop('error', '', error);
                        });
                    } else {
                        toaster.pop('error', '', 'Ошибка при сохранении');
                    }
                }
            });
        }
    };

    ModalAddEditVkCategoryCtrl.$inject = ['$uibModalInstance', '$http', '$filter', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditVkCategoryCtrl', ModalAddEditVkCategoryCtrl);

})(window.angular);