; (function (ng) {
    'use strict';

    var ProductViewModeCtrl = function (productViewService) {
        var ctrl = this;

        var view = productViewService.setView(ctrl.name, ctrl.current);

    };

    ng.module('productView')
      .controller('ProductViewModeCtrl', ProductViewModeCtrl);

    ProductViewModeCtrl.$inject = ['productViewService'];

})(window.angular);