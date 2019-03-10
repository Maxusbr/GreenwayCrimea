; (function (ng) {
    'use strict';

    var KanbanCtrl = function ($http, $location) {
        var ctrl = this,
            isFirstPageLoad = true;

        ctrl.$onInit = function () {

            ctrl._params = ctrl.kanbanParams;

            ctrl.optionsFromUrl();

            ctrl.fetchData().then(function () {
                if (ctrl.kanbanOnInit != null) {
                    ctrl.kanbanOnInit({ kanban: ctrl });
                }
            });
        }

        ctrl.filterInit = function (filter) {
            ctrl.filter = filter;
            if (ctrl.kanbanOnFilterInit != null) {
                ctrl.kanbanOnFilterInit({ filter: filter });
            }
        }

        ctrl.optionsFromUrl = function () {
            
            var kanbanParamsByUrl = ctrl.getParamsByUrl(ctrl.uid);

            if (kanbanParamsByUrl != null) {
                ng.extend(ctrl._params, kanbanParamsByUrl);
            }
        }


        ctrl.filterApply = function (params, item) {

            if (ng.isArray(params) === false) {
                throw new Error('Parameter "params" should be array')
            }

            for (var i = 0, len = params.length; i < len; i++) {
                ctrl._params[params[i].name] = params[i].value;
            }

            ctrl.resetColumnsData();

            ctrl.fetchData().then(function () {
                ctrl.setParamsByUrl(ctrl._params);
            });
        };

        ctrl.filterRemove = function (name, item) {

            if (item.filter.type === 'range') {
                delete ctrl._params[item.filter.rangeOptions.from.name];
                delete ctrl._params[item.filter.rangeOptions.to.name];
            } if (item.filter.type === 'datetime') {
                delete ctrl._params[item.filter.datetimeOptions.from.name];
                delete ctrl._params[item.filter.datetimeOptions.to.name];
            } else if (item.filter.type === 'date') {
                delete ctrl._params[item.filter.dateOptions.from.name];
                delete ctrl._params[item.filter.dateOptions.to.name];
            } else {
                delete ctrl._params[name];
            }

            ctrl.resetColumnsData();

            ctrl.fetchData()
                .then(function () {
                    ctrl.setParamsByUrl(ctrl._params);
                });
        };


        ctrl.setParamsByUrl = function (params) {
            $location.search( ctrl.uid,JSON.stringify(params));
        }

        ctrl.getParamsByUrl = function (uid) {
            return JSON.parse($location.search()[uid] || null);
        }

        ctrl.resetColumnsData = function () {
            if (ctrl.kanbanObj) {
                for (var i = 0; i < ctrl.kanbanObj.Columns.length; i++) {
                    ctrl.kanbanObj.Columns[i].Page = 1;
                }
            }
        }

        ctrl.getRequestParams = function () {
            var params = { columns: [] };
            if (ctrl.kanbanObj) {
                ctrl.kanbanObj.Columns.forEach(function (column) {
                    params.columns.push({
                        id: column.Id,
                        page: column.Page,
                        cardsPerColumn: column.CardsPerColumn
                    });
                });
            }
            ng.extend(params, ctrl._params);
            return params;
        }

        ctrl.fetchData = function () {
            return $http.post(ctrl.fetchUrl, { model: ctrl.getRequestParams() }).then(function (response) {
                ctrl.kanbanObj = response.data;
                ctrl._params = ctrl._params || {};
            });
        }

        ctrl.fetchColumnData = function (colIndex) {
            ctrl.kanbanObj.Columns[colIndex].Page += 1;
            var params = ctrl.getRequestParams();
            params.ColumnId = ctrl.kanbanObj.Columns[colIndex].Id;

            return $http.post(ctrl.fetchColumnUrl, { model: params }).then(function (response) {
                ctrl.kanbanObj.Columns[colIndex].Cards.push.apply(ctrl.kanbanObj.Columns[colIndex].Cards, response.data);
            });
        }
    };

    KanbanCtrl.$inject = ['$http', '$location'];

    ng.module('kanban')
        .controller('KanbanCtrl', KanbanCtrl);
})(window.angular);