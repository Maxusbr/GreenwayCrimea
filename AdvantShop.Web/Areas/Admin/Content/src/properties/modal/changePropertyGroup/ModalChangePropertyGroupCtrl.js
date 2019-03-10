; (function (ng) {
    'use strict';

    var ModalChangePropertyGroupCtrl = function ($http, $uibModalInstance, urlHelper) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
            ctrl.getData().then(function () {
                ctrl.group = ctrl.groups[0];
            });
        };

        ctrl.getData = function () {
            return $http.get('properties/getPropertyData').then(function (response) {
                var data = response.data;
                ctrl.groups = data.groups;
            });
        }

        ctrl.changeGroup = function () {

            var resolve = ctrl.$resolve;
            var params = resolve.params;
            var groupId = urlHelper.getUrlParamByName('groupId');

            $http.post('properties/changePropertyGroup', ng.extend(params || {}, { groupId: groupId, newid: ctrl.group.Value })).then(function (response) {
                $uibModalInstance.close('changeGroup');
            });
        }

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalChangePropertyGroupCtrl.$inject = ['$http', '$uibModalInstance', 'urlHelper'];

    ng.module('uiModal')
        .controller('ModalChangePropertyGroupCtrl', ModalChangePropertyGroupCtrl);

})(window.angular);