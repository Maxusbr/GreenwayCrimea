; (function (ng) {
    'use strict';

    var InplacePriceCtrl = function (inplaceService) {

        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.needReinit = {};

            ctrl.inplaceParams = ctrl.inplaceParams();

            ctrl.active = function () {
                ctrl.isShow = true;
            };
        };


        ctrl.save = function () {

            if (ctrl.product == null || ctrl.product.offerSelected == null) {
                return;
            }

            var content = ctrl.convertToFloat(ctrl.editor.getData()),
                params;

            if (content == null) {
                return;
            }

            params = ng.extend(ctrl.inplaceParams, {
                content: content,
                id: ctrl.product.offerSelected.OfferId,
                field: ctrl.type
            })

            inplaceService.save(ctrl.inplaceUrl, params).finally(function () {
                ctrl.isShow = false;
                ctrl.product.refreshPrice().then(function () {
                    ctrl.setNeedReinit();
                });
            });
        };

        ctrl.setNeedReinit = function () {
            for (var key in ctrl.needReinit) {
                if (ctrl.needReinit.hasOwnProperty(key)) {
                    ctrl.needReinit[key] = true;
                }
            }
        };

        ctrl.cancel = function () {
            ctrl.isShow = false;
            ctrl.editor.bodyElement.innerHTML = ctrl.startContent;
        };

        ctrl.convertToFloat = function (priceString) {
            var price = priceString.replace(/,/g, '.').replace(/ /g, '').replace(/&nbsp;/g, '');

            if (priceString.length === 0) {
                price = 0;
            } else if (/^[0-9]+(\.[0-9][0-9])?$/.test(price) === false) {
                price = null;
            } else {
                price = parseFloat(price);
            }

            return price;
        };
    };

    ng.module('inplace')
      .controller('InplacePriceCtrl', InplacePriceCtrl);

    InplacePriceCtrl.$inject = ['inplaceService'];

})(window.angular);