; (function (ng) {
    'use strict';

    var ProductListsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, $http, $q, SweetAlert, toaster) {

        var ctrl = this;
        
        /* product lists */
        ctrl.gridProductListsOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="productlists/products/{{row.entity.Id}}">{{COL_FIELD}}</a></div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    width: 80
                },
                {
                    name: 'Enabled',
                    displayName: 'Активность',
                    width: 100,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><label class="adv-checkbox-label">' +
                            '<input type="checkbox" disabled ng-model="row.entity.Enabled" class="adv-checkbox-input control-checkbox" data-e2e="switchOnOffSelect" />' +
                            '<span class="adv-checkbox-emul" data-e2e="switchOnOffInput"></span>' +
                        '</label></div>'
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 96,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div class="js-grid-not-clicked">' +
                            
                            '<ui-modal-trigger data-controller="\'ModalAddEditProductListCtrl\'" controller-as="ctrl" size="lg" ' +
                                        'template-url="../areas/admin/content/src/productlists/modal/addEditProductList/addEditProductList.html" ' +
                                        'data-resolve="{\'id\': row.entity.Id}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +

                            '<ui-grid-custom-delete url="productLists/deleteProductList" params="{\'id\': row.entity.Id}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ],
            uiGridCustom: {
                rowUrl: 'productlists/products/{{row.entity.Id}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'productlists/deleteProductLists',
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
            ctrl.gridProductLists = grid;
        };



        /* products */
        ctrl.gridProductsOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'ProductArtNo',
                    displayName: 'Артикул',
                    width: 100
                },
                {
                    name: 'Name',
                    displayName: 'Название',
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    width: 100,
                    type: 'number',
                    enableCellEdit: true,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<ui-grid-custom-delete url="productlists/deletefromlist"  params="{\'ListId\': row.entity.ListId, \'ProductId\': row.entity.ProductId}"></ui-grid-custom-delete>' +
                        '</div>'
                }
            ],
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные из списка',
                        url: 'productlists/deleteProductsFromList',
                        field: 'ProductId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                ]
            }
        });

        ctrl.addProductsModal = function (result) {
            $http.post('productlists/addproducts', ng.extend({ listId: ctrl.listId, }, result)).then(function (response) {
                var data = response.data,
                    _result;
                if (data.result === true) {
                    _result = ctrl.gridProducts.fetchData();
                } else {
                    _result = $q.reject()
                }

                return _result;

            }).catch(function () {
                toaster.pop('error', 'Ошибка при добавлении товаров')
            });
        };

        ctrl.gridProductsOnInit = function (grid) {
            ctrl.gridProducts = grid;
        };

        ctrl.initCatalogLeftMenu = function (catalogLeftMenu) {
            ctrl.catalogLeftMenu = catalogLeftMenu;
        };

        ctrl.onGridDeleteItem = function () {
            ctrl.catalogLeftMenu.updateData();
        };

        ctrl.onAddList = function () {
            ctrl.gridProductLists.fetchData();
            ctrl.catalogLeftMenu.updateData();
        };
    };

    ProductListsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', '$http', '$q', 'SweetAlert', 'toaster'];
    
    ng.module('productlists', ['uiGridCustom'])
      .controller('ProductListsCtrl', ProductListsCtrl);

})(window.angular);