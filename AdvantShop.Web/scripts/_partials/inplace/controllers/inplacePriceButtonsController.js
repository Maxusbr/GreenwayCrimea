; (function (ng) {
    'use strict';

    var InplacePriceButtonsCtrl = function ($element, inplaceService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.inplacePrice = inplaceService.getInplacePrice(ctrl.inplacePriceButtons);

            ctrl.inplacePrice.buttons = {
                element: $element,
                ctrl: ctrl
            };
        };

        ctrl.btnSave = function () {
            ctrl.inplacePrice.clickedButtons = true;
            ctrl.inplacePrice.save();
        };

        ctrl.btnCancel= function () {
            ctrl.inplacePrice.clickedButtons = true;
            ctrl.inplacePrice.cancel();
        };
    };

    ng.module('inplace')
      .controller('InplacePriceButtonsCtrl', InplacePriceButtonsCtrl);

    InplacePriceButtonsCtrl.$inject = ['$element', 'inplaceService'];
})(window.angular);