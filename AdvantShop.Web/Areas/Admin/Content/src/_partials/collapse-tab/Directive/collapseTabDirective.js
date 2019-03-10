; (function (ng) {
    'use strict';


    ng.module('collapseTab')
    .directive('collapseTab', ['$window', '$timeout', function ($window, $timeout) {
        return {
            controller: 'CollapseTabCtrl',
            controllerAs: 'collapseTab',
            bindToController: true,
            link: function (scope, element, attr, ctrl) {

                document.querySelector('body').style.overflow = 'scroll';

                var tabs = document.querySelectorAll('.nav-collapse-tab');

                if (tabs.length != 0) {
                    $timeout(function () {
                        return ctrl.init(tabs);
                    }, 100);
                }

            }
        };
    }]);


})(angular);