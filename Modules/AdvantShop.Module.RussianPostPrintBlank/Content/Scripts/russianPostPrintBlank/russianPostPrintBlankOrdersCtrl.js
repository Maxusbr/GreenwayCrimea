; (function (ng) {
    'use strict';

    var russianPostPrintBlankOrdersCtrl = function ($http, toaster, $uibModal, SweetAlert, russianPostPrintBlankService) {
        var ctrl = this;

        ctrl.page = 1;
        ctrl.status = "Любой";
        ctrl.payed = "null";
        ctrl.shipping = "Любой";
        ctrl.backPage = 0;
        ctrl.nextPage = 2;
        ctrl.formType = "";
        ctrl.orderNumber = "";
        ctrl.pages = 99;
        ctrl.active = "true";

        ctrl.getOrders = function (page, status, payed, shipping, orderNumber) {
            russianPostPrintBlankService.getOrders(page, status, payed, shipping, orderNumber).then(function (response) {
                ctrl.OrdersSearch = response;
                ctrl.status = response.SelectStatus;
                ctrl.payed = response.SelectPayed;
                ctrl.shipping = response.SelectShipping;
                ctrl.orderNumber = response.OrderNumber;
                ctrl.backPage = response.BackPage;
                ctrl.nextPage = response.NextPage;
            });
        };

        ctrl.LoadTemplates = function () {
            russianPostPrintBlankService.getOrders().then(function (response) {
                ctrl.OrdersSearch.FormTypes = response.FormTypes;
            });
        }

        ctrl.checkActive = function () {
            russianPostPrintBlankService.checkActive().then(function (response) {
                ctrl.active = response;
            });
        };

        ctrl.$onInit = function () {
            ctrl.getOrders(ctrl.page, ctrl.status, ctrl.payed, ctrl.shipping, ctrl.orderNumber);
            russianPostPrintBlankService.getPagesCount().then(function (response) {
                ctrl.pages = parseInt(response);
            });
        };

        ctrl.changeOrderPayed = function () {
            ctrl.getOrders(ctrl.page, ctrl.status, ctrl.payed, ctrl.shipping, ctrl.orderNumber);
        };

        ctrl.changeOrderNumber = function () {
            ctrl.getOrders(ctrl.page, ctrl.status, ctrl.payed, ctrl.shipping, ctrl.orderNumber)
        };

        ctrl.changeOrderShipping = function () {
            ctrl.getOrders(ctrl.page, ctrl.status, ctrl.payed, ctrl.shipping, ctrl.orderNumber);
        };

        ctrl.changeOrderStatus = function () {
            ctrl.getOrders(ctrl.page, ctrl.status, ctrl.payed, ctrl.shipping, ctrl.orderNumber);
        };

        ctrl.changePage = function (back) {
            ctrl.page = back ? ctrl.backPage : ctrl.nextPage;
            ctrl.getOrders(ctrl.page, ctrl.status, ctrl.payed, ctrl.shipping, ctrl.orderNumber);
        };

        ctrl.printBlank = function (orderId, formType, prepayment) {
            if (!ctrl.Validate(orderId, formType)) {
                return;
            }

            var href = "";
            var templateId = parseInt(formType.Value, 10);

            if (angular.isNumber(templateId)) {
                href = "../RussianPostPrintFormByTemplate/?formType=" + templateId + "&orderId=" + orderId + "&prepayment=" + prepayment;
            }

            var width = 600;
            var height = 400;
            var popup = window.open(href, "popup", "height=" + height + ", width=" + width + "");
        };

        ctrl.Validate = function (orderId, formType) {
            if (formType == null) {
                toaster.pop('error', '', 'Выберите шаблон для печати');
                return false;
            }

            if (formType.Value == null || formType.Value == "-1") {
                toaster.pop('error', '', 'Выберите шаблон для печати');
                return false;
            }

            if (orderId < 1) {
                toaster.pop('error', '', 'Такого заказа не существует');
                return false;
            }

            if (ctrl.active == "false") {
                toaster.pop('error', '', 'Для печати бланков необходимо активировать модуль');
                return false;
            }

            return true;
        };
    };

    ng.module('russianPostPrintBlankOrders', [])
        .controller('russianPostPrintBlankOrdersCtrl', russianPostPrintBlankOrdersCtrl)
        .component('russianPostPrintBlankOrders', {
            templateUrl: '../modules/RussianPostPrintBlank/content/scripts/russianPostPrintBlank/templates/russianPostPrintBlankOrders.html',
            controller: 'russianPostPrintBlankOrdersCtrl'
        });

    russianPostPrintBlankOrdersCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert', 'russianPostPrintBlankService'];

})(window.angular);