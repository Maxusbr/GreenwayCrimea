; (function (ng) {
    'use strict';

    var DemoCtrl = function () {
        var ctrl = this;
    };

    ng.module('demo')
      .controller('DemoCtrl', DemoCtrl);

    DemoCtrl.$inject = [];

})(window.angular);