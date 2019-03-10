; (function (ng) {
    'use strict';

    var OrderStatusesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Color',
                    displayName: '',
                    cellTemplate: '<div class="ui-grid-cell-contents"><i class="fa fa-circle" style="color:#{{COL_FIELD}}"></i></div>',
                    width: 40,
                },
                {
                    name: 'StatusName',
                    displayName: 'Название',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditOrderStatusCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/orderstatuses/modal/AddEditOrderStatus.html" ' +
                                        'data-resolve="{\'orderStatusId\': row.entity.OrderStatusId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="">{{COL_FIELD}}</a> ' +
                            '</ui-modal-trigger>' +
                        '</div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: 'IsDefault',
                    displayName: 'Значение по умолчанию',
                    cellTemplate: '<div class="ui-grid-cell-contents"><label class="adv-checkbox-label">' +
                                        '<input type="checkbox" disabled ng-model="row.entity.IsDefault" class="adv-checkbox-input control-checkbox" data-e2e="switchOnOffSelect" />' +
                                        '<span class="adv-checkbox-emul" data-e2e="switchOnOffInput"></span>' +
                                    '</label></div>',
                    width: 100,
                    filter: {
                        placeholder: 'Значение по умолчанию',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsDefault',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'IsCanceled',
                    displayName: 'Отмена заказа',
                    cellTemplate: '<div class="ui-grid-cell-contents"><label class="adv-checkbox-label">' +
                                        '<input type="checkbox" disabled ng-model="row.entity.IsCanceled" class="adv-checkbox-input control-checkbox" data-e2e="switchOnOffSelect" />' +
                                        '<span class="adv-checkbox-emul" data-e2e="switchOnOffInput"></span>' +
                                    '</label></div>',
                    width: 100,
                    filter: {
                        placeholder: 'Отмена заказа',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsCanceled',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'IsCompleted',
                    displayName: 'Заказ завершен',
                    cellTemplate: '<div class="ui-grid-cell-contents"><label class="adv-checkbox-label">' +
                                        '<input type="checkbox" disabled ng-model="row.entity.IsCompleted" class="adv-checkbox-input control-checkbox" data-e2e="switchOnOffSelect" />' +
                                        '<span class="adv-checkbox-emul" data-e2e="switchOnOffInput"></span>' +
                                    '</label></div>',
                    width: 100,
                    filter: {
                        placeholder: 'Заказ завершен',
                        type: uiGridConstants.filter.SELECT,
                        name: 'IsCompleted',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    width: 100,
                },
                {
                    name: 'CommandFormatted',
                    displayName: 'Команда',
                    width: 200,
                    filter: {
                        placeholder: 'Команда',
                        type: uiGridConstants.filter.SELECT,
                        name: 'CommandId',
                        fetch: 'orderstatuses/getcommands'
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.openModal(row.entity.OrderStatusId)"></a> ' +
                            ' <a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.CanBeDeleted, row.entity.OrderStatusId)" ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.openModal(row.entity.OrderStatusId);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'orderstatuses/deleteOrderSatuses',
                        field: 'OrderStatusId',
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

        ctrl.delete = function (canBeDeleted, orderStatusId) {

            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('orderstatuses/deleteOrderStatus', { 'OrderStatusId': orderStatusId }).then(function (response) {
                            ctrl.grid.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("Статус используется в заказах или его нельзя удалять", { title: "Удаление невозможно" });
            }
        }

        ctrl.openModal = function (orderStatusId) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditOrderStatusCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/orderstatuses/modal/AddEditOrderStatus.html',
                resolve: {
                    orderStatusId: function () {
                        return orderStatusId;
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

    OrderStatusesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('orderstatuses', ['uiGridCustom', 'urlHelper'])
      .controller('OrderStatusesCtrl', OrderStatusesCtrl);

})(window.angular);