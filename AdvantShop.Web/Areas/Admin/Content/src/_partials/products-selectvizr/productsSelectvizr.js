; (function (ng) {
    'use strict';

    var ProductsSelectvizrCtrl = function (uiGridConstants) {

        var ctrl = this;

        ctrl.ids = [];

        ctrl.$onInit = function () {
            ctrl.selectvizrGridParams = ctrl.selectvizrGridParams || {};
            ctrl.selectvizrGridParams.categoryId = ctrl.selectvizrGridParams.categoryId || 0;
            ctrl.selectvizrGridParams.showMethod = ctrl.selectvizrGridParams.showMethod || 'AllProducts';

        };

        ctrl.treeCallbacks = {
            select_node: function (event, data) {

                data.instance.open_node(data.node);

                ctrl.selectvizrGridParams.categoryId = data.node.id;

                ctrl.selectvizrGridParams.Page = 1;

                if (ctrl.selectvizrGridParams.categoryId == 0) {
                    ctrl.selectvizrGridParams.showMethod = 'AllProducts';
                } else {
                    ctrl.selectvizrGridParams.showMethod = null;
                }

                ctrl.selectvizrGridParams.Page = 1;

                ctrl.grid.setParams(ctrl.selectvizrGridParams);

                ctrl.grid.fetchData();

                if (ctrl.selectvizrOnChange != null) {
                    ctrl.selectvizrOnChange({
                        categoryId: ctrl.selectvizrGridParams.categoryId,
                        ids: ctrl.selectionCustom.getSelectedParams('ProductId').ids
                    });
                }
            }
        };

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.gridSelectionOnInit = function (selectionCustom) {
            ctrl.selectionCustom = selectionCustom;

            ctrl.selectvizrOnChange(ng.extend({
                categoryId: ctrl.selectvizrGridParams.categoryId,
            }, ctrl.selectionCustom.getSelectedParams('ProductId')));
        };

        ctrl.gridSelectionOnChange = function () {
            if (ctrl.selectvizrOnChange != null && ctrl.selectionCustom != null) {
                ctrl.selectvizrOnChange(ng.extend({
                    categoryId: ctrl.selectvizrGridParams.categoryId,
                    //ids: ctrl.ids
                }, ctrl.selectionCustom.getSelectedParams('ProductId')));
            }
        };

        ctrl.gridItemsSelectedFilterFn = function (rowEntity) {
            var result = false;

            if (ctrl.selectvizrGridItemsSelected != null && ctrl.selectvizrGridItemsSelected.length > 0) {

                for (var i = 0, len = ctrl.selectvizrGridItemsSelected.length; i < len; i++ ){
                    if (rowEntity.ProductId === ctrl.selectvizrGridItemsSelected[i]) {

                        ctrl.selectvizrGridItemsSelected.splice(i, 1);
  
                        result = true;

                        break;
                    }
                }

            }

            return result;
        }
    };

    ProductsSelectvizrCtrl.$inject = ['uiGridConstants'];

    ng.module('productsSelectvizr', ['uiGridCustom', 'ui.grid'])
      .controller('ProductsSelectvizrCtrl', ProductsSelectvizrCtrl);

})(window.angular);