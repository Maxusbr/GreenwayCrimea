; (function (ng) {
    'use strict';

    var ModalAddEditTaxCtrl = function ($uibModalInstance, $http, $filter, toaster) {
        var ctrl = this;
        ctrl.formInited = false;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.mode = ctrl.id != 0 ? "edit" : "add";

            ctrl.getTaxTypes().then(function() {
                if (ctrl.mode === "add") {
                    ctrl.name = "";
                    ctrl.rate = 0;
                    ctrl.enabled = false;
                    ctrl.formInited = true;
                    ctrl.isDefault = false;
                    ctrl.taxType = ctrl.TaxTypes[0];
                } else {
                    ctrl.getTax(ctrl.id);
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getTaxTypes = function() {
            return $http.get('settingsCheckout/getTaxTypes').then(function (response) {
                ctrl.TaxTypes = response.data;
            });
        }

        ctrl.getTax = function (id) {
            $http.get('settingsCheckout/getTax', {params: { id: id }}).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.id = data.TaxId;
                    ctrl.name = data.Name;
                    ctrl.rate = data.Rate;
                    ctrl.enabled = data.Enabled;
                    ctrl.isDefault = data.IsDefault;
                    ctrl.taxType = ctrl.TaxTypes.filter(function(x) { return x.value === data.TaxType })[0];
                }
                ctrl.form.$setPristine();
                ctrl.formInited = true;
            });
        }

        ctrl.save = function () {

            ctrl.btnSleep = true;

            var params = {
                taxid: ctrl.id,
                name: ctrl.name,
                rate: ctrl.rate,
                enabled: ctrl.enabled,
                isDefault: ctrl.isDefault,
                taxType: ctrl.taxType.value,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'settingsCheckout/addtax' : 'settingsCheckout/updatetax';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", ctrl.mode == "add" ? "Налог добавлен" : "Изменения сохранены");
                    $uibModalInstance.close('saveTax');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при " + ctrl.mode == "add" ? "создании" : "редактировании");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditTaxCtrl.$inject = ['$uibModalInstance', '$http', '$filter', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditTaxCtrl', ModalAddEditTaxCtrl);

})(window.angular);