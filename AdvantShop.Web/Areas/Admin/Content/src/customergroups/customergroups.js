; (function (ng) {
    'use strict';

    var CustomerGroupsCtrl = function ($window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'GroupName',
                    displayName: 'Название группы',
                    enableCellEdit: true
                },
                {
                    name: 'GroupDiscount',
                    displayName: 'Скидка',
                    enableCellEdit: true
                },
                {
                    name: 'MinimumOrderPrice',
                    displayName: 'Минимальная сумма заказа',
                    enableCellEdit: true
                },
                {
                    name: '_customersColumn',
                    displayName: '',
                    cellTemplate: '<div class="ui-grid-cell-contents"> <a href="customers?group={{row.entity.CustomerGroupId}}" class="link">Посмотреть покупателей</a> </div>',
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 55,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            //'<ui-grid-custom-delete data-ng-show="{{row.entity.CanBeDeleted}}" url="customergroups/deletecustomergroup" params="{\'CustomerGroupId\': row.entity.CustomerGroupId}"></ui-grid-custom-delete>' +
                            ' <a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.CanBeDeleted, row.entity.CustomerGroupId)" ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'customergroups/deletecustomergroups',
                        field: 'CustomerGroupId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.delete = function (canBeDeleted, groupId) {

            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('customergroups/deletecustomergroup', { 'CustomerGroupId': groupId }).then(function (response) {
                            ctrl.grid.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("Группу пользователей по-умолчанию нельзя удалять", { title: "Удаление невозможно" });
            }
        }
    };

    CustomerGroupsCtrl.$inject = ['$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http'];


    ng.module('customergroups', ['uiGridCustom', 'urlHelper'])
      .controller('CustomerGroupsCtrl', CustomerGroupsCtrl);

})(window.angular);