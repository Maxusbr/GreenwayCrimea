; (function (ng) {

    'use strict';

    var BlocksConstructorContainerCtrl = function () {

        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.styles = {};
        };

        ctrl.updateBackgroundContainer = function (cssString) {
            ctrl.styles['background'] = cssString;
        };

    };

    ng.module('blocksConstructor')
      .controller('BlocksConstructorContainerCtrl', BlocksConstructorContainerCtrl);

    BlocksConstructorContainerCtrl.$inject = [];

})(window.angular);