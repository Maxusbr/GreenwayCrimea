; (function (ng) {
    'use strict';

    var ModalSearchImagesCtrl = function ($uibModalInstance, $http, toaster, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;

            ctrl.enumSelectMode = {
                'single': 'single',
                'multiple': 'multiple'
            };

            ctrl.uploadbylinkUrl = params.uploadbylinkUrl;
            ctrl.uploadbylinkParams = params.uploadbylinkParams;
            ctrl.selectMode = params.selectMode != null ? params.selectMode : 'single';

            ctrl.page = 0;

            ctrl.value = [];

            ctrl.fetch();
        };

        ctrl.fetch = function () {
            $http.get('../bingImagesSearch/searchImagesById', { params: ng.extend(ctrl.uploadbylinkParams, { page: ctrl.page }) }).then(function (response) {

                var data = response.data;

                ctrl.error = '';

                ctrl.value.length = 0;

                if (data.errors != null) {
                    ctrl.error = data.error[0].message;
                }

                ctrl.images = ng.copy(data.items);

            })
        }

        ctrl.add = function () {
            if (ctrl.value != null && ctrl.value.length > 0) {

                ctrl.btnLoading = true;

                $http.post(ctrl.uploadbylinkUrl, ng.extend(ctrl.uploadbylinkParams, ctrl.selectMode === ctrl.enumSelectMode.single ? { fileLink: ctrl.value[0] } : { fileLinks: ctrl.value })).then(function (response) {

                    var data = response.data;

                    if (data.result === true) {
                        toaster.pop('success', 'Изображение сохранено');
                        $uibModalInstance.close({ result: data.obj });
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке', data.error);
                    }
                })
                .finally(function () {
                    ctrl.btnLoading = false;
                });
            }
        };

        ctrl.findMore = function () {
            ctrl.page = ctrl.page + 1;
            ctrl.fetch();
        }

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.change = function (value) {
            if (ctrl.selectMode === ctrl.enumSelectMode.single) {
                ctrl.value.splice(0, ctrl.value.length);
                ctrl.value.push(value);
            }
        }
    };

    ModalSearchImagesCtrl.$inject = ['$uibModalInstance', '$http', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalSearchImagesCtrl', ModalSearchImagesCtrl);

})(window.angular);