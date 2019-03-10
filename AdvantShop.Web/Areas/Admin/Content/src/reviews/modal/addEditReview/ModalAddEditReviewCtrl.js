; (function (ng) {
    'use strict';

    var ModalAddEditReviewCtrl = function ($uibModalInstance, $http, toaster, Upload) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.reviewId = params.reviewId != null ? params.reviewId : 0;
            ctrl.mode = ctrl.reviewId != 0 ? 'edit' : 'add';

            ctrl.getFormData().then(function (data) {
                ctrl.filesHelpText = data.filesHelpText;
                if (ctrl.mode == 'add') {
                    ctrl.AddDate = (new Date(Date.now()).toLocaleString()).replace(',', '');
                    ctrl.Checked = true;
                } else {
                    ctrl.getReview(ctrl.reviewId);
                }
            });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.getFormData = function () {
            return $http.post('reviews/getFormData').then(function (response) {
                return response.data;
            });
        };

        ctrl.getReview = function (reviewId) {
            $http.get('reviews/getReview', { params: { reviewId: reviewId } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.Name = data.Name;
                    ctrl.Email = data.Email;
                    ctrl.Text = data.Text;
                    ctrl.Checked = data.Checked;
                    ctrl.AddDate = data.AddDateFormatted;
                    ctrl.Ip = data.Ip;
                    ctrl.Photo = data.PhotoSrc;
                    ctrl.PhotoName = data.PhotoName;
                    ctrl.ArtNo = data.ArtNo;
                    ctrl.productName = data.ProductName;
                    ctrl.productUrl = data.ProductUrl;
                }
            });
        }

        ctrl.file = null;

        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {

            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                ctrl.file = $file;
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', '', 'Файл не соответствует требованиям');
                ctrl.loadingFiles = false;
            }
        };

        ctrl.deletePhoto = function() {
            $http.post('reviews/deletePhoto', { reviewId: ctrl.reviewId }).then(function (response) {
                ctrl.Photo = null;
                ctrl.PhotoName = null;
            });
        }

        ctrl.save = function () {

            var params = {
                reviewId: ctrl.reviewId,
                Name: ctrl.Name,
                Email: ctrl.Email,
                Text: ctrl.Text,
                Checked: ctrl.Checked,
                AddDate: ctrl.AddDate,
                ArtNo: ctrl.ArtNo,
            };

            var url = ctrl.mode == 'add' ? 'reviews/addReview' : 'reviews/updateReview';

            Upload.upload({
                url: url,
                data: ng.extend(params, {
                    photoFile: ctrl.file,
                    rnd: Math.random(),
                })
            }).then(function(response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                    $uibModalInstance.close('saveColor');
                } else {
                    if (data.error != null) {
                        toaster.pop('error', '', data.error);
                    } else if (data.errors != null) {
                        data.errors.forEach(function(error) {
                            toaster.pop('error', '', error);
                        });
                    } else {
                        toaster.pop('error', '', 'Ошибка при создании/редактировании');
                    }
                }
            });
        }
    };

    ModalAddEditReviewCtrl.$inject = ['$uibModalInstance', '$http', 'toaster', 'Upload'];

    ng.module('uiModal')
        .controller('ModalAddEditReviewCtrl', ModalAddEditReviewCtrl);

})(window.angular);