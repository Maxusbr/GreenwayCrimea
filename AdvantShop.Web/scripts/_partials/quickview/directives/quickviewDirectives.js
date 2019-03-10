; (function (ng) {
    'use strict';

    ng.module('quickview')
  .directive('quickviewTrigger', ['domService', function (domService) {
      return {
          require: ['quickviewTrigger', '^productViewItem'],
          restrict: 'A',
          scope: true,
          controller: 'QuickviewCtrl',
          controllerAs: 'quickview',
          bindToController: true,
          link: function (scope, element, attrs, ctrls) {
              element[0].addEventListener('click', function (event) {

                  event.preventDefault();
                  event.stopPropagation();

                  var quickviewCtrl = ctrls[0], productViewItemCtrl = ctrls[1], items, id;

                  if (quickviewCtrl.siblings == null) {

                      quickviewCtrl.siblings = [];

                      items = domService.closest(this, '.js-products-view-block').parentNode.children;

                      for (var i = 0, len = items.length - 1; i <= len; i++) {
                          id = parseInt(items[i].querySelector('.js-products-view-item').getAttribute('data-product-id'));

                          if (ng.isNumber(id)) {
                              quickviewCtrl.siblings.push(id);
                          }
                      }
                  }

                  quickviewCtrl.showModal(parseInt(attrs.productId), productViewItemCtrl.getSelectedColorId());

                  scope.$apply();
              });
          }
      }
  }]);

})(window.angular);