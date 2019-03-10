; (function (ng) {
    'use strict';

    var gradientPickerService = function () {
        var service = this,
            tplString = 'linear-gradient(to {direction}, {startColor} 0%, {middleColor} 50%, {endColor} 100%)';

        service.getString = function (direction, startColor, middleColor, endColor) {
            return tplString.replace('{direction}', direction)
                            .replace('{startColor}', startColor)
                            .replace('{middleColor}', middleColor)
                            .replace('{endColor}', endColor);
        };
    };

    ng.module('gradientPicker')
        .service('gradientPickerService', gradientPickerService);

    gradientPickerService.$inject = []

})(window.angular);