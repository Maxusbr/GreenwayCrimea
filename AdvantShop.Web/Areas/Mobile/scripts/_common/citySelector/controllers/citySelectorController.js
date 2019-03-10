; (function (ng) {
    'use strict';

    var CitySelectorCtrl = function ($http, zoneService, sidebarService, $timeout, domService) {

        var ctrl = this,
            timer;

        ctrl.curZone = {};

        zoneService.getCurrentZone().then(function (data) {
            ctrl.curZone.city = data.City;
        });

        ctrl.getOtherCities = function (countryId) {
            zoneService.getCitiesForAutocomplete(ctrl.curZone.city).then(function (cities) {
                ctrl.zones = cities;
            });
        };

        ctrl.changeCity = function ($event) {
            $event.preventDefault();
            sidebarService.toggleSidebar();
            $timeout(function () {
                ng.element('#citySelecorList').trigger('click').focus();
            }, 100);
        };

        ctrl.input = function () {

            ctrl.getOtherCities();

            if (timer != null) {
                clearTimeout(timer);
            }

            timer = setTimeout(function () {
                ctrl.setCurentCity(ctrl.curZone.city);
            }, 300);
        };

        ctrl.setCurentCity = function (city) {
            zoneService.setCurrentZone(city);
        };

        ctrl.selectCity = function (city) {
            ctrl.curZone.city = city;
            sidebarService.toggleSidebar();
            ctrl.setCurentCity(ctrl.curZone.city);
        };
    };


    ng.module('citySelector')
      .controller('CitySelectorCtrl', CitySelectorCtrl);

    CitySelectorCtrl.$inject = ['$http', 'zoneService', 'sidebarService', '$timeout', 'domService'];

})(window.angular);