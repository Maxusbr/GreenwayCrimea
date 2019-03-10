; (function (ng) {
    'use strict';

    var ModalAddLeadCtrl = function ($uibModalInstance, $http, $window, toaster, $q, lastStatisticsService) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve.params;

            ctrl.customerId = params != null && params.customerId != null ? params.customerId : null;
            ctrl.paramPhone = params != null && params.phone != null ? params.phone : null;
            ctrl.fromCart = params != null && params.fromCart != null ? params.fromCart : false;
            ctrl.clientCode = params != null && params.clientCode != null ? params.clientCode : null;

            ctrl.sum = 0;

            var defer = $q.defer(),
                promise = defer.promise;

            if (ctrl.customerId != null) {
                promise = ctrl.getCustomer();
            } else {
                defer.resolve();
            }

            promise.then(ctrl.getCustomerFields);

            ctrl.getLeadForm();
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getLeadForm = function() {
            $http.get('leads/getLeadForm', { params: { customerId: ctrl.customerId, fromCart: ctrl.fromCart } }).then(function (result) {
                var data = result.data;
                ctrl.managers = data.managers;
                ctrl.manager = data.managers.filter(function (x) { return x.value.toString() === data.managerId })[0];
                ctrl.currencySymbol = data.currencySymbol;
                ctrl.statuses = data.statuses;
                ctrl.dealStatus = ctrl.statuses[0];
                ctrl.products = data.products;
                ctrl.sum = data.sum;
            });
        }

        ctrl.addLead = function() {
            $http.post('leads/add',
                {
                    customerId: ctrl.customerId,
                    lastName: ctrl.lastName,
                    firstName: ctrl.firstName,
                    patronymic: ctrl.patronymic,
                    phone: ctrl.phone,
                    email: ctrl.email,
                    description: ctrl.description,
                    sum: ctrl.sum,
                    managerId: ctrl.manager != null ? ctrl.manager.value : null,
                    dealStatusId: ctrl.dealStatus != null ? ctrl.dealStatus.value : null,
                    products: ctrl.products != null ? ctrl.products : null,
                    customerFields: ctrl.customerFields
                })
                .then(function(result) {
                    var data = result.data;
                    if (data.result === true) {
                        lastStatisticsService.getLastStatistics();
                        $window.location.assign('leads/edit/' + data.leadId);
                        toaster.pop('success', '', 'Лид добавлен');
                    } else {
                        toaster.pop('error', 'Ошибка при добавлении лида', data.errors);
                        ctrl.btnLoading = false;
                    }
                });
        };

        ctrl.addItems = function(result) {
            if (result == null || result.ids == null || result.ids.length == 0)
                return;

            ctrl.getTempProducts(result.ids);
        }

        ctrl.getTempProducts = function (offerIds) {

            ctrl.products = ctrl.products || [];

            if (offerIds != null && offerIds.length > 0) {
                for (var i = 0; i < offerIds.length; i++) {
                    ctrl.products.push({ OfferId: offerIds[i], Amount: 1 });
                }
            }

            $http.post("leads/getTempProducts", { model: ctrl.products }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.products = response.data.products;
                    ctrl.sum = response.data.sum.toString().replace('.', ',');
                } else {
                    //ctrl.products = null;
                    ctrl.sum = '0';
                }
            });
        }

        ctrl.removeTempProduct = function(index) {
            ctrl.products.splice(index, 1);
            ctrl.getTempProducts();
        }

        ctrl.changeProductAmount = function() {
            var sum = 0;
            for (var i = 0; i < ctrl.products.length; i++) {
                sum += ctrl.products[i].Amount * ctrl.products[i].Price;
            }
            ctrl.sum = (Math.round(sum * 100) / 100).toString().replace('.', ',');
        }

        ctrl.getCustomerFields = function () {
            $http.get("leads/getCustomerFields", {params: {customerId: ctrl.customerId}}).then(function (response) {
                var data = response.data;

                for (var i = 0; i < data.length; i++) {
                    if (data[i].Value == null) {
                        data[i].Value = '';
                    }
                }

                ctrl.customerFields = data;
                angular.forEach(ctrl.customerFields, function (field, key) {
                    if (field.FieldType == 4) { // date
                        field.containerId = 'customerField_' + field.Id;
                        field.datetimepickerConfig = {
                            dropdownSelector: '#' + field.containerId,
                            minView: 'day'
                        }
                    }
                });
            });
        }
        
        ctrl.findCustomer = function (val) {
            return $http.get('customers/getCustomersAutocomplete', { params: { q: val, rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        ctrl.selectCustomer = function (item) {
            if (item == null) return;
            ctrl.lastName = item.LastName;
            ctrl.firstName = item.FirstName;
            ctrl.patronymic = item.Patronymic;
            ctrl.phone = item.Phone;
            ctrl.email = item.Email;
            ctrl.customerId = item.CustomerId;

            ctrl.getCustomerFields();
        }

        ctrl.clearCustomer = function() {
            ctrl.lastName = null;
            ctrl.firstName = null;
            ctrl.patronymic = null;
            ctrl.phone = null;
            ctrl.email = null;
            ctrl.customerId = null;

            ctrl.getCustomerFields();
        }

        ctrl.getCustomer = function() {
            return $http.get("customers/getCustomerWithContact", { params: { customerId: ctrl.customerId, code: ctrl.clientCode } }).then(function (response) {
                var customer = response.data;
                if (customer != null) {
                    ctrl.firstName = customer.FirstName;
                    ctrl.lastName = customer.LastName;
                    ctrl.patronymic = customer.Patronymic;
                    ctrl.email = customer.Email;
                    ctrl.phone = customer.Phone;
                } else {
                    ctrl.customerId = null;

                    if (ctrl.paramPhone != null) {
                        ctrl.phone = ctrl.paramPhone;
                    }
                }
            });
        }

    };

    ModalAddLeadCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q', 'lastStatisticsService'];

    ng.module('uiModal')
        .controller('ModalAddLeadCtrl', ModalAddLeadCtrl);

})(window.angular);