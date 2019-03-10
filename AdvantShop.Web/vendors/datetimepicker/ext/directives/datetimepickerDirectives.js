; (function (ng) {
    'use strict';

    ng.module('ui.bootstrap.datetimepicker')
      .directive('datetimepickerPopup', ['datetimepickerExtService', function (datetimepickerExtService) {
          return {
              restrict: 'A',
              transclude: true,
              replace: true,
              controller: 'DatetimepickerPopupCtrl',
              controllerAs: 'datetimepickerPopup',
              bindToController: true,
              scope: {
                  datetimepickerPopupTrigger: '@'
              },
              template: '<div class="datetimepicker-popup js-datetimepicker-popup" data-ng-show="datetimepickerPopup.isShow" data-ng-style="{top : datetimepickerPopup.pos.top , left:  datetimepickerPopup.pos.left}"><div data-window-click="datetimepickerPopup.clickOut(event)" data-ng-transclude></div></div>',
              link: function (scope, element, attrs, ctrl) {

                  datetimepickerExtService.getTrigger(ctrl.datetimepickerPopupTrigger).then(function (datetimepickerTrigger) {

                      var triggerSizes = datetimepickerTrigger.getSizes();

                      ctrl.pos = {
                          top: triggerSizes.top + triggerSizes.height,
                          left: triggerSizes.left
                      };

                      datetimepickerTrigger.addPopup(ctrl);
                  });
              }
          };
      }])
      .directive('datetimepickerTrigger', ['$filter', 'datetimepickerExtService', function ($filter, datetimepickerExtService) {
          return {
              require: ['datetimepickerTrigger', '?ngModel'],
              restrict: 'A',
              scope: true,
              controller: 'DatetimepickerTriggerCtrl',
              controllerAs: 'datetimepickerTrigger',
              bindToController: true,
              link: function (scope, element, attrs, ctrls) {

                  var datetimepickerTriggerCtrl = ctrls[0],
                      ngModelCtrl = ctrls[1];

                  element.on('click', function (event) {
                      event.stopPropagation();
                      scope.$apply(datetimepickerTriggerCtrl.active);
                  });

                  if (ngModelCtrl != null) {
                      ngModelCtrl.$formatters.push(function (value) {
                          if (value != null) {
                              return $filter('date')(value, attrs.formatDate);
                          }
                      });
                  }

                  datetimepickerExtService.addTrigger(attrs.id, datetimepickerTriggerCtrl);
              }
          };
      }]);

})(window.angular);