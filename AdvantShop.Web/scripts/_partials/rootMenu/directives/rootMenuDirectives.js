; (function (ng) {
    'use strict';

    var isTouchDevice = 'ontouchstart' in document.documentElement;

    ng.module('rootMenu')
    .directive('rootMenu', function () {
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs, ctrl) {

                if (isTouchDevice) {

                    element[0].addEventListener('click', function (event) {

                        if (element.hasClass("active") === false) {
                            event.preventDefault();
                        }

                        element.addClass("active");
                    });


                    element[0].addEventListener('mouseleave', function () {
                        element.removeClass("active");
                    });

                }

            }
        };
    });

})(angular);