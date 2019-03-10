; (function (ng) {
    'use strict';

    var InplaceSwitchCtrl = function ($window, inplaceService) {
        var ctrl = this;

        ctrl.change = function (enabled) {
            inplaceService.setEnable(enabled).then(function (result) {
                if (result === true) {
                    $window.location.reload();
                }
            });
        };

    };

    ng.module('inplace')
      .controller('InplaceSwitchCtrl', InplaceSwitchCtrl);

    InplaceSwitchCtrl.$inject = ['$window','inplaceService'];

})(window.angular);