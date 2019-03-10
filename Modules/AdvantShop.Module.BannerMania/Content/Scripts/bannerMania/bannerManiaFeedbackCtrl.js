; (function (ng) {
    'use strict';

    var bannerManiaFeedbackCtrl = function ($http, toaster, $uibModal, SweetAlert) {
        var ctrl = this;

        ctrl.Name = "";
        ctrl.Email = "";
        ctrl.Phone = "";
        ctrl.Message = "";

        ctrl.$onInit = function () {
            $http.get('../bmsettingsadmin/feedbacksettings').then(function success(response) {
                ctrl.Name = response.data.Name;
                ctrl.Email = response.data.Email;
                ctrl.Phone = response.data.Phone;
                ctrl.Message = response.data.Message;
            });
        };

        ctrl.send = function (form) {
            if (form.$valid) {
                $http.post('../bmsettingsadmin/feedbacksend', { Name: ctrl.Name, Email: ctrl.Email, Phone: ctrl.Phone, Message: ctrl.Message })
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

    bannerManiaFeedbackCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert'];

    ng.module('bannerManiaFeedback', [])
        .controller('bannerManiaFeedbackCtrl', bannerManiaFeedbackCtrl)
        .component('bannerManiaFeedback', {
            templateUrl: '../modules/BannerMania/content/scripts/bannerMania/templates/bannerManiaFeedback.html',
            controller: 'bannerManiaFeedbackCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);