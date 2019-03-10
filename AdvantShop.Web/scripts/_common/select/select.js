; (function (ng) {
    'use strict';

    ng.module('select', [])
      .directive('select', ['$parse', '$timeout', function ($parse, $timeout) {
          return {
              require: '?ngModel',
              restrict: 'E',
              link: function (scope, element, attrs, ctrl) {
                  if (ctrl != null && !attrs.ngOptions && attrs.disabledAutobind == null && (ctrl.$modelValue === undefined || isNaN(ctrl.$modelValue))) {
                      $parse(attrs.ngModel).assign(scope, element.val());
                  }
              }
          }
      }])
    .directive('convertToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (val) {
                    return parseFloat(val, 10);
                });
                ngModel.$formatters.push(function (val) {
                    return '' + val;
                });
            }
        };
    });
})(window.angular);