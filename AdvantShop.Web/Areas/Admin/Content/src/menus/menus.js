; (function (ng) {
    'use strict';

    var MenusCtrl = function () {

        var ctrl = this;

        ctrl.menuDictionary = {};

        ctrl.menuTreeviewInit = function (jstree, menuName) {
            ctrl.menuDictionary[menuName] = jstree;
        }

        ctrl.updateMenu = function (result, menuName) {
            ctrl.menuDictionary[menuName].refresh();
        }
    };

    MenusCtrl.$inject = [];


    ng.module('menus', [])
      .controller('MenusCtrl', MenusCtrl);

})(window.angular);