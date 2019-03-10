; (function (ng) {
    'use strict';

    ng.module('uiGridCustomSelection')
      .component('uiGridCustomSelection', {
          require: {
              uiGridCustomCtrl: '^uiGridCustom'
          },
          templateUrl: '../areas/admin/content/src/_shared/ui-grid-custom/templates/ui-grid-custom-selection-action.html',
          controller: 'UiGridCustomSelectionCtrl',
          bindings: {
              grid: '<',
              gridMenuItems: '<',
              gridApi: '<',
              gridOptions: '<',
              gridParams: '<?',
              gridOnAction: '&',
              gridSelectionOnChange: '&',
              gridSelectionOnInit: '&',
              gridOnRequestBefore: '&'
          }
      });

})(window.angular);