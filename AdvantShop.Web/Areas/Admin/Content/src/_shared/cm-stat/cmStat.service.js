; (function (ng) {
    'use strict';

    var cmStatService = function CmStatCTrl($http, $timeout) {
        var service = this,
            isRuning = false,
            countObsevarable = 0,
            data = {};

        service.getData = function getData() {

            countObsevarable += 1;

            data.Processed = 0;
            data.Total = 0;
            data.Update = 0;
            data.Add = 0;
            data.Error = 0;

            if (isRuning === false) {
                isRuning = true;

                service.startPooling();
            }

            return data;
        };

        service.makeRequest = function makeRequest() {
            return $http.get('ExportImportCommon/GetCommonStatistic').then(function (response) {
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

        service.deleteObsevarable = function deleteObsevarable() {
            countObsevarable -= 1;

            if (countObsevarable === 0) {
                service.stopPooling();
            }
        };
    };

    cmStatService.$inject = ['$http', '$timeout'];

    ng.module('cmStat')
      .service('cmStatService', cmStatService);

})(window.angular);