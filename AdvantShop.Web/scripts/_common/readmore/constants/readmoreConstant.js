; (function (ng) {
    'use strict';

    ng.module('readmore')
      .constant('readmoreConfig', {
          expanded: false,
          maxHeight: 190,
          speed: '0s',
          moreText: 'JS.Readmore.MoreText',
          lessText: 'JS.Readmore.LessText'
      });

})(window.angular);