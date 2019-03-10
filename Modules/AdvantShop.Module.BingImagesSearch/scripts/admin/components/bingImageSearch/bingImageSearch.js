; (function (ng) {
    'use strict';

    var BingImageSearchCtrl = function ($http) {
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

    BingImageSearchCtrl.$inject = ['$http'];

    ng.module('product')
        .controller('BingImageSearchCtrl', BingImageSearchCtrl)
        .component('bingImageSearch', {
            require: {
                PictureUploaderCtrl: '?^pictureUploader'
            },
            templateUrl: '../modules/bingImagesSearchModule/scripts/admin/components/bingImageSearch/bingImageSearch.html',
            controller: 'BingImageSearchCtrl',
            bindings: {
                uploadbylinkUrl: '@',
                uploadbylinkParams: '<?',
                selectMode: '@',
                onApply: '&'
            }
        });

})(window.angular);