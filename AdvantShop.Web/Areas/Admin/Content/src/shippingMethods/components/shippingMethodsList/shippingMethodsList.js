; (function (ng) {
    'use strict';

    var ShippingMethodsListCtrl = function ($http, toaster, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.fetch();

            if (ctrl.onInit != null) {
                ctrl.onInit({ methods: ctrl });
            }
        };

        ctrl.fetch = function () {
            $http.get('ShippingMethods/getShippingMethods').then(function (response) {
                ctrl.methods = response.data;
            });
        };


        ctrl.sortableOptions = {
            orderChanged: function(event) {
                var methodId = event.source.itemScope.item.ShippingMethodId,
                    prev = ctrl.methods[event.dest.index - 1],
                    next = ctrl.methods[event.dest.index + 1];
                
                $http.post('shippingMethods/changeSorting', {
                    Id: methodId,
                    prevId: prev != null ? prev.ShippingMethodId : null,
                    nextId: next != null ? next.ShippingMethodId : null
                }).then(function(response) {
                    if (response.data.result === true) {
                        toaster.pop('success', '', 'Изменения сохранены');
                    }
                });
            }
        };


        ctrl.setEnabled = function(methodId, checked) {
            $http.post('shippingMethods/setEnabled', { id: methodId, enabled: checked }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        }

        ctrl.deleteMethod = function (methodId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('shippingMethods/deleteMethod', { methodId: methodId }).then(function (response) {
                        if (response.data.result === true) {
                            ctrl.fetch();
                            toaster.pop('success', '', 'Метод доставки успешно удален');
                        }
                    });
                }
            });
        }

    };

    ShippingMethodsListCtrl.$inject = ['$http', 'toaster', 'SweetAlert'];

    ng.module('shippingMethodsList', ['as.sortable'])
        .controller('ShippingMethodsListCtrl', ShippingMethodsListCtrl)
        .component('shippingMethodsList', {
            templateUrl: '../areas/admin/content/src/shippingMethods/components/shippingMethodsList/templates/shippingMethodsList.html',
            controller: 'ShippingMethodsListCtrl',
            transclude: true,
            bindings: {
                onInit: '&'
            }
        });

})(window.angular);