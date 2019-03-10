; (function (ng) {
    'use strict';

    ng.module('uiGridCustomFilter')
      .component('uiGridCustomFilter', {
          templateUrl: '../areas/admin/content/src/_shared/ui-grid-custom/templates/ui-grid-custom-filter.html',
          controller: 'UiGridCustomFilterCtrl',
          bindings: {
              gridColumnDefs: '<',
              gridParams: '<?',
              gridSearchText: '<?',
              gridSearchPlaceholder: '<?',
              gridOptions: '<?',
              onInit: '&',
              onChange: '&',
              onRemove: '&'
          }
      });

})(window.angular);