; (function (ng) {
    'use strict';

    ng.module('personAvatar')
      .component('personAvatar', {
          templateUrl: '../areas/admin/content/src/_partials/person-avatar/templates/person-avatar.html',
          controller: 'PersonAvatarCtrl',
          transclude: true,
          bindings: {
              startValue: '@',
              noAvatarSrc: '@',
              customerId: '@'
          }
      });

})(window.angular);