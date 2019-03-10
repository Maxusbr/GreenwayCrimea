; (function (ng) {
    'use strict';

    ng.module('categoriesBlock')
      .component('categoriesBlock', {
          templateUrl: '../areas/admin/content/src/_partials/categories-block/templates/categories-block.html',
          controller: 'CategoriesBlockCtrl',
          transclude: true,
          bindings: {
              categoryId: '@',
              photoHeight: '@',
              categorysearch: '@',
              onDelete: '&'
          }
      })

})(window.angular);