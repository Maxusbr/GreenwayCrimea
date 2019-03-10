; (function (ng) {
    'use strict';

    var ModalOffersSelectvizrCtrl = function ($uibModalInstance, uiGridCustomConfig, uiGridConstants, $http, domService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.selectvizrTreeUrl = 'catalog/categoriestree';
            ctrl.selectvizrGridUrl = 'catalog/getOffersCatalog';

            ctrl.selectvizrGridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: [
                  {
                      name: 'ArtNo',
                      displayName: 'Артикул',
                      width: 100,
                      enableSorting: false,
                      //filter: {
                      //    placeholder: 'Артикул',
                      //    type: uiGridConstants.filter.INPUT,
                      //    name: 'ArtNo',
                      //}
                  },
                  {
                      name: 'Name',
                      displayName: 'Название',
                      enableSorting: false,
                      //filter: {
                      //    placeholder: 'Название',
                      //    type: uiGridConstants.filter.INPUT,
                      //    name: 'Name',
                      //}
                  },
                  {
                      name: 'ColorName',
                      displayName: 'Цвет',
                      width: 100,
                      enableSorting: false,
                  },
                  {
                      name: 'SizeName',
                      displayName: 'Размер',
                      width: 100,
                      enableSorting: false,
                  },
                  {
                      name: 'PriceFormatted',
                      displayName: 'Цена',
                      width: 120,
                      enableSorting: false,
                  }
                ],

                showTreeExpandNoChildren: false,
                uiGridCustom: {
                    rowClick: function ($event, row, grid) {
                        if (row.treeNode.children && row.treeNode.children.length > 0 && domService.closest($event.target, '.ui-grid-tree-base-row-header-buttons') == null) {
                            grid.gridApi.treeBase.toggleRowTreeState(row);
                        }
                    },
                    rowClasses: function (row) {
                        return row.treeNode.children == null || row.treeNode.children.length === 0 ? 'ui-grid-custom-prevent-pointer' : '';
                    }
                }
            });

            if (ctrl.$resolve.multiSelect === false) {
                ng.extend(ctrl.selectvizrGridOptions, {
                    multiSelect: false,
                    modifierKeysToMultiSelect: false,
                    enableRowSelection: true,
                    enableRowHeaderSelection: false
                });
            }
        };

        ctrl.onChange = function (categoryId, ids, selectMode) {
            ctrl.data = {
                categoryId: categoryId,
                ids: ids,
                selectMode: selectMode
            }
        };

        ctrl.gridOnFetch = function (grid) {
            if (grid != null && grid.gridOptions != null && grid.gridOptions.data != null && grid.gridOptions.data.length > 0) {
                for (var i = 0, len = grid.gridOptions.data.length; i < len; i++) {
                    if (grid.gridOptions.data[i].Main === true) {
                        grid.gridOptions.data[i].$$treeLevel = 0;
                    }
                }
            }
        }

        ctrl.select = function () {
            if (ctrl.data.selectMode == "all") {
                $http.get('catalog/getCatalogOfferIds', { params: ctrl.data }).then(function (response) {
                    if (response.data != null) {
                        ctrl.data.selectMode = "none";
                        ctrl.data.ids = response.data.ids.filter(function (item) {
                            return ctrl.data.ids.indexOf(item) === -1;
                        });
                    }
                    $uibModalInstance.close(ctrl.data);
                });
            } else {
                $uibModalInstance.close(ctrl.data);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalOffersSelectvizrCtrl.$inject = ['$uibModalInstance', 'uiGridCustomConfig', 'uiGridConstants', '$http', 'domService'];

    ng.module('uiModal')
        .controller('ModalOffersSelectvizrCtrl', ModalOffersSelectvizrCtrl);

})(window.angular);