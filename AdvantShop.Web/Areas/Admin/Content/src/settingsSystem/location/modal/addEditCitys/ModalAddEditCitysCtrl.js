; (function (ng) {
    'use strict';

    var ModalAddEditCitysCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            ctrl.entity = ng.copy(ctrl.entity = ctrl.$resolve != null ? (ctrl.$resolve.entity != null ? ctrl.$resolve.entity : {}) : {});
            ctrl.mode = ctrl.entity.CityId != null && ctrl.entity.CityId !== 0 ? 'edit' : 'add';

            var paramRegionObjId,
                paramRegionObjType;

            if (ctrl.entity.CountryId != null && ctrl.entity.CountryId != 0) {
                paramRegionObjId = ctrl.entity.CountryId;
                paramRegionObjType = 'country';
            }
            else if (ctrl.entity.RegionId != null && ctrl.entity.RegionId != 0) {
                paramRegionObjId = ctrl.entity.RegionId;
                paramRegionObjType = 'region';
            }
            else if (ctrl.entity.CityId != null && ctrl.entity.CityId !== 0) {
                paramRegionObjId = ctrl.entity.CityId;
                paramRegionObjType = 'city';
            } else if (ctrl.entity.cityCountrys != null && ctrl.entity.cityCountrys !== 0) {
                paramRegionObjId = ctrl.entity.cityCountrys;
                paramRegionObjType = 'country';
            }

            ctrl.getRegions(paramRegionObjId || 0, paramRegionObjType);
        };

        ctrl.getRegions = function (objId, objType) {
            $http.get('Cities/GetRegions', { params: { objId: objId, objType: objType, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.regions = data;
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.saveCity = function () {

            ctrl.btnSleep = true;

            var url = ctrl.mode == 'add' ? 'Cities/AddCity' : 'Cities/EditCity';

            $http.post(url, ctrl.entity).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                    $uibModalInstance.close('saveCity');
                    ctrl.entity = null;
                } else {
                    toaster.pop('error', 'Ошибка', 'Ошибка при создании/редактировании города');
                }

                return data;
            }).finally(function () {
                ctrl.btnSleep = false;
            });
        }
    };

    ModalAddEditCitysCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditCitysCtrl', ModalAddEditCitysCtrl);

})(window.angular);