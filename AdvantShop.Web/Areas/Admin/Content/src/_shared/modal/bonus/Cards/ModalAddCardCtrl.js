; (function (ng) {
    'use strict';

    var ModalAddCardCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            if (ctrl.$resolve != null) {
                var params = ctrl.$resolve.params;
                if (params != null) {
                    ctrl.customerId = params.customerId;
                    ctrl.selectedFirstName = params.firstName;
                    ctrl.selectedLastName = params.lastName;
                    ctrl.noredirect = params.noredirect;
                }
            }

            $http.get('grades/GetAllGrades')
                 .then(function (result) {
                     ctrl.Grades = result.data.obj;
                 },
                 function (err) {
                     toaster.pop('error', 'Ошибка получения грейдов', err);
                 });

            $http.get('grades/defaultgrade')
                .then(function (result) {
                    ctrl.GradeId = result.data.obj;
                },
                function (err) {
                    toaster.pop('error', 'Ошибка получения грейда по умолчанию', err);
                });

            $http.get('cards/generate')
                .then(function (result) {
                    ctrl.CardNumber = result.data.obj;
                },
                function (err) {
                    toaster.pop('error', 'Ошибка получения грейда по умолчанию',  err);
                });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.selectCustomer = function (result) {
            ctrl.getCustomer(result)
                .then(function (result) {
                    return result || $q.reject('error');
                });
        }

        ctrl.getCustomer = function (result) {
            if (result == null || result.customerId == null) {
                return false;
            }

            return $http.get("customers/getCustomerWithContact", { params: { customerId: result.customerId } }).then(function (response) {

                var customer = response.data;

                if (customer == null) return false;

                ctrl.customerId = customer.Id;

                //if (isNullOrWhitespace(ctrl.firstName))
                ctrl.firstName = ctrl.selectedFirstName = customer.FirstName;

                //if (isNullOrWhitespace(ctrl.lastName))
                ctrl.lastName = ctrl.selectedLastName = customer.LastName;

                //if (isNullOrWhitespace(ctrl.patronymic))
                ctrl.patronymic = customer.Patronymic;

                //if (isNullOrWhitespace(ctrl.email))
                ctrl.email = customer.Email;

                //if (isNullOrWhitespace(ctrl.phone))
                ctrl.phone = customer.Phone;

                //if (isNullOrWhitespace(ctrl.standardPhone))
                ctrl.standardPhone = customer.StandardPhone;

                ctrl.bonusCardNumber = customer.BonusCardNumber;
                ctrl.customerGroup = customer.CustomerGroup;
                var contacts = customer.Contacts;

                if (contacts != null && contacts.length > 0) {
                    var contact = contacts[0];

                    if (isNullOrWhitespace(ctrl.region))
                        ctrl.region = contact.Region;

                    if (isNullOrWhitespace(ctrl.city))
                        ctrl.city = contact.City;

                    if (isNullOrWhitespace(ctrl.zip))
                        ctrl.zip = contact.Zip;

                    if (isNullOrWhitespace(ctrl.address))
                        ctrl.address = contact.Address;

                    ctrl.customField1 = contact.CustomField1;
                    ctrl.customField2 = contact.CustomField2;
                    ctrl.customField3 = contact.CustomField3;
                }
                return true;
            });
        }

        ctrl.addCard = function () {
            ctrl.btnLoading = true;
            $http.post('cards/add',
                {
                    CardId: ctrl.customerId,
                    CardNumber: ctrl.CardNumber,
                    GradeId: ctrl.GradeId
                })
                .then(function (result) {
                    var data = result.data.result;
                    if (data === true) {
                        if (ctrl.noredirect) {
                            $uibModalInstance.close();
                        } else {
                            $window.location.assign('cards/edit/' + ctrl.customerId);
                        }
                        toaster.pop('success', 'Карта добавлена');
                    } else {
                        var er = '';
                        if (result.data.errors != null) {
                            er = result.data.errors.join('</br>');
                        }
                        toaster.pop('error', 'Ошибка при добавлении карты', er);
                    }
                },
                    function (err) {
                        toaster.pop('error', 'Ошибка при добавлении карты');
                    }).finally(function () {
                        ctrl.btnLoading = false;
                    });;
        };
    };

    ModalAddCardCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalAddCardCtrl', ModalAddCardCtrl);

})(window.angular);