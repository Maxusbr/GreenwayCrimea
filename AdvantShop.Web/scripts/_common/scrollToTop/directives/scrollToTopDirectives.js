; (function (ng) {
    'use strict';
    //show only desk or more 
    ng.module('scrollToTop')
        .directive('scrollToTop', ['$window', function ($window) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs, ctrl) {

                    $window.addEventListener('scroll', function () {
                        if ($window.pageYOffset >= $window.innerHeight) {
                            element[0].classList.add('scroll-to-top-active');
                        } else {
                            element[0].classList.remove('scroll-to-top-active');
                        }
                    });

                    element[0].addEventListener('click', function () {
                        $window.scrollTo(0, 0);
                    });
                }
            };
        }]);
})(window.angular);