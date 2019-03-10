; (function (ng) {
    'use strict';

    ng.module('ngTextcomplete')
      .directive('textcomplete', ['Textcomplete', function (Textcomplete) {
          return {
              require: 'ngModel',
              scope: {
                  textcompleteData: '<',
                  textcompleteOptions: '<'
              },
              link: function (scope, element, attrs, ngModelCtrl) {
                  var textcomplete = new Textcomplete(element, [{
                      match: /(^|\s)#(\w*)$/,
                      search: function (term, callback) {
                          callback(scope.textcompleteData.filter(function (items) {
                              return items.toLowerCase().indexOf(term.toLowerCase()) === 0;
                          }));
                      },
                      replace: function (item) {
                          return ' ' + item;
                      }
                  }])
                      .on({
                          'textComplete:select': function (e, value) {
                              ngModelCtrl.$setViewValue(value);
                              scope.$digest();
                          },
                          'textComplete:show': function (e) {
                              textcomplete.data('autocompleting', true);
                          },
                          'textComplete:hide': function (e) {
                              textcomplete.data('autocompleting', false);
                          }
                      });
              }
          }
      }])

})(window.angular);