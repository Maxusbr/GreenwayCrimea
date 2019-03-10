; (function (ng) {
    'use strict';
    ng.module('uiModal').config(['$provide', function ($provide) {
        $provide.decorator('$uibModal', ['$delegate', function ($delegate) {
            var originalWarn = $delegate.open;
            $delegate.open = function () {
                if (navigator.userAgent.toLowerCase().match(/(ipad|iphone)/)) {
                    window.scrollTo(0, 0);
                }
                return originalWarn.apply($delegate, arguments);
            };
            return $delegate;
        }]);
    }]);

})(window.angular);