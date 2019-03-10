; (function (ng) {
    'use strict';

    var isWindowLoaded = false;

    window.addEventListener('load', function () {
        isWindowLoaded = true;
    });

    ng.module('carousel')
      .directive('carousel', ['$window', '$timeout', 'carouselDefault', 'carouselService', function ($window, $timeout, carouselDefault, carouselService) {
          return {
              restrict: 'A',
              scope: {
                  isVertical: '&',
                  scrollCount: '&',
                  nav: '&',
                  dots: '&',
                  speed: '&',
                  auto: '&',
                  autoPause: '&',
                  indexActive: '=?',
                  prevIcon: '@',
                  nextIcon: '@',
                  prevIconVertical: '@',
                  nextIconVertical: '@',
                  prevClass: '@',
                  nextClass: '@',
                  dotsClass: '@',
                  dotsItemClass: '@',
                  visibleMax: '&',
                  visibleMin: '&',
                  itemSelectClass: '@',
                  carouselClass: '@',
                  stretch: '&',
                  navPosition: '@',
                  initOnLoad: '&',
                  load: '=?',
                  initFn: '&',
                  itemSelect: '&',
                  initilazeTo: '@'
              },
              controller: 'CarouselCtrl',
              controllerAs: 'carousel',
              bindToController: true,
              link: function (scope, element, attrs, ctrl, trasclude) {

                  var scrollCount = ctrl.scrollCount(),
                      isVertical = ctrl.isVertical(),
                      nav = ctrl.nav(),
                      dots = ctrl.dots(),
                      speed = ctrl.speed(),
                      auto = ctrl.auto(),
                      autoPause = ctrl.autoPause(),
                      visibleMax = ctrl.visibleMax(),
                      visibleMin = ctrl.visibleMin(),
                      stretch = ctrl.stretch();

                  ctrl.isVertical = isVertical != null ? isVertical : carouselDefault.isVertical;
                  ctrl.scrollCount = scrollCount != null ? scrollCount : carouselDefault.scrollCount;
                  ctrl.nav = nav != null ? nav : carouselDefault.nav;
                  ctrl.dots = dots != null ? dots : carouselDefault.dots;
                  ctrl.speed = speed != null ? speed : carouselDefault.speed;
                  ctrl.auto = auto != null ? auto : carouselDefault.auto;
                  ctrl.autoPause = autoPause != null ? autoPause : carouselDefault.autoPause;
                  ctrl.indexActive = ng.isNumber(ctrl.indexActive) ? ctrl.indexActive : carouselDefault.indexActive;
                  ctrl.prevIcon = ctrl.prevIcon != null ? ctrl.prevIcon : carouselDefault.prevIcon;
                  ctrl.nextIcon = ctrl.nextIcon != null ? ctrl.nextIcon : carouselDefault.nextIcon;
                  ctrl.prevIconVertical = ctrl.prevIconVertical != null ? ctrl.prevIconVertical : carouselDefault.prevIconVertical;
                  ctrl.nextIconVertical = ctrl.nextIconVertical != null ? ctrl.nextIconVertical : carouselDefault.nextIconVertical;
                  ctrl.prevClass = ctrl.prevClass != null ? ctrl.prevClass : carouselDefault.prevClass;
                  ctrl.nextClass = ctrl.nextClass != null ? ctrl.nextClass : carouselDefault.nextClass;
                  ctrl.dotsClass = ctrl.dotsClass != null ? ctrl.dotsClass : carouselDefault.dotsClass,
                  ctrl.dotsItemClass = ctrl.dotsItemClass != null ? ctrl.dotsItemClass : carouselDefault.dotsItemClass;
                  ctrl.visibleMax = visibleMax != null ? visibleMax : carouselDefault.visibleMax;
                  ctrl.visibleMin = visibleMin != null ? visibleMin : carouselDefault.visibleMin;
                  ctrl.itemSelectClass = ctrl.itemSelectClass != null ? ctrl.itemSelectClass : carouselDefault.itemSelectClass;
                  ctrl.stretch = stretch != null ? stretch : carouselDefault.stretch;
                  ctrl.navPosition = ctrl.navPosition != null ? ctrl.navPosition : carouselDefault.navPosition;

                  var carouselOptions =  {
                      isVertical: ctrl.isVertical,
                      scrollCount: ctrl.scrollCount,
                      nav: ctrl.nav,
                      dots: ctrl.dots,
                      speed: ctrl.speed,
                      auto: ctrl.auto,
                      autoPause: ctrl.autoPause,
                      indexActive: ctrl.indexActive,
                      prevIcon: ctrl.prevIcon,
                      nextIcon: ctrl.nextIcon,
                      prevIconVertical: ctrl.prevIconVertical,
                      nextIconVertical: ctrl.nextIconVertical,
                      prevClass: ctrl.prevClass,
                      nextClass: ctrl.nextClass,
                      dotsClass: ctrl.dotsClass,
                      dotsItemClass: ctrl.dotsItemClass,
                      visibleMax: ctrl.visibleMax,
                      visibleMin: ctrl.visibleMin,
                      itemSelectClass: ctrl.itemSelectClass,
                      carouselClass: ctrl.carouselClass,
                      stretch: ctrl.stretch,
                      navPosition: ctrl.navPosition,
                      animateString: ctrl.animateString,
                      initFn: function (carousel) {
                          ctrl.initFn({ carousel: carousel });

                          scope.$apply();
                      },
                      itemSelect: function (carousel, item, index) {

                          ctrl.itemSelect({ carousel: carousel, item: item.carouselItemData, index: index });

                          scope.$apply();
                      }
                  };

                  var initFunction = function (element) {
                      carouselService.waitLoadImages(element.querySelectorAll('img')).then(function () {
                          setTimeout(function () {

                              var carouselEl = element;

                              if (ctrl.initilazeTo != null) {
                                  carouselEl = carouselEl.querySelector(ctrl.initilazeTo);
                              }

                              ctrl.carouselNative = (new Carousel(carouselEl, carouselOptions)).init();

                              scope.$digest();
                          }, 0);
                      });
                  };

                  if (ctrl.initOnLoad() === true) {
                      var unbind = scope.$watch('carousel.load', function (newValue, oldValue) {
                          if (newValue != null && newValue === true) {
                              if (isWindowLoaded === false) {
                                  $window.addEventListener('load', function () {
                                      initFunction(element[0]);
                                      unbind();
                                  });
                              } else {
                                  initFunction(element[0]);
                                  unbind();
                              }
                          }
                      });
                  } else {

                      if (isWindowLoaded === false) {
                          $window.addEventListener('load', function () {
                              initFunction(element[0]);
                          });
                      } else {
                          initFunction(element[0]);
                      }
                  };
              }
          };
      }]);

})(window.angular);