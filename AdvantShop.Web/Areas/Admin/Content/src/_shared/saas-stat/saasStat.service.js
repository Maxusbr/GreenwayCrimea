; (function (ng) {
    'use strict';

    var saasStatService = function SaasStatCTrl($http, $timeout) {
        var service = this,
            isRuning = null,
            countObsevarable = 0,
            data = {};

        service.getData = function getData() {

            countObsevarable += 1;

            $http.get('ExportImportCommon/GetSaasBlockInformation').then(function (response) {
                return ng.extend(data, response.data);
            });

            //var isSaas = false;
            
            //if (service.getIsSaas() && isRuning === null) {
            if ( isRuning === null) {
                isRuning = true;

                service.startPooling();
            }

            return data;
        };

        service.makeRequest = function makeRequest() {
            return $http.get('ExportImportCommon/GetSaasBlockInformation').then(function (response) {
                return ng.extend(data, response.data);
            });
        };

        service.startPooling = function startPooling() {
            service.makeRequest().then(function () {
                return $timeout(function () {
                    if (isRuning === true && countObsevarable > 0) {
                        service.startPooling();
                    }
                }, 1000);
            });
        };

        service.stopPooling = function stopPooling() {
            isRuning = false;
        };

        service.getIsSaas = function getIsSaas() {
            return $http.get('ExportImportCommon/GetSaasBlockInformation').then(function (response) {
                return response.data.isSaas;
            });
        };

        service.deleteObsevarable = function deleteObsevarable() {
            countObsevarable -= 1;
        };
    };

    saasStatService.$inject = ['$http', '$timeout'];

    ng.module('saasStat')
      .service('saasStatService', saasStatService);

})(window.angular);