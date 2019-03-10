; (function (ng) {
    'use strict';

    var CallRecordCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {
        }

        ctrl.getRecordLink = function () {
            var timer = setTimeout(function () {
                ctrl.loading = true;
            }, 200);
            return $http.post('calls/getRecordLink', { callId: ctrl.callId, type: ctrl.operatorType }).then(function (response) {
                clearTimeout(timer);
                ctrl.loading = false;
                return response.data != null ? response.data.link : '';
            });
        }
    };

    CallRecordCtrl.$inject = ['$http'];

    ng.module('callRecord', [])
        .controller('CallRecordCtrl', CallRecordCtrl)
        .component('callRecord', {
            templateUrl: '../areas/admin/content/src/calls/components/callRecord/callRecord.html',
            controller: CallRecordCtrl,
            bindings: {
                callId: '<',
                operatorType: '<',
            }

        });

})(window.angular);