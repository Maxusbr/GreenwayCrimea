; (function (ng) {
    'use strict';

    var SettingsSocialCtrl = function ($http, $q) {
        var ctrl = this;

        ctrl.showBlockCustomCode = false;
    };

    SettingsSocialCtrl.$inject = ['$http', '$q'];

    ng.module('settingsSocial', [])
      .controller('SettingsSocialCtrl', SettingsSocialCtrl);

})(window.angular);