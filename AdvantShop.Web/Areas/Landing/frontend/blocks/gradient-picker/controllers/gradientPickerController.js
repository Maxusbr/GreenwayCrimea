; (function (ng) {
    'use strict';

    var GradientPickerCtrl = function (gradientPickerService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.direction = 'right';
            ctrl.cssString = '';
        };

        ctrl.changeDirection = function () {
            ctrl.cssString = gradientPickerService.getString(ctrl.direction, ctrl.startColor, ctrl.middleColor, ctrl.endColor);

            ctrl.onUpdate({ cssString: ctrl.cssString });
        };

        ctrl.changeColor = function (event, color) {
            ctrl.cssString = gradientPickerService.getString(ctrl.direction, ctrl.startColor, ctrl.middleColor, ctrl.endColor);

            ctrl.onUpdate({ cssString: ctrl.cssString });
        };
    };

    ng.module('gradientPicker')
      .controller('GradientPickerCtrl', GradientPickerCtrl);

    GradientPickerCtrl.$inject = ['gradientPickerService'];

})(window.angular);