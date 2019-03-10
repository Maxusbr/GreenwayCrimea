; (function (ng) {
    'use strict';

    var InplacePricePanelCtrl = function ($element, inplaceService) {

        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.inplacePrice = inplaceService.getInplacePrice(ctrl.inplacePricePanel);

            ctrl.inplacePrice.panel = {
                element: $element,
                ctrl: ctrl
            };
        };
    };

    ng.module('inplace')
      .controller('InplacePricePanelCtrl', InplacePricePanelCtrl);

    InplacePricePanelCtrl.$inject = ['$element', 'inplaceService'];

})(window.angular);