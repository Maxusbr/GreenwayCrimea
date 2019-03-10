; (function (ng) {
    'use strict';

    var OffersSelectvizrCtrl = function (uiGridConstants) {

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
                        ids: []
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
            }, ctrl.selectionCustom.getSelectedParams('OfferId')));
        };

        ctrl.gridSelectionOnChange = function () {
            if (ctrl.selectvizrOnChange != null && ctrl.selectionCustom != null) {
                ctrl.selectvizrOnChange(ng.extend({
                    categoryId: ctrl.selectvizrGridParams.categoryId,
                    //ids: ctrl.ids
                }, ctrl.selectionCustom.getSelectedParams('OfferId')));
            }
        };

        ctrl.gridOnFetch = function (grid) {
            if (ctrl.selectvizrGridOnFetch != null) {
                ctrl.selectvizrGridOnFetch({ grid: grid });
            }
        }
    };

    OffersSelectvizrCtrl.$inject = ['uiGridConstants'];

    ng.module('offersSelectvizr', ['uiGridCustom', 'ui.grid'])
      .controller('OffersSelectvizrCtrl', OffersSelectvizrCtrl);

})(window.angular);