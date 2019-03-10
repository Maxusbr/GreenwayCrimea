; (function (ng) {
    'use strict';

    ng.module('offersSelectvizr')
      .component('offersSelectvizr', {
          templateUrl: '../areas/admin/content/src/_partials/offers-selectvizr/templates/offers-selectvizr.html',
          controller: 'OffersSelectvizrCtrl',
          transclude: true,
          bindings: {
              selectvizrTreeUrl: '<',
              selectvizrGridUrl: '<',
              selectvizrGridOptions: '<',
              selectvizrGridParams: '<?',
              selectvizrOnChange: '&',
              selectvizrGridOnFetch: '&'
          }
      });

})(window.angular);