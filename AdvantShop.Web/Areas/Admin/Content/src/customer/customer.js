; (function (ng) {
    'use strict';

    var CustomerCtrl = function ($http, SweetAlert, $window, toaster) {

        var ctrl = this;

        ctrl.initCustomer = function(customerId, isEditMode, standardPhone) {
            ctrl.customerId = customerId;
            ctrl.isEditMode = isEditMode;
            ctrl.standardPhone = standardPhone;
        }

        ctrl.delete = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('customers/deleteCustomer', { customerId: ctrl.customerId }).then(function (response) {
                        var data = response.data;
                        if (data) {
                            $window.location.assign('customers');
                        } else {
                            toaster.pop('error', '', 'Пользователь не может быть удален');
                        }
                    });
                }
            });
        }

        ctrl.createOrderFromCart = function () {
            ctrl.addingOrder = false;
            $http.post('orders/addOrderFromCart', { customerId: ctrl.customerId }).then(function (response) {
                var data = response.data;
                if (data.result == true && response.data.obj != 0) {
                    toaster.pop("success", "", "Создан черновик заказа №" + response.data.obj);
                    $window.location.assign('orders/edit/' + response.data.obj);
                } else {
                    toaster.pop('error', 'Ошибка при создании заказа', data.errors != null ? '</br>' + data.errors.join('</br>') : '');
                    ctrl.addingOrder = false;
                }
            });
        }
    };

    CustomerCtrl.$inject = ['$http', 'SweetAlert', '$window', 'toaster'];


    ng.module('customer', ['uiGridCustom', 'customerOrders', 'customerLeads'])
      .controller('CustomerCtrl', CustomerCtrl);

})(window.angular);