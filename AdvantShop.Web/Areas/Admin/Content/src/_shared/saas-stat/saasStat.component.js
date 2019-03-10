; (function (ng) {
    'use strict';

    ng.module('saasStat')
      .directive('saasStat', function () {
          return {
              scope: true,
              controller: 'SaasStatCTrl',
              controllerAs: 'saasStat',
              bindToController: true
          }
      });

})(window.angular);