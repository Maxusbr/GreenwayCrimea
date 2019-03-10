; (function (ng) {
    'use strict';

    var PropertiesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $q, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents">'+
                                        '<ui-modal-trigger data-controller="\'ModalAddEditPropertyCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/properties/modal/addEditProperty/addEditProperty.html" ' +
                                        'data-resolve="{\'propertyId\': row.entity.PropertyId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                        '<a ng-href="">{{COL_FIELD}}</a>' +
                                   '</ui-modal-trigger></div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name'
                    }
                },
                {
                    name: 'GroupName',
                    displayName: 'Группа',
                },
                {
                    name: 'UseInFilter',
                    displayName: 'Показ. в фильтре',
                    width: 80,
                    enableCellEdit: true,
                    type: 'checkbox',
                    filter: {
                        placeholder: 'Показывать в фильтре',
                        type: uiGridConstants.filter.SELECT,
                        name: 'UseInFilter',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'UseInDetails',
                    displayName: 'Показ. в карточке товара',
                    width: 80,
                    enableCellEdit: true,
                    type: 'checkbox',
                    filter: {
                        placeholder: 'Показывать в карточке товара',
                        type: uiGridConstants.filter.SELECT,
                        name: 'UseInDetails',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'UseInBrief',
                    displayName: 'Показ. в брифе',
                    width: 80,
                    enableCellEdit: true,
                    type: 'checkbox',
                    filter: {
                        placeholder: 'Показывать в брифе',
                        type: uiGridConstants.filter.SELECT,
                        name: 'UseInBrief',
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    width: 80,
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Сортировка',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'SortingFrom'
                            },
                            to: {
                                name: 'SortingTo'
                            }
                        }
                    },
                },
                {
                    name: 'ProductsCount',
                    displayName: 'Используется у товаров',
                    width: 80,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 90,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +

                            '<ui-modal-trigger data-controller="\'ModalAddEditPropertyCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/properties/modal/addEditProperty/addEditProperty.html" ' +
                                        'data-resolve="{\'propertyId\': row.entity.PropertyId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +
                            '<a ng-href="propertyValues?propertyId={{row.entity.PropertyId}}" class="link-invert ui-grid-custom-service-icon fa fa-list"></a>' +
                            '<ui-grid-custom-delete url="properties/deleteProperty" params="{\'propertyId\': row.entity.PropertyId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'propertyValues?propertyId={{row.entity.PropertyId}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'properties/deleteproperties',
                        field: 'PropertyId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                    {
                        template: '<ui-modal-trigger data-on-close="$ctrl.gridOnAction()" data-controller="\'ModalChangePropertyGroupCtrl\'" controller-as="ctrl" ' +
                            'data-resolve=\"{params:$ctrl.getSelectedParams(\'PropertyId\')}\" template-url="../areas/admin/content/src/properties/modal/changePropertyGroup/changePropertyGroup.html"> ' +
                            'Изменить группу</ui-modal-trigger>'
                    },
                    {
                        text: 'Выводить в фильтр',
                        url: 'properties/useInFilter',
                        field: 'PropertyId'
                    },
                    {
                        text: 'Не выводить в фильтр',
                        url: 'properties/notUseInFilter',
                        field: 'PropertyId'
                    },

                    {
                        text: 'Использовать в карточке товара',
                        url: 'properties/useInDetails',
                        field: 'PropertyId'
                    },
                    {
                        text: 'Не использовать в карточке товара',
                        url: 'properties/notUseInDetails',
                        field: 'PropertyId'
                    },

                    {
                        text: 'Использовать в брифе',
                        url: 'properties/useInBrief',
                        field: 'PropertyId'
                    },
                    {
                        text: 'Не использовать в брифе',
                        url: 'properties/notUseInBrief',
                        field: 'PropertyId'
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
            toaster.pop('success', '', 'Группа свойств создана');
            ctrl.propertyGroups.fetch();
        }

    };

    PropertiesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$q', 'SweetAlert'];


    ng.module('properties', ['uiGridCustom', 'urlHelper', 'propertyGroups'])
      .controller('PropertiesCtrl', PropertiesCtrl);

})(window.angular);