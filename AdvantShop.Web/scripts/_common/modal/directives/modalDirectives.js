; (function (ng) {
    'use strict';

    var modalIdIncrement = 0,
        transformName,
        inIframe = (function () {
            try {
                return window.self !== window.top;
            } catch (e) {
                return true;
            }
        })();

    transformName = (function () {
        var arr = ['webkitTransform', 'MozTransform', 'msTransform', 'OTransform', 'transform'],
            noopStyle = document.documentElement.style;

        for (var i = 0, il = arr.length; i < il; i += 1) {
            if (noopStyle[arr[i]] !== undefined) {
                return arr[i];
            }
        }
    })();;

    ng.module('modal')
      .directive('modalControl', ['$compile', '$document', '$parse', '$timeout', 'modalService', 'modalDefaultOptions', 'domService', 'urlHelper', function ($compile, $document, $parse, $timeout, modalService, modalDefaultOptions, domService, urlHelper) {
          return {
              restrict: 'EA',
              transclude: true,
              scope: true,
              replace: true,
              controller: 'ModalCtrl',
              controllerAs: 'modal',
              bindToController: true,
              templateUrl: function (element, attrs) {
                  return attrs.templatePath || urlHelper.getAbsUrl('/scripts/_common/modal/templates/modal.html', true);
              },              
              compile: function (cElement, cAttrs) {
                  return function (scope, element, attrs, ctrl, transclude) {

                      ctrl.id = attrs.id;

                      if (ctrl.id == null || ctrl.id.length === 0) {
                          ctrl.id = 'modal_' + modalIdIncrement;
                          modalIdIncrement += 1;
                          attrs.$set('id', ctrl.id);
                      }

                      if (modalService.hasModal(ctrl.id) === true) {
                          return;
                      }

                      ctrl._form = scope.form;
                      ctrl.inIframe = inIframe;
                      ctrl.modalClass = attrs.modalClass;
                      ctrl.modalOverlayClass = attrs.modalOverlayClass;
                      ctrl.isFloating = ng.isDefined(attrs.isFloating) ? attrs.isFloating == 'true' : modalDefaultOptions.isFloating;
                      ctrl.crossEnable = ng.isDefined(attrs.crossEnable) ? attrs.crossEnable == 'true' : modalDefaultOptions.crossEnable;
                      ctrl.backgroundEnable = ng.isDefined(attrs.backgroundEnable) ? attrs.backgroundEnable === 'true' : modalDefaultOptions.backgroundEnable;
                      ctrl.closeOut = ng.isDefined(attrs.closeOut) ? attrs.closeOut === 'true' : modalDefaultOptions.closeOut;

                      ctrl.isFloating = ng.isDefined(ctrl.isFloating) ? ctrl.isFloating : modalDefaultOptions.isFloating;
                      ctrl.crossEnable = ng.isDefined(ctrl.crossEnable) ? ctrl.crossEnable : modalDefaultOptions.crossEnable;
                      ctrl.backgroundEnable = ng.isDefined(ctrl.backgroundEnable) ? ctrl.backgroundEnable : modalDefaultOptions.backgroundEnable;
                      ctrl.closeOut = ng.isDefined(ctrl.closeOut) ? ctrl.closeOut : modalDefaultOptions.closeOut;
                      ctrl.isOpen = ng.isDefined(ctrl.isOpen) ? ctrl.isOpen : modalDefaultOptions.isOpen;
                      ctrl.startOpenDelay = (ng.isDefined(attrs.startOpenDelay) && attrs.startOpenDelay.length > 0) ? parseInt(attrs.startOpenDelay) : null;
                      ctrl.callbackOpen = $parse(attrs.callbackOpen);
                      ctrl.callbackClose = $parse(attrs.callbackClose);
                      ctrl.callbackInit = $parse(attrs.callbackInit);
                      ctrl.closeEsc = ng.isDefined(attrs.closeEsc) ? attrs.closeEsc === 'true' : modalDefaultOptions.closeEsc;

                      ctrl.destroyOnClose = ng.isDefined(attrs.destroyOnClose) ? attrs.destroyOnClose === 'true' : false;

                      var modalElementTransclude = ng.element(element[0].querySelector('.js-modal-transclude'));

                      transclude(scope, function (clone) {
                          modalElementTransclude.replaceWith(clone);
                      });

                      modalService.addStorage(attrs.id, element, ctrl);

                      if (element[0].parentNode !== document.body) {
                          document.body.appendChild(element[0]);
                      }

                      if (attrs.callbackInit != null) {
                          ctrl.callbackInit(scope);
                      }

                      if (ctrl.startOpenDelay != null) {
                          $timeout(modalService.open.bind(this, ctrl.id), ctrl.startOpenDelay);
                      }

                      if (ctrl.closeEsc === true) {
                          $document[0].addEventListener('keyup', function (event) {
                              if (event.keyCode === 27 && ctrl.isOpen === true) { //esc
                                  ctrl.close();
                                  scope.$apply();
                              }
                          });
                      }
                  }
              }
          };
      }]);

    ng.module('modal')
      .directive('modalHeader', ['$q', '$document', '$window', 'domService', function ($q, $document, $window, domService) {
          return {
              restrict: 'EA',
              require: '^modalControl',
              transclude: true,
              replace: true,
              template: '<div data-ng-transclude class="modal-header js-modal-header"></div>',
              scope: true,
              link: function (scope, element, attrs, ctrl) {

                  ctrl.headerExist = true;

                  var modalHeader = element[0],
                      modal = ctrl.getModalElement()[0];

                  if (ctrl.isFloating === true) {

                      var mouseStartPosition = { x: 0, y: 0 },
                          transitionStartPosition = { x: 0, y: 0 },
                          transitionMovePosition = { x: 0, y: 0 },
                          isFirstMove = true,
                          isMove = false,
                          isModalForseFloatDisable = false;

                      var start = function (e) {

                          var computedStyleTransform, parsedTransform;


                          if (isModalForseFloatDisable === true || domService.closest(e.target, modal) == null || domService.closest(e.target, '.js-modal-header') == null) {
                              return;
                          }


                          mouseStartPosition.x = e.pageX;
                          mouseStartPosition.y = e.pageY;

                          isMove = true;

                          if (isFirstMove === true) {

                              isFirstMove = false;

                              computedStyleTransform = window.getComputedStyle(modal)[transformName];

                              if (computedStyleTransform.length > 0) {

                                  if (computedStyleTransform === 'none') {
                                      transitionStartPosition.x = 0;
                                      transitionStartPosition.y = 0;
                                  } else {
                                      parsedTransform = JSON.parse(computedStyleTransform.replace(/^\w+\(/, "[").replace(/\)$/, "]"));
                                      transitionStartPosition.x = parsedTransform[4];
                                      transitionStartPosition.y = parsedTransform[5];
                                  }
                              }
                          }


                          $document[0].addEventListener('mousemove', move);
                          $document[0].addEventListener('touchmove', move);

                          modalHeader.addEventListener('mouseup', end);
                          modalHeader.addEventListener('touchend', end);

                          //e.stopPropagation();
                          //e.preventDefault();
                      };

                      var move = function (e) {

                          //if (isModalForseFloatDisable === true || domService.closest(e.target, modal) == null || domService.closest(e.target, '.js-modal-header') == null || isMove === false) {
                          //    isMove = false;
                          //    return;
                          //}

                          transitionMovePosition.x = transitionStartPosition.x + (e.pageX - mouseStartPosition.x);
                          transitionMovePosition.y = transitionStartPosition.y + (e.pageY - mouseStartPosition.y);

                          modal.style[ctrl.getTransformMethodString()] = ctrl.getTransformValue(transitionMovePosition.x, transitionMovePosition.y);

                          //e.stopPropagation();
                          //e.preventDefault();
                      };

                      var end = function (e) {
                          //if (isModalForseFloatDisable === true || domService.closest(e.target, modal) == null || domService.closest(e.target, '.js-modal-header') == null) {
                          //    return;
                          //}

                          mouseStartPosition.x = e.pageX;
                          mouseStartPosition.y = e.pageY;

                          transitionStartPosition.x = transitionMovePosition.x;
                          transitionStartPosition.y = transitionMovePosition.y;

                          transitionMovePosition.x = 0;
                          transitionMovePosition.y = 0;

                          isMove = false;

                          $document[0].removeEventListener('mousemove', move);
                          $document[0].removeEventListener('touchmove', move);

                          modalHeader.removeEventListener('mouseup', end);
                          modalHeader.removeEventListener('touchend', end);

                          //e.stopPropagation();
                          //e.preventDefault();
                      };

                      modalHeader.addEventListener('mousedown', start);
                      modalHeader.addEventListener('touchstart', start);

                      $window.matchMedia('(max-width: 30em)').addListener(function () {
                          isModalForseFloatDisable = true;
                      });

                      $window.matchMedia('(min-width: 31em)').addListener(function () {
                          isModalForseFloatDisable = false;
                      });
                  }

              }
          };
      }]);

    ng.module('modal')
      .directive('modalFooter', function () {
          return {
              restrict: 'EA',
              require: '^modalControl',
              transclude: true,
              replace: true,
              template: '<div class="modal-footer" data-ng-show="modal.isShowFooter" data-ng-transclude></div>',
              scope: true
          }
      });

    ng.module('modal')
      .directive('modalOpen', ['$parse', 'modalService', function ($parse, modalService) {
          return {
              restrict: 'A',
              link: function (scope, element, attrs, ctrl) {

                  var callback;

                  if (attrs.modalOpenCallback != null && attrs.modalOpenCallback.length > 0) {
                      callback = $parse(attrs.modalOpenCallback);
                  }

                  element.on('click', function () {

                      if (callback != null) {
                          callback(scope);
                      }

                      modalService.open(attrs.modalOpen, attrs.modalOpenSkipQueue === 'true');
                      scope.$apply();
                  });
              }
          };
      }]);

    ng.module('modal')
    .directive('modalClose', ['$parse', 'modalService', function ($parse, modalService) {
        return {
            require: '?^modalControl',
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {

                var modalCtrl = ctrl,
                    callback;

                if (attrs.modalCloseCallback != null && attrs.modalCloseCallback.length > 0) {
                    callback = $parse(attrs.modalCloseCallback);
                }

                element.on('click', function () {

                    if (callback != null) {
                        callback(scope);
                    }

                    if (attrs.modalClose != null && attrs.modalClose.length > 0) {
                        modalService.close(attrs.modalClose);
                    } else if (modalCtrl != null) {
                        modalCtrl.close();
                    }

                    scope.$apply();
                });
            }
        };
    }]);

})(angular);