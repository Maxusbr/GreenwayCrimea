//#region module
; (function (ng) {

    'use strict';

    ng.module('shippingPaymentPage', []);

})(window.angular);
//#endregion

//#region controllers
; (function (ng) {

    'use strict';

    var ShippingPaymentPageCtrl = function ($http, zoneService, checkoutService, $q) {
        var ctrl = this,
            relationship,
            preorderList;

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
            }
        };

        ctrl.contact = {};
        ctrl.Payment = {};
        ctrl.Shipping = {};
        ctrl.Cart = {};
        ctrl.shippingDataReceived = false;
        ctrl.paymentDataReceived = false;
        ctrl.useCart = false;

        ctrl.fetchShipping = function () {
            return checkoutService.getShipping(ctrl.useCart === false ? preorderList : null)
                .then(function (response) {
                    ctrl.ngSelectShipping = ctrl.getSelectedItem(response.option, response.selectShipping);

                    if (response != null && response.option != null) {
                        for (var i = 0, len = response.option.length ; i < len; i++) {
                            if (response.option[i].ShippingPoints != null) {
                                response.option[i].SelectedPoint = response.option[i].SelectedPoint || response.option[i].ShippingPoints[0];
                            }
                        }
                    }

                    ctrl.shippingDataReceived = true;

                    return ctrl.Shipping = response;
                });
        };

        ctrl.fetchPayment = function () {
            return checkoutService.getPayment(ctrl.useCart === false ? preorderList : null)
                .then(function (response) {

                    ctrl.ngSelectPayment = ctrl.getSelectedItem(response.option, response.selectPayment);

                    ctrl.paymentDataReceived = true;

                    //return ng.extend(ctrl.Payment, response);
                    return ctrl.Payment = response;
                });
        };

        ctrl.fetchCart = function () {
            return checkoutService.getCheckoutCart()
                .then(function (response) {

                    ctrl.shippingDataReceived = true;
                    ctrl.paymentDataReceived = true;

                    return ng.extend(ctrl.Cart, response);
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

        ctrl.changeShipping = function (shipping) {

            ctrl.shippingDataReceived = false;
            ctrl.paymentDataReceived = false;

            if (ctrl.ngSelectShipping !== shipping) {
                ctrl.ngSelectShipping = shipping;
            }

            return checkoutService.saveShipping(shipping, ctrl.useCart === false ? preorderList : null)
                 .then(function (response) {
                     ctrl.shippingDataReceived = true;

                     return ctrl.ngSelectShipping = ng.extend(ctrl.ngSelectShipping, response.selectShipping);
                 })
                 .then(relationship['shipping']);
        };

        ctrl.changePayment = function (payment) {

            if (ctrl.ngSelectPayment !== payment) {
                ctrl.ngSelectPayment = payment;
            }

            return checkoutService.savePayment(payment, ctrl.useCart === false ? preorderList : null).then(relationship['payment']);
        };

        ctrl.setZone = function (city, obj, countryid) {
            zoneService.setCurrentZone(city, obj, countryid);
        };

        ctrl.reloadData = function () {

            ctrl.shippingDataReceived = false;
            ctrl.paymentDataReceived = false;

            relationship['address']();
        };

        $http.get('shipping-payment/getlistproduct').then(function (response) {

            preorderList = response.data;

            zoneService.addCallback('set', function (data) {
                ctrl.shippingDataReceived = false;
                ctrl.paymentDataReceived = false;

                ctrl.contact.Country = data.CountryName;
                ctrl.contact.City = data.City;
                ctrl.contact.Region = data.Region;

                checkoutService.saveContact(ctrl.contact)
                                .then(relationship['address']);
            });

            zoneService.getCurrentZone().then(function (data) {
                ctrl.contact.Country = data.CountryName;
                ctrl.contact.City = data.City;
                ctrl.contact.Region = data.Region;

                checkoutService.saveContact(ctrl.contact)
                               .then(relationship['address']);
            });


        });
    };

    ng.module('shippingPaymentPage')
      .controller('ShippingPaymentPageCtrl', ShippingPaymentPageCtrl);

    ShippingPaymentPageCtrl.$inject = ['$http', 'zoneService', 'checkoutService', '$q'];

})(window.angular);
//#endregion