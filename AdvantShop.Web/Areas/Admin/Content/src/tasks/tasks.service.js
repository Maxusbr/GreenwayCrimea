; (function (ng) {
    'use strict';

    var tasksService = function ($http) {
        var service = this;

        service.getTasks = function () {
            return $http.get('tasks/getTasks').then(function (response) {
                return response.data;
            });
        }

        service.getFormData = function () {
            return $http.get('tasks/getTaskFormData').then(function (response) {
                return response.data;
            });
        }

        service.getManagers = function () {
            return $http.get('managers/getManagersSelectOptions').then(function (response) {
                return response.data;
            });
        }

        service.getTaskGroups = function () {
            return $http.get('taskgroups/getTaskGroupsSelectOptions').then(function (response) {
                return response.data;
            });
        }

        service.getTaskPriorities = function () {
            return $http.get('tasks/getTaskPrioritiesSelectOptions').then(function (response) {
                return response.data;
            });
        }

        service.getTaskStatuses = function () {
            return $http.get('tasks/getTaskStatusesSelectOptions').then(function (response) {
                return response.data;
            });
        }

        service.getTaskAttachments = function (id) {
            return $http.post('tasks/getTaskAttachments', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.getTask = function (id) {
            return $http.post('tasks/getTask', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.deleteTask = function (id) {
            return $http.post('tasks/deleteTask', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.addTask = function (params) {
            return $http.post('tasks/addTask', params).then(function (response) {
                return response.data;
            });
        }

        service.editTask = function (params) {
            return $http.post('tasks/editTask', params).then(function (response) {
                return response.data;
            });
        }

        service.changeTaskStatus = function (id, status) {
            return $http.post('tasks/changeTaskStatus', { id: id, status: status }).then(function (response) {
                return response.data;
            });
        }

        service.changeTaskStatuses = function (params) {
            return $http.post('tasks/changeTaskStatuses', params).then(function (response) {
                return response.data;
            });
        }

        service.completeTask = function (id, result, orderStatusId, dealStatusId) {
            return $http.post('tasks/completeTask', { id: id, taskResult: result, orderStatusId: orderStatusId, dealStatusId: dealStatusId }).then(function (response) {
                return response.data;
            });
        }

        service.getOrderStatuses = function () {
            return $http.get('orderStatuses/getStatuses').then(function (response) {
                return response.data.DataItems;
            });
        }

        service.getDealStatuses = function () {
            return $http.get('leads/getDealStatuses').then(function (response) {
                return response.data.items;
            });
        }

        service.acceptTask = function (id) {
            return $http.post('tasks/acceptTask', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.acceptTasks = function (params) {
            return $http.post('tasks/accepttasks', params).then(function (response) {
                return response.data;
            });
        }

        service.changeSorting = function (id, prevId, nextId) {
            return $http.post('tasks/changeSorting', { id: id, prevId: prevId, nextId: nextId }).then(function (response) {
                return response.data;
            });
        }

        service.deleteAttachment = function (id, taskId) {
            return $http.post('tasks/deleteAttachment', { id: id, taskId: taskId }).then(function (response) {
                return response.data;
            });
        }
    };

    tasksService.$inject = ['$http'];

    ng.module('tasks')
        .service('tasksService', tasksService);

})(window.angular);