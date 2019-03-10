; (function (ng) {
    'use strict';

    var options = {
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        useExternalPagination: true,
        enablePaginationControls: false,
        useExternalSorting: true,
        enableCellEditOnFocus: false,
        enableCellEdit: false,
        enableSelectAll: true,
        enableColumnMenus: false,
        rowHeight: 60,
        paginationCurrentPage: 1,
        paginationPageSize: 10,
        paginationPageSizes: [10, 20, 50, 100],
        uiGridCustom: {
            selectionOptions: null
        }
    }

    ng.module('uiGridCustom')
      .constant('uiGridCustomConfig', options)
        .constant('uiGridCustomParamsConfig', {
            paginationCurrentPage: options.paginationCurrentPage,
            paginationPageSize: options.paginationPageSize
        })
       .constant('uiGridCustomDataScheme', {
           DataItems: 'data',
           ItemsPerPage: 'paginationPageSize',
           Page: 'paginationCurrentPage',
           TotalItemsCount: 'totalItems'
       });

})(window.angular);