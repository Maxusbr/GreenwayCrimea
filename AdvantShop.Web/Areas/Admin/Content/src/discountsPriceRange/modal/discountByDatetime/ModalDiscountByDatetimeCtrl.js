; (function (ng) {
    'use strict';

    var ModalDiscountByDatetimeCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.getData();
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getData = function () {
            $http.get('discountsPriceRange/getDiscountByDatetimeData').then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.enabled = data.Enabled;
                    ctrl.dateFrom = data.FromDateTime;
                    ctrl.dateTo = data.ToDateTime;
                    ctrl.percent = data.DiscountByTime;
                    ctrl.showPopup = data.ShowPopup;
                    ctrl.popupText = data.PopupText;
                }
            });
        }

        ctrl.save = function() {
            var params = {
                enabled: ctrl.enabled,
                dateFrom: ctrl.dateFrom,
                dateTo: ctrl.dateTo,
                percent: ctrl.percent,
                showPopup: ctrl.showPopup,
                popupText: ctrl.popupText
            }
            $http.post('discountsPriceRange/saveDiscountByDatetimeData', params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop('success', '', 'Изменения успешно сохранены');
                    $uibModalInstance.close();
                }
            });
        }
    };

    ModalDiscountByDatetimeCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalDiscountByDatetimeCtrl', ModalDiscountByDatetimeCtrl);

})(window.angular);