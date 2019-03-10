; (function (ng) {
    'use strict';

    var UiGridCustomPaginationCtrl = function ($element, domService) {
        var ctrl = this;

        ctrl.change = function (paginationCurrentPage, paginationPageSize, paginationPageSizes, $event) {

            var gridElement = domService.closest($element[0], 'ui-grid-custom');

            //if (ctrl.gridTotalItems < paginationCurrentPage * paginationPageSize) {
            //    paginationCurrentPage = 1;
            //}

            if (gridElement != null) {
                gridElement.scrollIntoView();
            }

            ctrl.onChange({ paginationCurrentPage: paginationCurrentPage, paginationPageSize: paginationPageSize, paginationPageSizes: paginationPageSizes })
        };

    };

    UiGridCustomPaginationCtrl.$inject = ['$element','domService'];

    ng.module('uiGridCustomPagination', ['ui.bootstrap', 'dom'])
      .controller('UiGridCustomPaginationCtrl', UiGridCustomPaginationCtrl);

})(window.angular);