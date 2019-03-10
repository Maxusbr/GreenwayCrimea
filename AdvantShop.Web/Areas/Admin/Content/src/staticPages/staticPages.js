; (function (ng) {
    'use strict';

    var StaticPagesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'PageName',
                    displayName: 'Заголовок страницы',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a href="staticpages/edit/{{row.entity.StaticPageId}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Заголовок',
                        type: uiGridConstants.filter.INPUT,
                        name: 'PageName',
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
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    width: 150,
                    enableCellEdit: true,
                },
                {
                    name: 'ModifyDateFormatted',
                    displayName: 'Изменен',
                    width: 150,
                    filter: {
                        placeholder: 'Дата изменения',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'ModifyDateFrom'
                            },
                            to: {
                                name: 'ModifyDateTo'
                            }
                        }
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a ng-href="staticpages/edit/{{row.entity.StaticPageId}}" class="ui-grid-custom-service-icon fa fa-pencil"></a>' +
                            '<ui-grid-custom-delete url="staticpages/deleteStaticPage" params="{\'StaticPageId\': row.entity.StaticPageId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'staticpages/deleteStaticPages',
                        field: 'StaticPageId',
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
    };

    StaticPagesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('staticPages', ['uiGridCustom', 'urlHelper'])
      .controller('StaticPagesCtrl', StaticPagesCtrl);

})(window.angular);