; (function (ng) {
    'use strict';

    var SubmenuParentCtrl = function () {

        var ctrl = this;

        ctrl.addSubmenu = function (submenu) {
            ctrl.submenu = submenu;
        };

        ctrl.addParent = function (parent) {
            ctrl.parent = parent;
        };

        ctrl.memoryElement = function (element) {
            ctrl.element = element;
        }
    };

    ng.module('submenu')
      .controller('SubmenuParentCtrl', SubmenuParentCtrl);

})(window.angular);