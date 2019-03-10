; (function (ng) {
    'use strict';

    var StaticBlockCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Key',
                    displayName: 'Ключ доступа',
                    enableSorting: true,
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Ключ доступа',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Key'
                    },
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditStaticBlockCtrl\'" controller-as="ctrl" size="middle" ' +
                                    'template-url="../areas/admin/content/src/staticBlock/modal/addEditStaticBlock/addEditStaticBlock.html" ' +
                                    'data-resolve="{\'id\': row.entity.StaticBlockId}" ' +
                                    'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                    '<a href="">{{COL_FIELD}}</a>' +
                            '</ui-modal-trigger>' +
                        '</div></div>'
                },
                {
                    name: 'InnerName',
                    displayName: 'Название',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'InnerName'
                    }
                },
                {
                    name: 'Enabled',
                    displayName: 'Актив.',
                    enableCellEdit: false,
                    cellTemplate: '<ui-grid-custom-switch row="row"></ui-grid-custom-switch>',
                    width: 76,
                    filter: {
                        placeholder: 'Активность',
                        name: 'Enabled',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: 'AddedFormatted',
                    displayName: 'Дата добавления',
                    width: 150,
                    enableCellEdit: false,
                },

                {
                    name: 'ModifiedFormatted',
                    displayName: 'Дата модификации',
                    width: 165,
                    enableCellEdit: false,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditStaticBlockCtrl\'" controller-as="ctrl" size="middle" ' +
                                    'template-url="../areas/admin/content/src/staticBlock/modal/addEditStaticBlock/addEditStaticBlock.html" ' +
                                    'data-resolve="{\'id\': row.entity.StaticBlockId}" ' +
                                    'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                    '<a href="" class="ui-grid-custom-service-icon fa fa-pencil news-category-pointer">{{COL_FIELD}}</a>' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="StaticBlock/deleteItem" params="{\'id\': row.entity.StaticBlockId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'staticBlock/delete',
                        field: 'StaticBlockId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                    {
                        text: 'Сделать активными',
                        url: 'staticBlock/active',
                        field: 'StaticBlockId'
                    },
                    {
                        text: 'Сделать неактивными',
                        url: 'staticBlock/deactive',
                        field: 'StaticBlockId'
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
    };

    StaticBlockCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('staticBlock', ['uiGridCustom', 'urlHelper'])
      .controller('StaticBlockCtrl', StaticBlockCtrl);

})(window.angular);