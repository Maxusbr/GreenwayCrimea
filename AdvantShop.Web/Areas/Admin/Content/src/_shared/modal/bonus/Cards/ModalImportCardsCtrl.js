; (function (ng) {
    'use strict';

    var ModalImportCardsCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
        var ctrl = this;
       
        ctrl.isStartExport = false;

        ctrl.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.close = function () {
            $uibModalInstance.close('close');
        };

        ctrl.onBeforeSend = function () {
            ctrl.isStartExport = true;
            ctrl.btnLoading = true;
        };

        ctrl.onSuccess = function (data) {
            toaster.pop('success','Карты загружены');
        };
    };

    ModalImportCardsCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalImportCardsCtrl', ModalImportCardsCtrl);

})(window.angular);