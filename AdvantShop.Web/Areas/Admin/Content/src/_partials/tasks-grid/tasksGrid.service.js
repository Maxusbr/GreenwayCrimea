; (function (ng) {
    'use strict';

    var tasksGridService = function ($http) {
        var service = this;
    };

    tasksGridService.$inject = ['$http'];

    ng.module('tasksGrid')
        .service('tasksGridService', tasksGridService);

})(window.angular);