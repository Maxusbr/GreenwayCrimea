; (function (ng) {
    'use strict';

    ng.module('productsSelectvizr')
      .component('productsSelectvizr', {
          templateUrl: '../areas/admin/content/src/_partials/products-selectvizr/templates/products-selectvizr.html',
          controller: 'ProductsSelectvizrCtrl',
          transclude: true,
          bindings: {
              selectvizrTreeUrl: '<',
              selectvizrGridUrl: '<',
              selectvizrGridOptions: '<',
              selectvizrGridParams: '<?',
              selectvizrGridItemsSelected: '<?',
              selectvizrOnChange: '&'
          }
      });

})(window.angular);