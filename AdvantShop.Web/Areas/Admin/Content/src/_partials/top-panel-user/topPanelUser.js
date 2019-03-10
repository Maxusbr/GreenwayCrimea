; (function (ng) {
    'use strict';

    var topPanelUserCtrl = function ($location, $uibModal) {

        var ctrl = this;

        ctrl.$onInit = function () {
            if ($location.search() != null && $location.search().user == 'me') {
                ctrl.loadCurrentUser();
            }
        };

        ctrl.loadCurrentUser = function () {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditUserCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../areas/admin/content/src/settings/modal/addEditUser/AddEditUser.html',
                resolve: {
                    params: {
                        customerId: 'me'
                    }
                }
            }).result.then(function (result) {
                ctrl.userName = result.FirstName + ' ' + result.LastName;
                $location.search('user', null);
                return result;
            }, function (result) {
                $location.search('user', null);
                return result;
            });
        };

    };

    topPanelUserCtrl.$inject = ['$location', '$uibModal'];

    ng.module('topPanelUser', [])
      .controller('topPanelUserCtrl', topPanelUserCtrl);

})(window.angular);