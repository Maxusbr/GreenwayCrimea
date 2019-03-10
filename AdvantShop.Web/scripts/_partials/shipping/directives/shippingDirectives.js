; (function (ng) {
    'use strict';

    ng.module('shipping')
      .directive('shippingList', ['urlHelper', function (urlHelper) {
          return {
              restrict: 'A',
              scope: {
                  items: '=',
                  selectShipping: '=',
                  countVisibleItems: '=',
                  change: '&',
                  anchor: '@',
                  isProgress: '=?',
                  contact: '<?',
                  isCanAddCustom: '<?',
                  customShipping: '<?',
                  iconWidth: '@',
                  iconHeight: '@',
                  editPrice: '<?'
              },
              controller: 'ShippingListCtrl',
              controllerAs: 'shippingList',
              bindToController: true,
              replace: true,
              templateUrl: function () {
                  return urlHelper.getAbsUrl('/scripts/_partials/shipping/templates/shippingList.html', true);
              }
          };
      }]);

    ng.module('shipping')
      .directive('shippingTemplate', [function () {
          return {
              restrict: 'A',
              scope: {
                  templateUrl: '=',
                  shipping: '=',
                  isSelected: '=',
                  changeControl: '&',
                  contact: '<?'
              },
              controller: 'ShippingTemplateCtrl',
              controllerAs: 'shippingTemplate',
              bindToController: true,
              replace: true,
              template: '<div data-ng-include="shippingTemplate.templateUrl"></div>'
          };
      }]);


    ng.module('shipping')
  .directive('shippingVariants', function () {
      return {
          restrict: 'A',
          scope: {
              type: '@',
              offerId: '=',
              amount: '=',
              svCustomOptions: '=',
              startOfferId: '@',
              startAmount: '@',
              startSvCustomOptions: '@',
              zip: '@',
              initFn: '&'
          },
          controller: 'ShippingVariantsCtrl',
          controllerAs: 'shippingVariants',
          bindToController: true,
          replace: true,
          templateUrl: '/scripts/_partials/shipping/templates/shippingVariants.html'
      };
  });

})(window.angular);