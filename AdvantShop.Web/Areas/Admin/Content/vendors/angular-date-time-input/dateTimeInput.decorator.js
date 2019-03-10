; (function (ng) {
    'use strict';

    ng.module('ui.dateTimeInput')
      .decorator('dateTimeInputDirective', ['$delegate', function ($delegate) {
          var directive = $delegate[0],
              originalCompileFunc = directive.compile || ng.noop;


          directive.compile = function (element, attrs) {
              if (attrs.modelType == null) {
                  attrs.$set('modelType', 'YYYY-MM-DDTHH:mm:ss');
              }

              return directive.link;
          };

          return $delegate;
      }]);

})(window.angular);