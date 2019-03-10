; (function (ng) {
    'use strict';

    var ModalAddYandexGlobalDeliveryCostCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            //$http.get('category/getAllPropertyGroups').then(function (response) {
            //    ctrl.groups = response.data;
            //    if (response.data != null && response.data.length > 0) {
            //        ctrl.group = response.data[0];
            //    }
            //});
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addGlobalDeliveryCost = function () {
            
            $uibModalInstance.close({ Cost: ctrl.Cost, Days: ctrl.Days, OrderBefore: ctrl.OrderBefore });
            //$http.post('exportFeeds/AddGlobalDeliveryCosts', { Cost: ctrl.$resolve.categoryId, Days: ctrl.group.value, OrderBefore:}).then(function (response) {
            //    if (response.data == true) {
            //        toaster.pop('success', '', 'Изменения сохранены');
            //    } else {
            //        toaster.pop('error', 'Ошибка', 'Изменения не сохранены');
            //    }
            //    
            //});
        }
    };

    ModalAddYandexGlobalDeliveryCostCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddYandexGlobalDeliveryCostCtrl', ModalAddYandexGlobalDeliveryCostCtrl);

})(window.angular);