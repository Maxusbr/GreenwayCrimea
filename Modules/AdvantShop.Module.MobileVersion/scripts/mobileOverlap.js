//#region module

; (function (ng) {
    'use strict';

    ng.module('mobileOverlap', []);

})(window.angular);

//#endregion

//#region controller

; (function (ng) {
    'use strict';

    var mobileOverlapCtrl = function ($location, $cookies, $element) {

        var ctrl = this;

        ctrl.stayOnDesktop = function () {
            $cookies.remove('ForcedMobile');
            $cookies.put('ForcedDesktop', 'true');

            $element.remove();
        };

        ctrl.goToMobile = function () {
            $cookies.put('ForcedMobile', 'true');
            $cookies.remove('ForcedDesktop');

            window.location = $location.absUrl();
        }

        if (!$cookies.get('ForcedDesktop') && !$cookies.get('ForcedMobile')) {
            $cookies.put('ForcedDesktop', 'true');
        }
    };

    ng.module('mobileOverlap')
      .controller('mobileOverlapCtrl', mobileOverlapCtrl);

    mobileOverlapCtrl.$inject = ['$location', '$cookies', '$element'];

})(window.angular);

//#endregion