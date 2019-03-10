; (function (ng) {
    'use strict';

    var ModalEditDealStatusCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.Id = params.Id;
            
            ctrl.getDealStatus();
        };

        ctrl.getDealStatus = function () {
            $http.get('leads/getDealStatus', { params: { id: ctrl.Id } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.Name = data.Name;
                    ctrl.SortOrder = data.SortOrder;
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.save = function () {

            ctrl.btnSleep = true;
            
            $http.post('leads/updateDealStatus', {Id: ctrl.Id,Name: ctrl.Name,SortOrder: ctrl.SortOrder}).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close();
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при редактировании");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalEditDealStatusCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalEditDealStatusCtrl', ModalEditDealStatusCtrl);

})(window.angular);