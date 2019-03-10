//#region module
; (function (ng) {

    'use strict';

    ng.module('checkoutInShoppingCart', []);

})(window.angular);
//#endregion


//#region controllers
; (function (ng) {

    'use strict';


    var CheckoutInShoppingCartCtrl = function ($http, cartService, $translate, toaster) {
        var ctrl = this;

        ctrl.newCustomer = {};

        cartService.getData().then(function (cart) {
            ctrl.cart = cart;
        });
		
		ctrl.validate = function() {

            var result = true;

            if (ctrl.agreement === false) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                result = false;
            }

            return result;
        }

        ctrl.saveNewCustomer = function (field) {
            if (field === 'email') {
                $(document).trigger('customer.email', ctrl.newCustomer);
            }

            $http.post('/checkout/CheckoutUserPost', { customer: ctrl.newCustomer, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

    };

    ng.module('checkoutInShoppingCart')
      .controller('CheckoutInShoppingCartCtrl', CheckoutInShoppingCartCtrl);

    CheckoutInShoppingCartCtrl.$inject = ['$http', 'cartService', '$translate', 'toaster'];

})(window.angular);
//#endregion