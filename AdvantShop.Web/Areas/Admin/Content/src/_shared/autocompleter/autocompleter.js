; (function (ng) {
    'use strict';

    var AutocompleterCustomCtrl = function ($http, autocompleterUrls) {
        var ctrl = this;

        ctrl.find = function (viewValue) {

            if (viewValue == null || viewValue.length < ctrl.minLengthFind)
                return;

            var url = autocompleterUrls[ctrl.onType];

            if (url == null) {
                throw Error('Not find url by onType for autocompleter');
            }

            return $http.get(url, { params: { q: viewValue } }).then(function (response) {
                return response.data;
            });
        }
    };

    AutocompleterCustomCtrl.$inject = ['$http', 'autocompleterUrls'];


    ng.module('autocompleter', [])
      .constant('autocompleterUrls', {
          'country': 'countries/getCountriesAutocomplete',
          'region': 'regions/getRegionsAutocomplete',
          'city': 'cities/getCitiesAutocomplete'
      })
      .controller('AutocompleterCustomCtrl', AutocompleterCustomCtrl)
      .directive('autocompleter', function () {
          return {
              require: ['autocompleter', 'ngModel'],
              template: '<input uib-typeahead="item for item in autocompleter.find(autocompleter.ngModel.$viewValue)" typeahead-focus-first="false">',
              replace: true,
              controller: 'AutocompleterCustomCtrl',
              controllerAs: 'autocompleter',
              bindToController: true,
              //scope: {
              //    onType: '@',
              //    minLengthFind: '<?'
              //},
              scope: true,
              link: function (scope, element, attrs, ctrls) {
                  var autocompleterCtrl = ctrls[0],
                      ngModelCtrl = ctrls[1];

                  autocompleterCtrl.minLengthFind = 1;
                  autocompleterCtrl.onType = attrs.onType;
                  autocompleterCtrl.ngModel = ngModelCtrl;
              }
          }
      });

})(window.angular);