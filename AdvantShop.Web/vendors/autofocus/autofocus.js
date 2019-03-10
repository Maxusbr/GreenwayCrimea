(function() {
    'use strict';
    angular
        .module('autofocus', [])
        .directive('autofocus', ['$parse','$timeout', Autofocus]);

    function Autofocus($parse, $timeout) {
        function link($scope, $element, $attrs) {
            var dom = $element[0],
                delay = $attrs.autofocusDelay;

            if ($attrs.autofocus) {
                focusIf($parse($attrs.autofocus)($scope));
                $scope.$watch($attrs.autofocus, focusIf);
            } else {
                focusIf(true);
            }

            function focusIf(condition) {
                delay = delay || 0;
                if (condition) {
                    $timeout(function() {
                        dom.focus();
                    }, delay);
                }
            }
        }

        return {
            restrict: 'A',
            link: link
        };
    }
})();
