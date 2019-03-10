; (function (ng) {
    'use strict';

    ng.module('settingsSystem')
      .component('gridRegion', {
          templateUrl: '../areas/admin/content/src/settingsSystem/location/region.html',
          controller: 'SettingsSystemLocationRegionCtrl',
          bindings: {
              onGridInit: '&',
              onSelect: '&',
              gridParams: '<?'
          }
      });

})(window.angular);