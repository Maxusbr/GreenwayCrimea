; (function (ng) {
    'use strict';

    var ModalMoveProductInOtherCategoryCtrl = function ($http, $uibModalInstance, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var resolve = ctrl.$resolve;
            ctrl.params = resolve.params;
            ctrl.removeFromCurrentCategories = resolve.removeFromCurrentCategories || false;
            ctrl.btnMoveDisabled = true;
        };

        ctrl.move = function () {
            $http.post('catalog/changeproductcategory', ng.extend(ctrl.params || {}, { newCategoryId: ctrl.categoryId, removeFromCurrentCategories: ctrl.removeFromCurrentCategories })).then(function (response) {

                if (response.data.result === true) {
                    toaster.pop('success', '', ctrl.removeFromCurrentCategories ? 'Товары перемещены в другую категорию' : 'Товары добавлены в другую категорию');
                }

                $uibModalInstance.close({
                    categoryId: ctrl.categoryId,
                    removeFromCurrentCategories: ctrl.removeFromCurrentCategories
                });
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.selectCategory = function (event, data) {
            ctrl.categoryId = data.node.id;
            ctrl.btnMoveDisabled = false;
        };
    };

    ModalMoveProductInOtherCategoryCtrl.$inject = ['$http', '$uibModalInstance', 'toaster'];

    ng.module('uiModal')
      .controller('ModalMoveProductInOtherCategoryCtrl', ModalMoveProductInOtherCategoryCtrl)

})(window.angular);