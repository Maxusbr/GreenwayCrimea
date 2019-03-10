; (function (ng) {

    'use strict';

    var MyAccountCtrl = function ($http, $window) {

        var ctrl = this;

        ctrl.orderHistoryMode = 'all';
        ctrl.commonInfo = {};

        ctrl.changeTempEmail = function(email) {
            $http.post("myaccount/updatecustomeremail", { email: email }).then(function (response) {

                if (response.data === true) {
                    ctrl.modalWrongNewEmail = false;
                    $window.location.reload(true);
                } else {
                    ctrl.modalWrongNewEmail = true;
                }
            });
        }
    };

    ng.module('myaccount')
      .controller('MyAccountCtrl', MyAccountCtrl);

    MyAccountCtrl.$inject = ['$http', '$window'];

})(window.angular);

