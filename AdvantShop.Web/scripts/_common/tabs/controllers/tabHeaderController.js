; (function (ng) {
    'use strict';

    var TabHeaderCtrl = function () {
        this.selected = {};
        this.isRender = true;
    };

    ng.module('tabs')
      .controller('TabHeaderCtrl', TabHeaderCtrl);

})(window.angular);