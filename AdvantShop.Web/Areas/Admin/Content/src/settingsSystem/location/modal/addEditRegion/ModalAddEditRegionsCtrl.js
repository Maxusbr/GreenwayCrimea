; (function (ng) {
    'use strict';

    var ModalAddEditRegionsCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.entity = ng.copy(ctrl.$resolve != null ? (ctrl.$resolve.entity != null ? ctrl.$resolve.entity : {}) : {});

            ctrl.mode = ctrl.entity.RegionId  != null && ctrl.entity.RegionId != 0 ? 'edit' : 'add';
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.saveRegions = function () {

            ctrl.btnSleep = true;

            var url = ctrl.mode == 'add' ? 'Regions/AddRegion' : 'Regions/EditRegion';

            $http.post(url, ctrl.entity).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close('saveRegions');
                    ctrl.entity = null;
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании региона");
                }
            }).finally(function () {
                ctrl.btnSleep = false;
            });
        }
    };

    ModalAddEditRegionsCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditRegionsCtrl', ModalAddEditRegionsCtrl);

})(window.angular);