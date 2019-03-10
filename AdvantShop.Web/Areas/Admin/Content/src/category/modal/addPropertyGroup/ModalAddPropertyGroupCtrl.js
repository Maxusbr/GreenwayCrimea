; (function (ng) {
    'use strict';

    var ModalAddPropertyGroupCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            $http.get('category/getAllPropertyGroups').then(function (response) {
                ctrl.groups = response.data;
                if (response.data != null && response.data.length > 0) {
                    ctrl.group = response.data[0];
                }
            });
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addPropertyGroup = function () {
            
            $http.post('category/addgrouptocategory', { categoryId: ctrl.$resolve.categoryId, groupId: ctrl.group.value }).then(function (response) {
                if (response.data == true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                } else {
                    toaster.pop('error', 'Ошибка', 'Изменения не сохранены');
                }
                $uibModalInstance.close();
            });
        }
    };

    ModalAddPropertyGroupCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddPropertyGroupCtrl', ModalAddPropertyGroupCtrl);

})(window.angular);