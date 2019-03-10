; (function (ng) {
    'use strict';

    ng.module('settingsSystem')
      .component('gridCountry', {
          templateUrl: '../areas/admin/content/src/settingsSystem/location/country.html',
          controller: 'SettingsSystemLocationCountryCtrl',
          bindings: {
              onGridInit: '&',
              onSelect: '&',
              gridParams: '<?'
          }
      });

})(window.angular);