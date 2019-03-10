//#region module
; (function (ng) {
    'use strict';

    ng.module('productSets', []);

})(window.angular);
//#endregion

//#region controllers
; (function (ng) {

    'use strict';

    var ProductSetsCtrl = function ($http, $q, cartService, productService, moduleService) {
        var ctrl = this;

        ctrl.items = [];

        ctrl.hasChecked = true;

        ctrl.init = function (productCtrl) {
            ctrl.productCtrl = productCtrl;
        }

        ctrl.initValues = function (ind, item) {
            ctrl.items[ind] = item;
        };

        ctrl.calcPrice = function () {

            var offerIds = [];
            ctrl.hasChecked = false;
            for (var i = 0; i < ctrl.items.length; i++) {
                if (ctrl.items[i].selected === true) {
                    ctrl.hasChecked = true;
                    offerIds.push(i === 0 ? ctrl.productCtrl.offerSelected.OfferId : ctrl.items[i].offerId);
                }
            }
            if (offerIds.length) {
                $http.post('productset/gettotalprice', { productId: ctrl.productCtrl.productId, offerIds: offerIds }).success(function (data) {
                    ctrl.totalPricePrepared = data.result;
                });
            }
        };

        ctrl.addToCart = function () {
            var itemsForSend = [];
            for (var i = 0; i < ctrl.items.length; i++) {
                if (ctrl.items[i].selected === true) {
                    itemsForSend.push({
                        offerId: (i === 0 ? ctrl.productCtrl.offerSelected.OfferId : ctrl.items[i].offerId),
                        productId: 0,
                        amount: 0,
                        attributesXml: i === 0 && ctrl.productCtrl.customOptions != null ? ctrl.productCtrl.customOptions.xml : null,
                        payment: null
                    });
                }
            }

            cartService.addItems(itemsForSend);

            if (itemsForSend.length > 0) {
                $(document).trigger("add_to_cart", "");

                for (var i = 0; i < itemsForSend.length; i++) {
                    $(document).trigger("cart.add", [itemsForSend[i].offerId, itemsForSend[i].productId, itemsForSend[i].amount, itemsForSend[i].attributesXml]);
                };
            }
        };

        productService.addCallback('refreshPrice', ctrl.calcPrice);
    };

    ng.module('productSets')
      .controller('ProductSetsCtrl', ProductSetsCtrl);

    ProductSetsCtrl.$inject = ['$http', '$q', 'cartService', 'productService', 'moduleService'];

})(window.angular);
//#endregion