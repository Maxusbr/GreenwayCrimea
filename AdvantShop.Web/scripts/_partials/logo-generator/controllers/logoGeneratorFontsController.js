; (function (ng) {
    'use strict';

    var LogoGeneratorFontsCtrl = function () {
        var ctrl = this;

        ctrl.selectFont = function (font) {
            if (ctrl.onSelect != null) {
                ctrl.onSelect({ font: font });
            }
        };

        ctrl.getStyle = function (style, font, objType, currentType) {

            var _style = ng.copy(style);

            if (objType === currentType) {
                //шрифты, у которых в наименовании есть числа не вставляются в стили, надо обязательно обернуть в кавычки
                _style.fontFamily = /\d/.test(font.fontFamily) ? '"' + font.fontFamily + '"' : font.fontFamily;
            }

            return _style;
        };
    };

    LogoGeneratorFontsCtrl.$inject = [];

    ng.module('logoGenerator')
      .controller('LogoGeneratorFontsCtrl', LogoGeneratorFontsCtrl);

})(window.angular);