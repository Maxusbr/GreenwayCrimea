; (function (ng) {
    'use strict';

    var ModalAddAvatarCtrl = function ($uibModalInstance, Upload, toaster, $http, $window, urlHelper, Cropper, $scope, $timeout) {
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

        // save cropped image 
        ctrl.saveCropedImg = function () {
            if (!file || !data) return;
            Cropper.crop(file, data).then(Cropper.encode).then(function (dataUrl) {

                $http.post('common/uploadAvatarCropped', { name: file.name != null ? file.name : fileName, base64String: dataUrl }).then(function (response) {
                    var data = response.data;
                    if (data.result === true) {
                        ctrl.avatarSrc = data.file + "?rnd=" + Math.random();
                        $uibModalInstance.close(ctrl.avatarSrc);
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке аватара', data.error);
                    }
                });
            });
        }

        ctrl.showEvent = 'show';
        ctrl.hideEvent = 'hide';

        function showCropper() { $scope.$broadcast(ctrl.showEvent); }
        function hideCropper() { $scope.$broadcast(ctrl.hideEvent); }

        /* end cropper */

        // upload avatar by file upload
        ctrl.uploadAvatar = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Cropper.encode((file = $file)).then(function (dataUrl) {

                    if (ctrl.avatarSrc != null) {
                        $timeout(hideCropper);
                    }
                    ctrl.avatarSrc = dataUrl;

                    $timeout(showCropper);
                });
            }
        }

        ctrl.addAvatarByUrl = function () {
            ctrl.showUploadByUrl = true;
        }

        // upload avatar by url
        ctrl.uploadAvatarByUrl = function () {
            if (ctrl.url == null || ctrl.url == "")
                return;

            $http.post('common/uploadavatarbyurl', { url: ctrl.url }).then(function (response) {
                var data = response.data;

                if (data.result === true) {

                    fileName = data.file;

                    $http.get(data.file, { responseType: 'blob' }).then(function (responseUploaded) {

                        var blob = responseUploaded.data;

                        Cropper.encode((file = blob)).then(function (dataUrl) {
                            ctrl.avatarSrc = dataUrl;
                            $timeout(showCropper);
                        });
                    });

                } else {
                    toaster.pop('error', 'Ошибка при загрузке аватара', data.error);
                }
            });
        };

        ctrl.deleteAvatar = function () {
            $http.post('common/deleteavatar').then(function(response) {
                ctrl.avatarSrc = null;
                $uibModalInstance.close(ctrl.avatarSrc);
            });
        }
    };

    ModalAddAvatarCtrl.$inject = ['$uibModalInstance', 'Upload', 'toaster', '$http', '$window', 'urlHelper', 'Cropper', '$scope', '$timeout'];

    ng.module('uiModal')
        .controller('ModalAddAvatarCtrl', ModalAddAvatarCtrl);

})(window.angular);