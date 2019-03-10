; (function (ng) {
    'use strict';

    var BillStampCtrl = function ($http, toaster, Upload, SweetAlert) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.StampImageName = ctrl.stampImg;
            ctrl.StampImageSrc = ctrl.stampImgSrc;
        };


        ctrl.uploadBillStampImage = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                ctrl.sendIcon($file);
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке', 'Файл не соответствует требованиям');
            }
        };

        ctrl.deleteBillStampImage = function () {

            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" })
                .then(function (result) {
                    if (result === true) {
                        return $http.post('paymentMethods/deleteBillStamp', { methodId: ctrl.methodId }).then(function (response) {
                            var data = response.data;
                            if (data.result === true) {

                                ctrl.StampImageName = null;
                                ctrl.StampImageSrc = null;

                                toaster.pop('success', '', 'Изображение удалено');
                            } else {
                                toaster.pop('error', 'Ошибка при удалении', data.error);
                            }
                        });
                    }
                });
        };

        ctrl.sendIcon = function (file) {
            return Upload.upload({
                url: 'paymentMethods/uploadBillStamp',
                data: {
                    file: file,
                    methodId: ctrl.methodId,
                    rnd: Math.random(),
                }
            }).then(function (response) {
                var data = response.data;

                if (data.result === true) {

                    ctrl.StampImageName = data.imgName;
                    ctrl.StampImageSrc = data.src;

                    toaster.pop('success', '', 'Изображение сохранено');
                } else {
                    toaster.pop('error', 'Ошибка при загрузке', data.error);
                }
            });
        }

    };

    BillStampCtrl.$inject = ['$http', 'toaster', 'Upload', 'SweetAlert'];

    ng.module('paymentMethod')
        .controller('BillStampCtrl', BillStampCtrl)
        .component('billStamp', {
            templateUrl: '../areas/admin/content/src/paymentMethods/components/billStamp/templates/billStamp.html',
            controller: 'BillStampCtrl',
            bindings: {
                onInit: '&',
                methodId: '@',
                stampImg: '@',
                stampImgSrc: '@'
            }
        });

})(window.angular);