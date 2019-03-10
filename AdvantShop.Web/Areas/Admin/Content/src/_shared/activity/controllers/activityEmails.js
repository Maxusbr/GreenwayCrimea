; (function (ng) {
    'use strict';

    var ActivityEmailsCtrl = function ($http) {
        var ctrl = this;

        ctrl.$onInit = function () {

            if (ctrl.email == null || ctrl.email == '') {
                return;
            }

            $http.get('activity/getEmails', { params: { customerId: ctrl.customerId, email: ctrl.email } }).then(function(response) {
                ctrl.items = response.data.DataItems;
            });
        };
    };

    ActivityEmailsCtrl.$inject = ['$http'];

    ng.module('activity')
        .component('activityEmails', {
            templateUrl: '../areas/admin/content/src/_shared/activity/templates/activity-emails.html',
            controller: ActivityEmailsCtrl,
            bindings: {
                customerId: '<?',
                email: '<?',
            }
        });

})(window.angular);