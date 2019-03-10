; (function (ng) {
    'use strict';
    ng.module('uiHelper', [])
        .directive('hndlrEnter', function () {
            return function (scope, element, attrs) {
                element.bind("keydown keypress", function (event) {
                    if (event.which === 13) {
                        scope.$apply(function () {
                            scope.$eval(attrs.hndlrEnter);
                        });
                        event.preventDefault();
                    }
                });
            };
        });
})(window.angular);