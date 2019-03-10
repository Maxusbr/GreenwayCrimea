; (function (ng) {
    'use strict';

    var InplaceLandingImageButtonsCtrl = function (inplaceLandingService) {
        var ctrl = this;


        ctrl.$onInit = function () {
            ctrl.inplaceLandingImage = inplaceLandingService.getInplaceImage(ctrl.inplaceLandingImageButtons);

            ctrl.inplaceLandingImage.buttons = ctrl;

            ctrl.buttonsVisible = ctrl.inplaceLandingImage.inplaceImageButtonsVisible;
        }

        ctrl.add = function (files, event) {
            if (event.type === 'change') { //ng-upload-file call callback two events: click and change
                ctrl.inplaceLandingImage.fileChange(files, event, 'add');
            }
        };

        ctrl.update = function (files, event) {
            if (event.type === 'change') { //ng-upload-file call callback two events: click and change
                ctrl.inplaceLandingImage.fileChange(files, event, 'update');
            }
        };

        ctrl.delete = function (event) {
            ctrl.inplaceLandingImage.fileChange(null, event, 'delete');
        };
    };

    ng.module('inplaceLanding')
      .controller('InplaceLandingImageButtonsCtrl', InplaceLandingImageButtonsCtrl);

    InplaceLandingImageButtonsCtrl.$inject = ['inplaceLandingService'];
})(window.angular);