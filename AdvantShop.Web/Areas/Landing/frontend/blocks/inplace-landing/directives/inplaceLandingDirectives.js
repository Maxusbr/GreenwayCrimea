; (function (ng) {
    'use strict';

    var imageIdIncrement = 0;

    ng.module('inplaceLanding')
        .directive('inplaceLandingSwitch', function () {
            return {
                restrict: 'A',
                scope: true,
                controller: 'InplaceLandingSwitchCtrl',
                controllerAs: 'inplaceLandingSwitch',
                bindToController: true
            }
        });


    //#region inplaceImage
    ng.module('inplaceLanding')
    .directive('inplaceLandingImage', ['$compile', '$document', '$parse', '$timeout', '$window', 'inplaceLandingService', function ($compile, $document, $parse, $timeout, $window, inplaceLandingService) {
        return {
            restrict: 'A',
            require: ['inplaceLandingImage', '^?carousel', '?^productViewItem', '?^zoomer'],
            scope: true,
            controller: 'InplaceLandingImageCtrl',
            controllerAs: 'inplaceLandingImage',
            bindToController: true,
            replace: true,
            link: function (scope, element, attrs, ctrls) {

                var inplaceLandingImage = ctrls[0],
                    carousel = ctrls[1],
                    productViewItem = ctrls[2],
                    zoomer = ctrls[4],
                    documentProduct = document.querySelector('[data-ng-controller="ProductCtrl as product"]'),
                    setPosition,
                    renderButtons,
                    mouseenter,
                    mouseleave,
                    timer;


                inplaceLandingImage.tagImage = element[0].querySelector('img');

                if (attrs.id == null) {
                    attrs.$set('id', 'inplaceLandingImage_' + imageIdIncrement);
                    imageIdIncrement += 1;
                }

                inplaceLandingImage.carousel = carousel;
                inplaceLandingImage.productViewItem = productViewItem;
                inplaceLandingImage.product = documentProduct != null ? ng.element(documentProduct).controller() : null; //get controller product on details page;
                inplaceLandingImage.inplaceParams = $parse(attrs.inplaceParams)(scope);
                inplaceLandingImage.inplaceUrl = attrs.inplaceUrl;
                inplaceLandingImage.inplaceImageButtonsVisible = ng.extend({ 'add': true, 'update': true, 'delete': true, 'permanentVisible': false }, (new Function('return ' + (attrs.inplaceImageButtonsVisible)))() || {});

                setPosition = function (element, buttons) {

                    if (timer != null) {
                        $timeout.cancel(timer);
                    }

                    return timer = $timeout(function () {

                        buttons.css({
                            'top': element[0].offsetTop + element[0].height,
                            'left': element[0].offsetLeft + element[0].width - buttons[0].offsetWidth
                        });

                        //setPosition(element, buttons);

                        return true;

                    }, 100);
                };

                renderButtons = function (element) {
                    var buttons = ng.element('<div inplace-landing-image-buttons="' + attrs.id + '"></div>')

                    element[0].appendChild(buttons[0]);

                    $compile(buttons)(scope);

                    inplaceLandingImage.buttonsRendered = true;

                    setPosition(element, buttons).finally(function () {
                        inplaceLandingImage.buttons.rendered = true;
                    });
                };

                mouseenter = function () {

                    element[0].classList.add('inplace-image-focus');

                    scope.$apply(function () {

                        inplaceLandingImage.active();

                        if (inplaceLandingImage.buttonsRendered == null) {
                            renderButtons(element);
                        } else if (inplaceLandingImage.buttons != null && inplaceLandingImage.buttons.element != null) {
                            setPosition(element, inplaceLandingImage.buttons.element);
                        }
                    });
                };

                mouseleave = function () {

                    element[0].classList.remove('inplace-image-focus');

                    inplaceLandingImage.isActive = false;

                    setTimeout(function () {
                        scope.$apply(function () {
                            if (inplaceLandingImage.buttons != null && (inplaceLandingImage.buttons.isHoverButtons == false || inplaceLandingImage.buttons.isHoverButtons == null) && inplaceLandingImage.inplaceImageButtonsVisible.permanentVisible !== true) {
                                inplaceLandingImage.showButtons = false;
                            }
                        });
                    }, 100);

                    if (timer != null) {
                        $timeout.cancel(timer);
                    }
                };

                if (inplaceLandingImage.inplaceImageButtonsVisible.permanentVisible === true) {
                    renderButtons(element);
                }

                if (zoomer != null) {
                    //bind to zoomer blocks
                    element[0].parentNode.addEventListener('mouseenter', mouseenter);
                    element[0].parentNode.addEventListener('mouseleave', mouseleave);
                } else {
                    element[0].addEventListener('mouseenter', mouseenter);
                    element[0].addEventListener('mouseleave', mouseleave);
                }

                inplaceLandingService.addInplaceImage(attrs.id, inplaceLandingImage);
            }
        };
    }]);

    ng.module('inplaceLanding')
      .directive('inplaceLandingImageButtons', function () {
          return {
              restrict: 'A',
              scope: {
                  inplaceLandingImageButtons: '@'
              },
              controller: 'InplaceLandingImageButtonsCtrl',
              controllerAs: 'inplaceLandingImageButtons',
              bindToController: true,
              replace: true,
              templateUrl: 'areas/landing/frontend/blocks/inplace-landing/templates/inplaceImageButtons.html',
              link: function (scope, element, attrs, ctrl) {

                  ctrl.element = element;

                  if (ctrl.buttonsVisible.permanentVisible !== true) {
                      element[0].addEventListener('mouseenter', function () {
                          scope.$apply(function () {
                              ctrl.isHoverButtons = true;
                          });
                      });

                      element[0].addEventListener('mouseleave', function () {
                          scope.$apply(function () {
                              ctrl.isHoverButtons = false;
                          });

                          setTimeout(function () {
                              scope.$apply(function () {
                                  if (ctrl.inplaceLandingImage.isActive == false || ctrl.inplaceLandingImage.isActive == null) {
                                      ctrl.inplaceLandingImage.showButtons = false;
                                  }
                              });
                          }, 100);
                      });
                  } else {
                      ctrl.inplaceLandingImage.showButtons = true;
                  }
              }
          };
      });
    //#endregion
})(window.angular);