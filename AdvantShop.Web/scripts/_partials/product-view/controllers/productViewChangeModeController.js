; (function (ng) {
    'use strict';

    var ProductViewChangeModeCtrl = function (productViewService) {
        var ctrl = this;

        ctrl.setView = function (name, view) {
            ctrl.current = view;
            productViewService.setView(name, view);
        };

    };

    ng.module('productView')
      .controller('ProductViewChangeModeCtrl', ProductViewChangeModeCtrl);

    ProductViewChangeModeCtrl.$inject = ['productViewService'];

})(window.angular);