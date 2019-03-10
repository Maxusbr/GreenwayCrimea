; (function (ng) {
    'use strict';

    var ModalCropImageCtrl = function ($uibModalInstance, Upload, toaster, $http, $window, urlHelper, Cropper, $scope, $timeout) {
        var ctrl = this;

        ctrl.showUploadByUrl = false;

        ctrl.close = function () {
            ctrl.showUploadByUrl = false;
            $uibModalInstance.dismiss('cancel');
        };
        
        /* cropper */
        var file, fileName, data;

        //$scope.cropper = {};
        ctrl.cropperProxy = 'cropper.first';

        ctrl.cropOptions = {
            viewMode: 1,
            maximize: true,
            preview: '.preview-container',
            aspectRatio: 1 / 1,
            crop: function (dataNew) {
                data = dataNew;
            }
        };

        ctrl.cropImage = function () {
            if (!file || !data) return;
            Cropper.crop(file, data).then(Cropper.encode).then(function (dataUrl) {
                $uibModalInstance.close({ fileName: file.name != null ? file.name : fileName, base64String: dataUrl });
            });
        }

        ctrl.showEvent = 'show';
        ctrl.hideEvent = 'hide';

        function showCropper() { $scope.$broadcast(ctrl.showEvent); }
        function hideCropper() { $scope.$broadcast(ctrl.hideEvent); }

        /* end cropper */

        // upload image by file upload
        ctrl.uploadImage = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Cropper.encode((file = $file)).then(function (dataUrl) {
                    ctrl.imageSrc = dataUrl;
                    $timeout(showCropper);
                });
            }
        }

        ctrl.addImageByUrl = function () {
            ctrl.showUploadByUrl = true;
        }

        // upload image by url
        ctrl.uploadImageByUrl = function () {
            if (ctrl.url == null || ctrl.url == "")
                return;

            $http.post('common/uploadimagebyurl', { url: ctrl.url }).then(function (response) {
                var data = response.data;

                if (data.result === true) {

                    fileName = data.file;

                    $http.get(data.file, { responseType: 'blob' }).then(function (responseUploaded) {

                        var blob = responseUploaded.data;
                        Cropper.encode((file = blob)).then(function (dataUrl) {
                            ctrl.imageSrc = dataUrl;
                            $timeout(showCropper);
                        });
                    });

                } else {
                    toaster.pop('error', 'Ошибка при загрузке изображения', data.error);
                }
            });
        };
    };

    ModalCropImageCtrl.$inject = ['$uibModalInstance', 'Upload', 'toaster', '$http', '$window', 'urlHelper', 'Cropper', '$scope', '$timeout'];

    ng.module('uiModal')
        .controller('ModalCropImageCtrl', ModalCropImageCtrl);

})(window.angular);