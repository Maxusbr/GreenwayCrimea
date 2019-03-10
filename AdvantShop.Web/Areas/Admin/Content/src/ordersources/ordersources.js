; (function (ng) {
    'use strict';

    var OrderSourcesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.openModal(row.entity.Id)">{{COL_FIELD}}</a> ' +
                        '</div>',
                },
                {
                    name: 'TypeFormatted',
                    displayName: 'Группа',
                    width: 220,
                },
                {
                    name: 'Main',
                    displayName: 'Основной в группе',
                    width: 90,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<label class="ui-grid-custom-edit-field adv-checkbox-label"> ' +
                                '<input type="checkbox" class="adv-checkbox-input" data-e2e="switchOnOffInput" ng-model="row.entity.Main" disabled /> ' +
                                '<span class="adv-checkbox-emul" data-e2e="switchOnOffSelect"></span> ' +
                            '</label>' +
                        '</div>',
                },
                {
                    name: 'SortOrder',
                    displayName: 'Порядок сортировки',
                    width: 90,
                },
                {
                    name: 'OrdersCount',
                    displayName: 'Кол-во заказов',
                    width: 90,
                },
                {
                    name: 'LeadsCount',
                    displayName: 'Кол-во лидов',
                    width: 90,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.openModal(row.entity.Id)"></a> ' +
                            ' <a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.CanBeDeleted, row.entity.Id)" ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.openModal(row.entity.Id);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'ordersources/deleteOrderSources',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.delete = function (canBeDeleted, id) {

            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('ordersources/deleteOrderSource', { 'id': id }).then(function (response) {
                            ctrl.grid.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("Удаление невозможно, есть назначенные заказы, лиды", { title: "Удаление невозможно" });
            }
        }

        ctrl.openModal = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditOrderSourceCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/ordersources/modal/AddEditOrderSource.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.grid.fetchData();
                return result;
            }, function (result) {
                ctrl.grid.fetchData();
                return result;
            });
        };
    };

    OrderSourcesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('ordersources', ['uiGridCustom', 'urlHelper'])
      .controller('OrderSourcesCtrl', OrderSourcesCtrl);

})(window.angular);