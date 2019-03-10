; (function (ng) {
    'use strict';

    ng.module('cmStat')
      .directive('cmStat', function () {
          return {
              scope: true,
              controller: 'CmStatCTrl',
              controllerAs: 'cmStat',
              bindToController: true
          }
      });

})(window.angular);