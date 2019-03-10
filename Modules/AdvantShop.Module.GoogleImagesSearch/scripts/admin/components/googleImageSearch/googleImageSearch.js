; (function (ng) {
    'use strict';

    var GoogleImageSearchCtrl = function ($http) {
        var ctrl = this;

        ctrl.apply = function (params) {

            if (ctrl.PictureUploaderCtrl != null) {
                ctrl.PictureUploaderCtrl.updatePhotoData(params.result.pictureId, params.result.picture);
            }

            if (ctrl.onApply != null) {
                ctrl.onApply(params);
            }
        }
    };

    GoogleImageSearchCtrl.$inject = ['$http'];

    ng.module('product')
        .controller('GoogleImageSearchCtrl', GoogleImageSearchCtrl)
        .component('googleImageSearch', {
            require: {
                PictureUploaderCtrl: '?^pictureUploader'
            },
            templateUrl: '../modules/googleImagesSearchModule/scripts/admin/components/googleImageSearch/googleImageSearch.html',
            controller: 'GoogleImageSearchCtrl',
            bindings: {
                uploadbylinkUrl: '@',
                uploadbylinkParams: '<?',
                selectMode: '@',
                onApply: '&'
            }
        });

})(window.angular);