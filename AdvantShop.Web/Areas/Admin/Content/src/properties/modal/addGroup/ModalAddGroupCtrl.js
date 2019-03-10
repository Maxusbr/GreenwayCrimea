; (function (ng) {
    'use strict';

    var ModalAddGroupCtrl = function ($uibModalInstance, $http, $window, urlHelper) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.groupId = params.groupId != null ? params.groupId : 0;
            ctrl.mode = ctrl.groupId != 0 ? "edit" : "add";
            

            if (ctrl.mode == "add") {
                ctrl.sortOrder = 0;
            } else {
                $http.get('properties/getGroup', { params: { groupId: ctrl.groupId } }).then(function (response) {
                    var data = response.data;
                    ctrl.name = data.Name;
                    ctrl.nameDisplayed = data.NameDisplayed;
                    ctrl.sortOrder = data.SortOrder;
                });
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addGroup = function () {

            if (ctrl.name == null || ctrl.name === "") return;

            if (ctrl.mode == "add") {
                $http.post('properties/addGroup', { name: ctrl.name, nameDisplayed: ctrl.nameDisplayed, sortOrder: ctrl.sortOrder }).then(function(response) {
                    $uibModalInstance.close({ groupId: response.groupId, name: ctrl.name });
                });
            } else {
                $http.post('properties/updateGroup', { propertyGroupId: ctrl.groupId, name: ctrl.name, nameDisplayed: ctrl.nameDisplayed, sortOrder: ctrl.sortOrder }).then(function (response) {
                    $uibModalInstance.close('');
                });
            }
        };
    };

    ModalAddGroupCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'urlHelper'];

    ng.module('uiModal')
        .controller('ModalAddGroupCtrl', ModalAddGroupCtrl);

})(window.angular);