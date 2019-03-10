; (function (ng) {
    'use strict';

    ng.module('settingsSystem')
      .component('gridCity', {
          templateUrl: '../areas/admin/content/src/settingsSystem/location/city.html',
          controller: 'SettingsSystemLocationCityCtrl',
          bindings: {
              onGridInit: '&',
              onSelect: '&',
              gridParams: '<?',
              onGridPreinit: '&'
          }
      });

})(window.angular);