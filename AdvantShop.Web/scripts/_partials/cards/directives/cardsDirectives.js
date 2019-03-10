; (function (ng) {
    'use strict';

    ng.module('cards')
      .directive('cardsForm', function () {
          return {
              restrict: 'A',
              scope: {
                  applyFn: '&'
              },
              controller: 'CardsFormCtrl',
              controllerAs: 'cardsForm',
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/cards/templates/cardsForm.html'
          };
      });

    ng.module('cards')
  .directive('cardsRemove', function () {
      return {
          restrict: 'A',
          scope: {
              applyFn: '&',
              type: '@'
          },
          controller: 'CardsRemoveCtrl',
          controllerAs: 'cardsRemove',
          bindToController: true,
          replace: true,
          template: '<a class="icon-cancel-before link-text-decoration-none" data-ng-click="cardsRemove.remove(cardsRemove.type)"></a>'
      };
  });


})(window.angular);