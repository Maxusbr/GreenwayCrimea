; (function (ng) {
    'use strict';

    var FileUploaderCtrl = function ($http, Upload, toaster, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.src = ctrl.startSrc;
            ctrl.step = "uploadStep";
        };



        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (ctrl.uploadParams == null) {
                ctrl.uploadParams = {};
            }

            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                ctrl.file = $file;
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

            if (ctrl.onBeforeSend != null) {
                ctrl.onBeforeSend({ data: ctrl.uploadbylinkParams });
            }

            $http.post(ctrl.uploadbylinkUrl, ctrl.uploadbylinkParams).then(function (response) {
                var data = response.data;

                if (data.Result === true) {
                    ctrl.src = data.FilePath;
                    //toaster.pop('success', 'Файл сохранен');
                    if (ctrl.onSuccess != null) {
                        ctrl.onSuccess({ result: data });
                    }
                } else {
                    toaster.pop('error', 'Ошибка при загрузке', data.error);
                }

                if (ctrl.onUpdate != null) {
                    ctrl.onUpdate({ result: data });
                }
            });
        };

        ctrl.delete = function () {

            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" })
                .then(function (result) {
                    if (result === true) {
                        return $http.post(ctrl.deleteUrl, ctrl.deleteParams).then(function (response) {
                            var data = response.data;
                            if (data.Result === true) {
                                
                                ctrl.file = null;
                                toaster.pop('success', 'Файл удален');
                            } else {
                                toaster.pop('error', 'Ошибка при удалении', data.error);
                            }
                            ctrl.step = "uploadStep";
                        });
                    }
                });
        };

        ctrl.send = function (file) {

            ctrl.progressPercentage = 0;
            ctrl.step = "progressStep";

            ctrl.showAfterUpload = false;

            var data = ng.extend(ctrl.uploadParams, {
                file: file,
                rnd: Math.random(),
            });

            if (ctrl.onBeforeSend != null) {
                ctrl.onBeforeSend({ data: data });
            }

            return Upload.upload({
                url: ctrl.uploadUrl,
                data: data
            }).then(function (response) {                
                ctrl.showAfterUpload = false;

                var data = response.data;
                if (data.Result === true) {
                    ctrl.src = data.FilePath;
                    //toaster.pop('success', 'Файл сохранен');
                    if (ctrl.onSuccess != null) {
                        ctrl.onSuccess({ result: data });
                    }
                    ctrl.step = "resultStep";

                } else {                   
                    ctrl.step = "uploadStep";
                    toaster.pop('error', 'Ошибка при загрузке', data.error);                    
                }

                if (ctrl.onUpdate != null) {
                    ctrl.onUpdate({ result: data });
                }
            }, function (response) {                                
                toaster.pop('error', 'Ошибка при загрузке', response.status);
            }, function (evt) {
                ctrl.progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                if (ctrl.progressPercentage >= 100) {
                    ctrl.showAfterUpload = true;
                }
            })
        }
    };

    FileUploaderCtrl.$inject = ['$http', 'Upload', 'toaster', 'SweetAlert'];

    ng.module('fileUploader', ['uiModal', 'toaster'])
        .controller('FileUploaderCtrl', FileUploaderCtrl);

})(window.angular);