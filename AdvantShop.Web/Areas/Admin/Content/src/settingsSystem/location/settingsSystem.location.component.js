; (function (ng) {
    'use strict';

    ng.module('settingsSystem')
      .component('settingsLocation', {
          templateUrl: '../areas/admin/content/src/settingsSystem/location/settings-location.html',
          controller: 'SettingsSystemLocationCtrl'
      });

})(window.angular);