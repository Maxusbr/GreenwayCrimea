; (function (ng) {
    'use strict';

    var CmStatCTrl = function CmStatCTrl(cmStatService) {
        var ctrl = this;

        ctrl.$onInit = function $onInit() {
            ctrl.entity = cmStatService.getData();
        };

        ctrl.$onDestroy = function $onDestroy() {
            cmStatService.deleteObsevarable();
        };
    };

    CmStatCTrl.$inject = ['cmStatService'];

    ng.module('cmStat', [])
      .controller('CmStatCTrl', CmStatCTrl);

})(window.angular);