; (function (ng) {
    'use strict';

    ng.module('telephony')
      .directive('telephonyForm', function () {
          return {
              restrict: 'A',
              scope: true,
              controller: 'TelephonyFormCtrl',
              controllerAs: 'telephonyForm',
              bindToController: true
          };
      });

})(window.angular);