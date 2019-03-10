; (function (ng) {

    'use strict';

    var logoGeneratorService = function ($controller, $http, $q, $window, modalService) {
        var service = this,
            localStorageKey = 'logoGenerator',
            storage = {},
            listWait = {},
            emptySymbol = '🞎',
            regexLatin = new RegExp('[a-zA-Z]', 'g'),
            regexCyrilic = new RegExp('[а-яА-Я]', 'g');

        service.addLogoGenerator = function (logoGeneratorId, logoGenerator) {

            if (logoGeneratorId == null || logoGeneratorId.length === 0) {
                throw Error('logoGeneratorId is required parameter');
            }

            storage[logoGeneratorId] = storage[logoGeneratorId] || {};

            storage[logoGeneratorId].logoGenerator = logoGenerator;

            if (listWait[logoGeneratorId] != null) {
                listWait[logoGeneratorId].forEach(function (item) {
                    item.resolve(storage[logoGeneratorId].logoGenerator);
                });
            }

            return storage[logoGeneratorId].logoGenerator;
        };

        service.getLogoGenerator = function (logoGeneratorId) {
            var defer = $q.defer();

            if (storage[logoGeneratorId] != null && storage[logoGeneratorId].logoGenerator != null) {
                defer.resolve(storage[logoGeneratorId].logoGenerator);
            } else {
                listWait[logoGeneratorId] = listWait[logoGeneratorId] || [];

                listWait[logoGeneratorId].push(defer);
            }

            return defer.promise;
        };

        service.addLogoGeneratorPreview = function (logoGeneratorId, logoGeneratorPreview) {
            storage[logoGeneratorId] = storage[logoGeneratorId] || {};

            storage[logoGeneratorId].preview = logoGeneratorPreview;

            return storage[logoGeneratorId].preview;
        };

        service.getLogoGeneratorPreview = function (logoGeneratorId) {
            return $q.when(storage[logoGeneratorId].preview);
        };

        service.showModal = function (logoGeneratorId) {

            var modalId = logoGeneratorId + 'Modal';

            storage[logoGeneratorId] = storage[logoGeneratorId] || {};

            if (modalService.hasModal(modalId)) {
                modalService.open(modalId);
            } else {
                modalService.renderModal(modalId,
                                         'Генерация логотипа',
                                         '<logo-generator data-logo-generator-id="logoMain"></logo-generator>',
                                         '<div><button type="button" data-ladda="logoGeneratorModal.savingLogo" class="btn btn-small btn-submit" data-ng-click="logoGeneratorModal.save(\'' + logoGeneratorId + '\')">Сохранить</button> <button type="button" data-modal-close="' + modalId + '" class="btn btn-small btn-action">Закрыть</button></div>',
                                         { backgroundEnable: false, isFloating: true, modalOverlayClass: 'logo-generator-modal', isOpen: true, callbackClose: 'logoGeneratorModal.callbackClose(\'' + logoGeneratorId + '\')' },
                                         { logoGeneratorModal: $controller('LogoGeneratorModalCtrl') });
            }

            service.setActivity(logoGeneratorId, true);
        };

        service.closeModal = function (logoGeneratorId) {
            var modalId = logoGeneratorId + 'Modal';

            if (modalService.hasModal(modalId)) {
                modalService.close(modalId);
                service.setActivity(logoGeneratorId, false);
            }
        }

        service.getFontsList = function () {
            return $http.get('/scripts/_partials/logo-generator/fonts/data.json', { params: { rnd: Math.random() } })
                        .then(function (response) {
                            return response.data;
                        });
        };

        service.saveLogo = function (dataUrl, fileExtension, options) {

            return $http.post('logogenerator/savelogo', { dataUrl: dataUrl, fileExtension: fileExtension, fontFamilyLogo: options.logo.font.fontFamily })
                        .then(function (response) {
                            return response.data;
                        })
                        .then(function (data) {

                            service.saveData(options);

                            return data;
                        })
        };

        service.saveData = function (data) {
            $window.localStorage.setItem(localStorageKey, JSON.stringify(data));
        };

        service.getDataFromStorage = function () {
            var valueString = $window.localStorage.getItem(localStorageKey);

            return valueString != null && valueString.length > 0 ? JSON.parse(valueString) : null;
        };

        service.getData = function () {
            return $q.when(service.getDataFromStorage() || $http.get('logogenerator/getdata')
                        .then(function (response) {
                            return response.data;
                        }));
        };

        service.setActivity = function (logoGeneratorId, isActive) {
            service.getLogoGenerator(logoGeneratorId)
                    .then(function (logoGenerator) {
                        logoGenerator.isActive = isActive;
                    });

        };

        service.updateLogoSrc = function (logoGeneratorId, src) {
            storage[logoGeneratorId].preview.img.setAttribute('src', src);
        };

        service.parseLanguage = function (text) {
            var result = null;

            if (regexCyrilic.test(text)) {
                result = 'cyrillic';
            } if (regexLatin.test(text)) {
                result = 'latin';
            }

            return result;
        };

        service.isCyrillic = function (text) {
            return service.parseLanguage(text) === 'cyrillic';
        };

        service.isLatin = function (text) {
            return service.parseLanguage(text) === 'latin';
        };

        service.replaceUnsupportOnSymbol = function (text, language) {
            return text.replace(language === 'cyrillic' ? regexCyrilic : regexLatin, emptySymbol);
        };

        service.fontFamilyEscape = function (fontFamily) {
            //шрифты, у которых в наименовании есть числа не вставляются в стили, надо обязательно обернуть в кавычки
            return /\d/.test(fontFamily) ? '"' + fontFamily + '"' : fontFamily;
        };
    };

    logoGeneratorService.$inject = ['$controller', '$http', '$q', '$window', 'modalService'];

    ng.module('logoGenerator')
      .service('logoGeneratorService', logoGeneratorService);

})(window.angular);