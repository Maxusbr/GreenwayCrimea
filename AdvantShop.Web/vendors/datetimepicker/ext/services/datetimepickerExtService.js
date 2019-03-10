; (function (ng) {
    'use strict';

    var datetimepickerExtService = function ($q) {

        var service = this,
            triggers = {},
            queue = {};

        service.addTrigger = function (id, ctrl) {

            triggers[id] = ctrl;

            if (queue[id] != null) {
                queue[id].resolve(ctrl).finally(function () {
                    delete queue[id];
                });
            }
        };

        service.getTrigger = function (id) {

            var defer = $q.defer();

            if (triggers[id] != null) {
                defer.resolve(triggers[id]);
            } else {
                queue[id] = defer;
            }

            return defer.promise;
        };
    };

    ng.module('ui.bootstrap.datetimepicker')
      .service('datetimepickerExtService', datetimepickerExtService);

    datetimepickerExtService.$inject = ['$q'];

})(window.angular);