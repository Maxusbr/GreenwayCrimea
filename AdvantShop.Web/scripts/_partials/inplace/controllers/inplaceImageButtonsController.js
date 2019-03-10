; (function (ng) {
    'use strict';

    var InplaceImageButtonsCtrl = function (inplaceService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.inplaceImage = inplaceService.getInplaceImage(ctrl.inplaceImageButtons);

            ctrl.inplaceImage.buttons = ctrl;

            ctrl.buttonsVisible = ctrl.inplaceImage.inplaceImageButtonsVisible;
        };

        ctrl.add = function (files, event) {
            if (event.type === 'change') { //ng-upload-file call callback two events: click and change
                ctrl.inplaceImage.fileChange(files, event, 'add');
            }
        };

        ctrl.update = function (files, event) {
            if (event.type === 'change') { //ng-upload-file call callback two events: click and change
                ctrl.inplaceImage.fileChange(files, event, 'update');
            }
        };

        ctrl.delete = function (event) {
            ctrl.inplaceImage.fileChange(null, event, 'delete');
        };
    };

    ng.module('inplace')
      .controller('InplaceImageButtonsCtrl', InplaceImageButtonsCtrl);

    InplaceImageButtonsCtrl.$inject = ['inplaceService'];
})(window.angular);