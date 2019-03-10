; (function (ng) {
    'use strict';

    var NewsCategoryCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents news-category-link"><ui-modal-trigger data-controller="\'ModalAddEditNewsCategoryCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/newsCategory/modal/addEditNewsCategory/addEditNewsCategpry.html" ' +
                                    'data-resolve="{\'id\': row.entity.NewsCategoryId}" ' +
                                    'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon">{{COL_FIELD}}</a>' +
                                  '</ui-modal-trigger></div>',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: 'UrlPath',
                    displayName: 'URL cиноним',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'URL cиноним',
                        type: uiGridConstants.filter.INPUT,
                        name: 'UrlPath'
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    enableCellEdit: true
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditNewsCategoryCtrl\'" controller-as="ctrl" ' +
                                    'template-url="../areas/admin/content/src/newsCategory/modal/addEditNewsCategory/addEditNewsCategpry.html" ' +
                                    'data-resolve="{\'id\': row.entity.NewsCategoryId}" ' +
                                    'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                    '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil news-category-pointer">{{COL_FIELD}}</a>' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="NewsCategory/DeleteNewsCategory" params="{\'Ids\': row.entity.NewsCategoryId}" confirm-text="Вы уверены, что хотите удалить?<br/> При удалении категории новостей будут удалены все новости из данной категории."></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'NewsCategory/DeleteNewsCategory',
                        field: 'NewsCategoryId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?<br/> При удалении категории новостей будут удалены все новости из данной категории.", { title: "Удаление" }).then(function (result) {
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

    NewsCategoryCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('newsCategory', ['uiGridCustom', 'urlHelper'])
      .controller('NewsCategoryCtrl', NewsCategoryCtrl);

})(window.angular);