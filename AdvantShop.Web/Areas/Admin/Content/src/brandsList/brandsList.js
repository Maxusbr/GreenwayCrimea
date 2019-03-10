; (function (ng) {
    'use strict';

    var BrandsListCtrl = function ($q, $location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, brandsListService, SweetAlert) {
        var ctrl = this,
            columnDefs = [
                {
                    name: 'PhotoSrc',
                    headerCellClass: 'ui-grid-custom-header-cell-center',
                    displayName: 'Изобр.',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a class="ui-grid-custom-flex-center ui-grid-custom-link-for-img" ng-href="brands/edit/{{row.entity.BrandId}}"><img class="ui-grid-custom-col-img" ng-src="{{row.entity.PhotoSrc}}"></a></div>',
                    width: 80,
                    enableSorting: false,
                    filter: {
                        placeholder: 'Изображение',
                        type: uiGridConstants.filter.SELECT,
                        name: 'HasPhoto',
                        selectOptions: [{ label: 'С фотографией', value: true }, { label: 'Без фотографии', value: false }]
                    }
                },
                {
                    name: 'BrandName',
                    displayName: 'Название',
                    enableCellEdit: false,
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="brands/edit/{{row.entity.BrandId}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'BrandName'
                    }
                },
                {
                    name: 'CountryName',
                    displayName: 'Страна',
                    enableCellEdit: false,
                    width: 250,
                    filter: {
                        placeholder: 'Страна',
                        type: uiGridConstants.filter.SELECT,
                        name: 'CountryId',
                        fetch: 'countries/getcountries'
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    type: 'number',
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
                    name: 'Enabled',
                    displayName: 'Актив.',
                    enableCellEdit: false,
                    cellTemplate: '<ui-grid-custom-switch row="row"></ui-grid-custom-switch>',
                    width: 76,
                    filter: {
                        placeholder: 'Активность',
                        name: 'Enabled',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div class="js-grid-not-clicked"><a ng-href="brands/edit/{{row.entity.BrandId}}" class="ui-grid-custom-service-icon fa fa-pencil"></a><ui-grid-custom-delete url="brands/deletebrand" params="{\'BrandId\': row.entity.BrandId}"></ui-grid-custom-delete></div></div>'
                }
            ];



        ctrl.categories = [];
        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'brands/edit/{{row.entity.BrandId}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'brands/deletebrands',
                        field: 'BrandId',
                        before: function() {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function(result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                    {
                        text: 'Сделать активными',
                        url: 'brands/activatebrands',
                        field: 'BrandId'
                    },
                    {
                        text: 'Сделать неактивными',
                        url: 'brands/disablebrands',
                        field: 'BrandId'
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
    };

    BrandsListCtrl.$inject = ['$q', '$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'brandsListService', 'SweetAlert'];

    ng.module('brandsList', ['uiGridCustom'])
      .controller('BrandsListCtrl', BrandsListCtrl);

})(window.angular);