; (function (ng) {
    'use strict';

    var windowService = function ($window) {
        var service = this,
            windowElement = ng.element($window),
            callbackList = {};

        service.print = function (url, name, parameters) {
            var wPrintOrder = $window.open(url, name, parameters);
            wPrintOrder.onload = wPrintOrder.print;
            wPrintOrder.focus();

            return wPrintOrder;
        };

        service.addCallback = function (eventName, callback) {
            if (callbackList[eventName] == null) {
                 callbackList[eventName] = [];
                 service.addBindEvent(eventName);
            }

             callbackList[eventName].push(callback);
        }

        service.addBindEvent = function (eventName) {
            windowElement.on(eventName, function (event) {
                service.processCallbacks(eventName, event);
            });
        };

        service.processCallbacks = function (eventName, event) {

            var eventFunctions = callbackList[eventName];

            for (var i = eventFunctions.length - 1; i >= 0; i--) {
                eventFunctions[i]({ event: event });
            };
        };
    };

    ng.module('windowExt')
      .service('windowService', windowService);

    windowService.$inject = ['$window'];

})(window.angular);