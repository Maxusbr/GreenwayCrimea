; (function (ng) {
    'use strict';


    var settingsSystemLogsService = function ($http) {

        var service = this;

        service.getLogs = function (type, page) {
            return $http.get('logErrors/getLogErrors', { params: { 'type': type, 'page': page } }).then(function (response) {
                return response.data;
            });
        };

        service.getLogsItem = function (type, datetime) {
            return $http.get('logErrors/getItemLogError', { params: { 'type': type, 'time': datetime } }).then(function (response) {
                return response.data;
            });
        };
    }

    settingsSystemLogsService.$inject = ['$http'];

    ng.module('settingsSystem')
      .service('settingsSystemLogsService', settingsSystemLogsService);

})(window.angular);