; (function (ng) {
    'use strict';

    ng.module('sizesViewer', [])
      .constant('sizesViewerConfig', {
          isEnableSlider: 'true',
          visibleItems: 7,
          width: '35px',
          height: '35px'
      });

})(window.angular);