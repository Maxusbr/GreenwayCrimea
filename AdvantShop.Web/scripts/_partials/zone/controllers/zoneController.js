; (function (ng) {
    'use strict';

    var ZoneCtrl = function (zoneService) {
        var ctrl = this;

        ctrl.zoneCity = "";

        zoneService.getDataForPopup().then(function (data) {
            ctrl.data = data;
            ctrl.countrySelected = ctrl.data[0];

            for (var i = ctrl.data.length - 1 ; i >= 0; i--){
                ctrl.data[i].Columns = zoneService.sliceCitiesForDialog(ctrl.data[i].Cities);
            }

        });

        ctrl.changeCity =  function (city, obj, countryId) {
            if (!city.length)
                return;
            zoneService.setCurrentZone(city, obj, countryId);
            zoneService.zoneDialogClose();
            ctrl.zoneCity = "";
        };

        ctrl.keyup = function ($event, val) {
            $event.stopPropagation();
            var keyCode = $event.keyCode;

            switch (keyCode) {
                case 13: //enter
                    ctrl.changeCity(ctrl.zoneCity, null, ctrl.countrySelected.CountryId);
                    break;
            }
        };

    };

    ng.module('zone')
      .controller('ZoneCtrl', ZoneCtrl);
    
    ZoneCtrl.$inject = ['zoneService'];

})(window.angular);