; (function (ng) {
    'use strict';

    var PictureUploaderCtrl = function ($http, Upload, toaster, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.src = ctrl.startSrc;
            ctrl.pictureId = ctrl.startPictureId
        };

        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (ctrl.uploadParams == null) {
                ctrl.uploadParams = {};
            }

            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                ctrl.send($file);
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
            }
        };

        ctrl.uploadByLink = function (result) {
            if (ctrl.uploadbylinkParams == null) {
                ctrl.uploadbylinkParams = {};
            }
            ctrl.uploadbylinkParams.fileLink = result;
            $http.post(ctrl.uploadbylinkUrl, ctrl.uploadbylinkParams).then(function (response) {
                var data = response.data;

                if (data.result === true) {
                    ctrl.src = data.obj.picture;
                    ctrl.pictureId = data.obj.pictureId;
                    toaster.pop('success', '', 'Изображение сохранено');
                } else {
                    toaster.pop('error', 'Ошибка при загрузке', data.error);
                }

                if (ctrl.onUpdate != null) {
                    ctrl.onUpdate({ result: data.obj });
                }
            });
        };

        ctrl.delete = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" })
                .then(function (result) {
                    if (result === true) {
                        var objId = (ctrl.uploadParams && ctrl.uploadParams.objId) || (ctrl.uploadbylinkParams && ctrl.uploadbylinkParams.objId);

                        return $http.post(ctrl.deleteUrl, { pictureId: ctrl.pictureId, objId: objId }).then(function (response) {
                            var data = response.data;
                            if (data.result === true) {
                                ctrl.updatePhotoData(null, data.obj.picture);
                                toaster.pop('success', '', 'Изображение удалено');
                            } else {
                                toaster.pop('error', 'Ошибка при удалении', data.error);
                            }
                        });
                    }
                });
        };

        ctrl.send = function (file) {
            return Upload.upload({
                url: ctrl.uploadUrl,
                data: ng.extend(ctrl.uploadParams, {
                    file: file,
                    rnd: Math.random(),
                })
            }).then(function (response) {
                var data = response.data;

                if (data.result === true) {

                    ctrl.updatePhotoData(data.obj.pictureId, data.obj.picture);

                    toaster.pop('success', '', 'Изображение сохранено');
                } else {
                    toaster.pop('error', 'Ошибка при загрузке', data.error);
                }

                if (ctrl.onUpdate != null) {
                    ctrl.onUpdate({ result: data.obj });
                }
            })
        };

        ctrl.updatePhotoData = function (pictureId, src) {
            ctrl.pictureId = pictureId;
            ctrl.src = src;
        };
    };

    PictureUploaderCtrl.$inject = ['$http', 'Upload', 'toaster', 'SweetAlert'];

    ng.module('pictureUploader', ['uiModal', 'toaster'])
        .controller('PictureUploaderCtrl', PictureUploaderCtrl);

})(window.angular);