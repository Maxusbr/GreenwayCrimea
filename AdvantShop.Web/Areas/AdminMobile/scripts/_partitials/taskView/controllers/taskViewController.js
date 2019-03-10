; (function (ng) {
    'use strict';

    var TaskViewCtrl = function ($http) {

        var taskCtrl = this;

        //taskCtrl.statusSaved = false;
        taskCtrl.resultCode = -1;

        //var resetStatusSaved = function () {
        //    taskCtrl.statusSaved = false;
        //    $timeout(resetStatusSaved, 5000);
        //};

        taskCtrl.changeStatus = function () {
            //taskCtrl.statusSaved = false;
            $http.get("/adminmobile/Tasks/ChangeStatus", { params: { 'taskId': taskCtrl.taskId, 'status': taskCtrl.status } })
                .then(function (response) {
                    taskCtrl.statusSaved = response.data != null && response.data.ResultCode === 0;
                    alert("Статус задачи сохранен");
                });
        }
    };

    ng.module("tasksView")
        .controller("TaskViewController", TaskViewCtrl);

    TaskViewCtrl.$inject = ['$http'];

})(window.angular);
