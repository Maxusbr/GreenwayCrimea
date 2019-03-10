; (function (ng) {
    'use strict';

    var ModalAddEditOrderStatusCtrl = function ($uibModalInstance, $http, $filter, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.orderStatusId = params.orderStatusId != null ? params.orderStatusId : 0;
            ctrl.type = ctrl.orderStatusId != 0 ? "edit" : "add";

            ctrl.colorPickerOptions = {
                swatchBootstrap: false,
                format: 'hex',
                alpha: false,
                swatchOnly: false,
                'case': 'lower',
                allowEmpty: false,
                required: false,
                preserveInputFormat: false,
                restrictToFormat: false,
                inputClass: 'form-control'
            };

            ctrl.getCommands().then(function () {

                if (ctrl.type === "add") {
                    ctrl.command = ctrl.commands[0];
                    ctrl.sortOrder = 0;
                    ctrl.color = "#000000";
                    ctrl.colorPickerApi.getScope().AngularColorPickerController.setNgModel(ctrl.color);
                } else {
                    ctrl.loadOrderStatus(ctrl.orderStatusId)
                        .then(function (data) {
                            ctrl.colorPickerApi.getScope().AngularColorPickerController.setNgModel(ctrl.color);
                        });
                }
            });

            ctrl.colorPickerEventApi = {};

            ctrl.colorPickerEventApi.onBlur = function () {
                ctrl.colorPickerApi.getScope().AngularColorPickerController.update();
            };
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getCommands = function() {
            return $http.get('orderstatuses/getCommands').then(function (response) {
                ctrl.commands = response.data;
            });
        }

        ctrl.loadOrderStatus = function (id) {
            return $http.get('orderstatuses/getOrderStatus', { params: { orderStatusId: id } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.name = data.StatusName;
                    ctrl.isDefault = data.IsDefault;
                    ctrl.isCanceled = data.IsCanceled;
                    ctrl.isCompleted = data.IsCompleted;
                    ctrl.hidden = data.Hidden;
                    ctrl.sortOrder = data.SortOrder;
                    ctrl.command = $filter('filter')(ctrl.commands, { value: data.Command }, true)[0];
                    ctrl.color = data.Color;
                }

                return data;
            });
        }


        ctrl.saveStatus = function() {
            var params = {
                orderStatusId: ctrl.orderStatusId,
                statusName: ctrl.name,
                isDefault: ctrl.isDefault,
                isCanceled: ctrl.isCanceled,
                isCompleted: ctrl.isCompleted,
                hidden: ctrl.hidden,
                sortOrder: ctrl.sortOrder,
                commandId: ctrl.command.value,
                color: ctrl.color,
            }
            var url = ctrl.type == "add" ? 'orderstatuses/addOrderStatus' : 'orderstatuses/updateOrderStatus';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    $uibModalInstance.close();
                } else {
                    toaster.pop('error', '', 'Ошибка при сохранении');
                }
            });
        }
    };

    ModalAddEditOrderStatusCtrl.$inject = ['$uibModalInstance', '$http', '$filter', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditOrderStatusCtrl', ModalAddEditOrderStatusCtrl);

})(window.angular);