; (function (ng) {
    'use strict';

    var SubblockInplaceButtonModalCtrl = function (subblockInplaceTextColors, subblockInplaceBackgroundColors, subblockInplaceService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.borderRadiusParsed = parseFloat(ctrl.settings.style['border-radius']);
            ctrl.colorCodeSelected = ctrl.settings.style['background'];
        };

        ctrl.textColorsArray = subblockInplaceTextColors;
        ctrl.backgroundColorsArray = subblockInplaceBackgroundColors;

        ctrl.changeText = function () {
            ctrl.onUpdate({ settings: ctrl.settings });
        };

        ctrl.changeTextColor = function ($event, color) {
            ctrl.processStylesChange('color', color);
        };

        ctrl.selectTextColor = function (event, color) {
            ctrl.processStylesChange('color', color.ColorCode);
        };

        ctrl.changeBackgroundColor = function ($event, color) {
            ctrl.colorCodeSelected = color;

            ctrl.useShadow(ctrl.isUseShadow);

            ctrl.processStylesChange('background', color);
        };

        ctrl.selectBackgroundColor = function (event, color) {

            ctrl.colorCodeSelected = color.ColorCode;

            ctrl.useShadow(ctrl.isUseShadow);

            ctrl.processStylesChange('background', color.ColorCode);
        };

        ctrl.useShadow = function (isUse) {
            if (isUse === false) {
                ctrl.processStylesChange('box-shadow', null);
            } else {
                ctrl.processStylesChange('box-shadow', '0 3px 7px 0 ' + subblockInplaceService.convertToRGBA(ctrl.colorCodeSelected, 50));
            }
        };

        ctrl.processStylesChange = function (styleName, styleValue) {

            ctrl.settings.style[styleName] = styleValue;

            ctrl.onUpdate({ settings: ctrl.settings });
        };
    };

    ng.module('subblockInplace')
      .controller('SubblockInplaceButtonModalCtrl', SubblockInplaceButtonModalCtrl);

    SubblockInplaceButtonModalCtrl.$inject = ['subblockInplaceTextColors', 'subblockInplaceBackgroundColors', 'subblockInplaceService'];

})(window.angular);