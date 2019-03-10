; (function (ng) {
    'use strict';

    var FeedbackPromoCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.Name = "";
        ctrl.Email = "";
        ctrl.Phone = "";
        ctrl.Message = "";

        ctrl.$onInit = function () {
            $http.get('../amadmin/feedbacksettings').then(function success(response) {
                ctrl.Name = response.data.Name;
                ctrl.Email = response.data.Email;
                ctrl.Phone = response.data.Phone;
                ctrl.Message = response.data.Message;
            });
        };

        ctrl.send = function (form) {
            if (form.$valid) {
                $http.post('../amadmin/feedbacksend', { Name: ctrl.Name, Email: ctrl.Email, Phone: ctrl.Phone, Message: ctrl.Message })
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

    FeedbackPromoCtrl.$inject = ['$http', 'toaster'];

    ng.module('russianPostPrintBlankFeedback', [])
        .controller('FeedbackPromoCtrl', FeedbackPromoCtrl)
        .component('feedbackPromo', {
            templateUrl: '../modules/AdditionalMarkers/content/scripts/feedback/feedback-promo.html',
            controller: 'FeedbackPromoCtrl'
        });

})(window.angular);