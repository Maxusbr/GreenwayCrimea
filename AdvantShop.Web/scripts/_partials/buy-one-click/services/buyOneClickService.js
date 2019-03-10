; (function (ng) {
    'use strict';

    var buyOneClickService = function ($http, $sce, modalService) {
        var service = this,
            modalId = 'modalBuyOneClick',
            isRenderDialog = false;

        service.showDialog = function (modalId) {
            modalService.open(modalId);
            $(document).trigger("buy_one_click_pre");
        };

        service.getFieldsOptions = function () {
            return $http.get('checkout/checkoutbuyinoneclickfields').then(function (response) {
                return response.data;
            });
        };

        service.getCustomerInfo = function () {
              return $http.get('checkout/checkoutbuyinoneclickcustomer').then(function (response) {
                return response.data;
            });
        };

        service.setFields = function () {
            modalService.setVisibleFooter(modalId, show);
        };

        service.checkout = function (page, orderType, offerId, productId, amount, attributesXml, name, email, phone, comment) {
            var params = {
                page: page,
                orderType: orderType,
                offerId: offerId,
                productId: productId,
                amount: amount,
                attributesXml: attributesXml,
                name: name,
                email: email,
                phone: phone,
                comment: comment
            };

            return $http.post('checkout/checkoutbuyinoneclick', params).then(function (response) {

                if (response.data.error === null || response.data.length === 0) {
                    $(document).trigger("buy_one_click_confirm");
                }

                return response.data;
            });
        };
    };

    ng.module('buyOneClick')
      .service('buyOneClickService', buyOneClickService);

    buyOneClickService.$inject = ['$http', '$sce', 'modalService'];

})(window.angular);