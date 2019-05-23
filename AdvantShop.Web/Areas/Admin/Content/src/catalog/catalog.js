; (function (ng) {
    'use strict';

    var CatalogCtrl = function ($q, $filter, $http, $location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, urlHelper, catalogService, SweetAlert, toaster) {

        var ctrl = this,
            showMethod = urlHelper.getUrlParam('showMethod'),
            columnDefs = [
                {
                    name: '_noopColumnArtNo',
                    visible: false,
                    filter: {
                        placeholder: 'Артикул',
                        type: uiGridConstants.filter.INPUT,
                        name: 'ArtNo',
                    }
                },
                {
                    name: '_noopColumnName',
                    visible: false,
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: 'ProductArtNo',
                    displayName: 'Арт.',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a class="link-invert word-break" ng-href="product/edit/{{row.entity.ProductId}}" title="{{COL_FIELD}}">{{COL_FIELD}}</a></div>',
                    width: 80
                },
                {
                    name: 'PhotoSrc',
                    headerCellClass: 'ui-grid-custom-header-cell-center',
                    displayName: 'Изобр.',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a class="ui-grid-custom-flex-center ui-grid-custom-link-for-img" ng-href="product/edit/{{row.entity.ProductId}}"><img class="ui-grid-custom-col-img" ng-src="{{row.entity.PhotoSrc}}"></a></div>',
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
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="product/edit/{{row.entity.ProductId}}">{{COL_FIELD}}</a></div>',
                    //enableCellEdit: true
                },
                {
					name: 'Weight',
					displayName: 'Объем',
					enableCellEdit: true,
					type: 'number',
					width: 80,
                },
                {
                    name: 'PriceString',
                    displayName: 'Цена',
                    enableCellEdit: true,
                    type: 'text',
                    uiGridCustomEdit: {
                        attributes: {
                            'input-ghost': '""',
                            'ng-pattern': 'uiGridEditCustom.pattern'
                        },
                        customModel: 'priceEdit',
                        onInit: function (rowEntity, colDef, newValue, uiGridEditCustom) {
                            uiGridEditCustom.priceEdit = rowEntity.PriceString;
                        },
                        onActive: function (rowEntity, colDef, newValue, uiGridEditCustom) {
                            uiGridEditCustom.priceEdit = $filter('number')(rowEntity.Price);
                            uiGridEditCustom.pattern = '^[\\d\\.,\\s]*$';
                        },
                        onDeactive: function (rowEntity, colDef, newValue, uiGridEditCustom) {
                            uiGridEditCustom.priceEdit = rowEntity.PriceString;
                            uiGridEditCustom.pattern = null;
                        },
                        onChange: function (rowEntity, colDef, newValue, uiGridEditCustom) {
                            rowEntity.Price = newValue;
                            uiGridEditCustom.pattern = null;
                        }
                    },
                    width: 100,
                    filter: {
                        placeholder: 'Цена',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'PriceFrom'
                            },
                            to: {
                                name: 'PriceTo'
                            },
                        },
                        fetch: 'catalog/getpricerangeforpaging'
                    },
                    cellEditableCondition: function ($scope) {
                        return $scope.row.entity.OffersCount === 1;
                    }
                },
                {
                    name: 'Amount',
                    displayName: 'Кол-во',
                    enableCellEdit: true,
                    type: 'number',
                    width: 80,
                    cellEditableCondition: function ($scope) {
                        return $scope.row.entity.OffersCount === 1;
                    },
                    filter: {
                        placeholder: 'Количество',
                        type: 'range',
                        rangeOptions: {
                            from: {
                                name: 'AmountFrom'
                            },
                            to: {
                                name: 'AmountTo'
                            }
                        },
                        fetch: 'catalog/getamountrangeforpaging'
                    },
                }
            ];

        if (showMethod == null || showMethod.toLowerCase() === 'normal') {
            columnDefs.push({
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
            });
        }

        columnDefs = columnDefs.concat([{
            name: 'Enabled',
            displayName: 'Актив.',
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
            visible: false,
            name: 'BrandId',
            filter: {
                placeholder: 'Производитель',
                type: uiGridConstants.filter.SELECT,
                name: 'BrandId',
                fetch: 'catalog/getBrandList'
            }
        },
        {
            name: '_serviceColumn',
            displayName: '',
            width: 60,
            cellTemplate: '<div class="ui-grid-cell-contents"><div class="js-grid-not-clicked"><a ng-href="product/edit/{{row.entity.ProductId}}" class="ui-grid-custom-service-icon fa fa-pencil"></a><ui-grid-custom-delete url="catalog/deleteproduct" params="{\'ProductId\': row.entity.ProductId}"></ui-grid-custom-delete></div></div>'
        }])

        ctrl.categories = [];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowUrl: 'product/edit/{{row.entity.ProductId}}',
                selectionOptions: [
                                    {
                                        text: 'Удалить выделенные',
                                        url: 'catalog/deleteproducts',
                                        field: 'ProductId',
                                        before: function () {
                                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                            });
                                        }
                                    },
                                    {
                                        hide: showMethod == 'OnlyWithoutCategories',
                                        text: 'Удалить выделенные из категории',
                                        url: 'catalog/deletefromcategoryproducts',
                                        field: 'ProductId',
                                        before: function () {
                                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                                            });
                                        }
                                    },
                                    {
                                        template: '<ui-modal-trigger data-on-close="$ctrl.gridActionWithCallback($ctrl.clearStorage);" data-controller="\'ModalMoveProductInOtherCategoryCtrl\'" ' +
                                            'data-resolve=\"{params: $ctrl.getSelectedParams(\'ProductId\'), removeFromCurrentCategories: true }\" template-url="../areas/admin/content/src/_shared/modal/moveProductInOtherCategory/moveProductInOtherCategory.html">' +
                                            'Перенести товары в другую категорию</ui-modal-trigger>'
                                    },
                                    {
                                        template: '<ui-modal-trigger data-on-close="$ctrl.gridOnAction()" data-controller="\'ModalMoveProductInOtherCategoryCtrl\'" controller-as="ctrl" ' +
                                            'data-resolve=\"{params:$ctrl.getSelectedParams(\'ProductId\')}\" template-url="../areas/admin/content/src/_shared/modal/moveProductInOtherCategory/moveProductInOtherCategory.html">' +
                                            'Добавить товары в другую категорию</ui-modal-trigger>'
                                    },
                                    {
                                        text: 'Сделать активными',
                                        url: 'catalog/activateproducts',
                                        field: 'ProductId'
                                    },
                                    {
                                        text: 'Сделать неактивными',
                                        url: 'catalog/disableproducts',
                                        field: 'ProductId'
                                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.deleteCategory = function (categoryId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result) {
                    $http.post('category/delete', { id: categoryId }).then(function (response) {

                        if (response.data.result === true) {
                            if (response.data.needRedirect) {
                                window.location = 'catalog?categoryid=' + response.data.id;
                            }
                        } else {
                            toaster.pop('error', 'Ошибка при удалении', "");
                        }

                    });
                    $q.resolve('sweetAlertConfirm');
                }
                else {
                    $q.reject('sweetAlertCancel');
                }
            });
        };

        ctrl.initCatalogTreeview = function (jstree) {
            ctrl.jstree = jstree;
        };

        ctrl.initCatalogLeftMenu = function (catalogLeftMenu) {
            ctrl.catalogLeftMenu = catalogLeftMenu;
        };

        ctrl.onGridDeleteItem = function () {
            ctrl.jstree.refresh(true, true);
            ctrl.catalogLeftMenu.updateData();
        };

        ctrl.onDeleteChildCategories = function () {
            ctrl.jstree.refresh(true, true);
            ctrl.catalogLeftMenu.updateData();
        };
    };

    CatalogCtrl.$inject = ['$q', '$filter', '$http', '$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'urlHelper', 'catalogService', 'SweetAlert', 'toaster'];

    ng.module('catalog', ['uiGridCustom', 'categoriesBlock', 'urlHelper'])
      .controller('CatalogCtrl', CatalogCtrl);

})(window.angular);