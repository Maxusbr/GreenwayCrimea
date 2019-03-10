; (function (ng) {
    'use strict';


    var AppCtrl = function () {
        this.customer = {};
    };

    var module = ng.module('app');

    module.controller('AppCtrl', AppCtrl);

})(window.angular);