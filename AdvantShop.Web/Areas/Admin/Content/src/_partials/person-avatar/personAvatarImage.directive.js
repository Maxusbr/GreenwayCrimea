; (function (ng) {
    'use strict';

    ng.module('personAvatar')
      .directive('personAvatarImage', function () {
          return {
              require: {
                  personAvatarCtrl: '^personAvatar'
              },
              bindToController: true,
              controller: function () { },
              link: function (scope, element, attrs, ctrl) {
                  ctrl.personAvatarCtrl.addImgElement(element[0]);
              }
          }
      });

})(window.angular);