; (function (ng) {
    'use strict';

    ng.module('carousel', [])
      .constant('carouselDefault', {
          isVertical: false,
          scrollCount: 1,
          nav: true,
          dots: false,
          speed: 600,
          auto: false,
          autoPause: 5000,
          indexActive: 0,
          prevIcon: 'icon-left-open-after',
          nextIcon: 'icon-right-open-after',
          prevIconVertical: 'icon-up-open-after',
          nextIconVertical: 'icon-down-open-after',
          prevClass: 'cs-l-1-interactive',
          nextClass: 'cs-l-1-interactive',
          dotsClass: undefined,
          dotsItemClass: 'cs-bg-i-1',
          visibleMax: null,
          visibleMin: null,
          itemSelectClass: null,
          carouselClass: null,
          stretch: true,
          navPosition: 'inside' //inside or outside
      });

})(window.angular);