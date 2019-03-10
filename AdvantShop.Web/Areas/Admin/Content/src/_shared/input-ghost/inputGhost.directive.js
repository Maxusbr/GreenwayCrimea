; (function (ng) {
    'use strict';

    ng.module('inputGhost')
      .directive('inputGhost', function () {
          return {
              require: ['inputGhost', 'ngModel'],
              controller: 'InputGhostCtrl',
              controllerAs: 'inputGhost',
              bindToController: true,
              compile: function (cElement, cAttrs) {
                  cAttrs.$set('size', '{{inputGhost.ngModel.$viewValue.length === 1 ? 1 : inputGhost.ngModel.$viewValue.length - 1}}');
                  cElement[0].classList.add('input-ghost');
              },
              link: function (scope, element, attrs, ctrls) {
                  var inputGhostCtrl = ctrls[0],
                      ngModelCtrl = ctrls[1];

                  inputGhostCtrl.ngModel = ngModelCtrl;
              }
          }
      });

})(window.angular);