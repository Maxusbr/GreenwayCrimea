; (function (ng) {
    'use strict';

    ng.module('zone')
      .directive('zoneDialogTrigger', ['zoneService', function (zoneService) {
          return {
              restrict: 'A',
              scope: {},
              link: function (scope, element, attrs, ctrl) {
                  element.on('click', function (e) {
                      e.stopPropagation();
                      scope.$apply(zoneService.zoneDialogOpen)
                  });
              }
          }
      }]);

    ng.module('zone')
      .directive('zoneDialog', function () {
          return {
              restrict: 'A',
              scope: {},
              replace: true,
              templateUrl: '/scripts/_partials/zone/templates/dialog.html',
              controller: 'ZoneCtrl',
              controllerAs: 'zone',
              bindToController: true
          }
      });

    ng.module('zone')
      .directive('zoneCurrent', ['zoneService', function (zoneService) {
          return {
              restrict: 'A',
              scope: true,
              link: function (scope, element, attrs, ctrl) {

                  var startVal = new Function('return ' + attrs.startVal)();

                  scope.zone = {};

                  //if (attrs.startCity != null) {
                  //    scope.zone.City = attrs.startCity;
                  //}

                  if (startVal != null) {
                      ng.extend(scope.zone, zoneService.trustZone(startVal));
                  }

                  zoneService.addUpdateList(scope);

                  zoneService.getCurrentZone().then(function (data) {
                      scope.zone = zoneService.trustZone(data);
                  });
              }
          }
      }]);


    ng.module('zone')
  .directive('zonePopover', function () {
      return {
          restrict: 'A',
          scope: true,
          controller: 'ZonePopoverCtrl',
          controllerAs: 'zonePopover'
      }
  });

})(window.angular);