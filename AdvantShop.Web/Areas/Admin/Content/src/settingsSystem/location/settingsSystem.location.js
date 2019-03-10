; (function (ng) {
    'use strict';


    var SettingsSystemLocationCtrl = function ($location, $q, $http) {

        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.enumLocation = {
                country: 'country',
                region: 'region',
                city: 'city'
            };

            ctrl.shower = {};
            ctrl.shower[ctrl.enumLocation.country] = ctrl.showCountry;
            ctrl.shower[ctrl.enumLocation.region] = ctrl.showGridRegion;
            ctrl.shower[ctrl.enumLocation.city] = ctrl.showGridCity;

            ctrl.locationGrids = {};

            ctrl.locationGridsParams = {};
            ctrl.locationGridsParams[ctrl.enumLocation.country] = {};
            ctrl.locationGridsParams[ctrl.enumLocation.region] = {};
            ctrl.locationGridsParams[ctrl.enumLocation.city] = {};

            ctrl.lastParamsData = {};

            var urlSearch = $location.search();

            if (urlSearch != null && urlSearch.locationType != null && ctrl.enumLocation[urlSearch.locationType] != null) {
                ctrl.setLocationType(urlSearch.locationType, true);
            }
        }

        ctrl.showCountry = function () {
            ctrl.locationGridsParams[ctrl.enumLocation.country].search = null;
        };

        ctrl.selectCountry = function (id, name) {

            ctrl.lastParamsData[ctrl.enumLocation.region] = {
                id: id,
                name: name
            };

            ctrl.showGridRegion(id, name);
        };

        ctrl.showGridRegion = function (id, name) {

            ctrl.setLocationType(ctrl.enumLocation.region);

            ctrl.locationGridsParams[ctrl.enumLocation.region].id = id || ctrl.countryIdFromUrl;
            ctrl.locationGridsParams[ctrl.enumLocation.region].countryName = name || ctrl.countryNameFromUrl;
            ctrl.locationGridsParams[ctrl.enumLocation.region].search = null;
        };

        ctrl.selectRegion = function (id, name) {

            ctrl.lastParamsData[ctrl.enumLocation.city] = {
                id: id,
                name: name
            };

            ctrl.showGridCity(id, name);
        };

        ctrl.showGridCity = function (id, name) {

            ctrl.setLocationType(ctrl.enumLocation.city);

            ctrl.locationGridsParams[ctrl.enumLocation.city].id = id;
            ctrl.locationGridsParams[ctrl.enumLocation.city].regionName = name;
            ctrl.locationGridsParams[ctrl.enumLocation.city].search = null;
            ctrl.locationGridsParams[ctrl.enumLocation.city].cityCountrys = null;
        };

        ctrl.revertSubject = function (type) {

            ctrl.setLocationType(type);

            var lastParams = ctrl.lastParamsData[type],
                countryId;

            if (type === ctrl.enumLocation.region && (lastParams == null || lastParams.id == null || lastParams.id.length === 0) && ctrl.locationGridsParams[ctrl.enumLocation.city].cityCountrys) {
                countryId = ctrl.locationGridsParams[ctrl.enumLocation.city].cityCountrys;
            }

            ctrl.shower[type](lastParams != null ? lastParams.id : countryId, lastParams != null ? lastParams.name : null);
        }

        ctrl.viewAllCity = function () {

            ctrl.setLocationType(ctrl.enumLocation.city);

            ctrl.shower[ctrl.enumLocation.city](null, null);

            ctrl.locationGridsParams[ctrl.enumLocation.city].cityCountrys = ctrl.lastParamsData[ctrl.enumLocation.region] != null ? ctrl.lastParamsData[ctrl.enumLocation.region].id : ctrl.countryIdFromUrl;
        };

        ctrl.setLocationType = function (type, skipChangeUrl) {
            ctrl.locationTypeSelected = type;

            if (!skipChangeUrl) {
                $location.search('locationType', type);
            }

            //очищаем адресную строку от параметров
            type !== ctrl.enumLocation.country && ctrl.locationGrids[ctrl.enumLocation.country] && $location.search(ctrl.locationGrids[ctrl.enumLocation.country].gridUniqueId, null);
            type !== ctrl.enumLocation.region && ctrl.locationGrids[ctrl.enumLocation.region] && $location.search(ctrl.locationGrids[ctrl.enumLocation.region].gridUniqueId, null);
            type !== ctrl.enumLocation.city && ctrl.locationGrids[ctrl.enumLocation.city] && $location.search(ctrl.locationGrids[ctrl.enumLocation.city].gridUniqueId, null);
        }

        ctrl.gridCountryOnInit = function (grid) {
            ctrl.locationGrids[ctrl.enumLocation.country] = grid;
            ctrl.locationGridsParams[ctrl.enumLocation.country] = grid._params;
        };

        ctrl.gridRegionOnInit = function (grid) {
            ctrl.locationGrids[ctrl.enumLocation.region] = grid;
            ctrl.locationGridsParams[ctrl.enumLocation.region] = grid._params;
            grid.setParams();
        };

        ctrl.gridCityOnPreinit = function (grid) {

            var defer = $q.defer();

            ctrl.locationGrids[ctrl.enumLocation.city] = grid;
            ctrl.locationGridsParams[ctrl.enumLocation.city] = grid._params;

            if (ctrl.locationGridsParams[ctrl.enumLocation.city].regionName == null && ctrl.locationGridsParams[ctrl.enumLocation.city].cityCountrys != null) {
                $http.get('countries/getcountryitem', { params: { countryId: ctrl.locationGridsParams[ctrl.enumLocation.city].cityCountrys } }).then(function (response) {
                    var data = response.data;

                    if (data.result === true) {
                        ctrl.countryIdFromUrl = data.obj.CountryId;
                        ctrl.countryNameFromUrl = data.obj.Name;
                    }

                    grid.setParams();

                    defer.resolve();
                });
            } else {
                grid.setParams();
                defer.resolve();
            }

            return defer.promise;
        }
    }

    SettingsSystemLocationCtrl.$inject = ['$location', '$q', '$http'];

    ng.module('settingsSystem')
      .controller('SettingsSystemLocationCtrl', SettingsSystemLocationCtrl);

})(window.angular);