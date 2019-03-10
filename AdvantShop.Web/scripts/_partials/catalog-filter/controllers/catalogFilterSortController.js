; (function (ng) {

    'use strict';

    var CatalogFilterSortCtrl = function ($window, catalogFilterService) {

        var ctrl = this;

        ctrl.sort = function () {
            var search, filterData, selectedItems, objForUrl, sortValue;

            search = catalogFilterService.parseSearchString($window.location.search);

            filterData = catalogFilterService.getFilterData();
            selectedItems = catalogFilterService.getSelectedData(filterData) || {};

            if (search != null && search.sort != null) {
                sortValue = search.sort.toLowerCase() == ctrl.asc.toLowerCase() ? ctrl.desc : ctrl.asc;
            } else {
                sortValue = ctrl.asc;
            }

            delete search.page;
            objForUrl = ng.extend({}, search, selectedItems, { sort: sortValue.toLowerCase() });
            $window.location.search = catalogFilterService.buildUrl(objForUrl);
        };
    };

    ng.module('catalogFilter')
      .controller('CatalogFilterSortCtrl', CatalogFilterSortCtrl);

    CatalogFilterSortCtrl.$inject = ['$window', 'catalogFilterService'];

})(window.angular);