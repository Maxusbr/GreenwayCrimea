; (function (ng) {

    'use strict';

    var PopoverOverlayCtrl = function (popoverService) {
        var ctrl = this;

        ctrl.overlayHide = function () {
            popoverService.getPopoverScope(ctrl.popoverId).then(function (popoverScope) {
                popoverScope.deactive();

                ctrl.popoverId = null;

                ctrl.isVisibleOverlay = false;
            });
        };

        popoverService.addPopoverOverlay(ctrl);

    };

    ng.module('popover')
    .controller('PopoverOverlayCtrl', PopoverOverlayCtrl);

    PopoverOverlayCtrl.$inject = ['popoverService'];

})(window.angular);