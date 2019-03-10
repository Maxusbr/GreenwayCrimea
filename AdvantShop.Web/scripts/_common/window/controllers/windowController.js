; (function (ng) {
    'use strict';

    var WindowCtrl = function () {
        var ctrl = this;
    };

    ng.module('windowExt')
    .controller('WindowCtrl', WindowCtrl);

    WindowCtrl.$inject = [];

})(angular);