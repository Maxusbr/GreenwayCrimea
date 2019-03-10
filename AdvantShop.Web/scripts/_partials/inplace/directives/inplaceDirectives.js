; (function (ng) {
    'use strict';

    var richIdIncrement = 0,
        autocompleteIdIncrement = 0,
        imageIdIncrement = 0,
        priceIdIncrement = 0;

    if (typeof (CKEDITOR) !== 'undefined' && CKEDITOR != null && CKEDITOR.env.isCompatible === true) {

        //#region inplaceRich
        ng.module('inplace')
          .directive('inplaceRich', ['$compile', '$document', '$locale', '$timeout', '$window', 'inplaceService', 'inplaceRichConfig', function ($compile, $document, $locale, $timeout, $window, inplaceService, inplaceRichConfig) {
              return {
                  restrict: 'A',
                  scope: {
                      inplaceRich: '&',
                      //inplaceParams: '@',  //view linking function
                      inplaceUrl: '@'
                  },
                  controller: 'InplaceRichCtrl',
                  controllerAs: 'inplaceRich',
                  bindToController: true,
                  link: function (scope, element, attrs, ctrl) {

                      ctrl.getParams = function () {
                          return (new Function('return ' + element.attr('data-inplace-params'))()) || {};
                      };

                      //get time for ngBind
                      $timeout(function () {

                          var options = ng.extend(ng.copy(inplaceRichConfig), ctrl.inplaceRich() || {}, {
                              language: $locale.id === 'ru-ru' ? 'ru' : ($locale.id === 'uk-ua' ? 'ua' : 'en'),
                          }),

                         timerPositionButtons,
                         timerPositionButtonsFunc,
                         setPosition,
                         saveFn,
                         scriptsInContent;

                          if (attrs.id == null) {
                              attrs.$set('id', 'inplaceRich_' + richIdIncrement);
                              richIdIncrement += 1;
                          }

                          if (options.editorSimple === true) {
                              options.removePlugins = 'toolbar, showborders, magicline';
                              options.enterMode = CKEDITOR.ENTER_BR;
                              options.forcePasteAsPlainText = true;
                              element.addClass('inplace-rich-simple');
                          }

                          setPosition = function (buttons, rect) {
                              buttons.css({
                                  'top': ($window.pageYOffset + rect.bottom) + 'px',
                                  'right': ($document[0].body.clientWidth - rect.right) + 'px'
                              });
                          };

                          timerPositionButtonsFunc = function (buttons, element) {

                              if (timerPositionButtons != null) {
                                  clearTimeout(timerPositionButtons);
                              }

                              timerPositionButtons = setTimeout(function () {

                                  setPosition(buttons, element[0].getBoundingClientRect());

                                  timerPositionButtonsFunc(buttons, element);
                              }, 100);
                          };

                          element.find('script').removeAttr('type');

                          if (document.getElementById(attrs.id) == null) {
                              return;
                          }

                          ctrl.editor = CKEDITOR.inline(attrs.id, options);

                          if (ctrl.editor == null) {
                              return;
                          }

                          ctrl.editor.on('instanceReady', function (e) {
                              if (ctrl.editor.getData().trim().length === 0 && attrs.placeholder != null) {
                                  element.addClass('inplace-rich-empty');
                                  ctrl.editor.setData(attrs.placeholder);
                              }
                          });

                          ctrl.editor.on('focus', function (e) {

                              if (ctrl.editor.getData().trim() === attrs.placeholder) {
                                  element.removeClass('inplace-rich-empty');
                                  ctrl.editor.setData('');
                              }

                              scope.$apply(function () {

                                  var buttons;

                                  ctrl.active();

                                  if (ctrl.buttonsRendered == null || ctrl.buttonsRendered === false) {

                                      buttons = ng.element('<div inplace-rich-buttons="' + attrs.id + '"></div>')

                                      document.body.appendChild(buttons[0]);

                                      $compile(buttons)(scope);

                                      ctrl.buttonsRendered = true;
                                  } else {
                                      buttons = ctrl.buttons.element;
                                  }

                                  timerPositionButtonsFunc(buttons, element);
                              });
                          });

                          ctrl.editor.on('blur', function (e) {
                              clearTimeout(timerPositionButtons);

                              setTimeout(function () { //задержка чтобы узнать щелкнули ли на кнопки
                                  scope.$apply(function () {

                                      ctrl.isShow = false;

                                      if (ctrl.clickedButtons === false || ctrl.clickedButtons == null) {
                                          ctrl.save();
                                      }

                                      ctrl.clickedButtons = false;

                                      if (ctrl.editor.getData().trim().length === 0 && attrs.placeholder != null) {
                                          element.addClass('inplace-rich-empty');
                                          ctrl.editor.setData(attrs.placeholder);
                                      }
                                  });
                              }, 100);
                          });

                          ctrl.editor.on('key', function (event) {

                              var keyCode = event.data.keyCode;

                              switch (keyCode) {
                                  case 13://enter
                                      if (options.editorSimple === true) {
                                          var inputTemp = document.createElement('input'),
                                              pos = element[0].getBoundingClientRect();

                                          inputTemp.className = 'inplace-input-fake';
                                          inputTemp.style.top = pos.top + 'px';
                                          inputTemp.style.left = pos.left + 'px';
                                          document.body.appendChild(inputTemp);
                                          inputTemp.focus();
                                          setTimeout(function () { inputTemp.parentNode.removeChild(inputTemp); }, 100);

                                          //event.editor.focusManager.blur(false);

                                          event.stop();
                                          event.cancel();
                                      }
                                      break;
                                      //case 27://esc
                                      //    event.editor.focusManager.blur(false);
                                      //    event.stop();
                                      //    event.cancel();
                                      //    break;
                              }
                          });

                          CKEDITOR.on('dialogDefinition', function (ev) {
                              var dialogName = ev.data.name;
                              var dialogDefinition = ev.data.definition;

                              if (dialogName == 'table') {
                                  var info = dialogDefinition.getContents('info');

                                  info.get('txtWidth')['default'] = null;
                                  info.get('txtBorder')['default'] = null;
                                  info.get('txtCellPad')['default'] = null;
                                  info.get('txtCellSpace')['default'] = null;
                              }
                          });

                          inplaceService.addRich(attrs.id, ctrl);
                      });
                  }
              };
          }]);

        ng.module('inplace')
          .directive('inplaceRichButtons', function () {
              return {
                  restrict: 'A',
                  scope: {
                      inplaceRichButtons: '@'
                  },
                  controller: 'InplaceRichButtonsCtrl',
                  controllerAs: 'inplaceRichButtons',
                  bindToController: true,
                  replace: true,
                  templateUrl: '/scripts/_partials/inplace/templates/richButtons.html'
              };
          });
        //#endregion

        //#region inplacePrice
        ng.module('inplace')
          .directive('inplacePrice', ['$compile', '$document', '$locale', '$window', 'inplaceService', 'productService', 'domService', function ($compile, $document, $locale, $window, inplaceService, productService, domService) {
              return {
                  restrict: 'A',
                  scope: {
                      inplaceParams: '&',
                      inplaceUrl: '@'
                  },
                  controller: 'InplacePriceCtrl',
                  controllerAs: 'inplacePrice',
                  bindToController: true,
                  link: function (scope, element, attrs, ctrl) {

                      var priceNumber, init;

                      ctrl.product = productService.getProduct();

                      element[0].classList.add('inplace-price-container');
                      element[0].classList.add('inplace-offset');

                      init = function (event) {

                          var priceCurrentBlock = domService.closest(event.target, '.price-current'),
                              priceUnknowBlock = domService.closest(event.target, '.price-unknow'),
                              pricOldBlock = domService.closest(event.target, '.price-old'),
                              pricDiscountPercentBlock = domService.closest(event.target, '.price-discount-percent'),
                              el,
                              options = { language: $locale.id === 'ru-ru' ? 'ru' : ($locale.id === 'uk-ua' ? 'ua' : 'en') },
                              timerPositionButtons,
                              timerPositionButtonsFunc,
                              setPosition,
                              saveFn;

                          if (priceCurrentBlock != null) {
                              el = priceCurrentBlock;
                              ctrl.type = 'price';
                          } else if (priceUnknowBlock != null) {
                              el = priceUnknowBlock;
                              ctrl.type = 'price';
                          } else if (pricOldBlock != null) {
                              el = pricOldBlock;
                              ctrl.type = 'price';
                          } else if (pricDiscountPercentBlock != null) {
                              el = pricDiscountPercentBlock;
                              ctrl.type = 'discountPercent';
                          };

                          if (el == null || (ctrl.needReinit[ctrl.type] === false && priceNumber != null)) {
                              return;
                          };

                          ctrl.needReinit[ctrl.type] = true;

                          priceNumber = el.querySelector('.price-number') || priceUnknowBlock || pricDiscountPercentBlock;

                          if (priceNumber == null) {
                              return;
                          }

                          if (priceNumber.id == null || priceNumber.id.length === 0) {
                              priceNumber.id = 'inplacePrice_' + priceIdIncrement;
                              priceIdIncrement += 1;
                          }

                          if (CKEDITOR.instances[priceNumber.id] != null) {
                              return;
                          }

                          options.removePlugins = 'toolbar, showborders, magicline';
                          options.enterMode = CKEDITOR.ENTER_BR;
                          options.forcePasteAsPlainText = true;

                          priceNumber.classList.add('inplace-rich-simple');

                          setPosition = function (buttons, panel, rect) {
                              buttons.css({
                                  'top': $window.pageYOffset + rect.bottom,
                                  'right': $document[0].body.clientWidth - rect.right
                              });

                              panel.css({
                                  'top': $window.pageYOffset + rect.top - 40, //height panel
                                  'left': rect.left
                              });
                          };

                          timerPositionButtonsFunc = function (buttons, panel, element) {

                              if (timerPositionButtons != null) {
                                  clearTimeout(timerPositionButtons);
                              }

                              timerPositionButtons = setTimeout(function () {

                                  if (priceNumber != null) {
                                      setPosition(buttons, panel, priceNumber.getBoundingClientRect());

                                      timerPositionButtonsFunc(buttons, panel, element);
                                  }

                              }, 100);
                          };

                          priceNumber.setAttribute('contenteditable', 'true');

                          ctrl.editor = CKEDITOR.inline(priceNumber.id, options);

                          ctrl.editor.on('focus', function (e) {
                              if (priceNumber.classList.contains('price-unknown') === true && ctrl.convertToFloat(ctrl.editor.getData({ format: 'text' })) == null) {
                                  ctrl.editor.setData('0');
                              }

                              scope.$apply(function () {

                                  var buttons, panel;

                                  ctrl.active();

                                  if (ctrl.buttonsRendered == null || ctrl.buttonsRendered === false) {

                                      buttons = ng.element('<div data-inplace-price-buttons="' + priceNumber.id + '"></div>')

                                      document.body.appendChild(buttons[0]);

                                      $compile(buttons)(scope);

                                      panel = ng.element('<div data-inplace-price-panel="' + priceNumber.id + '"></div>');

                                      document.body.appendChild(panel[0]);

                                      $compile(panel)(scope);

                                      ctrl.buttonsRendered = true;
                                  } else {
                                      buttons = ctrl.buttons.element;
                                      panel = ctrl.panel.element;
                                  }

                                  timerPositionButtonsFunc(buttons, panel, priceNumber);
                              });
                          });

                          ctrl.editor.on('blur', function (e) {
                              clearTimeout(timerPositionButtons);

                              setTimeout(function () { //задержка чтобы узнать щелкнули ли на кнопки
                                  scope.$apply(function () {

                                      ctrl.isShow = false;

                                      if (ctrl.clickedButtons === false || ctrl.clickedButtons == null) {
                                          ctrl.save();
                                      }

                                      ctrl.clickedButtons = false;
                                  });
                              }, 100);
                          });

                          ctrl.editor.on('key', function (event) {

                              var keyCode = event.data.keyCode;

                              if (keyCode === 13) {
                                  var inputTemp = document.createElement('input'),
                                      pos = element[0].getBoundingClientRect();

                                  inputTemp.className = 'inplace-input-fake';
                                  inputTemp.style.top = pos.top + 'px';
                                  inputTemp.style.left = pos.left + 'px';
                                  document.body.appendChild(inputTemp);
                                  inputTemp.focus();
                                  setTimeout(function () { inputTemp.parentNode.removeChild(inputTemp); }, 100);

                                  //event.editor.focusManager.blur(false);

                                  event.stop();
                                  event.cancel();
                              } else {
                                  var current = ctrl.convertToFloat(ctrl.editor.getData());

                                  if (current === null) {
                                      priceNumber.classList.add('inplace-price-error');
                                  } else {
                                      priceNumber.classList.remove('inplace-price-error');
                                  }
                              }
                          });

                          inplaceService.addInplacePrice(priceNumber.id, ctrl);
                      };

                      element[0].addEventListener('mouseover', function (event) {

                          var el = this;

                          scope.$apply(function () {
                              init(event, el);
                          });
                      });
                  }
              };
          }]);

        ng.module('inplace')
      .directive('inplacePriceButtons', function () {
          return {
              restrict: 'A',
              scope: {
                  inplacePriceButtons: '@'
              },
              controller: 'InplacePriceButtonsCtrl',
              controllerAs: 'inplacePriceButtons',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/inplace/templates/priceButtons.html'
          };
      });

        ng.module('inplace')
        .directive('inplacePricePanel', ['$compile', '$document', '$window', 'inplaceService', function ($compile, $document, $window, inplaceService) {
            return {
                restrict: 'A',
                scope: {
                    inplacePricePanel: '@'
                },
                controller: 'InplacePricePanelCtrl',
                controllerAs: 'inplacePricePanel',
                templateUrl: '/scripts/_partials/inplace/templates/pricePanel.html',
                replace: true,
                bindToController: true
            };
        }]);
        //#endregion
    }

    //#region modal
    ng.module('inplace')
      .directive('inplaceModal', [function () {
          return {
              restrict: 'A',
              scope: {
                  inplaceParams: '&',
                  inplaceUrl: '@'
              },
              controller: 'InplaceModalCtrl',
              controllerAs: 'inplaceModal',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  element[0].addEventListener('click', function (event) {
                      event.preventDefault();
                      scope.$apply(ctrl.modalOpen);
                  });
              }
          };
      }]);
    //#endregion

    //#region inplaceAutocomplete
    ng.module('inplace')
  .directive('inplaceAutocomplete', ['$compile', '$document', '$window', 'inplaceService', function ($compile, $document, $window, inplaceService) {
      return {
          restrict: 'A',
          scope: {
              inplaceParams: '&',
              autocompleteParams: '&inplaceAutocomplete',
              inplaceAutocompleteSelectorBlock: '@'
          },
          controller: 'InplaceAutocompleteCtrl',
          controllerAs: 'inplaceAutocomplete',
          bindToController: true,
          templateUrl: '/scripts/_partials/inplace/templates/inplaceAutocomplete.html',
          replace: true,
          transclude: true,
          link: function (scope, element, attrs, ctrl, transclude) {

              var input = element[0].querySelector('input'),
                  transcludeEl = transclude()[0],
                  setPosition;

              if (transcludeEl) {
                  ctrl.value = transcludeEl.textContent;
              }


              setPosition = function (buttons, rect) {
                  buttons.css({
                      'top': $window.pageYOffset + rect.bottom,
                      'right': $document[0].body.clientWidth - rect.right
                  });
              };

              input.addEventListener('focus', function () {

                  var buttons;

                  element[0].classList.add('inplace-autocomplete-focus');

                  scope.$apply(function () {

                      ctrl.startContent = ctrl.value;

                      ctrl.active();

                      if (ctrl.buttonsRendered == null) {

                          buttons = ng.element('<div inplace-autocomplete-buttons="' + attrs.id + '"></div>')

                          setPosition(buttons, element[0].getBoundingClientRect());

                          document.body.appendChild(buttons[0]);

                          $compile(buttons)(scope);

                          ctrl.buttonsRendered = true;
                      }
                  });
              });

              input.addEventListener('blur', function () {

                  element[0].classList.remove('inplace-autocomplete-focus');

                  setTimeout(function () { //задержка чтобы узнать щелкнули ли на кнопки
                      scope.$apply(function () {

                          ctrl.isShow = false;

                          if (ctrl.clickedButtons === false || ctrl.clickedButtons == null) {
                              ctrl.save();
                          }

                          ctrl.clickedButtons = false;
                      });
                  }, 100);
              });

              input.addEventListener('keyup', function (event) {
                  var inputTemp, pos;

                  if (event.keyCode === 13) {
                      element[0].classList.remove('inplace-autocomplete-focus');

                      inputTemp = document.createElement('input');
                      pos = element[0].getBoundingClientRect();

                      inputTemp.className = 'inplace-input-fake';
                      inputTemp.style.top = pos.top + 'px';
                      inputTemp.style.left = pos.left + 'px';
                      document.body.appendChild(inputTemp);
                      inputTemp.focus();

                      setTimeout(function () { inputTemp.parentNode.removeChild(inputTemp); }, 100);
                  }
              });

              if (attrs.id == null) {
                  attrs.$set('id', 'inplaceAutocomplete_' + autocompleteIdIncrement);
                  autocompleteIdIncrement += 1;
              }

              inplaceService.addInplaceAutocomplete(attrs.id, ctrl);
          }
      };
  }]);

    ng.module('inplace')
  .directive('inplaceAutocompleteButtons', function () {
      return {
          restrict: 'A',
          scope: {
              inplaceAutocompleteButtons: '@'
          },
          controller: 'InplaceAutocompleteButtonsCtrl',
          controllerAs: 'inplaceAutocompleteButtons',
          bindToController: true,
          replace: true,
          templateUrl: '/scripts/_partials/inplace/templates/inplaceAutocompleteButtons.html'
      };
  });

    //#endregion

    //#region inplaceProperties
    ng.module('inplace')
    .directive('inplacePropertiesNew', function () {
        return {
            restrict: 'A',
            scope: {
                productId: '@'
            },
            controller: 'InplacePropertiesNewCtrl',
            controllerAs: 'inplacePropertiesNew',
            bindToController: true,
            replace: true,
            templateUrl: '/scripts/_partials/inplace/templates/propertiesNew.html'
        };
    });
    //#endregion

    //#region inplaceImage
    ng.module('inplace')
    .directive('inplaceImage', ['$compile', '$document', '$parse', '$timeout', '$window', 'inplaceService', function ($compile, $document, $parse, $timeout, $window, inplaceService) {
        return {
            restrict: 'A',
            require: ['inplaceImage', '^?carousel', '?^productViewItem', '?^^', '?^zoomer'],
            scope: true,
            controller: 'InplaceImageCtrl',
            controllerAs: 'inplaceImage',
            bindToController: true,
            replace: true,
            link: function (scope, element, attrs, ctrls) {

                var inplaceImage = ctrls[0],
                    carousel = ctrls[1],
                    productViewItem = ctrls[2],
                    zoomer = ctrls[4],
                    documentProduct = document.querySelector('[data-ng-controller="ProductCtrl as product"]'),
                    setPosition,
                    renderButtons,
                    mouseenter,
                    mouseleave,
                    timer;


                if (attrs.id == null) {
                    attrs.$set('id', 'inplaceImage_' + imageIdIncrement);
                    imageIdIncrement += 1;
                }

                inplaceImage.carousel = carousel;
                inplaceImage.productViewItem = productViewItem;
                inplaceImage.product = documentProduct != null ? ng.element(documentProduct).controller() : null; //get controller product on details page;
                inplaceImage.inplaceParams = $parse(attrs.inplaceParams)(scope);
                inplaceImage.inplaceUrl = attrs.inplaceUrl;
                inplaceImage.inplaceImageButtonsVisible = ng.extend({ 'add': true, 'update': true, 'delete': true, 'permanentVisible': false }, (new Function('return ' + (attrs.inplaceImageButtonsVisible)))() || {});

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
                    var buttons = ng.element('<div inplace-image-buttons="' + attrs.id + '"></div>')

                    element[0].parentNode.appendChild(buttons[0]);

                    $compile(buttons)(scope);

                    inplaceImage.buttonsRendered = true;

                    setPosition(element, buttons).finally(function () {
                        inplaceImage.buttons.rendered = true;
                    });
                };

                mouseenter = function () {

                    element[0].classList.add('inplace-image-focus');

                    scope.$apply(function () {

                        inplaceImage.active();

                        if (inplaceImage.buttonsRendered == null) {
                            renderButtons(element);
                        } else if (inplaceImage.buttons != null && inplaceImage.buttons.element != null) {
                            setPosition(element, inplaceImage.buttons.element);
                        }
                    });
                };

                mouseleave = function () {

                    element[0].classList.remove('inplace-image-focus');

                    inplaceImage.isActive = false;

                    setTimeout(function () {
                        scope.$apply(function () {
                            if (inplaceImage.buttons != null && (inplaceImage.buttons.isHoverButtons == false || inplaceImage.buttons.isHoverButtons == null) && inplaceImage.inplaceImageButtonsVisible.permanentVisible !== true) {
                                inplaceImage.showButtons = false;
                            }
                        });
                    }, 100);

                    if (timer != null) {
                        $timeout.cancel(timer);
                    }
                };

                if (inplaceImage.inplaceImageButtonsVisible.permanentVisible === true) {
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

                element.on('$destroy', function () {
                    inplaceImage.buttonsRendered = null;
                });

                inplaceService.addInplaceImage(attrs.id, inplaceImage);
            }
        };
    }]);

    ng.module('inplace')
      .directive('inplaceImageButtons', function () {
          return {
              restrict: 'A',
              scope: {
                  inplaceImageButtons: '@'
              },
              controller: 'InplaceImageButtonsCtrl',
              controllerAs: 'inplaceImageButtons',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/inplace/templates/inplaceImageButtons.html',
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
                                  if (ctrl.inplaceImage.isActive == false || ctrl.inplaceImage.isActive == null) {
                                      ctrl.inplaceImage.showButtons = false;
                                  }
                              });
                          }, 100);
                      });
                  } else {
                      ctrl.inplaceImage.showButtons = true;
                  }
              }
          };
      });
    //#endregion

})(window.angular);