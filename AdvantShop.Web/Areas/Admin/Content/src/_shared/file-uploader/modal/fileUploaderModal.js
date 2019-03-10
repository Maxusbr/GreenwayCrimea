; (function (ng) {
    'use strict';

    var ModalFileUploaderCtrl = function ($uibModalInstance) {
        var ctrl = this;
        
        ctrl.save = function (url) {
            $uibModalInstance.close(url);
        };

        ctrl.dismiss = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalFileUploaderCtrl.$inject = ['$uibModalInstance'];

    ng.module('uiModal')
        .controller('ModalFileUploaderCtrl', ModalFileUploaderCtrl);

})(window.angular);