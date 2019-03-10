; (function (ng) {
    'use strict';

    var ModalAddEditCouponCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.CouponId = params.CouponId != null ? params.CouponId : 0;
            ctrl.mode = ctrl.CouponId != 0 ? "edit" : "add";

            ctrl.getTypes().then(function() {
                ctrl.getCurrencies().then(function() {
                    if (ctrl.mode == "edit") {
                        ctrl.getCoupon(ctrl.CouponId);
                    } else {
                        ctrl.getCouponCode();
                        ctrl.Value = 0;
                        ctrl.MinimalOrderPrice = 0;
                        ctrl.Enabled = true;
                        ctrl.UsePosibleUses = true;
                        ctrl.UseExpirationDate = true;
                        ctrl.Type = ctrl.Types[0];
                        ctrl.CurrencyIso3 = ctrl.Currencies[0];
                        ctrl.AddingDate = new Date();
                        ctrl.AddingDateFormatted = (new Date()).toLocaleDateString();
                        ctrl.CategoryIds = [];
                        ctrl.ProductsIds = [];
                    }
                });
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };


        ctrl.getCoupon = function (id) {
            $http.get('coupons/getCoupon', { params: { couponId: id } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.CouponId = data.CouponId;
                    ctrl.Code = data.Code;
                    ctrl.Value = data.Value;
                    ctrl.Type = ctrl.Types.filter(function(x) { return x.value == data.Type; })[0];
                    ctrl.PossibleUses = data.PossibleUses;
                    ctrl.UsePosibleUses = ctrl.PossibleUses == 0;

                    ctrl.ExpirationDate = data.ExpirationDate;
                    ctrl.UseExpirationDate = ctrl.ExpirationDate == null;

                    ctrl.AddingDate = data.AddingDate;
                    ctrl.AddingDateFormatted = data.AddingDateFormatted;

                    ctrl.CurrencyIso3 = ctrl.Currencies.filter(function(x) { return x.value == data.CurrencyIso3; })[0];
                    
                    ctrl.Enabled = data.Enabled;
                    ctrl.MinimalOrderPrice = data.MinimalOrderPrice;
                    ctrl.ActualUses = data.ActualUses;
                    ctrl.CategoryIds = data.CategoryIds;
                    ctrl.ProductsIds = data.ProductsIds;
                }
            });
        }

        ctrl.getCouponCode = function (id) {
            return $http.get('coupons/getCouponCode', { params: { couponId: id } }).then(function (response) {
                ctrl.Code = response.data.code;
            });
        }

        ctrl.getTypes = function () {
            return $http.get('coupons/getTypes').then(function (response) {
                ctrl.Types = response.data;
            });
        }

        ctrl.getCurrencies = function () {
            return $http.get('coupons/getCurrencies').then(function (response) {
                ctrl.Currencies = response.data;
            });
        }

        ctrl.selectCategories = function(result) {
            ctrl.CategoryIds = result.categoryIds;
        }

        ctrl.selectProducts = function (result) {
            ctrl.ProductsIds = result.ids;
        }

        ctrl.resetCategories = function() {
            if (ctrl.mode === "add") {
                ctrl.CategoryIds = [];
            } else {
                $http.post('coupons/resetCouponCategories', { couponId: ctrl.CouponId }).then(function (response) {
                    ctrl.CategoryIds = [];
                });
            }
        }

        ctrl.resetProducts = function () {
            if (ctrl.mode === "add") {
                ctrl.ProductsIds = [];
            } else {
                $http.post('coupons/resetCouponProducts', { couponId: ctrl.CouponId }).then(function (response) {
                    ctrl.ProductsIds = [];
                });
            }
        }

        ctrl.save = function() {
            var params = {
                CouponId: ctrl.CouponId,
                Code: ctrl.Code,
                Value: ctrl.Value,
                Type: ctrl.Type.value,
                PossibleUses: !ctrl.UsePosibleUses ? ctrl.PossibleUses : 0,
                ExpirationDate: !ctrl.UseExpirationDate ? ctrl.ExpirationDate : null,
                AddingDate: ctrl.AddingDate,
                CurrencyIso3: ctrl.CurrencyIso3.value,
                Enabled: ctrl.Enabled,
                MinimalOrderPrice: ctrl.MinimalOrderPrice,
                CategoryIds: ctrl.CategoryIds,
                ProductsIds: ctrl.ProductsIds,
            };

            var url = ctrl.mode == "add" ? 'coupons/addCoupon' : 'coupons/updateCoupon';

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

    ModalAddEditCouponCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditCouponCtrl', ModalAddEditCouponCtrl);

})(window.angular);