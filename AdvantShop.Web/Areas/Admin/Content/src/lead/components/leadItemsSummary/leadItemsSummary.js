; (function (ng) {
    'use strict';

    var LeadItemsSummaryCtrl = function ($http, $timeout, $window, toaster, SweetAlert, $uibModal) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.toggleselectCurrencyLabel('1');

            if (ctrl.onInit != null) {
                ctrl.onInit({ leadItemsSummary: ctrl });
            }
        }

        ctrl.toggleselectCurrencyLabel = function (val) {
            ctrl.selectCurrency = val;
        };

        ctrl.getLeadItemsSummary = function () {
            return $http.get('leads/getLeadItemsSummary', { params: { leadId: ctrl.leadId } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.Summary = data;
                    return data;
                } else {
                    return null;
                }
            });
        }

        ctrl.changeDiscount = function (discount) {
            
            if (discount == null || isNaN(parseFloat(discount))) {
                toaster.pop('error', '', 'Скидка может быть от 0 до 100');
                return;
            }

            $http.post("leads/changeDiscount", { leadId: ctrl.leadId, discount: discount, isValue: ctrl.selectCurrency === "0" }).then(function (response) {
                if (response.data.result == true) {
                    toaster.pop('success', '', 'Скидка сохранена');
                    ctrl.Discount = discount;
                }

                return response.data;
            }).finally(function () {
                //ctrl.getLeadItemsSummary();
                ctrl.discountPopoverClose();
                ctrl.onChangeDiscount();
            });
        }

        ctrl.discountPopoverOpen = function () {
            ctrl.discountPopoverIsOpen = true;
        };

        ctrl.discountPopoverClose = function () {
            ctrl.discountPopoverIsOpen = false;
        };

        ctrl.discountPopoverToggle = function () {
            ctrl.discountPopoverIsOpen === true ? ctrl.discountPopoverClose() : ctrl.discountPopoverOpen();
        }

        ctrl.changeShipping= function() {
            ctrl.getLeadItemsSummary();
        }

        var popoverShippingTimer;

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


        ctrl.createOrderWithNotice = function () {
            SweetAlert.confirm("Для выставления счета необходимо создать заказ. Создать заказ?", { title: "" }).then(function (result) {
                if (result === true) {
                    $http.post('leads/createOrder', { leadId: ctrl.leadId }).then(function (response) {
                        var data = response.data;
                        if (data.result === true) {
                            toaster.pop('success', '', 'Заказ успешно создан');
                            $window.location.assign('orders/edit/' + data.orderId);
                        } else {
                            toaster.pop('error', '', 'Не удалось создать заказ');
                        }
                    });
                }
            });
        }

        ctrl.chooseDeliveryResult = function(result) {
            if (result != null) {
                ctrl.chooseDelivery();
            }
        }

        ctrl.chooseDelivery = function() {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalShippingsCtrl',
                controllerAs: 'ctrl',
                size: 'lg',
                templateUrl: '../areas/admin/content/src/order/modal/shippings/shippings.html',
                resolve: {
                    order: function () {
                        return { orderId: ctrl.leadId, isLead: true, country: ctrl.Summary.Country, region: ctrl.Summary.Region, city: ctrl.Summary.City };
                    }
                }
            }).result.then(function (result) {
                ctrl.changeShipping(result);
                return result;
            }, function (result) {
                ctrl.changeShipping(result);
                return result;
            });
        }


    };

    LeadItemsSummaryCtrl.$inject = ['$http', '$timeout', '$window', 'toaster', 'SweetAlert', '$uibModal'];

    ng.module('leadItemsSummary', [])
        .controller('LeadItemsSummaryCtrl', LeadItemsSummaryCtrl)
        .component('leadItemsSummary', {
            templateUrl: '../areas/admin/content/src/lead/components/leadItemsSummary/leadItemsSummary.html',
            controller: LeadItemsSummaryCtrl,
            bindings: {
                leadId: '=',
                onInit: '&',
                onChangeDiscount: '&',
            }

        });

})(window.angular);