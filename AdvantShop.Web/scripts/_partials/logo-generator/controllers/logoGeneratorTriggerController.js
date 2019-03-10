; (function (ng) {
    'use strict';

    var LogoGeneratorTriggerCtrl = function ($element, logoGeneratorService) {
        var ctrl = this;

        ctrl.$postLink = function () {
            $element[0].addEventListener('click', function () {
                ctrl.showModal(ctrl.logoGeneratorId);
            });
        };

        ctrl.showModal = function (logoGeneratorId) {
            logoGeneratorService.showModal(logoGeneratorId);
        };
    };

    LogoGeneratorTriggerCtrl.$inject = ['$element', 'logoGeneratorService'];

    ng.module('logoGenerator')
      .controller('LogoGeneratorTriggerCtrl', LogoGeneratorTriggerCtrl);

})(window.angular);