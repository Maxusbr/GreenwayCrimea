; (function (ng) {
    'use strict';

    ng.module('productPicker')
        .component('productPicker', {
            templateUrl: '../areas/admin/content/src/product-picker/templates/productPicker.html',
            controller: 'ProductPickerCtrl',
            bindings: {
                treeUrl: '@',
                gridUrl: '@',
                treeConfig: '<?',
                gridConfig: '<?'
            }
        });
})(window.angular);