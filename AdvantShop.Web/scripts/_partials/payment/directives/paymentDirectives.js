; (function (ng) {
    'use strict';

    ng.module('payment')
      .directive('paymentList', ['urlHelper', function (urlHelper) {
          return {
              restrict: 'A',
              scope: {
                  items: '=',
                  selectPayment: '=',
                  countVisibleItems: '=',
                  change: '&',
                  anchor: '@',
                  isProgress: '=?',
                  iconWidth: '@',
                  iconHeight: '@'
              },
              controller: 'PaymentListCtrl',
              controllerAs: 'paymentList',
              bindToController: true,
              replace: true,
              templateUrl: function () {
                  return urlHelper.getAbsUrl('/scripts/_partials/payment/templates/paymentList.html', true);
              }
          };
      }]);

    ng.module('payment')
     .directive('paymentTemplate', ['$compile', '$http', '$templateCache', function ($compile, $http, $templateCache) {
         return {
             restrict: 'A',
             scope: {
                 templateUrl: '=',
                 payment: '=',
                 changeControl: '&'
             },
             controller: 'PaymentTemplateCtrl',
             controllerAs: 'paymentTemplate',
             bindToController: true,
             replace: true,
             template: '<div data-ng-include="paymentTemplate.templateUrl"></div>'
         };
     }]);

})(window.angular);