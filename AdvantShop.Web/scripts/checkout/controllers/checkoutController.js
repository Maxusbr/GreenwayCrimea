; (function (ng, $) {

    'use strict';

    var CheckOutCtrl = function ($http, $q, $sce, $rootScope, zoneService, checkoutService) {
        var ctrl = this, relationship;

        //ctrl.address = {};
        ctrl.Payment = {};
        ctrl.Shipping = {};
        ctrl.Cart = {};
        ctrl.isShowCouponInput = false;
        ctrl.newCustomer = {};
        ctrl.contact = {};
        ctrl.shippingLoading = true;
        ctrl.paymentLoading = true;

        relationship = {
            'address': function () {
                return ctrl.fetchShipping()
                        .then(ctrl.fetchPayment)
                        .then(ctrl.fetchCart);
            },
            'shipping': function () {
                return ctrl.fetchPayment()
                        .then(ctrl.fetchCart);
            },
            'payment': function () {
                return ctrl.fetchCart();
            },
            'bonus': function () {
                return ctrl.fetchShipping()
                        .then(ctrl.fetchPayment)
                        .then(ctrl.fetchCart);
            },
            'coupon': function () {
                return ctrl.fetchShipping()
                        .then(ctrl.fetchPayment)
                        .then(ctrl.fetchCart);
            }
        };

        ctrl.startShippingProgress = function () {
            ctrl.shippingLoading = true;
        };

        ctrl.stopShippingProgress = function () {
            ctrl.shippingLoading = false;
        };

        ctrl.startPaymentProgress = function () {
            ctrl.paymentLoading = true;
        };

        ctrl.stopPaymentProgress = function () {
            ctrl.paymentLoading = false;
        };

        ctrl.getAddress = function (contactsExits) {
            if (contactsExits === true) {
                ctrl.changeListAddress = function (address) {

                    ctrl.contact = address;

                    ctrl.startShippingProgress();
                    ctrl.startPaymentProgress();

                    ctrl.saveContact().then(function () {
                        ctrl.stopShippingProgress();
                        ctrl.stopPaymentProgress();
                    });
                };
            } else {
                zoneService.addCallback('set', function (data) {
                    ctrl.contact.Country = data.CountryName;
                    ctrl.contact.City = data.City;
                    ctrl.contact.Region = data.Region;

                    ctrl.startShippingProgress();
                    ctrl.startPaymentProgress();

                    ctrl.saveContact().then(function () {
                        ctrl.stopShippingProgress();
                        ctrl.stopPaymentProgress();
                    });
                });

                zoneService.getCurrentZone().then(function (data) {
                    ctrl.contact.Country = data.CountryName;
                    ctrl.contact.City = data.City;
                    ctrl.contact.Region = data.Region;

                    ctrl.saveContact().then(function () {
                        ctrl.stopShippingProgress();
                        ctrl.stopPaymentProgress();
                    });
                });
            }
        };

        ctrl.changeShipping = function (shipping) {

            ctrl.startShippingProgress();
            ctrl.startPaymentProgress();

            if (ctrl.ngSelectShipping !== shipping) {
                ctrl.ngSelectShipping = shipping;
            }

            checkoutService.saveShipping(shipping)
                 .then(function (response) {
                     ctrl.stopShippingProgress();
                     ctrl.stopPaymentProgress();
                     return ctrl.ngSelectShipping = ng.extend(ctrl.ngSelectShipping, response.selectShipping);
                 })
                 .then(relationship['shipping']);
        };

        ctrl.changePayment = function (payment) {

            ctrl.startPaymentProgress();

            if (ctrl.ngSelectPayment !== payment) {
                ctrl.ngSelectPayment = payment;
            }

            checkoutService.savePayment(payment).then(relationship['payment']).then(function () {
                ctrl.stopPaymentProgress();
            });
        };

        ctrl.fetchShipping = function () {

            return checkoutService.getShipping()
                .then(function (response) {

                    ctrl.ngSelectShipping = ctrl.getSelectedItem(response.option, response.selectShipping);

                    for (var i = 0, len = response.option.length ; i < len; i++) {

                        if (response.option[i].ShippingPoints != null) {
                            response.option[i].SelectedPoint = response.option[i].SelectedPoint || response.option[i].ShippingPoints[0];
                        }
                    }

                    if (ctrl.ngSelectShipping == null) {
                        return ctrl.Shipping = null;
                    }

                    return ctrl.Shipping = response;
                });
        };

        ctrl.fetchPayment = function () {
            return checkoutService.getPayment()
                .then(function (response) {

                    ctrl.ngSelectPayment = ctrl.getSelectedItem(response.option, response.selectPayment);

                    return ctrl.Payment = response;
                });
        };

        ctrl.fetchCart = function () {
            return checkoutService.getCheckoutCart()
                .then(function (response) {
                    ctrl.showCart = true;
                    ctrl.isShowCouponInput = response.Certificate == null && response.Coupon == null;
                    if (ctrl.Cart.Discount != null) {
                        ctrl.Cart.Discount.Key = $sce.trustAsHtml(ctrl.Cart.Discount.Key);
                    }
                    if (ctrl.Cart.Coupon != null) {
                        ctrl.Cart.Coupon.Key = $sce.trustAsHtml(ctrl.Cart.Coupon.Key);
                    }

                    return ctrl.Cart = response;
                });
        };

        ctrl.getSelectedItem = function (array, selectedItem) {
            var item;

            for (var i = array.length - 1; i >= 0; i--) {
                if (array[i].Id === selectedItem.Id) {
                    //selectedItem имеет заполненные поля какие опции выбраны, поэтому объединяем
                    array[i] = ng.extend(array[i], selectedItem);
                    item = array[i];
                    break;
                }
            }

            return item;
        };

        ctrl.autorizeBonus = function (cardNumber) {
            return checkoutService.autorizeBonus(cardNumber).then(function () {
                return relationship['bonus']();
            });
        };

        ctrl.changeBonus = function (isApply) {
            return checkoutService.toggleBonus(isApply).then(relationship['bonus']);
        };

        ctrl.applyCoupon = function () {
            ctrl.isShowCoupon = false;
            return checkoutService.couponApplied().then(relationship['coupon']);
        };

        ctrl.deleteCard = function () {
            ctrl.isShowCoupon = true;
            return checkoutService.couponApplied().then(relationship['coupon']);
        };

        ctrl.commentSave = function (message) {
            checkoutService.commentSave(message);
        };

        ctrl.saveNewCustomer = function (field) {
            if (field === 'email') {
                $(document).trigger('customer.email', ctrl.newCustomer);
            }

            checkoutService.saveNewCustomer(ctrl.newCustomer).then(ctrl.fetchCart);;
        };

        ctrl.saveWantBonusCard = function () {
            checkoutService.saveWantBonusCard(ctrl.wantBonusCard).then(ctrl.fetchCart);;
        };

        ctrl.saveContact = function (stopUpdateShipping) {
            return checkoutService.saveContact(ctrl.contact).then(function(data) {
                if (stopUpdateShipping == null || stopUpdateShipping === false) {
                    ctrl.startShippingProgress();
                    ctrl.startPaymentProgress();

                    relationship['address'](data).finally(function () {
                        ctrl.stopShippingProgress();
                        ctrl.stopPaymentProgress();
                    });
                }
            });
        };

        ctrl.submitOrder = function (event) {

            event.preventDefault();

            checkoutService.saveContact(ctrl.contact)
                .then(function () {

                    if (ctrl.checkoutNewCustomerForm != null) {
                        checkoutService.saveNewCustomer(ctrl.newCustomer);
                    }

                    return checkoutService.saveShipping(ctrl.ngSelectShipping);
                })
                .then(function () {
                    return checkoutService.savePayment(ctrl.ngSelectPayment);
                })
                .then(function () {
                    return checkoutService.commentSave(ctrl.comment);
                }).
                then(function () {
                    //todo: remove this code
                    document.querySelector('.js-checkout-form').submit();
                });
        };

        ctrl.submitMobile = function () {
            if (ctrl.process) {
                return;
            }
            ctrl.process = true;

            $http.post('mobile/checkoutmobile/confirm', { name: ctrl.name, phone: ctrl.phone, email: ctrl.email, message: ctrl.message, rnd: Math.random() })
                .then(function (response) {
                    var data = response.data;

                    ctrl.responseOrderNo = data.orderNumber;
                    if (data.error == null || data.error == "") {
                        $(document).trigger("order_from_mobile");

                        setTimeout(function () {
                            window.location = window.location.pathname.replace('index', 'success') + '?code=' + (data.code != null ? data.code : "");
                            ctrl.process = false;
                            $rootScope.$apply();
                        }, 2000);
                    } else {
                        ctrl.process = false;
                        alert(data.error);
                        console.log("Error " + data.error);
                    }
                },
                function () {
                    console.log("Error");
                    ctrl.process = false;
                });
        };
    };

    ng.module('checkout')
      .controller('CheckOutCtrl', CheckOutCtrl);

    CheckOutCtrl.$inject = ['$http', '$q', '$sce', '$rootScope', 'zoneService', 'checkoutService'];

})(window.angular, window.jQuery);

