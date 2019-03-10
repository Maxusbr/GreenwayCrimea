; (function (ng) {
    'use strict';

    ng.module('buyOneClick')
      .directive('buyOneClickTrigger', ['buyOneClickService', function (buyOneClickService) {
          return {
              restrict: 'A',
              scope: true,
              controller: 'BuyOneClickTriggerCtrl',
              controllerAs: 'buyOneClickTrigger',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  element.on('click', function (event) {
                      event.preventDefault();

                      var modalId = element[0].getAttribute('data-buy-one-click-modal');

                      ctrl.modalId = modalId != null ? modalId : 'modalBuyOneClick';

                      scope.$apply(function () {
                          buyOneClickService.showDialog(ctrl.modalId);
                      });
                  });
              }
          };
      }]);

    ng.module('buyOneClick')
  .directive('buyOneClickForm', function () {
      return {
          restrict: 'A',
          scope: {
              buttonText: '@',
              page: '@',
              orderType: '@',
              offerId: '=?',
              productId: '=?',
              amount: '=?',
              attributesXml: '=?',
              formInit: '&',
              successFn: '&',
              fieldsOptions: '=?',
              autoReset: '=?',
              buyOneClickValid: '&'
          },
          controller: 'BuyOneClickFormCtrl',
          controllerAs: 'buyOneClickForm',
          bindToController: true,
          templateUrl: '/scripts/_partials/buy-one-click/templates/form.html',
          replace: true
      };
  });

})(window.angular);