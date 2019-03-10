; (function (ng) {
    'use strict';

    ng.module('order')
      .directive('orderHistory', function () {
          return {
              restrict: 'A',
              scope: {
                  mode: '='
              },
              bindToController: true,
              controller: 'OrderHistoryCtrl',
              controllerAs: 'orderHistory',
              replace: true,
              template: '<div data-ng-switch="orderHistory.mode"><div data-order-history-items data-ng-switch-when="all"></div><div data-order-history-details data-ng-switch-when="details"></div></div>',
          };
      });

    ng.module('order')
      .directive('orderHistoryItems', function () {
          return {
              require: '^orderHistory',
              restrict: 'A',
              scope: {},
              replace: true,
              templateUrl: '/scripts/_partials/order/templates/items.html',
              link: function (scope, element, attrs, ctrl) { 
                  scope.parentScope = ctrl;
              }
          };
      });

    ng.module('order')
      .directive('orderHistoryDetails', function () {
          return {
              require: '^orderHistory',
              restrict: 'A',
              scope: {},
              replace: true,
              templateUrl: '/scripts/_partials/order/templates/details.html',
              link: function (scope, element, attrs, ctrl) {
                  scope.parentScope = ctrl;
              }
          };
      });

})(window.angular);