; (function (ng) {
    'use strict';

    var TagsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $http, $q, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="tags/edit/{{row.entity.Id}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name'
                    }
                },
                {
                    name: 'UrlPath',
                    displayName: 'Синоним для URL',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Синоним для URL',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Url'
                    }
                },
                {
                    name: 'Enabled',
                    displayName: 'Актив.',
                    enableCellEdit: false,
                    cellTemplate: '<ui-grid-custom-switch row="row" field-name="Enabled"></ui-grid-custom-switch>',
                    width: 76,
                    filter: {
                        placeholder: 'Активность',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                      name: 'VisibilityForUsers',
                      displayName: 'Видимость',
                      enableCellEdit: false,
                      cellTemplate: '<ui-grid-custom-switch row="row" field-name="VisibilityForUsers"></ui-grid-custom-switch>',
                      width: 100,
                      filter: {
                          placeholder: 'Видимость',
                          type: uiGridConstants.filter.SELECT,
                          selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
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
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="tags/edit/{{row.entity.Id}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '<ui-grid-custom-delete url="tags/deleteTag" params="{\'id\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'tags/edit/{{row.entity.Id}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'tags/deleteTags',
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

        ctrl.setActive = function (active, id) {
            if (id <= 0) return;

            $http.post('tags/setTagActive', { id: id, active: active }).then(function (response) {
                if (response.data === true) {
                    ctrl.Enabled = active;
                    toaster.pop('success', 'Изменения сохранены');
                } else {
                    toaster.pop('error', 'Ошибка при изменении активности', "");
                }
            });
        };

        ctrl.deleteTag = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('tags/deleteTag', { id: id }).then(function (response) {
                        $window.location.assign('tags');
                    });
                }
            });
        }
    };

    TagsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$http', '$q', 'SweetAlert'];


    ng.module('tags', ['uiGridCustom', 'urlHelper'])
      .controller('TagsCtrl', TagsCtrl);

})(window.angular);