; (function (ng) {
    'use strict';

    ng.module('registration')
      .directive('registrationForm', function () {
          return {
              restrict: 'A',
              scope: {},
              controller: 'RegistrationFormCtrl',
              templateUrl: '/scripts/_partials/registration/templates/registrationForm.html',
              replace: true,
          };
      });

})(window.angular);