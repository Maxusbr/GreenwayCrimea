; (function (ng) {
    'use strict';

    var addressService = function ($http, $q, modalService, $translate) {
        var service = this,
            fields;

        service.dialogRender = function (callbackClose, parentScope) {

            var options = {
                'modalClass': 'address-dialog',
                'callbackClose': callbackClose,
                'isOpen': true
            };

            modalService.renderModal(
                'modalAddress',
                $translate.instant('Js.Address.Address'),
                '<div data-ng-include="\'/scripts/_partials/address/templates/modal.html\'"></div>',
                '<div data-ng-include="\'/scripts/_partials/address/templates/modalFooter.html\'"></div>',
                options,
                { addressList: parentScope });
        };

        service.dialogOpen = function () {
            modalService.open('modalAddress');
        };

        service.dialogClose = function () {
            modalService.close('modalAddress');
        };

        service.getDialogScope = function () {
            return modalService.getModal('modalAddress').then(function (dialog) { return dialog.modalScope });
        };

        service.removeAddress = function (contactId) {
            return $http.post('MyAccount/DeleteCustomerContact', { contactId: contactId, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.getAddresses = function () {
            return $http.get('MyAccount/GetCustomerContacts', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        service.getFields = function () {
            return fields != null ? $q.when(fields) : $http.get('MyAccount/GetFieldsForCustomerContacts').then(function (response) {
                return fields = response.data;
            });
        };

        service.addUpdateCustomerContact = function (account) {
            return $http.post('MyAccount/AddUpdateCustomerContact', { account: account, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        }
    };

    ng.module('address')
      .service('addressService', addressService);

    addressService.$inject = ['$http', '$q', 'modalService', '$translate'];
})(window.angular);