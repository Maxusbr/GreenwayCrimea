; (function (ng) {
    'use strict';

    var TasksViewCtrl = function ($http) {

        var currentPage = 1,
            isEnd = false,
            tasksCtrl = this;

        tasksCtrl.status = -1;
        tasksCtrl.tasks = [];

        tasksCtrl.getTasks = function () {
            $http.get("/adminmobile/Tasks/GetTasks", { params: { 'page': currentPage, 'status': tasksCtrl.status } }).then(function (response) {
                currentPage += 1;
                tasksCtrl.tasks = tasksCtrl.tasks.concat(response.data);
                isEnd = response.data == null || response.data.length == 0 || response.data.length < 10;
            });
        };

        tasksCtrl.FilterByStatus = function() {
            currentPage = 1;
            isEnd = false;
            tasksCtrl.tasks = [];
            tasksCtrl.getTasks();
        }

        tasksCtrl.onWinScroll = function() {
            if (!isEnd && (getDocHeight() - 50 < getScrollXY()[1] + window.innerHeight)) {
                tasksCtrl.getTasks();
            }
        }

        tasksCtrl.getTasks();
    };

    ng.module("tasksView")
        .controller("TasksViewController", TasksViewCtrl);

    TasksViewCtrl.$inject = ['$http'];

})(window.angular);
