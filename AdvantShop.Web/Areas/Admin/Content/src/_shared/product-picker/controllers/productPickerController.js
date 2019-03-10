; (function (ng) {
    'use strict';

    var ProductPickerCtrl = function (productPickerConfig, productPickerService) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.tree = ng.extend(productPickerConfig.tree, ctrl.treeConfig);

            ctrl.grid = ng.extend(productPickerConfig.grid, ctrl.gridConfig);

            ctrl.treeUrl = ctrl.treeUrl || productPickerConfig.self.treeUrl;

            ctrl.gridUrl = ctrl.gridUrl || productPickerConfig.self.gridUrl;

            ctrl.gridCallbacks = {
                geProductsByCategory: function (treeNodeObj) {
                    productPickerService.geProductsByCategory(ctrl.gridUrl, treeNodeObj.node.id).then(function (result) {
                        ctrl.grid.options.data = result;
                    });
                }
            };

            ctrl.treeCallbacks = {
                selectNode: function (event, treeObj) {
                    ctrl.gridCallbacks.geProductsByCategory(treeObj);
                }
            };
        };
    };

    ng.module('productPicker')
      .controller('ProductPickerCtrl', ProductPickerCtrl);

    ProductPickerCtrl.$inject = ['productPickerConfig', 'productPickerService'];

})(window.angular);