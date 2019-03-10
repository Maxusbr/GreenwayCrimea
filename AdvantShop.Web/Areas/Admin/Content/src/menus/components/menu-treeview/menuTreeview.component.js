; (function (ng) {
    'use strict';

    ng.module('menus')
      .component('menuTreeview', {
          templateUrl: '../areas/admin/content/src/menus/components/menu-treeview/templates/menuTreeview.html',
          controller: 'MenuTreeviewCtrl',
          bindings: {
              selectedId: '@',
              type: '@',
              menuTreeviewOnInit: '&',
              level: '<?'
          }
      })

})(window.angular);