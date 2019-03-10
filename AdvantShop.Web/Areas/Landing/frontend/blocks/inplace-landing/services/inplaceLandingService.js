; (function (ng) {
    'use strict';

    var inplaceLandingService = function ($http) {
        var service = this,
            progressState = {
                show: false
            },
            storage = {
                rich: {},
                inplaceAutocomplete: {},
                inplaceImage: {},
                inplacePrice: {}
            };

        service.addRich = function (id, obj) {
            storage.rich[id] = obj;
        };

        service.getRich = function (id) {
            return storage.rich[id];
        };

        service.addInplaceAutocomplete = function (id, obj) {
            storage.inplaceAutocomplete[id] = obj;
        };

        service.getInplaceAutocomplete = function (id) {
            return storage.inplaceAutocomplete[id];
        };

        service.addInplaceImage = function (id, obj) {
            storage.inplaceImage[id] = obj;
        };

        service.getInplaceImage = function (id) {
            return storage.inplaceImage[id];
        };

        service.addInplacePrice = function (id, obj) {
            storage.inplacePrice[id] = obj;
        };

        service.getInplacePrice = function (id) {
            return storage.inplacePrice[id];
        };

        service.save = function (url, params) {
            return $http.post(url, ng.extend(params, { rnd: Math.random() }));
        };

        service.setEnable = function (isEnabled) {
            return $http.post('inplaceeditor/setenable', { isEnabled: isEnabled }).then(function (response) {
                return response.data;
            });
        };

        service.getProgressState = function () {
            return progressState;
        };

        service.startProgress = function () {
            progressState.show = true;
        };

        service.stopProgress = function () {
            progressState.show = false;
        };
    };

    ng.module('inplaceLanding')
      .service('inplaceLandingService', inplaceLandingService);

    inplaceLandingService.$inject = ['$http'];

})(window.angular);