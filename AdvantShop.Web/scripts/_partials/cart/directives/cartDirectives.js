; (function (ng) {
    'use strict';

    ng.module('cart')
  .directive('cartMini', ['cartService', function (cartService) {
      return {
          restrict: 'A',
          scope: true,
          controller: 'CartMiniCtrl',
          controllerAs: 'cartMini',
          bindToController: true
      };
  }]);

    ng.module('cart')
      .directive('cartMiniTrigger', function () {
          return {
              require: '^cartMini',
              restrict: 'A',
              scope: {},
              link: function (scope, element, attrs, ctrl) {
                  element.on('click', function (event) {
                      ctrl.triggerClick(event);
                      scope.$apply();
                  });
              }
          };
      });

    ng.module('cart')
      .directive('cartMiniList', ['$window', function ($window) {
          return {
              require: ['cartMiniList', '^cartMini'],
              restrict: 'EA',
              scope: {
                  cartData: '='
              },
              replace: true,
              controller: 'CartMiniListCtrl',
              controllerAs: 'cartMiniList',
              bindToController: true,
              templateUrl: '/scripts/_partials/cart/templates/cart-mini.html',
              link: function (scope, element, attrs, ctrls) {
                  var cartMiniList = ctrls[0],
                      cartMini = ctrls[1];

                  cartMiniList.initialized = true;

                  cartMini.addMinicartList(cartMiniList);

                  element[0].addEventListener('mouseenter', function () {
                      cartMiniList.clearTimerClose();
                      scope.$digest();
                  });

                  element[0].addEventListener('mouseleave', function () {
                      cartMiniList.startTimerClose();
                      scope.$digest();
                  });
              }
          };
      }]);


    ng.module('cart')
      .directive('cartFull', function () {
          return {
              restrict: 'EA',
              scope: {
                  photoWidth: '@'
              },
              controller: 'CartFullCtrl',
              controllerAs: 'cartFull',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/cart/templates/cart-full.html'
          };
      });

    ng.module('cart')
      .directive('cartMobileFull', function () {
          return {
              restrict: 'EA',
              scope: {},
              controller: 'CartMobileFullCtrl',
              controllerAs: 'cartMFull',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/cart/templates/cart-mobile-full.html'
          };
      });

    ng.module('cart')
      .directive('cartAdd', function () {
          return {
              restrict: 'EA',
              scope: {
                  offerId: '=',
                  productId: '=',
                  amount: '=',
                  attributesXml: '=',
                  payment: '=',
                  href: '@',
                  cartAddValid: '&'
              },
              controller: 'CartAddCtrl',
              controllerAs: 'cartAdd',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  element[0].addEventListener('click', function (event) {
                      ctrl.addItem(event, ctrl.offerId, ctrl.productId, ctrl.amount, ctrl.attributesXml, ctrl.payment, ctrl.href).then(function () {
                          //scope.$apply();
                      });
                  });
              }
          };
      });

    ng.module('cart')
    .directive('cartPreorder', function () {
        return {
            replace: true,
            transclude: true,
            restrict: 'EA',
            scope: {
                offerId: '=',
                amount: '=',
                attributesXml: '=',
                cartPreorderValid: '&'
            },
            template: '<a data-ng-click="cartPreorder.cartPreorderValid() === false ? $event.preventDefault(): null" ng-href="preorder?{{' +
                '(\'offerId=\' + cartPreorder.offerId) + ' +
                '(cartPreorder.amount != null && cartPreorder.amount != \'\' ? \'&amount=\' + cartPreorder.amount : \'\') + ' +
                '(cartPreorder.attributesXml != null && cartPreorder.attributesXml != \'\' ? \'&options=\' + cartPreorder.attributesXml : \'\') ' +
                '}}" data-ng-transclude></a>',
            controller: 'CartPreorderCtrl',
            controllerAs: 'cartPreorder',
            bindToController: true
        };
    });

    ng.module('cart')
    .directive('cartCount', ['$sce', function ($sce) {
        return {
            restrict: 'A',
            scope: true,
            controller: 'CartCountCtrl',
            controllerAs: 'cartCount',
            bindToController: true,
            link: function (scope, element, attrs, ctrl, transclude) {
                ctrl.type = attrs.type;
                var startValue = element.html();
                ctrl.startValue = $sce.trustAsHtml(startValue);
            }
        };
    }]);

    ng.module('cart')
    .directive('cartConfirm', function () {
        return {
            restrict: 'A',
            scope: true,
            controller: 'CartConfirmCtrl',
            controllerAs: 'cartConfirm',
            bindToController: true
        };
    });
})(angular);