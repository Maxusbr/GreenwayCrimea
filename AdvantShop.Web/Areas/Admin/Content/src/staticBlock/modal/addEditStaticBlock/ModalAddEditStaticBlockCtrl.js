; (function (ng) {
    'use strict';

    var ModalAddEditStaticBlockCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.StaticBlockId = params.id != null ? params.id : 0;
            ctrl.mode = ctrl.StaticBlockId != 0 && ctrl.StaticBlockId != undefined && ctrl.StaticBlockId != null ? 'edit' : 'add';

            if (ctrl.mode === 'edit') {
                ctrl.getStaticBlock(ctrl.StaticBlockId);
            }
            else {
                ctrl.StaticBlockId = 0;
            }
        };

        ctrl.getStaticBlock = function (id) {
            $http.get('staticBlock/get', { params: { id: id, rnd: Math.random() } }).then(function (response) {
                var sb = response.data;
                if (sb != null) {
                    ctrl.Key = sb.Key;
                    ctrl.InnerName = sb.InnerName;
                    ctrl.Content = sb.Content;
                    ctrl.Enabled = sb.Enabled;
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.saveStaticBlock = function () {

            ctrl.btnSleep = true;

            var params = {
                StaticBlockId : ctrl.StaticBlockId,
                Key: ctrl.Key,
                InnerName: ctrl.InnerName,
                Content: ctrl.Content,
                Enabled: ctrl.Enabled,
                rnd: Math.random()
            };

            var url = ctrl.mode === 'add' ? 'staticBlock/add' : 'staticBlock/edit';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');
                    $uibModalInstance.close('saveStaticBlock');
                } else {
                    data.errors.forEach(function(error) {
                        toaster.pop('error', '', error);
                    });
                }
                ctrl.btnSleep = false;
            });
        }
    };

    ModalAddEditStaticBlockCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditStaticBlockCtrl', ModalAddEditStaticBlockCtrl);

})(window.angular);