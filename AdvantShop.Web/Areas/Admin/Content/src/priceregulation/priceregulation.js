; (function (ng) {
    'use strict';

    var PriceRegulationCtrl = function ($http, toaster, $uibModal) {

        var ctrl = this;

        ctrl.value = 0;
        ctrl.chooseProducts = false;
        ctrl.action = "Increment";
        ctrl.valueOption = "Percent";


        ctrl.categoryIds = [];


        ctrl.treeCallbacks = {
            select_node: function (event, data) {
                var tree = ng.element(event.target).jstree(true);
                ctrl.categoryIds = tree.get_selected(false);
            },

            deselect_node: function (event, data) {
                var tree = ng.element(event.target).jstree(true);
                ctrl.categoryIds = tree.get_selected(false);
            },
        };


        ctrl.changePrices = function () {

            if (ctrl.blockButton) {
                return;
            }

            ctrl.blockButton = true;

            var params = {
                chooseProducts: ctrl.chooseProducts,
                action: ctrl.action,
                valueOption: ctrl.valueOption,
                value: ctrl.value,
                categoryIds: ctrl.categoryIds
            };

            $http.post('priceregulation/changePrices', params).then(function (response) {
                var data = response.data;
                //if (data.result === true) {
                //    toaster.pop('success', '', data.msg);
                //} else {
                //    toaster.pop('error', 'Ошибка', data.msg);
                //}

                $uibModal.open({
                    bindToController: true,
                    controller: 'ResultPriceRegulationCtrl',
                    controllerAs: 'ctrl',
                    templateUrl: '../areas/admin/content/src/priceregulation/modals/resultPriceRegulation/resultPriceRegulation.html',
                    resolve: {
                        msg: function () { return data.msg; },
                        title: function () { return data.result === true ? 'Регулирование цен' : 'Ошибка'; }
                    },
                    backdrop: 'static'
                });

                ctrl.blockButton = false;
            });
        };
    };

    PriceRegulationCtrl.$inject = ['$http', 'toaster', '$uibModal'];


    ng.module('priceregulation', [])
      .controller('PriceRegulationCtrl', PriceRegulationCtrl);

})(window.angular);