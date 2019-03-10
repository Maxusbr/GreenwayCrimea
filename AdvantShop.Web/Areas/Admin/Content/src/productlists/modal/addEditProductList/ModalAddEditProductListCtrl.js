; (function (ng) {
    'use strict';

    var ModalAddEditProductListCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.sortOrder = 0;
                ctrl.enabled = true;
            } else {
                $http.get('productLists/getProductList', { params: { id: ctrl.id }}).then(function (response) {
                    var data = response.data;
                    ctrl.name = data.Name;
                    ctrl.sortOrder = data.SortOrder;
                    ctrl.enabled = data.Enabled;
                    ctrl.description = data.Description;
                });
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.save = function () {

            var url = ctrl.mode == "add" ? 'productLists/addProductList' : 'productLists/updateProductList';

            $http.post(url, { id: ctrl.id, name: ctrl.name, sortOrder: ctrl.sortOrder, enabled: ctrl.enabled, description: ctrl.description }).then(function (response) {
                if (ctrl.mode == "add") {
                    toaster.pop('success', '', 'Список товаров добавлен');
                } else {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
                $uibModalInstance.close('saveProductList');
            });
            
        };
    };

    ModalAddEditProductListCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditProductListCtrl', ModalAddEditProductListCtrl);

})(window.angular);