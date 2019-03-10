; (function (ng) {
    'use strict';

    ng.module('cartPopup', [])
      .controller('CartPopupCtrl', ['$compile', '$element', '$scope', '$timeout', 'cartService', 'cartConfig', 'modalService', 'cartPopupService', function ($compile, $element, $scope, $timeout, cartService, cartConfig, modalService, cartPopupService) {

          var ctrl = this;

          ctrl.add = function (cart, params) {

              ctrl.addedCount = ng.isArray(params) === true ? params.length : params.addedCount;

              ctrl.cartId = ng.isArray(params) === true ? params[0].cartId : params.cartId;

              cartPopupService.updateItem(ctrl.cartId).then(function (item) {

                  ctrl.item = cartPopupService.prepare(item);

                  modalService.open('modalCartPopup');

                  $timeout(function () {
                      var carouselBlock = document.querySelector('.js-cart-popup-carousel');
                      $compile(carouselBlock.childNodes)($scope);
                  }, 0)
              });
          };

          ctrl.close = function (cart, params) {
              modalService.close('modalCartPopup');
          };

          ctrl.updateAmount = function (value, itemId) {

              var item = {
                  Key: itemId,
                  Value: value
              };

              cartService.updateAmount([item]).then(function () {
                  cartPopupService.updateItem(ctrl.cartId).then(function (item) {
                      ctrl.item = cartPopupService.prepare(item);
                  });
              });
          };

          ctrl.modalCloseCallback = function () {
              $timeout(function () {
                  if (ctrl.item != null && ctrl.item.RelatedProducts != null) {
                      ctrl.item.RelatedProducts = null;
                  }
              }, 100);
          };

          cartService.removeCallback(cartConfig.callbackNames.add, 'cartMiniList');
          cartService.addCallback(cartConfig.callbackNames.add, ctrl.add);
      }])
     .service('cartPopupService', ['$http', '$sce', function ($http, $sce) {
         var service = this;

         service.updateItem = function (cartId) {
             return $http.get('shoppingcartpopup/getcartpopup', { params: { cartId: cartId, rnd: Math.random() } }).then(function (response) {
                 return response.data;
             });
         };

         service.prepare = function (item) {
             item.TotalCount = $sce.trustAsHtml(item.TotalCount);
             item.Price = $sce.trustAsHtml(item.Price);
             item.Cost = $sce.trustAsHtml(item.Cost);
             item.Name = $sce.trustAsHtml(item.Name);
             item.Reviews = $sce.trustAsHtml(item.Reviews);
             item.RelatedProducts = $sce.trustAsHtml(item.RelatedProducts);
             return item;
         };
     }])
     .directive('cartPopup', function () {
         return {
             restrict: 'A',
             scope: true,
             controller: 'CartPopupCtrl',
             controllerAs: 'cartPopup',
             bindToController: true,
             replace: true,
             templateUrl: 'modules/shoppingcartpopup/scripts/templates/cartPopup.html',
             link: function (scopr, element, attrs, ctrl) {
                 ctrl.buttonConfirmUrl = attrs.buttonConfirmUrl;
             }
         };
     });

})(window.angular);