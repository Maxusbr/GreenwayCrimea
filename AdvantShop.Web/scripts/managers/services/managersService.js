; (function (ng) {
    'use strict';

    var managersService = function ($http, $q, modalService, $translate) {

        var service = this;

        service.dialogRenderCall = function (parentScope) {

            var options = {
                'modalClass': 'managers-call-dialog'
            };

            modalService.renderModal(
                'modalManagersCall',
                $translate.instant("Js.Managers.CallRequestPopupTitle"),
                '<div data-ng-include="\'/scripts/managers/templates/modalCall.html\'"></div>',
                '<input type="submit" class="btn btn-middle btn-action" value="' + $translate.instant("Js.Managers.SendCallRequestButton") + '" data-button-validation data-button-validation-success="managers.sendRequestCall()" />',
                options,
                { managers: parentScope });
        };

        service.dialogRenderEmail = function (parentScope) {

            var options = {
                'modalClass': 'managers-email-dialog'
            };

            modalService.renderModal(
                'modalManagersEmail',
                $translate.instant("Js.Managers.EmailRequestPopupTitle"),
                '<div data-ng-include="\'/scripts/managers/templates/modalEmail.html\'"></div>',
                '<input type="submit" class="btn btn-middle btn-action" value="'+ $translate.instant("Js.Managers.SendEmailRequestButton") +'" data-button-validation data-button-validation-success="managers.sendRequestEmail()" />',
                options,
                { managers: parentScope });
        };
          

        service.dialogOpenCall = function (managerId) {
            modalService.open('modalManagersCall');
        };

        service.dialogCloseCall = function () {
            modalService.close('modalManagersCall');
        };


        service.dialogOpenEmail = function (managerId) {
            modalService.open('modalManagersEmail');
        };

        service.dialogCloseEmail = function () {
            modalService.close('modalManagersEmail');
        };

        service.getModalParams = function () {
            return $http.post('managers/getModalParams').then(function (response) {
                return response.data;
            });
        }

        service.sendEmail = function (clientName, emailText, email, managerId) {
            return $http.post('managers/sendemail', { clientName: clientName, emailText: emailText, email: email, managerId: managerId }).then(function (response) {
                return response.data;
            });
        };

        service.requestCall = function (clientName, clientPhone, comment, managerId) {
            return $http.post('managers/requestcall', { clientName: clientName, clientPhone: clientPhone, comment: comment, managerId: managerId }).then(function (response) {
                return response.data;
            });
        };

        service.setVisibleFooterEmail = function (visible) {
            modalService.setVisibleFooter('modalManagersEmail', visible);
        };

        service.setVisibleFooterCall = function (visible) {
            modalService.setVisibleFooter('modalManagersCall', visible);
        };

        service.setPristineFormEmail = function () {
            modalService.getModal('modalManagersEmail').then(function (modal) {
                modal.modalScope._form.$setPristine();
            });
        };

        service.setPristineFormCall = function () {
            modalService.getModal('modalManagersCall').then(function (modal) {
                modal.modalScope._form.$setPristine();
            });
        };
    };

    ng.module('managers')
      .service('managersService', managersService);

    managersService.$inject = ['$http', '$q', 'modalService', '$translate'];
})(window.angular);