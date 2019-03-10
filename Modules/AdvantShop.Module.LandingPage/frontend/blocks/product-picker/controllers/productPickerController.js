; (function (ng) {
    'use strict';

    var ProductPickerCtrl = function (productPickerService) {
        var ctrl = this;

        ctrl.treeConfig = {
            'core': {
                'data': {
                    'url': function (node) {
                        return 'landingpageadmin/categoriestree';
                    },
                    'data': function (node) {
                        return { 'categoryId': node.id };
                    }
                }
            }
        };

        ctrl.$onInit = function () {
            productPickerService.getCategories(0).then(function (result) {
                ctrl.treeData = result;
            });
        };

    };

    ng.module('productPicker')
      .controller('ProductPickerCtrl', ProductPickerCtrl);

    ProductPickerCtrl.$inject = ['productPickerService'];

})(window.angular);