; (function (ng) {
    'use strict';

    var PaymentMethodsListCtrl = function ($http, SweetAlert, toaster, urlHelper, $window) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.fetch();

            if (ctrl.onInit != null) {
                ctrl.onInit({ methods: ctrl });
            }
        };

        ctrl.fetch = function () {
            $http.get('paymentMethods/getPaymentMethods', {params: {rnd:Math.random() }}).then(function (response) {
                ctrl.methods = response.data;
            });
        };


        ctrl.sortableOptions = {
            orderChanged: function(event) {
                var methodId = event.source.itemScope.item.PaymentMethodId,
                    prev = ctrl.methods[event.dest.index - 1],
                    next = ctrl.methods[event.dest.index + 1];
                
                $http.post('paymentMethods/changeSorting', {
                    Id: methodId,
                    prevId: prev != null ? prev.PaymentMethodId : null,
                    nextId: next != null ? next.PaymentMethodId : null
                }).then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения сохранены');
                    }
                });
            }
        };

        ctrl.setEnabled = function (methodId, checked) {
            $http.post('paymentMethods/setEnabled', { id: methodId, enabled: checked }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        }

        ctrl.deleteMethod = function (methodId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('paymentMethods/deleteMethod', { methodId: methodId }).then(function (response) {
                        if (response.data.result === true) {
                            ctrl.fetch();
                            toaster.pop('success', '', 'Метод оплаты успешно удален');
                        }
                    });
                }
            });
        }

    };

    PaymentMethodsListCtrl.$inject = ['$http', 'SweetAlert', 'toaster', 'urlHelper', '$window'];

    ng.module('paymentMethodsList', ['as.sortable'])
        .controller('PaymentMethodsListCtrl', PaymentMethodsListCtrl)
        .component('paymentMethodsList', {
            templateUrl: '../areas/admin/content/src/paymentMethods/components/paymentMethodsList/templates/paymentMethodsList.html',
            controller: 'PaymentMethodsListCtrl',
            transclude: true,
            bindings: {
                onInit: '&'
            }
        });

})(window.angular);