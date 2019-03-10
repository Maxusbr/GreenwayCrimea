; (function (ng) {
    'use strict';

    var logoGeneratorFontSupportFilter =  function (logoGeneratorService) {
        return function (input, langs) {
            var result = input;

            if (langs != null && langs.length > 0) {
                if (langs.indexOf('cyrillic') === -1 && logoGeneratorService.isCyrillic(input) === true) {
                    result = logoGeneratorService.replaceUnsupportOnSymbol(input, 'cyrillic');
                } else if (langs.indexOf('latin') === -1 && logoGeneratorService.isLatin(input) === true) {
                    result = logoGeneratorService.replaceUnsupportOnSymbol(input, 'latin');
                }
            }

            return result;
        }
    };

    logoGeneratorFontSupportFilter.$inject = ['logoGeneratorService'];

    ng.module('logoGenerator')
      .filter('logoGeneratorFontSupport', logoGeneratorFontSupportFilter);

})(window.angular);