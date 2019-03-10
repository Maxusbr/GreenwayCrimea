; (function (ng) {
    'use strict';

    var CardsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $http, $q, SweetAlert, customerFieldsService) {
        
        var ctrl = this,
            columnDefs = [
            {
                name: 'CardNumber',
                displayName: '№ карты',
                cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="cards/edit/{{row.entity.CardId}}">{{COL_FIELD}}</a></div>',
                filter: {
                    placeholder: '№ карты',
                    type: uiGridConstants.filter.INPUT,
                    name: 'CardNumber'
                }
            },
            {
                name: 'FIO',
                displayName: 'ФИО',
            },
            {
                name: 'GradeName',
                displayName: 'Грейд',
                filter: {
                    placeholder: 'Грейд',
                    type: uiGridConstants.filter.SELECT,
                    name: 'GradeId',
                    fetch: 'grades/getGradesSelectItems'
                }
            },
            {
                name: 'GradePersent',
                displayName: 'Процент бонуса'
            },
            {
                name: 'CreatedFormatted',
                displayName: 'Дата выдачи карты',
                filter: {
                    placeholder: 'Дата выдачи карты',
                    type: 'datetime',
                    term: {
                        from: (new Date()).setMonth((new Date()).getMonth() - 1),
                        to: new Date()
                    },
                    datetimeOptions: {
                        from: { name: 'CreatedFrom' },
                        to: { name: 'CreatedTo' }
                    }
                }
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 70,
                cellTemplate:
                    '<div class="ui-grid-cell-contents"><div>' +
                        '<a href="cards/edit/{{row.entity.CardId}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                        '<ui-grid-custom-delete url="cards/deleteCard" params="{\'cardId\': row.entity.CardId}"></ui-grid-custom-delete>' +
                    '</div></div>'
            }
        ];

        // more filters
        columnDefs.push(
                {
                    name: '_noopColumnBonusAmount',
                    visible: false,
                    filter: {
                        placeholder: 'Кол-во бонусов',
                        type: 'range',
                        rangeOptions: {
                            from: { name: 'BonusAmountFrom' },
                            to: { name: 'BonusAmountTo' }
                        }
                    }
                },
                {
                    name: '_noopColumnFIO',
                    visible: false,
                    filter: {
                        placeholder: 'ФИО',
                        type: uiGridConstants.filter.INPUT,
                        name: 'FIO'
                    }
                },
                {
                    name: '_noopColumnEmail',
                    visible: false,
                    filter: {
                        placeholder: 'E-mail',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Email'
                    }
                },
                {
                    name: '_noopColumnMobilePhone',
                    visible: false,
                    filter: {
                        placeholder: 'Мобильный телефон',
                        type: uiGridConstants.filter.INPUT,
                        name: 'MobilePhone'
                    }
                },
                {
                    name: '_noopColumnRegDate',
                    visible: false,
                    filter: {
                        placeholder: 'Дата регистрации',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: { name: 'RegDateFrom' },
                            to: { name: 'RegDateTo' }
                        }
                    }
                },
                {
                    name: '_noopColumnLocation',
                    visible: false,
                    filter: {
                        placeholder: 'Местоположение',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Location',
                    }
                },
                {
                    name: '_noopColumnOrdersCount',
                    visible: false,
                    filter: {
                        placeholder: 'Кол-во оплаченных заказов',
                        type: 'range',
                        rangeOptions: {
                            from: { name: 'OrdersCountFrom' },
                            to: { name: 'OrdersCountTo' },
                        }
                    },
                },
                {
                    name: '_noopColumnOrdersSum',
                    visible: false,
                    filter: {
                        placeholder: 'Сумма заказов',
                        type: 'range',
                        rangeOptions: {
                            from: { name: 'OrderSumFrom' },
                            to: { name: 'OrderSumTo' },
                        }
                    },
                }
        );
        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'cards/edit/{{row.entity.CardId}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'cards/deleteCards',
                        field: 'CardId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.initCard = function (cardId, isEditMode, cardnumber) {
            ctrl.cardId = cardId;
            ctrl.customerId = cardId;
            ctrl.isEditMode = isEditMode;
            ctrl.cardNumber = cardnumber;
        }

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.gridOnFilterInit = function (filter) {
            ctrl.gridFilter = filter;
            customerFieldsService.getFilterColumns().then(function (columns) {
                Array.prototype.push.apply(ctrl.gridOptions.columnDefs, columns);
                ctrl.gridFilter.updateColumns();
            });
        };

        ctrl.deleteCard = function (cardId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('cards/deleteCard', { cardId: cardId }).then(function (response) {
                        var data = response.data;

                        if (data.result === true) {
                            $window.location.assign('cards');
                        } else {
                            data.errors.forEach(function (error) {
                                toaster.pop('error', '', error);
                            });
                        }
                    });
                }
            });
        }

        ctrl.startExport = function ()
        {
            $window.location.assign('cards/ExportCards');
        }
    };

    CardsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$http', '$q', 'SweetAlert', 'customerFieldsService'];


    ng.module('cards', ['uiGridCustom', 'urlHelper'])
      .controller('CardsCtrl', CardsCtrl);

})(window.angular);