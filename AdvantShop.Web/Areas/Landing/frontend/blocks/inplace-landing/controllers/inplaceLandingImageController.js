; (function (ng) {
    'use strict';

    var InplaceLandingImageCtrl = function ($compile, $scope, $window, domService, inplaceLandingService, Upload, subblockInplaceService) {
        var ctrl = this;

        //ctrl.tagImage = $element[0];

        ctrl.isActive = false;
        ctrl.isHoverButtons = false;

        ctrl.active = function () {
            ctrl.isActive = true;
            ctrl.showButtons = true;
        };

        ctrl.fileDrop = function (files, event, rejectedFiles) {
            return ctrl.fileChange(files, event, ctrl.inplaceParams.id != null && ctrl.inplaceParams.id !== 0 ? 'update' : 'add');
        };

        ctrl.fileChange = function (files, event, command) {

            if (command == null) {
                throw Error("Parameter 'command' required for inplace image");
            }

            //in productview
            if (ctrl.productViewItem != null) {
                if (ctrl.productViewItem.picture.PhotoId != null) {
                    ng.extend(ctrl.inplaceParams, { id: ctrl.productViewItem.picture.PhotoId });
                }

                if (ctrl.productViewItem.colorSelected != null) {
                    ng.extend(ctrl.inplaceParams, { colorId: ctrl.productViewItem.colorSelected.ColorId });
                }
            } else if (ctrl.product != null) {

                //inplaceLandingService.startProgress();

                if (ctrl.product.picture.PhotoId != null && ctrl.inplaceParams.field !== 'Review') {
                    ng.extend(ctrl.inplaceParams, { id: ctrl.product.picture.PhotoId });
                }

                if (ctrl.product.colorSelected != null) {
                    ng.extend(ctrl.inplaceParams, { colorId: ctrl.product.colorSelected.ColorId });
                }

            }

            ctrl.inplaceParams.command = command;
            ctrl.inplaceParams.picture = ctrl.inplaceParams.settings.src;

            return Upload.upload({
                url: ctrl.inplaceUrl,
                data: ng.extend(ctrl.inplaceParams, { rnd: Math.random() }),
                file: files // or list of files (files) for html5 only
            }).then(function (response) {

                ctrl.tagImage.src = response.data.Picture || ctrl.inplaceParams.nophoto;
                ctrl.inplaceParams.settings.src = response.data.Picture;

                subblockInplaceService.updateSubBlockSettings(ctrl.inplaceParams.subblockId, ctrl.inplaceParams.settings);
            });
        };
    };

    ng.module('inplaceLanding')
      .controller('InplaceLandingImageCtrl', InplaceLandingImageCtrl);

    InplaceLandingImageCtrl.$inject = ['$compile', '$scope', '$window', 'domService', 'inplaceLandingService', 'Upload', 'subblockInplaceService'];

})(window.angular);