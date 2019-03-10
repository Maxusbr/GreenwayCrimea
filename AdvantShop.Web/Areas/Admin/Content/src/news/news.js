; (function (ng) {
    'use strict';

    var NewsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Title',
                    displayName: 'Заголовок',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a href="news/edit/{{row.entity.NewsId}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Заголовок',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Title',
                    }
                },
                {
                    name: 'AddingDateFormatted',
                    displayName: 'Дата добавления',
                    width: 150,
                    filter: {
                        placeholder: 'Дата добавления',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'AddingDateFrom'
                            },
                            to: {
                                name: 'AddingDateTo'
                            }
                        }
                    }
                },
                {
                    name: 'NewsCategory',
                    displayName: 'Категория',
                    width: 200,
                    filter: {
                        placeholder: 'Категория',
                        type: uiGridConstants.filter.SELECT,
                        name: 'NewsCategoryId',
                        fetch: 'news/getNewsCategories'
                    }
                },
                {
                    name: 'ShowOnMainPage',
                    displayName: 'На гл. странице',
                    cellTemplate: '<ui-grid-custom-switch row="row" field-name="ShowOnMainPage" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                    enableCellEdit: false,
                    width: 90,
                    filter: {
                        placeholder: 'На гл. странице',
                        type: uiGridConstants.filter.SELECT,
                        name: 'ShowOnMainPage',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'Enabled',
                    displayName: 'Активна',
                    enableCellEdit: false,
                    cellTemplate: '<ui-grid-custom-switch row="row" field-name="Enabled" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                    width: 90,
                    filter: {
                        placeholder: 'Активность',
                        name: 'Enabled',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a ng-href="news/edit/{{row.entity.NewsId}}" class="ui-grid-custom-service-icon fa fa-pencil"></a>' +
                            '<ui-grid-custom-delete url="news/deleteNewsItem" params="{\'NewsId\': row.entity.NewsId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'news/deleteNews',
                        field: 'NewsId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                    {
                        text: 'Сделать активными',
                        url: 'news/setNewsEnabled',
                        field: 'NewsId'
                    },
                    {
                        text: 'Сделать неактивными',
                        url: 'news/setNewsDisabled',
                        field: 'NewsId'
                    },
                    {
                        text: 'Выводить на главной',
                        url: 'news/setNewsOnMainPage',
                        field: 'NewsId'
                    },
                    {
                        text: 'Не выводить на главной',
                        url: 'news/setNewsNotOnMainPage',
                        field: 'NewsId'
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
    };

    NewsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('news', ['uiGridCustom', 'urlHelper'])
      .controller('NewsCtrl', NewsCtrl);

})(window.angular);