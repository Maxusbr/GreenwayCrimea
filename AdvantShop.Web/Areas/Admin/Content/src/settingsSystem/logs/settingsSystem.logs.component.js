; (function (ng) {
    'use strict';

    ng.module('settingsSystem')
      .component('settingsLogs', {
          templateUrl: '../areas/admin/content/src/settingsSystem/logs/settings-logs.html',
          controller: 'SettingsSystemLogsCtrl'
      });

})(window.angular);