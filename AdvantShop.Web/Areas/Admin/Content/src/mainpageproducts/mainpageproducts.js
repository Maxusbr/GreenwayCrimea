; (function (ng) {
    'use strict';

    var MainPageProductsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, urlHelper, mainpageproductsService, $q, SweetAlert, toaster) {

        var ctrl = this,
            type = urlHelper.getUrlParam('type'),
            columnDefs = [
              {
                  name: 'ProductArtNo',
                  displayName: 'Артикул',
                  cellTemplate: '<div class="ui-grid-cell-contents"><a class="link-invert" ng-href="product/edit/{{row.entity.ProductId}}">{{COL_FIELD}}</a></div>',
                  width: 100
              },
              {
                  name: 'Name',
                  displayName: 'Название',
                  cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="product/edit/{{row.entity.ProductId}}">{{COL_FIELD}}</a></div>',
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
                  cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="product/edit/{{row.entity.ProductId}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a><ui-grid-custom-delete url="mainpageproducts/deletefromlist"  params="{\'ProductId\': row.entity.ProductId, type: \'' + type + '\'}"></ui-grid-custom-delete></div>'
              }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные из списка',
                        url: 'mainpageproducts/deleteProductsFromList?type=' + type,
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
            mainpageproductsService.addProducts(ng.extend({ type: type, }, result)).then(function (data) {
                var _result;

                if (data.result === true) {
                    _result = ctrl.grid.fetchData();

                    ctrl.catalogLeftMenu.updateData();
                } else {
                    return $q.reject(new Error());
                }

                return _result;
            }).catch(function () {
                toaster.pop('error', 'Ошибка при добавлении товара');
            });
        };

        ctrl.onInitGrid = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.initCatalogLeftMenu = function (catalogLeftMenu) {
            ctrl.catalogLeftMenu = catalogLeftMenu;
        };

        ctrl.onGridDeleteItem = function () {
            ctrl.catalogLeftMenu.updateData();
        };
    };

    MainPageProductsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'urlHelper', 'mainpageproductsService', '$q', 'SweetAlert', 'toaster'];

    ng.module('mainpageproducts', ['uiGridCustom', 'productsSelectvizr'])
      .controller('MainPageProductsCtrl', MainPageProductsCtrl);

})(window.angular);