; (function (ng) {
    'use strict';

    var ModalCertificateSettingsCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            ctrl.getCertificate();
        };

        ctrl.getCertificate = function () {
            $http.get('certificates/getSettings').then(function (response) {
                ctrl.settings = response.data;
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.save = function () {
            
            ctrl.btnSleep = true;

            var params = ctrl.settings;
            $http.post('certificates/saveSettings', params).then(function(response) {

                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                    $uibModalInstance.close();
                } else {
                    toaster.pop('error', '', 'Ошибка при ссохранении');
                }
                ctrl.btnSleep = false;
            });
        }
    };

    ModalCertificateSettingsCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalCertificateSettingsCtrl', ModalCertificateSettingsCtrl);

})(window.angular);