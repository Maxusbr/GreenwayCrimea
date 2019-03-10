; (function (ng) {
    'use strict';

    var checkoutService = function ($http, $q, toaster) {
        var service = this, contact;
        
        service.getContactFromCache = function() {
            return contact;
        }

        service.saveContact = function (address) {
            return $http.post('/checkout/CheckoutContactPost', { address: address, rnd: Math.random() }).then(function (response) {
                contact = address;
                return response.data;
            });
        };

        service.getShipping = function (preorderList) {
            var params = { rnd: Math.random() };
            if (preorderList != null) {
                params.preorderList = preorderList;
            }

            return $http.post('/checkout/CheckoutShippingJson', params).then(function (response) {
                return response.data;
            });
        };

        service.saveShipping = function (shipping, preorderList) {
            var params = { shipping: shipping, rnd: Math.random() };
            if (preorderList != null) {
                params.preorderList = preorderList;
            }

            return $http.post('/checkout/CheckoutShippingPost', params).then(function (response) {
                return response.data;
            });
        };

        service.getPayment = function (preorderList) {
            var params = { rnd: Math.random() };
            if (preorderList != null) {
                params.preorderList = preorderList;
            }

            return $http.post('/checkout/CheckoutPaymentJson', params).then(function (response) {
                return response.data;
            });
        };

        service.savePayment = function (payment, preorderList) {
            var params = { payment: payment, rnd: Math.random() };
            if (preorderList != null) {
                params.preorderList = preorderList;
            }

            return $http.post('/checkout/CheckoutPaymentPost', params).then(function (response) {
                return response.data;
            });
        };

        service.getCheckoutCart = function () {
            return $http.get('/checkout/CheckoutCartJson', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.autorizeBonus = function (cardNumber) {
            return $http.post('/checkout/CheckoutBonusAutorizePost', { cardNumber: cardNumber, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.toggleBonus = function (isApply) {
            return $http.post('/checkout/CheckoutBonusApplyPost', { isApply: isApply, rnd: Math.random() }).then(function (response) {
                if (!response.data && response.data.msg.length > 0)
                {
                    toaster.pop('error', '', response.data.msg);
                }
                return response.data;
            });
        };

        service.couponApplied = function () {
            return $http.post('/checkout/CheckoutCouponApplied', { irnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.commentSave = function (message) {
            return $http.post('/checkout/CommentPost', { message: message, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        }

        service.saveNewCustomer = function (customer) {
            return $http.post('/checkout/CheckoutUserPost', { customer: customer, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        }

        service.saveWantBonusCard = function (wantBonusCard) {
            return $http.post('/checkout/saveWantBonusCard', { wantBonusCard: wantBonusCard, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        }

        // billing
        service.getBillingPayment = function (orderId) {
            return $http.post('/checkout/BillingPaymentJson', { rnd: Math.random(), orderId: orderId }).then(function (response) {
                return response.data;
            });
        };

        service.getBillingCart = function (orderId) {
            return $http.get('/checkout/BillingCartJson', { params: { orderId: orderId, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.saveBillingPayment = function (payment, orderId) {
            return $http.post('/checkout/BillingPaymentPost', { payment: payment, orderId: orderId, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('checkout')
      .service('checkoutService', checkoutService);

    checkoutService.$inject = ['$http', '$q', 'toaster'];

})(window.angular);