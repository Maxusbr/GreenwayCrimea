; (function (ng) {
    'use strict';

    var CatalogFilterMobileCtrl = function ($window, $cookies, catalogFilterService) {
        var ctrl = this;

        ctrl.changeSort = function (curSort) {

            var selectedItems;

            var search = catalogFilterService.parseSearchString($window.location.search);
            var filterData = catalogFilterService.getFilterData();
            var sortValue = curSort;
            delete search.page;
            var objForUrl = ng.extend({}, search, {}, { sort: sortValue });
            $window.location.search = catalogFilterService.buildUrl(objForUrl);
        }

        ctrl.setView = function (viewmode) {
           //TODO: In Ng 1.4 will be path prop that we should use there instead this native method
           //var myCookie = (function getCookie(name) {
           //     var matches = document.cookie.match(new RegExp("(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"));
           //     return matches ? decodeURIComponent(matches[1]) : undefined;
           //})('mobile_viewmode');

           document.cookie = 'mobile_viewmode=' + viewmode;

           $window.location.reload();
        }

    };

    ng.module('catalogFilterMobile')
      .controller('CatalogFilterMobileCtrl', CatalogFilterMobileCtrl);

    CatalogFilterMobileCtrl.$inject = ['$window', '$cookies', 'catalogFilterService'];

})(window.angular);