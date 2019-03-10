; (function (ng) {
    'use strict';

    var ModalChangeParentMenuItemCtrl = function ($http, $uibModalInstance) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.btnChangeDisabled = true;

            var params = ctrl.$resolve.params;
            ctrl.selectedId = params.selected || 0;
            ctrl.excludeId = params.excludeId || null;
            ctrl.menuType = params.menuType;
        };

        ctrl.change = function () {
            $uibModalInstance.close({ menuItemId: ctrl.menuItemId, menuItemName: ctrl.menuItemName });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.selectItem = function (event, data) {
            ctrl.menuItemId = data.node.id;
            ctrl.menuItemName = data.node.original.name;
            ctrl.btnChangeDisabled = false;
        };
    };

    ModalChangeParentMenuItemCtrl.$inject = ['$http', '$uibModalInstance'];

    ng.module('uiModal')
      .controller('ModalChangeParentMenuItemCtrl', ModalChangeParentMenuItemCtrl);

})(window.angular);