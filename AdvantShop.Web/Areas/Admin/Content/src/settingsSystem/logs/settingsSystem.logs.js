; (function (ng) {
    'use strict';


    var SettingsSystemLogsCtrl = function ($location, settingsSystemLogsService) {

        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.enumViewMode = {
                'list': 'list',
                'details': 'details'
            };

            ctrl.enumTypes = {
                'err500': 'err500',
                'errHTTP': 'errHTTP',
                'info': 'info'
            };

            ctrl.enumFields = {
                'exception': 'ExceptionData',
                'request': 'RequestData',
                'browser': 'BrowserData',
                'session': 'SessionData'
            };

            var paramsFromUrl = $location.search(),
                paramsFromUrlParsed;

            if (paramsFromUrl != null && paramsFromUrl.settingsLogs != null) {

                paramsFromUrlParsed = JSON.parse(paramsFromUrl.settingsLogs);

                if (paramsFromUrlParsed.viewMode != null) {

                    if (paramsFromUrlParsed.viewMode) {
                        ctrl.viewMode = paramsFromUrlParsed.viewMode;
                    }

                    if (paramsFromUrlParsed.type) {
                        ctrl.type = paramsFromUrlParsed.type;
                    }

                    if (paramsFromUrlParsed.viewMode === ctrl.enumViewMode.list) {
                        ctrl.changeType(paramsFromUrlParsed.type);
                    } else if (paramsFromUrlParsed.viewMode === ctrl.enumViewMode.details) {
                        
                        if (paramsFromUrlParsed.time) {
                            ctrl.itemDatetime = paramsFromUrlParsed.time;
                        }

                        if (paramsFromUrlParsed.field) {
                            ctrl.field = paramsFromUrlParsed.field;
                        }

                        ctrl.goToDetails(ctrl.type, ctrl.itemDatetime, ctrl.field);
                    }

                }
            } else {
                ctrl.viewMode = ctrl.enumViewMode.list;
                ctrl.type = ctrl.enumTypes.err500;
                ctrl.getLogs(ctrl.enumTypes.err500);
            };
        };

        ctrl.changeType = function (type) {

            ctrl.type = type;

            ctrl.getLogs(type).then(function () {
                ctrl.setUrlParams(ctrl.viewMode, type);
            });
        };

        ctrl.getLogs = function (type, page) {
            return settingsSystemLogsService.getLogs(type, page).then(function (result) {
                return ctrl.data = result.obj;
            });
        }

        ctrl.goToList = function () {
            ctrl.viewMode = ctrl.enumViewMode.list;

            ctrl.getLogs(ctrl.type).then(function () {
                ctrl.setUrlParams(ctrl.viewMode, ctrl.type);
            });

            ctrl.setUrlParams(ctrl.viewMode, ctrl.type);
        };


        ctrl.changeField = function (field) {
            ctrl.field = field;

            ctrl.setUrlParams(ctrl.viewMode, ctrl.type, field, ctrl.itemDatetime);
        };

        ctrl.goToDetails = function (type, datetime, field) {

            ctrl.viewMode = ctrl.enumViewMode.details;

            ctrl.field = field || ctrl.enumFields.exception;

            ctrl.itemDatetime = datetime;

            settingsSystemLogsService.getLogsItem(type, datetime).then(function (result) {

                var exceptionColectionData = {};

                if (result.obj.ExceptionData != null) {
                    for (var key in result.obj.ExceptionData) {
                        if (result.obj.ExceptionData.hasOwnProperty(key)) {
                            exceptionColectionData[key] = result.obj.ExceptionData[key];
                            delete result.obj.ExceptionData[key];
                        }
                    }

                    result.obj.ExceptionData.ColectionData = exceptionColectionData;
                }

                ctrl.dataDetails = result.obj;

                ctrl.setUrlParams(ctrl.viewMode, type, ctrl.field, datetime);
            });
        };

        ctrl.changePagination = function (page) {
            ctrl.getLogs(ctrl.type, page);
        };

        ctrl.serializeFieldValue = function (value) {
            return ng.isObject(value) ? JSON.stringify(value) : value;
        };

        ctrl.setUrlParams = function (viewMode, type, field, itemDatetime) {

            var params = JSON.stringify({
                viewMode: viewMode,
                type: type,
                field: field,
                time: itemDatetime
            });

            $location.search('settingsLogs', params);
        };
    }

    SettingsSystemLogsCtrl.$inject = ['$location', 'settingsSystemLogsService'];

    ng.module('settingsSystem')
      .controller('SettingsSystemLogsCtrl', SettingsSystemLogsCtrl);

})(window.angular);