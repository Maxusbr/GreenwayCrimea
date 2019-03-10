; (function (ng) {
    'use strict';

    var ModalAddEditCallComentCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            var params = ctrl.$resolve;

            ctrl.id = params != null ? params.id : null;
            ctrl.objId = params != null ? params.objId : null;

            if (ctrl.id != null) {
                ctrl.getComment();
            }
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.getComment = function() {
            $http.get('admincomments/getcomment', {params: {id: ctrl.id }}).then(function(result) {
                var data = result.data;
                if (data != null) {
                    ctrl.objId = data.ObjId;
                    ctrl.text = data.Text;
                }
            });
        }
        
        ctrl.saveComment = function() {
            $http.post(ctrl.id == null ? 'adminComments/add' : 'adminComments/update',
                {
                    id: ctrl.id,
                    objId: ctrl.objId,
                    type: 'call',
                    text: ctrl.text != null ? ctrl.text : ''
                })
                .then(function(result) {
                    var data = result.data;
                    if (data.Result === true) {
                        toaster.pop('success', '', 'Изменения сохранены');

                        if (data.Comment != null) {
                            ctrl.id = data.Comment.Id;
                            ctrl.text = data.Comment.Text;
                        }
                        $uibModalInstance.close({ id: ctrl.id, text: ctrl.text, objId: ctrl.objId });
                    } else {
                        toaster.pop('error', '', 'Ошибка при сохранении');
                    }
                    ctrl.btnLoading = false;
                });
        }
    };

    ModalAddEditCallComentCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditCallComentCtrl', ModalAddEditCallComentCtrl);

})(window.angular);