; (function (ng) {

    'use strict';

    var PopoverControlCtrl = function ($element, popoverService) {
        var ctrl = this,
            popoverScope;

        popoverService.getPopoverScope(ctrl.popoverId).then(function () {
            popoverService.addControl(ctrl.popoverId, $element[0]);
        });

        ctrl.active = function () {
            if (popoverScope != null) {
                popoverScope.active();
            }
        };

        ctrl.deactive = function () {
            if (popoverScope != null) {
                popoverScope.deactive();
            }
        };

        ctrl.toggle = function () {
            if (popoverScope != null) {
                popoverScope.toggle();
            }
        };
    };

    ng.module('popover')
    .controller('PopoverControlCtrl', PopoverControlCtrl);

    PopoverControlCtrl.$inject = ['$element', 'popoverService'];

})(window.angular);