; (function (ng) {
    'use strict';

    var BackgroundPickerCtrl = function (backgroundPickerService, gradientPickerService) {
        var ctrl = this;

        ctrl.changeColor = function (event, color) {

            ctrl.colorCodeSelected = color.ColorCode;

            ctrl.processColors(ctrl.colorCodeSelected, ctrl.isShowGradientPanel);
        };

        ctrl.changeColorPicker = function (event, color) {
            ctrl.colorCodeSelected = color;

            ctrl.processColors(ctrl.colorCodeSelected, ctrl.isShowGradientPanel);
        };

        ctrl.changeUseGradient = function (state) {
            ctrl.processColors(ctrl.colorCodeSelected, state);
        };

        ctrl.changeGradient = function (cssString) {
            ctrl.onUpdate({ cssString: cssString });
        };

        ctrl.processColors = function (colorCode, state) {

            var colorGeneral = colorCode,
                colorAlt,
                cssString;

            colorAlt = state === true ? backgroundPickerService.colorLuminance(colorGeneral, 0.4) : colorGeneral;

            cssString = gradientPickerService.getString('right', colorGeneral, colorAlt, colorGeneral);

            ctrl.startColor = colorGeneral;
            ctrl.middleColor = colorAlt;
            ctrl.endColor = colorGeneral;

            ctrl.onUpdate({ cssString: cssString });
        };
    };

    ng.module('backgroundPicker')
      .controller('BackgroundPickerCtrl', BackgroundPickerCtrl);

    BackgroundPickerCtrl.$inject = ['backgroundPickerService', 'gradientPickerService'];

})(window.angular);