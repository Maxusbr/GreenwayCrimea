; (function (ng) {
    'use strict';

    var russianPostPrintBlankFeedbackCtrl = function ($http, toaster, $uibModal, SweetAlert) {
        var ctrl = this;

        ctrl.Name = "";
        ctrl.Email = "";
        ctrl.Phone = "";
        ctrl.Message = "";

        ctrl.$onInit = function () {
            $http.get('../rppbadmin/feedbacksettings').then(function success(response) {
                ctrl.Name = response.data.Name;
                ctrl.Email = response.data.Email;
                ctrl.Phone = response.data.Phone;
                ctrl.Message = response.data.Message;
            });
        };

        ctrl.send = function (form) {
            if (form.$valid) {
                $http.post('../rppbadmin/feedbacksend', { Name: ctrl.Name, Email: ctrl.Email, Phone: ctrl.Phone, Message: ctrl.Message })
                    .then(function success(response) {
                        if (response.data.success) {
                            toaster.pop('success', '', response.data.msg);
                        } else {
                            toaster.pop('error', '', response.data.msg);
                        }
                    })
            }
            else {
                toaster.pop('error', '', 'Заполните необходимые данные')
            }
        }
    };

    russianPostPrintBlankFeedbackCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert'];

    ng.module('russianPostPrintBlankFeedback', [])
        .controller('russianPostPrintBlankFeedbackCtrl', russianPostPrintBlankFeedbackCtrl)
        .component('russianPostPrintBlankFeedback', {
            templateUrl: '../modules/RussianPostPrintBlank/content/scripts/russianPostPrintBlank/templates/russianPostPrintBlankFeedback.html',
            controller: 'russianPostPrintBlankFeedbackCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);