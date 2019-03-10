; (function (ng) {
    'use strict';

    var ResultPriceRegulationCtrl = function ($uibModalInstance) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve;

            ctrl.title = params.title;
            ctrl.msg = params.msg;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ResultPriceRegulationCtrl.$inject = ['$uibModalInstance'];

    ng.module('uiModal')
        .controller('ResultPriceRegulationCtrl', ResultPriceRegulationCtrl);

})(window.angular);