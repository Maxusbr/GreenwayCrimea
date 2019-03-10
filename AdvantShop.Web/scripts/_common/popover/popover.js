; (function (ng) {

    'use strict';

    ng.module('popover', [])
      .constant('popoverConfig', {
          popoverTrigger: 'mouseenter',
          popoverTriggerHide: 'mouseleave',
          popoverShowOnLoad: false,
          popoverOverlayEnabled: false,
          popoverPosition: 'top',
          popoverIsFixed: false,
          popoverIsCanHover: false
      });

})(window.angular);