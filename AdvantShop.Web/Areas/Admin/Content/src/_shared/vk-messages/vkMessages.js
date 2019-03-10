; (function (ng) {
    'use strict';

    var VkMessagesCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function() {
            ctrl.getMessages();
        };

        ctrl.getMessages = function() {
            $http.get('vk/getCustomerMessages', { params: { customerId: ctrl.customerId } }).then(function (response) {
                ctrl.messages = response.data;
            });
        }

        ctrl.sendVkMessage = function() {
            $http.post('vk/sendVkMessage', { userId: ctrl.userId, message: ctrl.answerText }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', 'Сообщение отправлено');
                    ctrl.answerText = '';
                } else {
                    toaster.pop('error', '', 'Сообщение не отправлено');
                }
                 
                ctrl.getMessages();
            });
        }
    };

    VkMessagesCtrl.$inject = ['$http', 'toaster'];

    ng.module('vkMessages', [])
        .component('vkMessages', {
            templateUrl: '../areas/admin/content/src/_shared/vk-messages/vkMessages.html',
            controller: VkMessagesCtrl,
            bindings: {
                customerId: '<?',
                userId: '<?'
            }
        });

})(window.angular);