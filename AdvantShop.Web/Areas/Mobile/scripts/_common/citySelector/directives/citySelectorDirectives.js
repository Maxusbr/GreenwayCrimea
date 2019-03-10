; (function (ng) {
    'use strict';
    ng.module('citySelector')
      .directive('citySelectorPanel', function () {
          return {
              restrict: 'A',
              replace : true,
              templateUrl: '/Areas/Mobile/scripts/_common/citySelector/templates/citySelectorPanel.html',
              controller: 'CitySelectorCtrl',
              controllerAs: 'csCtrl',
              bindToController: true
          };
      });
})(window.angular);