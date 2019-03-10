; (function (ng) {
    'use strict';

    var AmModuleSettingsCtrl = function (toaster, $scope, SweetAlert, moduleAdminService) {
        var ctrl = this;
        var defaultMarker = {
            MarkerId: -1,
            //Name: "",
            Color: "aa0000",
            ColorName: "ffffff",
            Url: "",
            Description: "",
            OpenNewTab: true,
            SortOrder: 0
        };

        ctrl.changeIdentity = false;

        ctrl.oldKey = -1;

        ctrl.showCsvId = false;

        ctrl.tmpNewMarker = {};

        ctrl.markers = [];

        ctrl.$onInit = function () {
            
            ctrl.colorPickerOptions = {
                swatchBootstrap: false,
                format: 'hex',
                alpha: false,
                swatchOnly: false,
                'case': 'lower',
                allowEmpty: false,
                required: true,
                preserveInputFormat: false,
                restrictToFormat: false,
                inputClass: 'form-control'
            };
            ctrl.colorPickerEventApi = {};

            ctrl.colorPickerEventApi.onBlur = function () {
                ctrl.colorPickerApi.getScope().AngularColorPickerController.update();
            };

            ctrl.colorPickerOptionsN = {
                swatchBootstrap: false,
                format: 'hex',
                alpha: false,
                swatchOnly: false,
                'case': 'lower',
                allowEmpty: false,
                required: true,
                preserveInputFormat: false,
                restrictToFormat: false,
                inputClass: 'form-control'
            };
            ctrl.colorPickerEventApiN = {};

            ctrl.colorPickerEventApiN.onBlur = function () {
                ctrl.colorPickerApiN.getScope().AngularColorPickerController.update();
            };

            defaultNewMarker();
            ctrl.getAllMarkers();
        }

        ctrl.addMarker = function (form, marker) {
            if (form.$valid)
            {
                moduleAdminService.actionMarker(marker, false).then(function (data) {
                    if (data.marker.MarkerId !== -1)
                    {
                        ctrl.markers.push(data.marker);
                        defaultNewMarker();
                        form.$setPristine();
                        form.$setUntouched();
                        toaster.pop('success', '', 'Сохранено');                        
                    } else
                    {
                        toaster.pop('error', '', 'Ошибка при сохранении');
                    }
                })
            } else {
                toaster.pop('error','','Неверно заполнены данные в форме')
            }
        }

        ctrl.editMarker = function (form, marker) {
            if (form.$valid) {
                marker.SortOrder = parseInt(marker.SortOrder);
                moduleAdminService.actionMarker(marker, ctrl.changeIdentity, ctrl.oldKey).then(function (data) {
                    if (data.success && data.marker.MarkerId !== -1) {
                        resetNewMarker(ctrl.tmpNewMarker);
                        form.$setPristine();
                        form.$setUntouched();
                        ctrl.changeIdentity = false;
                        toaster.pop('success', '', 'Сохранено');
                    } else {
                        toaster.pop('error', '', data.msg);
                    }
                })
            } else {
                toaster.pop('error', '', 'Неверно заполнены данные в форме')
            }
        }

        ctrl.editIdentity = function () {
            ctrl.changeIdentity = true;
            ctrl.oldKey = ctrl.newMarker.MarkerId;
        }

        ctrl.removeMarker = function (marker) {
            SweetAlert.confirm('Вы уверены, что хотите удалить маркер "' + marker.Name + '"?', { title: "Удаление" }).then(function (result) {
                if (result)
                {
                    var MarkerId = marker.MarkerId;
                    moduleAdminService.actionMarker({ MarkerId }).then(function (data) {
                        if (data.MarkerId !== -1) {
                            var markerToDelete = {};
                            [].forEach.call(ctrl.markers, function (val) {
                                if (val.MarkerId === MarkerId)
                                {
                                    markerToDelete = val;
                                };
                            })

                            var index = ctrl.markers.indexOf(markerToDelete);
                            ctrl.markers.splice(index, 1);
                            defaultNewMarker();
                            toaster.pop('success', '', 'Удалено');
                        } else {
                            toaster.pop('error', '', 'Ошибка при удалении');
                        }
                    })
                }
            });            
        }

        ctrl.edit = function (marker) {
            if (ctrl.newMarker.MarkerId === -1) {
                ctrl.tmpNewMarker = ctrl.newMarker;
                ctrl.newMarker = marker;
            } else {
                ctrl.newMarker = marker;
            }
        }

        ctrl.cancelEdit = function () {
            ctrl.newMarker = ctrl.tmpNewMarker;
            ctrl.changeIdentity = false;
        }

        ctrl.getAllMarkers = function () {
            moduleAdminService.getAllMarkers().then(function (data) {
                ctrl.markers = data;
            });
        }

        

        function defaultNewMarker() {
            ctrl.newMarker = angular.copy(defaultMarker);
            moduleAdminService.getCurrSoring().then(function (data) {
                defaultMarker.SortOrder = data;
                ctrl.newMarker = angular.copy(defaultMarker);
            });
        }

        function resetNewMarker(resetMarker) {
            moduleAdminService.getCurrSoring().then(function (data) {
                resetMarker.SortOrder = data;
                ctrl.newMarker = angular.copy(resetMarker);
            });
        }


    }

    AmModuleSettingsCtrl.$inject = ['toaster', '$scope', 'SweetAlert', 'moduleAdminService'];

    ng.module('amModuleSettings', ['color.picker'])
        .controller('AmModuleSettingsCtrl', AmModuleSettingsCtrl)
        .component('amModuleSettings', {
            templateUrl: '../modules/additionalmarkers/content/scripts/module-settings/module-settings.html',
            controller: 'AmModuleSettingsCtrl'
        });

})(window.angular)