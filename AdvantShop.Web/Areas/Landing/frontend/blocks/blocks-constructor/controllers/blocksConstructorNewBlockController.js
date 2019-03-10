; (function (ng) {

    'use strict';

    var BlocksConstructorNewBlockCtrl = function (blocksConstructorService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            blocksConstructorService.getBlocks(ctrl.modalData.landingpageId).then(function (data) {
                ctrl.data = data;
                ctrl.categorySelected = ctrl.data[0];
            });
        };

        ctrl.selectCategory = function (category) {
            if (category !== ctrl.categorySelected) {
                ctrl.categorySelected = category;
            }
        };
    };

    ng.module('blocksConstructor')
      .controller('BlocksConstructorNewBlockCtrl', BlocksConstructorNewBlockCtrl);

    BlocksConstructorNewBlockCtrl.$inject = ['blocksConstructorService'];

})(window.angular);