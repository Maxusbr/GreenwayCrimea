; (function (ng) {
    'use strict';

    var PaymentListCtrl = function ($anchorScroll, $location) {
        var ctrl = this;

        ctrl.isProgress = null;

        $anchorScroll.yOffset = 50;

        //ctrl.selectedItemIndex = 0;
        //ctrl.collapsed = true;
        ctrl.visibleItems = Number.POSITIVE_INFINITY;

        ctrl.changePayment = function (payment, index) {

            if (index != null) {
                ctrl.selectedItemIndex = index;
            }

            ctrl.change({
                payment: payment
            });
        };

        ctrl.changePaymentControl = function (payment) {

            for (var i = ctrl.items.length - 1; i >= 0; i--) {
                if (ctrl.items[i] == payment) {
                    ctrl.selectedItemIndex = i;
                    break;
                }
            }

            ctrl.change({
                payment: payment
            });
        };


        ctrl.calc = function (index) {
            var selectItemPos = index + 1;

            ctrl.selectedItemIndex = index;

            ctrl.visibleItems = selectItemPos > ctrl.countVisibleItems ? selectItemPos : ctrl.countVisibleItems;

            return selectItemPos;
        };


        ctrl.toggleVisible = function () {

            var selectItemPos = ctrl.calc(ctrl.selectedItemIndex);

            if (ctrl.collapsed === true) {
                ctrl.visibleItems = ctrl.items.length;
                ctrl.collapsed = false;
            } else {

                if (selectItemPos === ctrl.items.length) {
                    return;
                }

                ctrl.visibleItems = selectItemPos > ctrl.countVisibleItems ? selectItemPos : ctrl.countVisibleItems;
                ctrl.collapsed = true;

                $location.hash(ctrl.anchor);
                $anchorScroll();
            }
        };

        ctrl.setSelectedIndex = function (index) {

            var selectItemPos = ctrl.calc(index);

            if (selectItemPos === ctrl.items.length) {
                ctrl.collapsed = false;
            } else {
                ctrl.collapsed = true;
            }
        };

    };

    ng.module('payment')
      .controller('PaymentListCtrl', PaymentListCtrl);

    PaymentListCtrl.$inject = ['$anchorScroll', '$location'];

})(window.angular);