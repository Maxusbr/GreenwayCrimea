; (function (ng) {
    'use strict';

    var returnCustomerRecordsListCtrl = function ($http, toaster, $uibModal, uiGridConstants, uiGridCustomConfig, SweetAlert, Upload, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {

        };

        ctrl.reset = function () {
            ctrl.grid.update();
        }

        var columnDefs = [
            {
                name: 'CustomerName',
                displayName: 'Имя пользователя',
                cellTemplate: '<div class="ui-grid-cell-contents"><a href="customers/edit/{{row.entity.CustomerID}}" target="_blank">{{COL_FIELD}}</a></div>',

                width: 200,
                filter: {
                    placeholder: 'Имя пользователя',
                    type: uiGridConstants.filter.INPUT,
                    name: 'CustomerName',
                }
            },
            {
                name: 'Email',
                displayName: 'Email',
                cellTemplate: '<div class="ui-grid-cell-contents"><a href=\"mailto:{{COL_FIELD}}\" target=\"_blank\">{{COL_FIELD}}</a></div>',

                width: 175,
                filter: {
                    placeholder: 'Email',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Email',
                }
            },
            {
                name: 'Phone',
                displayName: 'Phone',
                cellTemplate: '<div class="ui-grid-cell-contents"><a href=\"tel:{{COL_FIELD}}\" target=\"_blank\">{{COL_FIELD}}</a></div>',
                width: 125,
                filter: {
                    placeholder: 'Phone',
                    type: uiGridConstants.filter.INPUT,
                    name: 'Phone',
                    width: 100
                }
            },
            {
                name: 'LastActionDate',
                displayName: 'Последняя активность',
                cellTemplate: '<div class="ui-grid-cell-contents">{{COL_FIELD | date:"dd.MM.yy HH:mm"}}</div>',
                width: 100,
                filter: {
                    placeholder: 'Последняя активность',
                    type: 'datetime',
                    term: {
                        from: (new Date()).setMonth((new Date()).getMonth() - 1),
                        to: new Date()
                    },
                    datetimeOptions: {
                        from: {
                            name: 'LastActionDateFrom'
                        },
                        to: {
                            name: 'LastActionDateTo'
                        }
                    }
                }
            },
            {
                name: 'LastSendingDate',
                displayName: 'Дата последней отправки',
                cellTemplate: '<div class="ui-grid-cell-contents">{{COL_FIELD}}</div>',
                width: 100,
                filter: {
                    placeholder: 'Дата последней отправки',
                    type: 'datetime',
                    term: {
                        from: (new Date()).setMonth((new Date()).getMonth() - 1),
                        to: new Date()
                    },
                    datetimeOptions: {
                        from: {
                            name: 'LastSendingDateFrom'
                        },
                        to: {
                            name: 'LastSendingDateTo'
                        }
                    }
                }

            },
            {
                name: 'RecentlyView',
                displayName: 'Просмотренные товары',
                //cellTemplate: '<div class="ui-grid-cell-contents">' +
                cellTemplate: '<div style="height: 100%; text-align: center;">' +
                '<span ng-repeat="(productId, description) in row.entity.RecentlyView"><a title=\"{{ description }}\" alt=\"{{ description }}\" href="product/edit/{{ productId }}" target="_blank">{{ description }}</a><br /></span>'
                + '</div>',
                width: 250,
                enableSorting: false
            }
            /*,
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                '<div class="ui-grid-cell-contents"><div>' +
                '<ui-grid-custom-delete url="../rcpadmin/deleteReturnCustomerRecord" params="{\'CustomerID\': row.record.CustomerID}"></ui-grid-custom-delete>' +
                '</div></div>'
            }*/
        ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Отправить выбранным',
                        url: '../rcadmin/sendMails',
                        field: 'CustomerID',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите отправить письма выбранным пользователям?", { title: "Отправка писем" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        },
                        after: function () {
                            toaster.pop('success', '', 'Сообщения отправлены');
                        }
                    }
                    /*{
                        text: 'Удалить выделенные',
                        url: '../rcpadmin/deleteReturnCustomerRecords',
                        field: 'CustomerID',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }*/
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

    };

    returnCustomerRecordsListCtrl.$inject = ['$http', 'toaster', '$uibModal', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', 'Upload', '$q'];

    ng.module('returnCustomerRecordsList', [])
        .controller('returnCustomerRecordsListCtrl', returnCustomerRecordsListCtrl)
        .component('returnCustomerRecordsList', {
            templateUrl: '../modules/ReturnCustomer/content/Scripts/returnCustomer/templates/returnCustomerRecordsList.html',
            controller: 'returnCustomerRecordsListCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);