; (function (ng) {
    'use strict';

    var ModalEditMainPageListCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.type = params.type != null ? params.type : 0;

            $http.get('mainpageproducts/getMainPageList', { params: { type: ctrl.type }}).then(function (response) {
                ctrl.data = response.data;
                ctrl.data.type = ctrl.type;
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.save = function () {
            var params = ctrl.data;
            $http.post('mainpageproducts/updateMainPageList', params).then(function (response) {
                toaster.pop('success', '', 'Изменения сохранены');
                $uibModalInstance.close();
            });
            
        };
    };

    ModalEditMainPageListCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalEditMainPageListCtrl', ModalEditMainPageListCtrl);

})(window.angular);