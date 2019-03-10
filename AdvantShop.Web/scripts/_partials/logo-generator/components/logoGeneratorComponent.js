; (function (ng) {
    'use strict';

    ng.module('logoGenerator')
      .directive('logoGeneratorStart', ['$compile', function ($compile) {
          return function(scope, element, attrs, ctrl){
              var elms = document.querySelectorAll('logo-generator-trigger');

              if (elms != null && elms.length > 0) {
                  $compile(elms)(scope);
              }
          }
      }]);

    ng.module('logoGenerator')
      .component('logoGenerator', {
          controller: 'LogoGeneratorCtrl',
          templateUrl: '/scripts/_partials/logo-generator/templates/logo-generator.html',
          bindings: {
              logoGeneratorId: '@'
          }
      });

    ng.module('logoGenerator')
      .component('logoGeneratorFonts', {
          controller: 'LogoGeneratorFontsCtrl',
          templateUrl: '/scripts/_partials/logo-generator/templates/logo-generator-fonts.html',
          bindings: {
              fontsList: '<',
              logo: '<',
              slogan: '<?',
              isUseSlogan: '<?',
              language: '<?',
              objType: '@',
              onSelect: '&'
          }
      });

    ng.module('logoGenerator')
      .component('logoGeneratorPreview', {
          controller: 'LogoGeneratorPreviewCtrl',
          transclude: true,
          templateUrl: '/scripts/_partials/logo-generator/templates/logo-generator-preview.html',
          bindings: {
              logoGeneratorId: '@',
              editOnPageLoad: '<?'
          }
      });

    ng.module('logoGenerator')
      .directive('logoGeneratorPreviewImg', function () {
          return {
              scope: true,
              require: '^logoGeneratorPreview',
              link: function (scope, element, attrs, ctrl) {
                  ctrl.addImg(element[0]);
              }
          }
      });

    ng.module('logoGenerator')
      .directive('logoGeneratorPreviewLogo', function () {
          return {
              scope: true,
              require: {
                  logoGeneratorPreviewCtrl : '^logoGeneratorPreview'
              },
              controller: ['$element', function ($element) {
                  var ctrl = this;

                  ctrl.$onInit = function () {
                      ctrl.logoGeneratorPreviewCtrl.addLogo($element[0]);
                  };
              }],
              bindToController: true,
              controllerAs: 'logoGeneratorPreviewLogo'
          }
      });

    ng.module('logoGenerator')
      .directive('logoGeneratorPreviewSlogan', function () {
          return {
              scope: true,
              require: {
                  logoGeneratorPreviewCtrl: '^logoGeneratorPreview'
              },
              controller: ['$element', function ($element) {
                  var ctrl = this;

                  ctrl.$onInit = function () {
                      ctrl.logoGeneratorPreviewCtrl.addSlogan($element[0]);
                  };
              }],
              bindToController: true,
              controllerAs: 'logoGeneratorPreviewSlogan'
          }
      });

    ng.module('logoGenerator')
      .component('logoGeneratorTrigger', {
          controller: 'LogoGeneratorTriggerCtrl',
          bindings: {
              logoGeneratorId: '@',
          }
      });

})(window.angular);