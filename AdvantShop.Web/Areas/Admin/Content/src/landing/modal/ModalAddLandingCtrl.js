; (function (ng) {
    'use strict';

    var ModalAddLandingCtrl = function ($uibModalInstance, $http, $window, urlHelper) {
        var ctrl = this;
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addLanding = function () {

            if (ctrl.name == "") return;

            var params = {
                name: ctrl.name,
                type: ctrl.type,
                productIds: ctrl.productIds,
                goal: ctrl.goal,

            };
            
            $http.post('landing', params).then(function (response) {
                if (response.data.result == true) {
                    $window.location.assign(response.data.url);
                    $uibModalInstance.close();
                }
            });
        };
    };

    ModalAddLandingCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'urlHelper'];

    ng.module('uiModal')
        .controller('ModalAddLandingCtrl', ModalAddLandingCtrl);

})(window.angular);