; (function (ng) {
    'use strict';

    ng.module('adminWebNotifications')
      .directive('callNotification', [function () {
          return {
              templateUrl: '../areas/admin/content/src/_shared/admin-web-notifications/templates/call-notification.html'
          }
      }]);

})(window.angular);