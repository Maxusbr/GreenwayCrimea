; (function (ng) {
    'use strict';

    var RotateCtrl = function () {
        var ctrl = this;

        ctrl.rotateOptions = {
            totalFrames: parseInt(ctrl.totalFrames), // Total no. of image you have for 360 slider
            endFrame: parseInt(ctrl.endFrame), // end frame for the auto spin animation
            currentFrame: parseInt(ctrl.currentFrame) || 1, // This the start frame for auto spin
            imgList: ctrl.imgList, // selector for image list
            progress: ctrl.progress, // selector to show the loading progress
            imagePath: ctrl.imagePath, // path of the image assets
            ext: ctrl.ext || '.png', // extention for the assets
            height: parseInt(ctrl.height) || 300,
            width: parseInt(ctrl.width) || 300,
            navigation: ctrl.navigation() || false,
            responsive: ctrl.responsive() || false,
            autoplayDirection: parseInt(ctrl.autoplayDirection) || 1,
            framerate: parseInt(ctrl.framerate) || 60,
            disableSpin: false
        };
    };

    ng.module('rotate')
      .controller('RotateCtrl', RotateCtrl);

    RotateCtrl.$inject = [];

})(window.angular);