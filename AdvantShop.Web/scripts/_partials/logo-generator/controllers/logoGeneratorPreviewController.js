; (function (ng) {
    'use strict';

    var LogoGeneratorPreviewCtrl = function ($element, $q, logoGeneratorService) {
        var ctrl = this,
            deferLogo,
            deferSlogan;

        ctrl.$onInit = function () {

            ctrl.element = $element[0];

            logoGeneratorService.addLogoGeneratorPreview(ctrl.logoGeneratorId, ctrl);
            if (ctrl.editOnPageLoad === true) {
                logoGeneratorService.showModal(ctrl.logoGeneratorId);
            }

            logoGeneratorService.getLogoGenerator(ctrl.logoGeneratorId)
                .then(function (logoGenerator) {
                    ctrl.logoGenerator = logoGenerator;
                });
        };

        ctrl.addImg = function (img) {
            return ctrl.img = img;
        };

        ctrl.addLogo = function (logo) {
            ctrl.logo = logo;

            if(deferLogo != null){
                deferLogo.resolve(logo);
            }

            return logo;
        };

        ctrl.getLogo = function () {

            if (ctrl.logo == null) {
                deferLogo = $q.defer();
            } else {
                deferLogo.resolve(ctrl.logo);
            }

            return deferLogo.promise;
        };

        ctrl.addSlogan = function (slogan) {
            ctrl.slogan = slogan;

            if (deferSlogan != null) {
                deferSlogan.resolve(slogan);
            }

            return slogan;
        };

        ctrl.getSlogan = function () {

            if (ctrl.slogan == null) {
                deferSlogan = $q.defer();
            } else {
                deferSlogan.resolve(ctrl.logo);
            }

            return deferSlogan.promise;
        };
    };

    LogoGeneratorPreviewCtrl.$inject = ['$element', '$q', 'logoGeneratorService'];

    ng.module('logoGenerator')
      .controller('LogoGeneratorPreviewCtrl', LogoGeneratorPreviewCtrl);

})(window.angular);