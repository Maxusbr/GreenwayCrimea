; (function (ng) {
    'use strict';

    var ModalAddExportFeedCtrl = function ($uibModalInstance, $http, $window) {
        var ctrl = this;


        ctrl.exportfeedTypes = [];

        ctrl.$onInit = function () {            
            ctrl.name = '';
            var params = ctrl.$resolve.value;
            $http.post('exportfeeds/GetAvalableTypes', params).then(function (response) {
                if (response.data != null && response.data.result) {
                    ctrl.exportfeedTypes = response.data.obj;
                    if(params.param != "")
                    {
                        ctrl.type = params.param;
                    }
                    else {
                        ctrl.type = 'Csv';
                    }
                }
                else {
                    ctrl.error = 'Ошибка получения типов';
                }
            });
        };

        ctrl.add = function () {
                     
            if (ctrl.name == "") {
                return;
            }

            var params = {
                name: ctrl.name,
                description: ctrl.description,
                type: ctrl.type
            };

            $http.post('exportfeeds/add', params).then(function (response) {
                if (response.data != null) {
                    $window.location.assign('exportfeeds/index/' + response.data.id);
                }
                else
                {
                    ctrl.error = 'Ошибка при создании новой выгрузки';
                }
            });

            $uibModalInstance.dismiss('cancel');
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };        
    };

    ModalAddExportFeedCtrl.$inject = ['$uibModalInstance', '$http', '$window'];

    ng.module('uiModal')
        .controller('ModalAddExportFeedCtrl', ModalAddExportFeedCtrl);

})(window.angular);