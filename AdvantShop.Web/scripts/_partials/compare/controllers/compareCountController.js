; (function (ng) {
    'use strict';

    var CompareCountCtrl = function (compareService) {
        var ctrl = this;

        ctrl.countObj = compareService.getCountObj();
    };


    ng.module('compare')
      .controller('CompareCountCtrl', CompareCountCtrl);

    CompareCountCtrl.$inject = ['compareService'];

})(window.angular);