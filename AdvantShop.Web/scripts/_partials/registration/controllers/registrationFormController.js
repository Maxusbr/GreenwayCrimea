; (function (ng) {
    'use strict';

    var RegistrationFormCtrl = function ($scope) {

        $scope.createAccount = function () {
            alert('RegistrationFormCtrl');
        };
    };

    ng.module('registration')
      .controller('RegistrationFormCtrl', RegistrationFormCtrl);

    RegistrationFormCtrl.$inject = ['$scope'];


})(window.angular);