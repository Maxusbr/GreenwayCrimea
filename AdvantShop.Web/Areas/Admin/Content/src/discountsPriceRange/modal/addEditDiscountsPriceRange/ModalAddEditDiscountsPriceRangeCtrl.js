; (function (ng) {
    'use strict';

    var ModalAddEditDiscountsPriceRangeCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.OrderPriceDiscountId = params.OrderPriceDiscountId != null ? params.OrderPriceDiscountId : 0;
            ctrl.type = ctrl.OrderPriceDiscountId != 0 ? "edit" : "add";

            if (ctrl.type == "edit") {
                ctrl.getItem(ctrl.OrderPriceDiscountId);
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };


        ctrl.getItem = function (id) {
            $http.get('discountsPriceRange/getItem', { params: { orderPriceDiscountId: id } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.PriceRange = data.PriceRange;
                    ctrl.PercentDiscount = data.PercentDiscount;
                }
            });
        }


        ctrl.save = function() {
            var params = {
                OrderPriceDiscountId: ctrl.OrderPriceDiscountId,
                PriceRange: ctrl.PriceRange,
                PercentDiscount: ctrl.PercentDiscount
            }
            var url = ctrl.type == "add" ? 'discountsPriceRange/addItem' : 'discountsPriceRange/updateItem';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop('success', '', 'Изменения успешно сохранены');
                    $uibModalInstance.close();
                } else {
                    toaster.pop('error', 'Ошибка', data.errors);
                }
            });
        }
    };

    ModalAddEditDiscountsPriceRangeCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditDiscountsPriceRangeCtrl', ModalAddEditDiscountsPriceRangeCtrl);

})(window.angular);