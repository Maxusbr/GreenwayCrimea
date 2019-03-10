; (function (ng) {
    'use strict';

    ng.module('pictureUploader')
        .component('pictureUploader', {
            templateUrl: '../areas/admin/content/src/_shared/picture-uploader/templates/picture-uploader.html',
            controller: 'PictureUploaderCtrl',
            transclude: true,
            bindings: {
                startSrc: '@',
                pictureId: '@',
                uploadUrl: '@',
                uploadParams: '<?',
                deleteUrl: '@',
                deleteParams: '<?',
                uploadbylinkUrl: '@',
                uploadbylinkParams: '<?',
                onUpdate: '&',
                startPictureId: '@',
            }
        });

})(window.angular);