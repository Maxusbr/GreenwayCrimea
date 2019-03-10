; (function (ng) {
    'use strict';

    var ErrorCtrl = function () {

        var ctrl = this;

        ctrl.inputTypeLogin = 'password';
        ctrl.inputTypePassword = 'password';
        
    };

    ng.module('error', [])
      .controller('ErrorCtrl', ErrorCtrl);

})(window.angular);