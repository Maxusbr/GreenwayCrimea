; (function (ng) {
    'use strict';

    ng.module('spinbox', [])
      .constant('spinboxKeyCodeAllow', {
          'backspace': 8,
          'delete': 46,
          'decimalPoint': 110,
          'comma': 188,
          'period': 190,
          'forwardSlash': 191,
          'leftArrow': 37,
          'rightArrow': 39,
          'upArrow': 38,
          'downArrow': 40
      });

})(window.angular);