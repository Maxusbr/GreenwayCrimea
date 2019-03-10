; (function (ng) {
    'use strict';

    var CompareCtrl = function (compareService) {
        var ctrl = this;

        ctrl.add = function (offerId) {
            return compareService.add(offerId);
        };

        ctrl.remove = function (offerId) {
            return compareService.remove(offerId);
        };

        ctrl.change = function (offerId) {

            if (ctrl.isAdded) {
                ctrl.add(offerId);
            } else {
                ctrl.remove(offerId);
            }
        };

        ctrl.checkStatus = function (offerId) {
            compareService.getStatus(offerId).then(function (isAdded) {
                ctrl.isAdded = isAdded;
            });
        };
    };


    ng.module('compare')
      .controller('CompareCtrl', CompareCtrl);

    CompareCtrl.$inject = ['compareService'];

})(window.angular);