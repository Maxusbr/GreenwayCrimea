; (function (ng) {
    'use strict';

    ng.module('tasksGrid')
        .component('tasksGrid', {
            templateUrl: '../areas/admin/content/src/_partials/tasks-grid/templates/tasks-grid.html',
            controller: 'TasksGridCtrl',
            bindings: {
                objId: '<',
                type: '@',
            }
        });

})(window.angular);