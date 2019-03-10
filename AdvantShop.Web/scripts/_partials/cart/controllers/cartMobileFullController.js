; (function (ng) {
    'use strict';

    var CartMobileFullCtrl = function ($rootScope, cartService) {
        var ctrl = this;
        
        cartService.getData().then(function (data) {
            ctrl.cartData = data;
        });

        ctrl.getModuleData = function () {
            cartService.getModuleData().then(function (data) {
                ctrl.moduleHtml = data;
            });
        };

        ctrl.getModuleData();
                
        ctrl.updateAmount = function (value, itemId) {

            var item = {
                Key: itemId,
                Value: value
            };
            cartService.updateAmount([item]).then(function () {
                //moduleService.update('fullcartmessage');
            });
            ctrl.refresh
            ctrl.getModuleData();
        };

        ctrl.remove = function (shoppingCartItemId) {
            cartService.removeItem(shoppingCartItemId).then(function () {
                //moduleService.update('fullcartmessage');
            });
        };
        ctrl.clear = function () {
            cartService.clear().then(function () {
                //moduleService.update('fullcartmessage');
            });
        };

        ctrl.getOptions = function (min, step, amount, max) {
            var tempArr = [];

            var start = Math.ceil(min / step) * step;

            if (amount > max) {
                //if you need limit options number by availability
                for (var i = start; i <= amount; i = +((i + step).toFixed(4))) {
                    tempArr.push(i);
                }
            } else {
                for (var i = start; i <= max; i = +((i + step).toFixed(4))) {
                    tempArr.push(i);
                }
            }

            return tempArr;
        }

        ctrl.refresh = function () {
            return cartService.getData(false).then(function (data) {
                ctrl.cartData = data;
            });
        };
        
    };

    ng.module('cart')
      .controller('CartMobileFullCtrl', CartMobileFullCtrl);

    CartMobileFullCtrl.$inject = ['$rootScope', 'cartService'];

})(window.angular);