; (function (ng) {
    'use strict';

    var ProductPhotos360Ctrl = function ($http, $q, toaster, SweetAlert, Upload) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.getPhotos(ctrl.productId).then(function (photos) {
                ctrl.photos = photos;
            });

            ctrl.getActivity(ctrl.productId).then(function (isActive) {
                ctrl.showPhotos360 = isActive;
            });
        }

        ctrl.deletePhoto = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('product/deletePhoto360', { productId: ctrl.productId }).then(function (response) {
                        ctrl.getPhotos(ctrl.productId).then(function (photos) {
                            ctrl.photos = photos;
                        });
                    });
                }
            });
        }

        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {

            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {

                var photosExist = ctrl.photos != null;
                if (photosExist) {

                    SweetAlert.confirm("Вы уверены, что хотите залить новые изображения 360? Старые будут удалены.", { title: "Внимание" }).then(function (result) {
                        if (result === true)
                            uploadPhotos($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event);
                    });

                } else {
                    uploadPhotos($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event);
                }

            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
            }
        };

        ctrl.setActivity = function (productId, isActive) {
            $http.post('product/setActivityPhotos360', { productId: productId, isActive: isActive }).then(function (response) {
                if (response.data.result === true) {
                    toaster.pop('success', 'Изменения успешно применены');
                }
            }, function () {
                toaster.pop('error', 'Ошибка при сохранении настроек');
            });
        }

        ctrl.getActivity = function (productId) {
            return $http.get('product/getActivityPhotos360', { params: { productId: productId } }).then(function (response) {
                return response.data;
            });
        }

        function uploadPhotos($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            Upload.upload({
                url: 'product/uploadphotos360',
                data: {
                    files: $files,
                    productId: ctrl.productId
                }
            }).then(function (response) {
                if (response.data.result === true) {

                    ctrl.getPhotos(ctrl.productId).then(function (photos) {
                        ctrl.photos = photos;
                        toaster.pop('success', 'Загрузка фото выполнена успешно!');
                    });

                } else {
                    toaster.pop('error', 'Ошибка при загрузке фотографий', response.data.error);
                }
            }, function (error) {
                toaster.pop('error', 'Ошибка при загрузке фотографий');
            });
        }


        ctrl.getPhotos = function (productId) {
            return $http.get('product/getPhotos360', { params: { productId: productId } }).then(function (response) {
                return response.data;
            });
        }
    };

    ProductPhotos360Ctrl.$inject = ['$http', '$q', 'toaster', 'SweetAlert', 'Upload'];

    ng.module('productPhotos360', [])
        .controller('ProductPhotos360Ctrl', ProductPhotos360Ctrl)
        .component('productPhotos360', {
            templateUrl: '../areas/admin/content/src/product/components/productPhotos360/productPhotos360.html',
            controller: 'ProductPhotos360Ctrl',
            bindings: {
                productId: '@'
            }
        });

})(window.angular);