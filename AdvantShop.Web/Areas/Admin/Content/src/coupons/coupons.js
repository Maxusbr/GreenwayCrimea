; (function (ng) {
    'use strict';

    var CouponsCtrl = function (uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, toaster) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Code',
                    displayName: 'Код купона',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Код купона',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Code',
                    }
                },
                {
                    name: 'TypeFormatted',
                    displayName: 'Тип купона',
                    filter: {
                        placeholder: 'Тип купона',
                        type: uiGridConstants.filter.SELECT,
                        name: 'Type',
                        fetch: 'coupons/getTypes'
                    }
                },
                {
                    name: 'Value',
                    displayName: 'Значение',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Значение',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Value',
                    }
                },
                {
                    name: 'ExpirationDateFormatted',
                    displayName: 'Дата окончания',
                    width: 150,
                    filter: {
                        placeholder: 'Дата окончания',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'ExpirationDateFrom'
                            },
                            to: {
                                name: 'ExpirationDateTo'
                            }
                        }
                    }
                },
                {
                    name: 'ActualUses',
                    displayName: 'Использован',
                    cellTemplate: '<div class="ui-grid-cell-contents">{{row.entity.ActualUses}} / {{row.entity.PossibleUses}}</div>',
                },
                {
                    name: 'Enabled',
                    displayName: 'Активность',
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
                    name: 'MinimalOrderPrice',
                    displayName: 'Мин. сумма заказа',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Мин. сумма заказа',
                        type: uiGridConstants.filter.INPUT,
                        name: 'MinimalOrderPrice',
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
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditCouponCtrl\'" controller-as="ctrl" size="middle" ' +
                                        'template-url="../areas/admin/content/src/coupons/modal/addEditCoupon/addEditCoupon.html" ' +
                                        'data-resolve="{\'CouponId\': row.entity.CouponId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="coupons/deleteCoupon" params="{\'CouponId\': row.entity.CouponId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'coupons/deleteCoupons',
                        field: 'CouponId',
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

        ctrl.changeDiscountsState = function() {
            $http.post('discountsPriceRange/enableDiscounts', { state: ctrl.enableDiscounts }).then(function (response) {
                toaster.pop('success', '', 'Изменения успешно сохранены');
            });
        };
    };

    CouponsCtrl.$inject = ['uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', 'toaster'];


    ng.module('coupons', ['uiGridCustom', 'urlHelper'])
      .controller('CouponsCtrl', CouponsCtrl);

})(window.angular);