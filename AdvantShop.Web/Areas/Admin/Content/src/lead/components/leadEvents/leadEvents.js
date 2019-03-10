; (function (ng) {
    'use strict';

    var LeadEventsCtrl = function ($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.getLeadEvents();

            if (ctrl.onInit != null) {
                ctrl.onInit({ leadEvents: ctrl });
            }
        }

        ctrl.toggleselectCurrencyLabel = function (val) {
            ctrl.selectCurrency = val;
        };

        ctrl.getLeadEvents = function () {
            $http.get('leadsExt/getLeadEvents', { params: { leadId: ctrl.leadId } }).then(function (response) {
                var data = response.data;
                ctrl.groups = data.EventGroups;
                ctrl.types = data.EventTypes;
                ctrl.type = ctrl.types[0];
            });
        }

        ctrl.addLeadEvent = function () {
            $http.post('leadsExt/addLeadEvent', { leadId: ctrl.leadId, type: ctrl.type.value, message: ctrl.message }).then(function (response) {
                var data = response.data;
                if (data.result) {
                    toaster.pop('success', '', 'Событие добавлено');
                    ctrl.message = '';
                } else {
                    toaster.pop('error', '', 'Событие не добавлено');
                }
                ctrl.getLeadEvents();
            });
        }

        ctrl.addEventKeydown = function (e) {
            if (e.keyCode === 13) {
                ctrl.addLeadEvent();
                e.preventDefault();
            }
        };

        ctrl.changeType = function(type) {
            ctrl.type = ctrl.types.filter(function(x) { return x.value === type })[0];
        }

        ctrl.getIcon = function (type) {
            if (type == null) return '';

            switch(type.toLocaleLowerCase()) {
                case 'none':
                    return 'flag';
                case 'comment':
                    return 'comment';
                case 'call':
                    return 'phone';
                case 'sms':
                    return 'comments';
                case 'email':
                    return 'envelope';
                case 'task':
                    return 'calendar-check-o';
                case 'other':
                    return 'list';
                case 'vk':
                    return 'vk';
            }
        }

        ctrl.setComment = function (event, result) {
            event.CallComent = event.CallComent || {};
            event.CallComent.Id = result.id;
            event.CallComent.Text = result.text;
            event.CallComent.ObjId = result.objId;
        }

        ctrl.sendVkAnswer = function(event, index) {
            $http.post('vk/sendVkMessage', { userId: event.VkData.UserId, message: event.vkAnswerText }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', '', 'Сообщение отправлено');
                } else {
                    toaster.pop('error', '', 'Сообщение не отправлено');
                }
                event.showVkAnswer[index] = false;
                ctrl.getLeadEvents();
            });
        }
    };

    LeadEventsCtrl.$inject = ['$http', 'toaster'];

    ng.module('leadEvents', ['callRecord'])
        .controller('LeadEventsCtrl', LeadEventsCtrl)
        .component('leadEvents', {
            templateUrl: '../areas/admin/content/src/lead/components/leadEvents/leadEvents.html',
            controller: LeadEventsCtrl,
            bindings: {
                leadId: '=',
                customerId: '@',
                onInit: '&',
            }
        });

})(window.angular);