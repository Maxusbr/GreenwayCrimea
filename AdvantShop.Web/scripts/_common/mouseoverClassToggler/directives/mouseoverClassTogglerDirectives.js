; (function (ng) {
    'use strict';

    var isTouchDevice = 'ontouchstart' in document.documentElement;

    ng.module('mouseoverClassToggler')
    .directive('mouseoverClassToggler', function () {
        return {
            restrict: 'A',
            scope: {
                classToggle: '@'
            },
            link: function (scope, element, attrs, ctrl) {

                var classToggle = scope.classToggle ? scope.classToggle : "active";

                if (isTouchDevice) {

                    element[0].addEventListener('click', function (event) {

                        //if you need prevent click on href
                        if (element.hasClass(classToggle) === false) {
                            event.preventDefault();
                        }

                        element.addClass(classToggle);
                    });

                } else {

                    element[0].addEventListener('mouseover', function (event) {
                        element.addClass(classToggle);
                    });

                }

                element[0].addEventListener('mouseleave', function () {
                    element.removeClass(classToggle);
                });

            }
        };
    });

})(angular);