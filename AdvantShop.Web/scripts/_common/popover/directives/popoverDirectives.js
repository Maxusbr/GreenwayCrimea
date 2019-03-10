; (function (ng, body) {

    'use strict';

    ng.module('popover')
      .directive('popoverControl', ['popoverConfig', function (popoverConfig) {
          return {
              restrict: 'A',
              scope: {
                  popoverId: '@',
                  popoverTrigger: '@',
                  popoverTriggerHide: '@'
              },
              transclude: true,
              controller: 'PopoverControlCtrl',
              controllerAs: 'popoverControl',
              bindToController: true,
              template: function (element, attrs) {
                  var trigger, triggerHide, ngTrigger, ngTriggerHide;

                  trigger = attrs.popoverTrigger || popoverConfig.popoverTrigger;
                  triggerHide = attrs.popoverTriggerHide || popoverConfig.popoverTriggerHide;
                  ngTrigger = trigger !== triggerHide ? 'data-ng-' + trigger + '="popoverControl.active()"' : 'data-ng-' + trigger + '="popoverControl.toggle()"';
                  ngTriggerHide = trigger !== triggerHide ? 'data-ng-' + triggerHide + '="popoverControl.deactive()"' : '';

                  return ['<span data-ng-transclude', ' ', ngTrigger, ' ', ngTriggerHide, '></span>'].join('');
              }
          }
      }]);

    ng.module('popover')
      .directive('popover', ['popoverService', function (popoverService) {
          return {
              restrict: 'A',
              scope: {
                  id: '@',
                  popoverShowOnLoad: '&',
                  popoverOverlayEnabled: '&',
                  popoverPosition: '@',
                  popoverIsFixed: '&',
                  popoverIsCanHover: '&'
              },
              transclude: true,
              replace: true,
              templateUrl: '/scripts/_common/popover/templates/popover.html',
              controller: 'PopoverCtrl',
              controllerAs: 'popover',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  if (ctrl.popoverShowOnLoad === true) {
                      popoverService.getControl(ctrl.id).then(ctrl.active);
                  }
              }
          }
      }]);

    ng.module('popover')
    .directive('popoverOverlay', function () {
        return {
            restrict: 'A',
            scope: {},
            replace: true,
            template: '<div class="adv-popover-overlay" data-ng-show="popoverOverlay.isVisibleOverlay" data-ng-click="popoverOverlay.overlayHide()"></div>',
            controller: 'PopoverOverlayCtrl',
            controllerAs: 'popoverOverlay',
            bindToController: true
        }
    });

})(window.angular, document.body);