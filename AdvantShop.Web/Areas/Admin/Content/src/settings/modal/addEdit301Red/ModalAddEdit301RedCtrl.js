; (function (ng) {
    'use strict';

    var ModalAddEdit301RedCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.id = params.id != null ? params.id : 0;
            ctrl.RedirectFrom = params.from != undefined && params.from.value != null ? params.from.value : null;
            ctrl.mode = ctrl.id != 0 && ctrl.id != undefined && ctrl.id != null ? "edit" : "add";

            if (ctrl.mode == "edit") {
                ctrl.get301Red(ctrl.id);
            }
            else {
                ctrl.id = 0;
            }
        };

        ctrl.get301Red = function (ID) {
            $http.get('redirect301/getRedirect301Item', { params: { ID: ID, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {

                    ctrl.RedirectFrom = data.RedirectFrom;
                    ctrl.RedirectTo = data.RedirectTo;
                    ctrl.ProductArtNo = data.ProductArtNo;
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.save301Red = function () {

            ctrl.btnSleep = true;

            var params = {
                ID: ctrl.id,
                RedirectFrom: ctrl.RedirectFrom,
                RedirectTo: ctrl.RedirectTo,
                ProductArtNo: ctrl.ProductArtNo,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'redirect301/Add301Redirect' : 'redirect301/Edit301Redirect';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close('save301Red');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании редиректа");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEdit301RedCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEdit301RedCtrl', ModalAddEdit301RedCtrl);

})(window.angular);