; (function (ng) {
    'use strict';

    var ModalSelectCategoriesCtrl = function ($uibModalInstance, $http) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.btnChangeDisabled = true;

            var params = ctrl.$resolve.params;
            ctrl.selectedIds = params.selectedIds || null;
            ctrl.excludeIds = params.excludeIds || null;
        };

        ctrl.choose = function () {

            if (ctrl.categoryIds != null && ctrl.categoryIds.length > 0) {

                $http.get('catalog/getSelectedCategoriesTree', { params: { selectedIds: ctrl.categoryIds } }).then(function(response) {
                    $uibModalInstance.close({ categoryIds: response.data });
                });
            }

        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.treeCallbacks = {
            select_node: function (event, data) {
                var tree = ng.element(event.target).jstree(true);
                ctrl.categoryIds = tree.get_selected(false);

                ctrl.btnChangeDisabled = false;
            },

            deselect_node: function (event, data) {
                var tree = ng.element(event.target).jstree(true);
                ctrl.categoryIds = tree.get_selected(false);

                ctrl.btnChangeDisabled = false;
            },
        };
    };

    ModalSelectCategoriesCtrl.$inject = ['$uibModalInstance', '$http'];

    ng.module('uiModal')
        .controller('ModalSelectCategoriesCtrl', ModalSelectCategoriesCtrl);

})(window.angular);