; (function (ng) {
    'use strict';

    var DiscountsPriceRangeCtrl = function (uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, toaster) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'PriceRange',
                    displayName: 'Сумма заказа свыше',
                    enableCellEdit: true,
                    type: 'number',
                    filter: {
                        placeholder: 'Сумма заказа свыше',
                        type: 'number',
                        name: 'PriceRange',
                    }
                },
                {
                    name: 'PercentDiscount',
                    displayName: 'Скидка (%)',
                    enableCellEdit: true,
                    type: 'number',
                    filter: {
                        placeholder: 'Скидка (%)',
                        type: 'number',
                        name: 'PercentDiscount',
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditDiscountsPriceRangeCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/discountsPriceRange/modal/addEditDiscountsPriceRange/AddEditDiscountsPriceRange.html" ' +
                                        'data-resolve="{\'OrderPriceDiscountId\': row.entity.OrderPriceDiscountId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +
                            '<ui-grid-custom-delete url="discountsPriceRange/deleteItem" params="{\'OrderPriceDiscountId\': row.entity.OrderPriceDiscountId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'discountsPriceRange/deleteItems',
                        field: 'OrderPriceDiscountId',
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

    DiscountsPriceRangeCtrl.$inject = ['uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', 'toaster'];


    ng.module('discountsPriceRange', ['uiGridCustom', 'urlHelper'])
      .controller('DiscountsPriceRangeCtrl', DiscountsPriceRangeCtrl);

})(window.angular);