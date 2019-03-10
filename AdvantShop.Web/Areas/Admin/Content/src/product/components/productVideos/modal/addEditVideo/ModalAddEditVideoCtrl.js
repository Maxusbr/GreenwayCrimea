; (function (ng) {
    'use strict';

    var ModalAddEditVideoCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.productVideoId = params.productVideoId != null ? params.productVideoId : 0;
            ctrl.productId = params.productId != null ? params.productId : 0;
            ctrl.mode = ctrl.productVideoId != 0 ? "edit" : "add";

            if (ctrl.mode == "add") {
                ctrl.showLink = true;
                ctrl.videoSortOrder = 0;
            } else {
                ctrl.showLink = false;
                ctrl.getVideo(ctrl.productVideoId);
            }
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getVideo = function (productVideoId) {
            $http.get('product/getvideo', { params: { productVideoId: productVideoId } }).then(function (response) {
                var data = response.data;
                if (data != null) {
                    ctrl.productVideoId = data.ProductVideoId;
                    ctrl.productId = data.ProductId;
                    ctrl.name = data.Name;
                    ctrl.playerCode = data.PlayerCode;
                    ctrl.description = data.Description;
                    ctrl.videoSortOrder = data.VideoSortOrder;
                }
            });
        }

        ctrl.save = function () {

            if (ctrl.showLink && (ctrl.link == null || ctrl.link.trim() === '')) {
                toaster.pop('error', '', 'Укажите ссылку на видео');
                return;
            }
            if (!ctrl.showLink && (ctrl.playerCode == null || ctrl.playerCode.trim() === '')) {
                toaster.pop('error', '', 'Укажите код плеера');
                return;
            }

            var params = {
                productVideoId: ctrl.productVideoId,
                productId: ctrl.productId,
                name: ctrl.name,
                link: ctrl.link,
                playerCode: ctrl.playerCode,
                description: ctrl.description,
                videoSortOrder: ctrl.videoSortOrder,
            }
            var url = ctrl.mode === "add" ? 'product/addVideo' : 'product/updateVideo';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения успешно сохранены');
                    $uibModalInstance.close();
                } else {
                    toaster.pop('error', '', data.error);
                }
            });
        }
    };

    ModalAddEditVideoCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditVideoCtrl', ModalAddEditVideoCtrl);

})(window.angular);