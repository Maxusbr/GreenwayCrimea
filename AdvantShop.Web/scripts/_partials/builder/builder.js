; (function (ng) {
    'use strict';

    ng.module('builder', [])
      .constant('builderTypes', {
          colorScheme: 'colorScheme',
          theme: 'theme',
          background: 'background'
      });
       
})(window.angular);