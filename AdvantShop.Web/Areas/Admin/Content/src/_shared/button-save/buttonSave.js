; (function (ng) {
    'use strict';

    ng.module('buttonSave', [])
      .directive('buttonSave', ['domService', function (domService) {
          return {
              link: function (scope, element, attrs, ctrl) {
                  //element[0].addEventListener('click', function () {
                  //    var form = domService.closest(element[0], 'form');

                  //    if (form != null) {
                  //        form.submit();
                  //    }
                  //});
              }
          }
      }]);

})(window.angular);