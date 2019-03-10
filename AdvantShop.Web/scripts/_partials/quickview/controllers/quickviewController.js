; (function (ng) {
    'use strict';

    var QuickviewCtrl = function (quickviewService, cartService, cartConfig) {
        var ctrl = this,
            triggers = [];

        ctrl.addTrigger = function (productId) {
            triggers.push(productId);
        };

        ctrl.showModal = function (productId, colorId) {
            quickviewService.dialogOpen(ctrl, productId, colorId);
        };
        
        ctrl.hideModal = function () {
            quickviewService.dialogClose();
        };

        cartService.addCallback(cartConfig.callbackNames.add, ctrl.hideModal);
    };

    ng.module('quickview')
      .controller('QuickviewCtrl', QuickviewCtrl);

    QuickviewCtrl.$inject = ['quickviewService', 'cartService', 'cartConfig'];

})(window.angular);