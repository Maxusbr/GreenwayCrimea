; (function (ng) {
    'use strict';

    var isWindowLoaded = false;

    window.addEventListener('load', function () {
        isWindowLoaded = true;
    });

    ng.module('transformer')
      .directive('transformer', ['$window', function ($window) {
          return {
              restrict: 'A',
              controller: 'TransformerCtrl',
              controllerAs: 'transformer',
              bindToController: true,
              scope: true,
              link: function (scope, element, attrs, ctrl) {
                  function load() {
                      var offsetTop = parseFloat(attrs.offsetTop);

                      ctrl.elStartRect = element[0].getBoundingClientRect();
                      ctrl.widthBody = document.querySelector('body').clientWidth;
                      ctrl.limitPos = attrs.limitPos != null;
                      ctrl.offsetTop = isNaN(offsetTop) === false ? offsetTop : 0;
                      ctrl.checkoutResponsive = attrs.checkoutResponsive;

                      var containerLimit = document.getElementById(attrs.containerLimit),
                          parent = element.parent();

                      parent.css('minHeight', parent.height());

                      if (ctrl.isTouchDevice === true) {
                          element.addClass('transformer-touch');
                      } else {
                          element.addClass('transformer-notouch');
                      }

                      ctrl.addContainer(containerLimit);

                      ctrl.initialize = true;

                      if (ctrl.wait === true) {
                          ctrl.calc();
                          scope.$digest();
                      }

                      $window.addEventListener('scroll', function () {
                          ctrl.windowScroll();
                      });
                      $window.addEventListener('resize', function () {
                          ctrl.alignmentX();
                          ctrl.responsiveAlignmentX();
                      });

                  };

                  if (isWindowLoaded === false) {

                        $window.addEventListener('load', load);

                  } else {
                      load();
                  }

              }
          }
      }])
      .directive('transformerMutable', ['$window', function () {
          return {
              require: '^transformer',
              restrict: 'A',
              link: function (scope, element, attrs, transformerCtrl) {
                  transformerCtrl.addMutableItem(element[0]);
              }
          }
      }]);

})(window.angular);