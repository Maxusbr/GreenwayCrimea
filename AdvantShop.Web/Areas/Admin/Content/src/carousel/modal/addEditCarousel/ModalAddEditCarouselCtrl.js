; (function (ng) {
    'use strict';

    var ModalAddEditCarouselCtrl = function ($uibModalInstance, $http, toaster, Upload) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            ctrl.CarouselID = 0;
        };

        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.updateImage = function(result){
            ctrl.ImageSrc = result.pictureName;
        }

        ctrl.saveCarousel = function () {

            ctrl.btnSleep = true;

            if (ctrl.ImageSrc == undefined || ctrl.ImageSrc == null) {
                toaster.pop('error', 'Изображение не загружено', 'Пожалуйста загрузите изображение');
                ctrl.btnSleep = false;
                return;
            }

            var params = {
                CarouselID: ctrl.CarouselID,
                CaruselUrl: ctrl.CaruselUrl,
                DisplayInOneColumn: ctrl.DisplayInOneColumn,
                DisplayInTwoColumns: ctrl.DisplayInTwoColumns,
                DisplayInMobile: ctrl.DisplayInMobile,
                Blank: ctrl.Blank,
                SortOrder: ctrl.SortOrder,
                Enabled: ctrl.Enabled,
                ImageSrc: ctrl.ImageSrc,
                Description: ctrl.Description,
                rnd: Math.random()
            };

            var url = 'Carousel/AddCarousel';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close('saveCarousel');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при добавлении изображения");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditCarouselCtrl.$inject = ['$uibModalInstance', '$http', 'toaster', 'Upload'];

    ng.module('uiModal')
        .controller('ModalAddEditCarouselCtrl', ModalAddEditCarouselCtrl);

})(window.angular);