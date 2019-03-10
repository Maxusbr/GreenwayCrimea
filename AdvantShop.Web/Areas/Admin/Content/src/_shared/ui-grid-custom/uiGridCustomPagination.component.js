; (function (ng) {
    'use strict';

    ng.module('uiGridCustomPagination')
      .component('uiGridCustomPagination', {
          templateUrl: '../areas/admin/content/src/_shared/ui-grid-custom/templates/ui-grid-custom-pagination.html',
          controller: 'UiGridCustomPaginationCtrl',
          bindings: {
              gridTotalItems: '<',
              gridPaginationPageSize: '<',
              gridPaginationPageSizes: '<',
              gridPaginationCurrentPage: '<',
              onChange: '&'
          },
          transclude: true
      });

})(window.angular);