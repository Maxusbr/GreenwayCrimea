; (function (ng) {

    'use strict';

    ng.module('productPicker')
    .constant('productPickerConfig', {
        tree: {
            core: {
            },
        },
        grid: {
            options: {
                data: [],
            }
        },
        self: {
            treeUrl: 'adminv2/catalog/categoriestree',
            gridUrl: 'adminv2/catalog/getproductsbycategory'
        }
    });

})(window.angular);