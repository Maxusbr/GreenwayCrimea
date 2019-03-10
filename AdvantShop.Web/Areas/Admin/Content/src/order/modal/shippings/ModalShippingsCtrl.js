; (function (ng) {
    'use strict';

    //
    // Используется в редактировании заказа и лида
    //
    var ModalShippingsCtrl = function ($uibModalInstance, $window, toaster, $q, $http) {
        var ctrl = this;

        ctrl.shippingLoading = true;


        ctrl.$onInit = function () {
            var params = ctrl.$resolve.order;
            ctrl.id = params.orderId;
            ctrl.isLead = params.isLead || false;
            ctrl.urlPath = !ctrl.isLead ? 'orders' : 'leads';
            
            ctrl.contact = {
                Country: params.country,
                Region: params.region,
                City: params.city,
                Zip: params.zip//City - с большой потому что используется в scripts\_partials\shipping\extend\yandexdelivery\yandexdelivery.js
            };

            ctrl.getShippings();
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getShippings = function () {

            $http.get(ctrl.urlPath + '/getShippings', { params: ng.extend(ctrl.contact, { id: ctrl.id }) }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.shippings = data.Shippings;
                    ctrl.selectShipping = data.SelectShipping;
                    ctrl.customShipping = data.CustomShipping;
                }
            }).finally(function () {
                ctrl.shippingLoading = false;
            });
        }

        ctrl.changeShipping = function (shipping) {

            $http.post(ctrl.urlPath + '/calculateShipping', ng.extend(ctrl.contact, { id: ctrl.id, shipping: shipping })).then(function (response) {
                if (response.data != null) {

                    if (shipping.IsCustom === true) {
                        ctrl.selectShipping = ng.extend(shipping, response.data.selectShipping);
                    } else {
                        ctrl.selectShipping = ctrl.getSelectedItem(ctrl.shippings, response.data.selectShipping);
                    }
                }
            });
        };

        ctrl.getSelectedItem = function (array, selectedItem) {
            var item;

            for (var i = array.length - 1; i >= 0; i--) {

                if (array[i].Id === selectedItem.Id) {
                    //selectedItem имеет заполненные поля какие опции выбраны, поэтому объединяем
                    array[i] = ng.extend(array[i], selectedItem);
                    item = array[i];
                    break;
                }
            }

            return item;
        };

        ctrl.save = function () {

            if (ctrl.contact.shipping == null) {
                toaster.pop('error', '', 'Выберите метод доставки');
                return;
            }

            var params = { id: ctrl.id };

            $http.post(ctrl.urlPath + '/saveShipping', ng.extend(ctrl.contact, params)).then(function (response) {
                $uibModalInstance.close({ shipping: ctrl.selectShipping });
            });
        }

    };

    ModalShippingsCtrl.$inject = ['$uibModalInstance', '$window', 'toaster', '$q', '$http'];

    ng.module('uiModal')
        .controller('ModalShippingsCtrl', ModalShippingsCtrl);

})(window.angular);