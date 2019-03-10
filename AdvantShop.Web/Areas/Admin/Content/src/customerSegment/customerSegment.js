; (function (ng) {
    'use strict';

    var CustomerSegmentCtrl = function ($http, $window, SweetAlert, uiGridCustomConfig) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Покупатель',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><a ng-href="customers/edit/{{row.entity.CustomerId}}">{{COL_FIELD}}</a></div>',
                },
                {
                    name: 'Phone',
                    displayName: 'Телефон',
                },
                {
                    name: 'Email',
                    displayName: 'Email',
                },
                {
                    name: 'OrdersCount',
                    displayName: 'Кол-во оплаченных заказов',
                    width: 150,
                    type: 'number',
                },
                {
                    name: 'RegistrationDateTimeFormatted',
                    displayName: 'Дата регистрации',
                    width: 150,
                }
            ];
        
        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'customers/edit/{{row.entity.CustomerId}}'
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };


        ctrl.deleteSegment = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('customerSegments/deleteSegment', { id: id }).then(function (response) {
                        $window.location.assign('customersegments');
                    });
                }
            });
        }

        ctrl.initCategories = function (selectedCategories) {
            ctrl.selectedCategories = selectedCategories === null || selectedCategories == '' ? null : JSON.parse(selectedCategories);
            ctrl.getCategories();
        }

        ctrl.getCategories = function () {
            $http.get('customerSegments/getCategories').then(function (response) {
                ctrl.Categories = response.data.categories;
            });
        }

        ctrl.initCities = function (selectedCities) {
            ctrl.selectedCities = selectedCities === null || selectedCities == '' ? null : JSON.parse(selectedCities);
            ctrl.getCities();
        }

        ctrl.getCities = function () {
            $http.get('customerSegments/getCities').then(function (response) {
                ctrl.Cities = response.data.cities;
            });
        }

        ctrl.initCountries = function (selectedCountries) {
            ctrl.selectedCountries = selectedCountries === null || selectedCountries == '' ? null : JSON.parse(selectedCountries);
            ctrl.getCountries();
        }

        ctrl.getCountries = function () {
            $http.get('customerSegments/getCountries').then(function (response) {
                ctrl.Countries = response.data.countries;
            });
        }

        ctrl.export = function() {
            ctrl.grid.export();
        }

    };

    CustomerSegmentCtrl.$inject = ['$http', '$window', 'SweetAlert', 'uiGridCustomConfig'];

    ng.module('customerSegment', ['uiGridCustom'])
      .controller('CustomerSegmentCtrl', CustomerSegmentCtrl);

})(window.angular);