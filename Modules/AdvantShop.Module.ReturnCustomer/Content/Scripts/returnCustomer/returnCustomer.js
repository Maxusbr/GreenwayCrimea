; (function (ng) {
    'use strict';

    var returnCustomerCtrl = function ($http, toaster, $uibModal, SweetAlert) {
        var ctrl = this;

        ctrl.MessageSubject = "";
        ctrl.MessageText = "";
        ctrl.AlternativeMessageSubject = "";
        ctrl.AlternativeMessageText = "";
        ctrl.DaysInterval = 0;
        ctrl.DisabledMailsList = "";
        ctrl.AutoSending = false;
        ctrl.isLogExists = false;

        ctrl.$onInit = function () {
            ctrl.Init();
        };

        ctrl.Init = function () {
            $http.get('../rcadmin/getsettings').then(function success(response) {
                ctrl.MessageSubject = response.data.settings.MessageSubject;
                ctrl.MessageText = response.data.settings.MessageText;
                ctrl.AlternativeMessageSubject = response.data.settings.AlternativeMessageSubject;
                ctrl.AlternativeMessageText = response.data.settings.AlternativeMessageText;
                ctrl.DisabledMailsList = response.data.settings.DisabledMailsList;
                ctrl.DaysInterval = response.data.settings.DaysInterval;
                ctrl.AutoSending = response.data.settings.AutoSending;
                ctrl.isLogExists = response.data.isLogExists;
            });
        };

        ctrl.saveChanges = function (form) {
            if (form.$valid)
            {
                $http.post('../rcadmin/saveChanges',
                    {
                        MessageSubject: ctrl.MessageSubject,
                        MessageText: ctrl.MessageText,
                        AlternativeMessageSubject: ctrl.AlternativeMessageSubject,
                        AlternativeMessageText: ctrl.AlternativeMessageText,
                        DisabledMailsList: ctrl.DisabledMailsList,
                        DaysInterval: ctrl.DaysInterval,
                        AutoSending: ctrl.AutoSending
                    })
                    .then(function success(response) {
                        if (response.data.success) {
                            toaster.pop('success', '', response.data.msg);
                        } else {
                            toaster.pop('error', '', response.data.msg);
                        }
                    })
            }
            else
            {
                toaster.pop('error', '', 'Заполните необходимые данные');
            }
        }

        ctrl.deleteLog = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить лог отправленных писем?", { title: "Удаление лога" }).then(function (result) {
                if (result === true) {
                    $http.post('../rcadmin/deletelog', {})
                        .then(function success(response) {
                            if (response.data.success) {
                                toaster.pop('success', '', response.data.msg);
                                ctrl.Init();
                            } else {
                                toaster.pop('error', '', response.data.msg);
                            }
                        })
                }
            });
        };
    };

    returnCustomerCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert'];

    ng.module('returnCustomer', [])
        .controller('returnCustomerCtrl', returnCustomerCtrl)
        .component('returnCustomer', {
            templateUrl: '../modules/ReturnCustomer/content/Scripts/returnCustomer/templates/returnCustomer.html',
            controller: 'returnCustomerCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);