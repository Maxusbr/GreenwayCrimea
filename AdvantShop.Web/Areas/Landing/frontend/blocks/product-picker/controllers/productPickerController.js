; (function (ng) {
    'use strict';

    var ProductPickerCtrl = function (productPickerService) {
        var ctrl = this;

        ctrl.tree = {
            core: {
                data: {
                    data: function (node) {
                        return { id: node.id };
                    }
                }
            },
            callbacks: {
                selectNode: function (event, treeObj) {
                    ctrl.grid.callbacks.geProductsByCategory(treeObj);
                }
            }
        };




        ctrl.grid = {
            options: {},
            callbacks: {
                geProductsByCategory: function (treeNodeObj) {
                    productPickerService.geProductsByCategory(treeNodeObj.node.id).then(function (result) {
                        ctrl.grid.options.data = result;
                    });
                }
            }
        };



        ctrl.$onInit = function () {

        };
    };

    ng.module('productPicker')
      .controller('ProductPickerCtrl', ProductPickerCtrl);

    ProductPickerCtrl.$inject = ['productPickerService'];

})(window.angular);