; (function (ng) {
    'use strict';

    var PropertyValuesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $q, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Value',
                    displayName: 'Значение',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Значение',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Search'
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    width: 80,
                    enableCellEdit: true,
                },
                {
                    name: 'ProductsCount',
                    displayName: 'Исп. у товаров',
                    width: 80,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 50,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="propertyvalues/deletePropertyValue" params="{\'propertyValueId\': row.entity.PropertyValueId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'propertyvalues/deletePropertyValues',
                        field: 'PropertyValueId',
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

        ctrl.initPropertyGroups = function (propertyGroups) {
            ctrl.propertyGroups = propertyGroups;
        };

        ctrl.updatePropertyGroups = function(result) {
            toaster.pop('success', '', 'Значение свойства добавлено');
            ctrl.propertyGroups.fetch();
        }

    };

    PropertyValuesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$q', 'SweetAlert'];


    ng.module('propertyvalues', ['uiGridCustom', 'urlHelper', 'propertyGroups'])
      .controller('PropertyValuesCtrl', PropertyValuesCtrl);

})(window.angular);