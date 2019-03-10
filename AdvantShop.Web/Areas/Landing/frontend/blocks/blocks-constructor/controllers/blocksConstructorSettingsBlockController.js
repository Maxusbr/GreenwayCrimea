; (function (ng) {

    'use strict';

    var BlocksConstructorSettingsBlockCtrl = function (blocksConstructorBackgroundColors, blocksConstructorService) {
        var ctrl = this;

        ctrl.backgroundColors = blocksConstructorBackgroundColors;
    };

    ng.module('blocksConstructor')
      .controller('BlocksConstructorSettingsBlockCtrl', BlocksConstructorSettingsBlockCtrl);

    BlocksConstructorSettingsBlockCtrl.$inject = ['blocksConstructorBackgroundColors', 'blocksConstructorService'];

})(window.angular);