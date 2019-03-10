; (function (ng) {
    'use strict';

    ng.module('input')
      .directive('input', ['$parse', '$window', function ($parse, $window) {
          return {
              restrict: 'E',
              require: '?ngModel',
              link: function (scope, element, attrs, ctrl) {
                  var valueDirty = attrs.value,
                      el,
                      type,
                      value;

                  if (ctrl != null && ctrl.modelValue === undefined) {
                      el = element[0];
                      type = el.type;

                      switch (type) {
                          case 'radio':
                              if (attrs.checked) {
                                  value = attrs.ngValue ? $parse(attrs.ngValue)(scope) : valueDirty;
                              }
                              break;
                          case 'checkbox':
                              if (attrs.checked) {
                                  value = attrs.ngTrueValue ? $parse(attrs.ngTrueValue)(scope) : true;
                              } else if (attrs.ngFalseValue) {
                                  value = $parse(attrs.ngFalseValue)(scope);
                              }
                              break;
                          case 'number':
                              if (attrs.value != null) {
                                  value = Number(valueDirty);
                              }
                              break;
                          default:
                              if (attrs.value != null && attrs.value.length > 0) {
                                  value = valueDirty;
                              }
                              break;
                      }

                      if (value != null) {
                          $parse(attrs.ngModel).assign(scope, value);
                      }

                      if (type === 'text') {
                          var callbackPaste = function (event) {

                              var content;

                              if ($window.clipboardData && $window.clipboardData.getData) { // IE
                                  content = $window.clipboardData.getData('Text');
                              }
                              else { // others
                                  content = event.clipboardData.getData('text/plain');
                              }

                              if (content != null) {
                                  ctrl.$setViewValue(content);
                              }

                          };

                          el.addEventListener('paste', callbackPaste);

                          scope.$on('$destroy', function () {
                              el.removeEventListener('paste', callbackPaste);
                          });
                      }
                  }
              }
          };
      }]);

    ng.module('input')
      .directive('textarea', ['$parse', '$window', function ($parse, $window) {
          return {
              restrict: 'E',
              require: '?ngModel',
              link: function (scope, element, attrs, ctrl) {
                  var el = element[0],
                      val = el.value;

                  if (ctrl != null && ctrl.modelValue === undefined) {
                      $parse(attrs.ngModel).assign(scope, val);

                      var callbackPaste = function (event) {

                          var content;

                          if ($window.clipboardData && $window.clipboardData.getData) { // IE
                              content = $window.clipboardData.getData('Text');
                          }
                          else { // others
                              content = event.clipboardData.getData('text/plain');
                          }

                          if (content != null) {
                              ctrl.$setViewValue(content);
                          }

                      };

                      el.addEventListener('paste', callbackPaste);

                      scope.$on('$destroy', function () {
                          el.removeEventListener('paste', callbackPaste);
                      });
                  }
              }
          };
      }]);

})(window.angular);




