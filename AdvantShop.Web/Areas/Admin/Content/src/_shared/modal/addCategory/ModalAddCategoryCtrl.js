; (function (ng) {
    'use strict';

    var ModalAddCategoryCtrl = function ($uibModalInstance) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.btnChangeDisabled = true;

            var params = ctrl.$resolve;
            ctrl.selectedCategoryId = params.selected || 0;
            ctrl.excludeIds = params.currentId || null;
        };

        ctrl.choose = function () {
            $uibModalInstance.close({ categoryId: ctrl.categoryId, categoryName: ctrl.categoryName });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.selectCategory = function (event, data) {
            ctrl.categoryId = data.node.id;
            ctrl.categoryName = data.node.original.name;
            ctrl.btnChangeDisabled = false;
        };
    };

    ModalAddCategoryCtrl.$inject = ['$uibModalInstance'];

    ng.module('uiModal')
        .controller('ModalAddCategoryCtrl', ModalAddCategoryCtrl);

})(window.angular);