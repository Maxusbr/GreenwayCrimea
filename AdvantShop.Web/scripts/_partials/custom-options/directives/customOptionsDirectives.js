; (function (ng) {
    'use strict';

    ng.module('customOptions')
      .directive('customOptions', function () {
          return {
              scope: {
                  productId: '@',
                  initFn: '&',
                  changeFn: '&'
              },
              replace: true,
              templateUrl: '/scripts/_partials/custom-options/templates/customOptions.html',
              controller: 'CustomOptionsCtrl',
              controllerAs: 'customOptions',
              bindToController: true
          }
      })

})(window.angular);