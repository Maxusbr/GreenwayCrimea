; (function (ng) {
    'use strict';

    var SubscriptionCtrl = function (Upload, toaster, $location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Email',
                    displayName: 'Email',
                    enableSorting: true,
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Email',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Email'
                    }
                },
                {
                    name: 'Enabled',
                    displayName: 'Активен',
                    enableCellEdit: false,
                    width:80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="text-align:center"><ui-grid-custom-switch row="row"></ui-grid-custom-switch></div></div>',
                    filter: {
                        placeholder: 'Активен',
                        type: uiGridConstants.filter.SELECT,
                        name: 'Enabled',
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: 'SubscribeDateStr',
                    displayName: 'Дата подписки',
                    width: 180,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Дата подписки',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'SubscribeFrom'
                            },
                            to: {
                                name: 'SubscribeTo'
                            }
                        }
                    }
                },
                {
                    name: 'UnsubscribeDateStr',
                    displayName: 'Дата отписки',
                    width: 180,
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div style="pointer-events: none;text-align:center;">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: 'Дата отписки',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'UnSubscribeFrom'
                            },
                            to: {
                                name: 'UnsubscribeTo'
                            }
                        }
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 50,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="Subscription/DeleteSubscription" params="{\'Ids\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'Subscription/DeleteSubscription',
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

        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Upload.upload({
                    url: '/Subscription/Import',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.Result === true) {
                        toaster.pop('success', 'Файл успешно загружен');
                        ctrl.grid.fetchData()
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке файла', data.Error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке файла', 'Файл не соответствует требованиям');
            }
        };
    };

    SubscriptionCtrl.$inject = ['Upload', 'toaster', '$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('subscription', ['uiGridCustom', 'urlHelper'])
      .controller('SubscriptionCtrl', SubscriptionCtrl);

})(window.angular);