; (function (ng) {
    'use strict';

    var OrderItemsSummaryCtrl = function ($http, $timeout, toaster, SweetAlert) {
        var ctrl = this, popoverShippingTimer, popoverPaymentTimer;

		ctrl.OrderDiscountNew = 0;

        ctrl.$onInit = function () {

			ctrl.grastinActionsUrl = 'grastin/getorderactions?orderid=' + ctrl.orderId;
            ctrl.toggleselectCurrencyLabel('0');

            ctrl.getOrderItemsSummary();

            if (ctrl.onInit != null) {
                ctrl.onInit({ orderItemsSummary: ctrl });
            }
        }

        ctrl.toggleselectCurrencyLabel = function (val) {
            ctrl.selectCurrency = val;
        };

        ctrl.getOrderItemsSummary = function () {
            $http.get('orders/getOrderItemsSummary', { params: { orderId: ctrl.orderId } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.Summary = data;
                }
            });
        }

        ctrl.changeShipping = function (result) {

            ctrl.getOrderItemsSummary();
            ctrl.grastinUpdateActions();

            if (result != null) {
                toaster.pop('success', '', 'Метод доставки сохранен');
            }
        }


        ctrl.changePayment = function (result) {
            ctrl.getOrderItemsSummary();

            if (result != null) {
                toaster.pop('success', '', 'Метод оплаты сохранен');
            }
        };

        
        /* shippings */
        ctrl.createYandexDeliveryOrder = function () {
            $http.post('orders/createYandexDeliveryOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.createCheckoutRuOrder = function () {
            $http.post('orders/createCheckoutRuOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.sdekOrderPrintForm = function () {
            window.location = 'orders/sdekOrderPrintForm?orderId=' + ctrl.orderId;
        }

        ctrl.sdekOrderReportStatus = function () {
            window.location = 'orders/sdekOrderReportStatus?orderId=' + ctrl.orderId;
        }

        ctrl.sdekReportOrderInfo = function () {
            window.location = 'orders/sdekReportOrderInfo?orderId=' + ctrl.orderId;
        }

        ctrl.createSdekOrder = function () {
            $http.post('orders/createSdekOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.sdekCallCustomer = function () {
            $http.post('orders/sdekCallCustomer', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.sdekDeleteOrder = function () {
            $http.post('orders/sdekDeleteOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.createBoxberryOrder = function () {
            $http.post('orders/createBoxberryOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {                    
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.deleteBoxberryOrder = function () {
            $http.post('orders/deleteBoxberryOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

  ctrl.grastinOrderPrintMark = function () {
            var params = {
                orderId: ctrl.orderId,
            };

            $http.post('orders/grastinSendRequestForMark', params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                    window.location = "orders/GrastinOrderPrintMark?filename=" + encodeURIComponent(data.obj.FileName);
                } else {
                    data.errors.forEach(function (error) {
                        toaster.pop('error', '', error);
                    });
                }
            });
        }

        ctrl.createDdeliveryOrder = function () {
            $http.post('orders/createDDeliveryOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.getDdeliveryOrderInfo = function () {
            window.location = 'orders/ddeliveryOrderInfo?orderId=' + ctrl.orderId;
            //$http.post('orders/DDeliveryOrderInfo', { orderId: ctrl.orderId }).then(function (response) {
            //    var data = response.data;
            //    if (data.result === true) {
            //        toaster.pop('success', '', data.message);
            //    } else {
            //        toaster.pop('error', '', data.error);
            //    }
            //});
        }

        ctrl.canselDdeliveryOrder = function () {
            $http.post('orders/CanselDDeliveryOrder', { orderId: ctrl.orderId }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', data.message);
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }

        ctrl.grastinUpdateActions = function () {
            ctrl.grastinActionsUrl = 'grastin/getorderactions?orderid=' + ctrl.orderId + '&rnd=' + Math.random();
        }

        ctrl.grastinUpdateActionsAndSummary = function () {
            ctrl.getOrderItemsSummary();
            ctrl.grastinUpdateActions();
        }

        /* end shippings */

        /* discount */
        ctrl.discountPopoverOpen = function () {
            ctrl.discountPopoverIsOpen = true;
        };

        ctrl.discountPopoverClose = function () {
            ctrl.discountPopoverIsOpen = false;
        };

		ctrl.discountPopoverToggle = function () {
			ctrl.OrderDiscountNew = ctrl.Summary.OrderDiscountValue;
            ctrl.discountPopoverIsOpen === true ? ctrl.discountPopoverClose() : ctrl.discountPopoverOpen();
        }

        ctrl.changeDiscount = function (discount) {

            if (ctrl.orderId === 0) return;

            if (discount == null || isNaN(parseFloat(discount))) {
                toaster.pop('error', '', 'Скидка может быть от 0 до 100');
                return;
            }

            $http.post("orders/changeDiscount", { orderId: ctrl.orderId, orderDiscount: discount, isValue: ctrl.selectCurrency === "0" }).then(function (response) {
                if (response.data.result == true) {
                    toaster.pop('success', '', 'Скидка сохранена');
                    if (ctrl.selectCurrency === "1") {
                        ctrl.Summary.OrderDiscount = discount;
                    } else {
                        ctrl.Summary.OrderDiscountValue = discount;
                    }
                }

                return response.data;
            }).finally(function () {
                ctrl.getOrderItemsSummary();
                ctrl.discountPopoverClose();
            });
        }

        ctrl.getPaymentDetailsLink = function() {
            var link = ctrl.Summary.PrintPaymentDetailsLink;

            if (ctrl.Summary.PaymentDetails != null) {
                if (ctrl.Summary.PaymentDetails.INN != null && ctrl.Summary.PaymentDetails.INN.length > 0) {
                    link += '&bill_INN=' + ctrl.Summary.PaymentDetails.INN;
                }

                if (ctrl.Summary.PaymentDetails.CompanyName != null && ctrl.Summary.PaymentDetails.CompanyName.length > 0) {
                    link += '&bill_CompanyName=' + ctrl.Summary.PaymentDetails.CompanyName;
                }
            }

            return link;
        }
        

        /* bonuses */
        ctrl.bonusesPopoverOpen = function () {
            ctrl.bonusesPopoverIsOpen = true;
        };

        ctrl.bonusesPopoverClose = function () {
            ctrl.bonusesPopoverIsOpen = false;
        };

        ctrl.bonusesPopoverToggle = function () {
            ctrl.bonusesPopoverIsOpen === true ? ctrl.bonusesPopoverClose() : ctrl.bonusesPopoverOpen();
        }

        ctrl.useBonuses = function (bonusesAmount) {
            SweetAlert.confirm("Вы уверены, что хотите списать бонусы?", { title: "Списание бонусов" }).then(function (result) {
                if (result === true) {
                    $http.post('orders/useBonuses', { orderId: ctrl.orderId, bonusesAmount: bonusesAmount }).then(function (response) {
                        var data = response.data;
                        if (data.result === true) {
                            toaster.pop('success', '', 'Изменения сохранены');
                        } else {
                            toaster.pop('error', '', data.error);
                        }
                    }).finally(function () {
                        ctrl.getOrderItemsSummary();
                        ctrl.bonusesPopoverClose();
                    });
                }
            });
        }

        ctrl.popoverShippingOpen = function () {

            if (popoverShippingTimer != null) {
                $timeout.cancel(popoverShippingTimer);
            }

            ctrl.popoverShippingIsOpen = true;
        };

        ctrl.popoverShippingClose = function () {

            popoverShippingTimer = $timeout(function () {
                ctrl.popoverShippingIsOpen = false;
            }, 500);
        };

        ctrl.popoverPaymentOpen = function () {

            if (popoverPaymentTimer != null) {
                $timeout.cancel(popoverPaymentTimer);
            }

            ctrl.popoverPaymentIsOpen = true;
        };

        ctrl.popoverPaymentClose = function () {

            popoverPaymentTimer = $timeout(function () {
                ctrl.popoverPaymentIsOpen = false;
            }, 500);
        };
    };

    OrderItemsSummaryCtrl.$inject = ['$http', '$timeout', 'toaster', 'SweetAlert'];

    ng.module('orderItemsSummary', [])
        .controller('OrderItemsSummaryCtrl', OrderItemsSummaryCtrl)
        .component('orderItemsSummary', {
            templateUrl: '../areas/admin/content/src/order/components/orderItemsSummary/orderItemsSummary.html',
            controller: OrderItemsSummaryCtrl,
            bindings: {
                orderId: '=',
                onInit: '&',
                country: '=',
                region: '=',
                city: '=',
                zip: '=',
                isEdit: '<',
                onStopEdit: '&'
            }

        });

})(window.angular);