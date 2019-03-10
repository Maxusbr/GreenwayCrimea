; (function (ng) {

    'use strict';

    var BrandCtrl = function ($location, $window, brandService) {
        var ctrl = this;

        ctrl.changeCountyId = function (curSort) {
            brandService.filterRefresh(brandService.buildUrlParams($window.location.search, "countryId", curSort));
        };

        ctrl.changeLetter = function (curLetter) {
            brandService.filterRefresh(brandService.buildUrlParams($window.location.search, "letter", curLetter));
        };

        ctrl.changeBrandname = function (event, curName) {
            if ((event.type == 'keypress' && event.keyCode == 13) || event.type == 'click') {
                brandService.filterRefresh(brandService.buildUrlParams($window.location.search, "q", curName));
            }
        };

    };

    ng.module('brand')
      .controller('BrandCtrl', BrandCtrl);

    BrandCtrl.$inject = ['$location', '$window', 'brandService'];

})(window.angular);