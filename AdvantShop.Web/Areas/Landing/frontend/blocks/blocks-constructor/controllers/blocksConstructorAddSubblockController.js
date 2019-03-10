; (function (ng) {

    'use strict';

    var BlocksConstructorAddSubblockCtrl = function (blocksConstructorBackgroundColors, blocksConstructorService) {
        var ctrl = this;

        ctrl.backgroundColors = blocksConstructorBackgroundColors;
    };

    ng.module('blocksConstructor')
      .controller('BlocksConstructorAddSubblockCtrl', BlocksConstructorAddSubblockCtrl);

    BlocksConstructorAddSubblockCtrl.$inject = ['blocksConstructorBackgroundColors', 'blocksConstructorService'];

})(window.angular);