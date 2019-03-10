; (function (ng) {
    'use strict';

    var LeadsCtrl = function ($http, $window) {

        var currentPage = 1,
            isEnd = false,
            isProcessing = false,
            leadsCtrl = this;

        leadsCtrl.status = '';
        leadsCtrl.leads = [];

        leadsCtrl.getLeads = function () {
            isProcessing = true;
            var page = currentPage;
            $http.get("/adminmobile/leads/getleads", { params: { 'page': page, 'status': leadsCtrl.status } }).then(function (response) {
                currentPage = page + 1;
                leadsCtrl.leads = leadsCtrl.leads.concat(response.data);
                isEnd = response.data == null || response.data.length == 0 || response.data.length < 10;
                isProcessing = false;
            });
        };

        leadsCtrl.FilterByStatus = function() {
            currentPage = 1;
            isEnd = false;
            leadsCtrl.leads = [];
            leadsCtrl.getLeads();
        }

        leadsCtrl.changeStatus = function() {
            $http.get("/adminmobile/leads/changestatus", { params: { 'id': leadsCtrl.Id, 'status': leadsCtrl.status } })
                .then(function(response) {
                    leadsCtrl.statusSaved = response.data != null && response.data.Result == "success";
                    if (leadsCtrl.statusSaved) {
                        alert("Статус сохранен");
                    }
            });
        }

        leadsCtrl.changeManager = function () {
            $http.get("/adminmobile/leads/changemanager", { params: { 'id': leadsCtrl.Id, 'managerid': leadsCtrl.manager } })
                .then(function (response) {
                    leadsCtrl.managerSaved = response.data != null && response.data.Result == "success";
                    if (leadsCtrl.managerSaved) {
                        alert("Менеджер сохранен");
                    }
                });
        }

        leadsCtrl.createOrder = function () {
            $http.post("/adminmobile/leads/createorder", { 'id': leadsCtrl.Id  })
                .then(function (response) {
                    leadsCtrl.orderSaved = response.data != null && response.data.Result == "success";
                    if (leadsCtrl.orderSaved) {
                        $window.location.assign(response.data.OrderUrl);
                    } else {
                        alert("Произошла ошибка: " + response.data.Error);
                    }
                });
        }

        leadsCtrl.createTask = function () {
            $http.post("/adminmobile/leads/createtask", { 'id': leadsCtrl.Id  })
                .then(function (response) {
                    leadsCtrl.taskSaved = response.data != null && response.data.Result == "success";
                    if (leadsCtrl.taskSaved) {
                        $window.location.assign(response.data.TaskUrl);
                    } else {
                        alert("Произошла ошибка: " + response.data.Error);
                    }
                });
        }

        leadsCtrl.onWinScroll = function() {
            if (!isEnd && !isProcessing && (getDocHeight() - 50 < getScrollXY()[1] + window.innerHeight)) {
                leadsCtrl.getLeads();
            }
        }

        leadsCtrl.getLeads();
    };

    ng.module("leads")
        .controller("LeadsController", LeadsCtrl);

    LeadsCtrl.$inject = ['$http', '$window'];

})(window.angular);
