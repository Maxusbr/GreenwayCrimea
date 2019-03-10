; (function (ng) {
    'use strict';

    var ModalEditPhotoCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
            ctrl.PhotoId = params.PhotoId != null ? params.PhotoId : 0;

            $http.get('product/getPhoto', { params: { photoId: ctrl.PhotoId } }).then(function(response) {
                var data = response.data;
                ctrl.Description = data.Description;
                ctrl.ColorId = data.ColorId;
            });
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.save = function () {
            $http.post('product/editPhoto', { photoId: ctrl.PhotoId, alt: ctrl.Description, colorId: ctrl.ColorId }).then(function (response) {
                if (response.data == true) {
                    toaster.pop("success", '', 'Изменения успешно сохранены');
                    $uibModalInstance.close();
                }
            });
        };
    };

    ModalEditPhotoCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalEditPhotoCtrl', ModalEditPhotoCtrl);

})(window.angular);