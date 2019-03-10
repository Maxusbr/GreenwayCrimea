; (function (ng) {
    'use strict';

    var SaasStatCTrl = function SaasStatCTrl(saasStatService) {
        var ctrl = this;

        ctrl.$onInit = function $onInit() {
            ctrl.entity = saasStatService.getData();
        };

        ctrl.$onDestroy = function $onDestroy() {
            saasStatService.deleteObsevarable();
        };
    };

    SaasStatCTrl.$inject = ['saasStatService'];

    ng.module('saasStat', [])
      .controller('SaasStatCTrl', SaasStatCTrl);

})(window.angular);