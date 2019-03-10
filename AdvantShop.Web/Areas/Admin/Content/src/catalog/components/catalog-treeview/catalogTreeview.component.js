; (function (ng) {
    'use strict';

    ng.module('catalog')
      .component('catalogTreeview', {
          templateUrl: '../areas/admin/content/src/catalog/components/catalog-treeview/templates/catalogTreeview.html',
          controller: 'CatalogTreeviewCtrl',
          bindings: {
              categoryIdSelected: '@',
              onInit: '&'
          }
      })

})(window.angular);