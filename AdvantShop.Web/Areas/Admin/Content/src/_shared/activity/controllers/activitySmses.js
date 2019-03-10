; (function (ng) {
    'use strict';

    var ActivitySmsesCtrl = function ($http, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.$onInit = function () {

            if (ctrl.standardPhone == null || ctrl.standardPhone == '') {
                return;
            }

            $http.get('activity/getSmses', { params: { customerId: ctrl.customerId, standardPhone: ctrl.standardPhone } }).then(function (response) {
                ctrl.items = response.data.DataItems;
            });
        };
    };

    ActivitySmsesCtrl.$inject = ['$http', 'uiGridCustomConfig'];

    ng.module('activity')
        .component('activitySmses', {
            templateUrl: '../areas/admin/content/src/_shared/activity/templates/activity-smses.html',
            controller: ActivitySmsesCtrl,
            bindings: {
                customerId: '<?',
                standardPhone: '<?',
            }
        });

})(window.angular);