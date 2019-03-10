; (function (ng) {
    'use strict';

    var ModalAddEditColorCtrl = function ($uibModalInstance, $http, $q, $timeout, toaster, Upload) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.colorId = params.colorId != null ? params.colorId : 0;
            ctrl.mode = ctrl.colorId != 0 ? "edit" : "add";

            ctrl.getFormData().then(function (data) {
                ctrl.iconSizesText = data.iconSizesText;
                ctrl.filesHelpText = data.filesHelpText;

                if (ctrl.mode == "add") {
                    ctrl.sortOrder = 0;
                    ctrl.colorIcon = null;

                    ctrl.colorCodeRequired = true;
                    ctrl.colorPickerApi.getScope().AngularColorPickerController.setNgModel('#000000');
                } else {
                    ctrl.getColor(ctrl.colorId)
                        .then(function (data) {
                            ctrl.colorPickerApi.getScope().AngularColorPickerController.setNgModel(ctrl.colorCode);
                            ctrl.colorCodeRequired = ctrl.colorIcon == null && ctrl.colorCode == null;
                        });
                }
            });

            ctrl.colorCodeRequired = false;

            ctrl.colorPickerOptions = {
                swatchBootstrap: false,
                format: 'hex',
                alpha: false,
                'case': 'lower',
                swatchOnly: false,
                allowEmpty: true,
                required: false,
                preserveInputFormat: false,
                restrictToFormat: false,
                inputClass: 'form-control'
            };

            ctrl.colorPickerEventApi = {};

            ctrl.colorPickerEventApi.onBlur = function () {
                ctrl.colorPickerApi.getScope().AngularColorPickerController.update();
            };
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getFormData = function () {
            return $http.post('colors/getFormData').then(function (response) {
                return response.data;
            });
        };

        ctrl.getColor = function (colorId) {
            return $http.get('colors/getColor', { params: { colorId: colorId } }).then(function (response) {
                var data = response.data;

                var colorCode = data.ColorCode.trim();

                if (data != null) {
                    ctrl.colorName = data.ColorName;
                    ctrl.colorCode = data.ColorCode;
                    ctrl.colorIcon = data.ColorIcon;
                    ctrl.sortOrder = data.SortOrder;
                } 

                return data;
            });
        }

        ctrl.file = null;

        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {

            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                ctrl.file = $file;
                ctrl.colorCodeRequired = false;
                ctrl.needDeleteIcon = false;
            }
        };

        ctrl.deleteIcon = function () {
            ctrl.needDeleteIcon = true;

            ctrl.colorIcon = null;
            ctrl.colorCodeRequired = true;
        }

        ctrl.save = function () {

            var params = {
                colorId: ctrl.colorId,
                colorName: ctrl.colorName,
                colorCode: ctrl.colorCode.length > 0 ? (ctrl.colorCode[0] != '#' ? '#' : '') + ctrl.colorCode : '',
                colorIconFile: ctrl.colorIconFile,
                sortOrder: ctrl.sortOrder,
            };

            var deferDeleteIcon = $q.defer();

            if (ctrl.needDeleteIcon === true) {
                $http.post('colors/deleteIcon', { colorId: ctrl.colorId }).then(function () {
                    deferDeleteIcon.resolve();
                });
            } else {
                deferDeleteIcon.resolve();
            }

            var url = ctrl.mode == "add" ? 'colors/addColor' : 'colors/updateColor';

            deferDeleteIcon.promise.then(function () {
                Upload.upload({
                    url: url,
                    data: ng.extend(params, {
                        colorIconFile: ctrl.file,
                        rnd: Math.random(),
                    })
                }).then(function (response) {
                    var data = response.data;
                    if (data.result === true) {
                        toaster.pop("success", "", "Изменения сохранены");
                        $uibModalInstance.close('saveColor');
                    } else {
                        toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании");
                    }
                });
            });
        }
    };

    ModalAddEditColorCtrl.$inject = ['$uibModalInstance', '$http', '$q', '$timeout', 'toaster', 'Upload'];

    ng.module('uiModal')
        .controller('ModalAddEditColorCtrl', ModalAddEditColorCtrl);

})(window.angular);