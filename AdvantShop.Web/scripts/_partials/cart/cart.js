; (function (ng) {
    'use strict';

    ng.module('cart', [])
      .constant('cartConfig', {
          callbackNames: {
              get: 'get',
              update:'update',
              remove:'remove',
              add: 'add',
              clear: 'clear',
              open: 'open',
          },
          cartMini: {
              delayHide: 3000
          }
      });

})(window.angular);