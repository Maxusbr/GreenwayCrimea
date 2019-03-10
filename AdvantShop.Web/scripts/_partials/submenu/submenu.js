; (function (ng) {
    'use strict';

    ng.module('submenu', [])
      .constant('submenuConfig', {
          delay: 100,
          delayHover: 50,
          tolerance: 300,
          submenuDirection: 'right',//right, left, below, above
          checkOrientation: false,
          type: 'default',
          blockOrientation: null
      })
})(angular);