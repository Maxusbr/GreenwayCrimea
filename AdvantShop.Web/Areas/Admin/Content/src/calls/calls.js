; (function (ng) {
    'use strict';

    var CallsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {


        var ctrl = this,
            columnDefs = [
                {
                    name: 'Type',
                    displayName: 'Тип',
                    enableCellEdit: false,
                    enableSorting: false,
                    width: 50,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<span class="icon-call" ng-class="grid.appScope.$ctrl.gridExtendCtrl.getTypeIcon(row.entity.TypeString, row.entity.CalledBack)" title="{{grid.appScope.$ctrl.gridExtendCtrl.getTypeTitle(row.entity)}}"></span>' +
                        '</div>',
                    filter: {
                        placeholder: 'Тип',
                        type: uiGridConstants.filter.SELECT,
                        name: 'Type',
                        fetch: 'calls/getTypesSelectOptions'
                    }
                },
                {
                    name: 'CallDateFormatted',
                    displayName: 'Дата',
                    enableCellEdit: false,
                    width: 100,
                    filter: {
                        placeholder: 'Дата',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'CallDateFrom'
                            },
                            to: {
                                name: 'CallDateTo'
                            }
                        }
                    },
                },
                {
                    name: 'SrcNum',
                    displayName: 'Откуда',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Откуда',
                        type: uiGridConstants.filter.INPUT,
                        name: 'SrcNum',
                    }
                },
                {
                    name: 'DstNum',
                    displayName: 'Куда',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Куда',
                        type: uiGridConstants.filter.INPUT,
                        name: 'DstNum',
                    }
                },
                {
                    name: 'Extension',
                    displayName: 'Добавочный номер',
                    enableCellEdit: false,
                    width: 100,
                    filter: {
                        placeholder: 'Добавочный номер',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Extension',
                    }
                },
                {
                    name: 'DurationFormatted',
                    displayName: 'Продолжительность',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Продолжительность, в секундах',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'DurationFrom'
                            },
                            to: {
                                name: 'DurationTo'
                            }
                        }
                    }
                },
                {
                    name: 'RecordLink',
                    displayName: 'Запись разговора',
                    enableCellEdit: false,
                    enableSorting: false,
                    width: 300,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div ng-if="row.entity.CallAnswerDate != null">' +
                            '<call-record call-id="row.entity.Id" operator-type="row.entity.OperatorType"></call-record>' +
                        '</div></div>'
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="calls/deleteCall" params="{\'callId\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'calls/deleteCalls',
                        field: 'Id',
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

        ctrl.getTypeIcon = function (type, calledBack) {
            if (type == null) return '';

            switch (type.toLowerCase()) {
                case 'in':
                    return 'calltype-in';
                case 'out':
                    return 'calltype-out';
                case 'missed':
                    if (calledBack === true)
                        return 'calltype-callback';
                    return 'calltype-missed';
            }
        }

        ctrl.getTypeTitle = function (call) {
            if (call.TypeString == null) return '';
            var result = call.TypeFormatted;
            if (call.TypeString.toLowerCase() == 'missed') {
                if (call.CalledBack === false && call.HangupStatus != 0)
                    result += ', ' + call.HangupStatusFormatted;
                else if (call.CalledBack === true)
                    result += ', перезвонили';
            }
            return result;
        }
    };

    CallsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('calls', ['uiGridCustom', 'urlHelper', 'callRecord'])
      .controller('CallsCtrl', CallsCtrl);

})(window.angular);