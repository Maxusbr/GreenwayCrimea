; (function (ng) {
    'use strict';

    var InplaceProgressCtrl = function (inplaceService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.state = inplaceService.getProgressState();
        };
    };

    ng.module('inplace')
      .controller('InplaceProgressCtrl', InplaceProgressCtrl);

    InplaceProgressCtrl.$inject = ['inplaceService'];
})(window.angular);