; (function (ng) {

    'use strict';

    ng.module('zoomer', [])
      .constant('zoomerConfig', {
          zoomWidth: 350,
          zoomHeight: 350,
          type: 'right' // right/inner
      });

})(window.angular);