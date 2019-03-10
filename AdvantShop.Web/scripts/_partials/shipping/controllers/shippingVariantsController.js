; (function (ng) {
    'use strict';

    var ShippingVariantsCtrl = function ($http, zoneService) {
        var ctrl = this;

        ctrl.getData = function (offerId, amount, customOptions) {

            ctrl.isProgress = true;

            return $http.post('product/getshippings', { offerId: offerId, amount: amount, customOptions: customOptions, rnd: Math.random() }).then(function (response) {
                if (response.data != null) {
                    ctrl.items = response.data.Shippings;
                }
                else {
                    ctrl.items = [];
                }
                ctrl.isProgress = false;

                return response.data;
            });
        };
        if (ctrl.type === "display") {
            ctrl.getData(ctrl.startOfferId, ctrl.startAmount, ctrl.startSvCustomOptions);
        }
        if (ctrl.type === "none") {
            ctrl.getData(ctrl.startOfferId, ctrl.startAmount, ctrl.startSvCustomOptions);
        }



        if (ctrl.type === "Always") {
            ctrl.getData(ctrl.startOfferId, ctrl.startAmount, ctrl.startSvCustomOptions);
        }
        
        ctrl.update = function () {
            ctrl.getData(ctrl.offerId, ctrl.amount, ctrl.svCustomOptions);
        };

        ctrl.calcShippings = function () {
            ctrl.type = "Always";
            ctrl.getData(ctrl.startOfferId, ctrl.startAmount, ctrl.startSvCustomOptions);
        }

        zoneService.addCallback('set', function () {
            ctrl.getData(ctrl.offerId, ctrl.amount, ctrl.svCustomOptions);
        });

        ctrl.initFn({ shippingVariants: ctrl });
    };

    ng.module('shipping')
      .controller('ShippingVariantsCtrl', ShippingVariantsCtrl);

    ShippingVariantsCtrl.$inject = ['$http', 'zoneService'];

})(window.angular);