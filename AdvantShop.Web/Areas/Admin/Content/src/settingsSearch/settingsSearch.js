; (function (ng) {
    'use strict';

    var SettingsSearchCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, SweetAlert, $http, $q) {

        var ctrl = this,
            columnDefs = [
               {
                   name: 'Title',
                   displayName: 'Заголовок',
                   enableCellEdit: true,
                   filter: {
                       placeholder: 'Заголовок',
                       type: uiGridConstants.filter.INPUT,
                       name: 'Title'
                   }
               },
                {
                    name: 'Link',
                    displayName: 'Ссылка',
                    width: 400,
                    enableCellEdit: false,
                    cellTemplate:
                       '<div class="ui-grid-cell-contents">' +
                               '<a href="{{COL_FIELD}}" target="_blank">{{COL_FIELD}}</a> ' +
                       '</div>',
                },
                 {
                     name: 'KeyWords',
                     displayName: 'Ключевые слова',
                     width: 400,
                     enableCellEdit: true,
                 },
                 {
                     name: 'SortOrder',
                     displayName: 'Сортировка',
                     width: 100,
                     enableCellEdit: true,
                 },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditSettingsSearchCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/SettingsSearch/modal/addEditSettingsSearch/addEditSettingsSearch.html" ' +
                                        'data-resolve="{value:{\'Id\': row.entity.Id, \'Title\': row.entity.Title, \'Link\': row.entity.Link, \'KeyWords\': row.entity.KeyWords, \'SortOrder\': row.entity.SortOrder }}"' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +

                            ' <a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.Id)" ng-class="(\'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            rowHeight: 40,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'SettingsSearch/DeleteSettingsSearches',
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


        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.delete = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('SettingsSearch/deleteSettingsSearch', { 'id': id }).then(function (response) {
                        ctrl.grid.fetchData();
                    });
                }
            });

        }

    };

    SettingsSearchCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', 'SweetAlert', '$http', '$q'];


    ng.module('settingsSearch', ['uiGridCustom', 'urlHelper'])
      .controller('SettingsSearchCtrl', SettingsSearchCtrl);

})(window.angular);