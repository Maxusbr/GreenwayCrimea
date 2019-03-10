; (function (ng) {
    'use strict';

    ng.module('productView')
      .directive('productViewItem', ['productViewService', 'domService', 'windowService', function (productViewService, domService, windowService) {
          return {
              require: ['^productViewItem'],
              restrict: 'A',
              controller: 'ProductViewItemCtrl',
              controllerAs: 'productViewItem',
              bindToController: true,
              scope: true,
              link: function (scope, element, attrs, ctrls) {

                  var productViewItemCtrl = ctrls[0],
                      timerHover;

                  productViewItemCtrl.productId = parseInt(attrs.productId);
                  productViewItemCtrl.offerId = parseInt(attrs.offerId);

                  productViewService.addCallback('setView', function () {
                      setTimeout(function () {

                          var colorsViewerCarousel = productViewItemCtrl.getControl('colorsViewerCarousel');

                          if (colorsViewerCarousel != null) {
                              colorsViewerCarousel.update();
                          }

                          scope.$digest();

                      }, 50);
                  });


                  element[0].addEventListener('mouseenter', function () {

                      if (timerHover != null) {
                          clearTimeout(timerHover);
                      }

                      timerHover = setTimeout(function () {
                          productViewItemCtrl.enter();
                          scope.$digest();
                      }, 100);
                  });

                  element[0].addEventListener('mouseleave', function () {

                      clearTimeout(timerHover);

                      productViewItemCtrl.leave();

                      scope.$digest();
                  });

                  element[0].addEventListener('touchstart', function () {
                      productViewItemCtrl.enter();
                      scope.$digest();
                  });

                  windowService.addCallback('touchstart', function (eventObj) {
                      var isClickedMe = domService.closest(eventObj.event.target, element[0]) != null;

                      if (isClickedMe === false) {
                          productViewItemCtrl.leave();

                          scope.$digest();
                      }
                  });
              }
          }
      }]);

    ng.module('productView')
      .directive('productViewCarouselPhotos', function () {
          return {
              require: ['^productViewCarouselPhotos', '^productViewItem'],
              restrict: 'A',
              scope: {
                  photoHeight: '@',
                  photoWidth: '@',
                  changePhoto: '&'
              },
              replace: true,
              templateUrl: '/scripts/_partials/product-view/templates/photos.html',
              controller: 'ProductViewCarouselPhotosCtrl',
              controllerAs: 'photosCarousel',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {

                  var carouselPhotosCtrl = ctrl[0],
                      productViewItemCtrl = ctrl[1];

                  carouselPhotosCtrl.parentScope = productViewItemCtrl;

                  productViewItemCtrl.addControl('photosCarousel', carouselPhotosCtrl);
              }
          }
      });

    ng.module('productView')
      .directive('productViewChangeMode', function () {
          return {
              restrict: 'A',
              scope: true,
              controller: 'ProductViewChangeModeCtrl',
              controllerAs: 'changeMode',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  ctrl.name = attrs.name;
                  ctrl.current = attrs.viewMode;
              }
          }
      });

    ng.module('productView')
    .directive('productViewMode', ['productViewService', function (productViewService) {
        return {
            restrict: 'A',
            scope: {
                name: '@',
                current: '@'
            },
            controller: 'ProductViewModeCtrl',
            controllerAs: 'productViewMode',
            bindToController: true,
            link: function (scope, element, attrs, ctrl) {
                productViewService.addCallback('setView', function (view) {
                    element[0].classList.remove('products-view-tile');
                    element[0].classList.remove('products-view-list');
                    element[0].classList.remove('products-view-table');

                    element[0].classList.add('products-view-' + view.viewName);
                });
            }
        }
    }]);

})(window.angular);