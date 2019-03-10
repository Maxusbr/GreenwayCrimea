; (function (ng) {
    'use strict';

    var ModalAddEditCountryCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.entity = ng.copy(ctrl.$resolve != null ? (ctrl.$resolve.entity != null ? ctrl.$resolve.entity : {}) : {});

            ctrl.mode = ctrl.entity.CountryId != null && ctrl.entity.CountryId != 0 ? 'edit' : 'add';
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.saveCountry = function () {

            ctrl.btnSleep = true;

            var url = ctrl.mode == 'add' ? 'Countries/AddCountry' : 'Countries/EditCountry';

            $http.post(url, ctrl.entity).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                    $uibModalInstance.close('saveCountry');
                    ctrl.entity = null;
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании страны");
                }
            }).finally(function () {
                ctrl.btnSleep = false;
            });
        }
    };

    ModalAddEditCountryCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditCountryCtrl', ModalAddEditCountryCtrl);

})(window.angular);