; (function (ng) {
    'use strict';

    ng.module('uiGridCustomFilter')
      .component('uiGridCustomFilterBlock', {
          templateUrl: '../areas/admin/content/src/_shared/ui-grid-custom/templates/ui-grid-custom-filter-block.html',
          controller: 'UiGridCustomFilterBlockCtrl',
          bindings: {
              item: '<',
              blockType: '<',
              onApply: '&',
              onClose: '&'
          }
      });

})(window.angular);