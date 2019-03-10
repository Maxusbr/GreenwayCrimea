; (function (ng) {
    'use strict';

    ng.module('defaultButton')
      .directive('defaultButton', [function () {
          return {
              restrict: 'A',
              link: function (scope, element, attrs) {
                  element[0].addEventListener('keyup', function (event) {

                      var btn;

                      //13 - enter
                      if (event.keyCode === 13) {
                          btn = document.querySelector(attrs.defaultButton);

                          if (btn != null) {
                              btn.click();
                          }
                      }
                  });
              }
          }
      }]);

})(window.angular);