; (function (ng) {
    'use strict';

    var GradesCtrl = function ($location, $window, $uibModal, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $http, $q, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents"><span class="link">{{COL_FIELD}}</span></div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name'
                    }
                },
                {
                    name: 'BonusPercent',
                    displayName: 'Процент',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Процент',
                        type: uiGridConstants.filter.INPUT,
                        name: 'BonusPercent'
                    }
                },
                {
                    name: 'PurchaseBarrier',
                    displayName: 'Порог перехода(сумма продаж)',
                    headerCellClass: 'grid-text-required',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Порог перехода(сумма продаж)',
                        type: uiGridConstants.filter.INPUT,
                        name: 'PurchaseBarrier'
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    enableCellEdit: true,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents js-grid-not-clicked"><div>' +
                            '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.loadGrade(row.entity.Id)"></a> ' +
                            '<ui-grid-custom-delete url="grades/delete" params="{\'id\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.loadGrade(row.entity.Id);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'grades/deleteGrade',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.loadGrade = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditGradeCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/grades/modal/addEditGrade/AddEditGrade.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.grid.fetchData();
                return result;
            }, function (result) {
                return result;
            });
        };

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
        
    };

    GradesCtrl.$inject = ['$location', '$window', '$uibModal', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$http', '$q', 'SweetAlert'];


    ng.module('grades', ['uiGridCustom', 'urlHelper'])
      .controller('GradesCtrl', GradesCtrl);

})(window.angular);