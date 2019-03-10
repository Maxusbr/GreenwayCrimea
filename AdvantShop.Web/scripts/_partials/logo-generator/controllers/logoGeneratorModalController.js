; (function (ng) {
    'use strict';

    var LogoGeneratorModalCtrl = function (logoGeneratorService, $timeout) {
        var ctrl = this;

        ctrl.save = function (logoGeneratorId) {

            ctrl.savingLogo = true;

            logoGeneratorService.getLogoGeneratorPreview(logoGeneratorId)
                                .then(function (logoGeneratorPreview) {
                                    return html2canvas(logoGeneratorPreview.element, {
                                        async: false,
                                        allowTaint: true,
                                        //foreignObjectRendering: true,
                                        backgroundColor: null,
                                    }).then(function (canvas) {
                                        return {
                                            logoGeneratorPreview: logoGeneratorPreview,
                                            canvas: canvas
                                        }
                                    });
                                })
                                .then(function (data) {
                                    return logoGeneratorService.saveLogo(data.canvas.toDataURL('image/png'), '.png', {
                                        logo: {
                                            style: data.logoGeneratorPreview.logoGenerator.logo.style,
                                            text: data.logoGeneratorPreview.logoGenerator.logo.text,
                                            font: data.logoGeneratorPreview.logoGenerator.logo.font
                                        },
                                        slogan: {
                                            style: data.logoGeneratorPreview.logoGenerator.slogan.style,
                                            text: data.logoGeneratorPreview.logoGenerator.slogan.text,
                                            font: data.logoGeneratorPreview.logoGenerator.slogan.font,
                                            marginValue: data.logoGeneratorPreview.logoGenerator.slogan.marginValue
                                        },
                                        isUseSlogan: data.logoGeneratorPreview.logoGenerator.isUseSlogan
                                    })
                                })
                .then(function (data) {
                    ctrl.close(logoGeneratorId);
                    $timeout(function () {
                        logoGeneratorService.updateLogoSrc(logoGeneratorId, data.ImgSource);
                    }, 0);
                })
                .catch(function (error) {
                    console.error('Error on generate logo: ' + error);
                })
                .finally(function () {
                    ctrl.savingLogo = false;
                });;
        };

        ctrl.close = function (logoGeneratorId) {
            logoGeneratorService.closeModal(logoGeneratorId);
        }

        ctrl.callbackClose = function (logoGeneratorId) {
            logoGeneratorService.setActivity(logoGeneratorId, false);
        }
    };

    LogoGeneratorModalCtrl.$inject = ['logoGeneratorService', '$timeout'];

    ng.module('logoGenerator')
      .controller('LogoGeneratorModalCtrl', LogoGeneratorModalCtrl);

})(window.angular);