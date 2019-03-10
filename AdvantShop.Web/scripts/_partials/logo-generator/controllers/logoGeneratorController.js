; (function (ng) {
    'use strict';

    var LogoGeneratorCtrl = function ($q, $window, logoGeneratorService, modalService) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.colorPickerOptions = {
                swatchBootstrap: false,
                format: 'hex',
                alpha: false,
                'case': 'lower',
                allowEmpty: false,
                required: true,
                inputClass: 'input input-small',
                pos: 'top left'
            };

            ctrl.loadData();

            //ctrl.colorPickerLogoBgValue = 'ffffff';

            //logoGeneratorService.getFontsList().then(function (fontsData) {
            //    ctrl.logo.style.fontFamily = ctrl.fontsList[0].fontFamily;
            //    ctrl.slogan.style.fontFamily = ctrl.fontsList[0].fontFamily;
            //});

            ctrl.colorPickerLogoEventApi = {};
            //ctrl.colorPickerLogoBgEventApi = {};
            ctrl.colorPickerSloganEventApi = {};

            ctrl.colorPickerLogoEventApi.onBlur = ctrl.colorPickerLogoEventApi.onChange = function (api, value) {
                if (value != null && value.length === 6) {
                    ctrl.logo.style.color = '#' + value;
                }
            };

            ctrl.colorPickerSloganEventApi.onBlur = ctrl.colorPickerSloganEventApi.onChange = function (api, value) {
                if (value != null && value.length === 6) {
                    ctrl.slogan.style.color = '#' + value;
                }
            };

            logoGeneratorService.addLogoGenerator(ctrl.logoGeneratorId, ctrl);
        };

        ctrl.loadData = function () {
            return logoGeneratorService.getData()
                .then(function (data) {

                    ctrl.logo = data.logo;
                    ctrl.slogan = data.slogan;
                    ctrl.isUseSlogan = data.isUseSlogan;
                    
                    ctrl.logo.style.color = ctrl.slogan.style.color = (data.logo.style.color.charAt(0) !== '#' ? '#': '') + data.logo.style.color;
                    ctrl.colorPickerLogoValue = ctrl.colorPickerSloganValue = data.logo.style.color.replace('#', '');

                    ctrl.isLoaded = true;
                });
        }

        ctrl.onSelectLogoFont = function (font) {
            ctrl.logo.font = font;
            ctrl.logo.style.fontFamily = logoGeneratorService.fontFamilyEscape(font.fontFamily);

            modalService.close('modalLogoGeneratorLogoFonts');
        };

        ctrl.onSelectSlogonFont = function (font) {
            ctrl.slogan.font = font;
            ctrl.slogan.style.fontFamily = logoGeneratorService.fontFamilyEscape(font.fontFamily);

            modalService.close('modalLogoGeneratorSloganFonts');
        };

        ctrl.setSloganMargin = function (value) {
            ctrl.slogan.style[ctrl.slogan.style.verticalAlign === 'top' ? 'marginBottom' : 'marginTop'] = value;
        };

        ctrl.showFontsModal = function (modalId, objType) {
            $q.when(ctrl.fonts != null && ctrl.fonts.length > 0 ? ctrl.fonts : logoGeneratorService.getFontsList())
                .then(function (fontsData) {
                    ctrl.fonts = fontsData;

                    if (objType === 'logo') {
                        ctrl.logoLanguage = logoGeneratorService.parseLanguage(ctrl.logo.text) || '';
                    } else if (objType === 'slogan') {
                        ctrl.sloganLanguage = logoGeneratorService.parseLanguage(ctrl.slogan.text) || '';
                    }

                    modalService.open(modalId, true);
                });
        };
    };

    LogoGeneratorCtrl.$inject = ['$q', '$window', 'logoGeneratorService', 'modalService'];

    ng.module('logoGenerator')
      .controller('LogoGeneratorCtrl', LogoGeneratorCtrl);

})(window.angular);