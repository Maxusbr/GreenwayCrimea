; (function (ng) {
    'use strict';

    var AdminCommentsItemCtrl = function ($timeout, toaster, adminCommentsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            
        };

        ctrl.edit = function () {
            ctrl.text = ctrl.comment.Text;
            ctrl.parent.editingComment = ctrl.comment.Id;
            //ctrl.editMode = true;
            ctrl.setFocus();
        };

        ctrl.save = function () {
            if (ctrl.text == null) {
                ctrl.setFocus();
                return;
            }
            adminCommentsService.updateComment(ctrl.comment.Id, ctrl.text).then(function (response) {
                if (response.Result === true) {
                    ctrl.comment.Text = ctrl.text;
                    ctrl.cancelEdit();
                    toaster.success('Изменения сохранены');
                }
                else {
                    toaster.error('Ошибка', response.Error);
                }
            });
        };

        ctrl.cancelEdit = function () {
            ctrl.parent.editingComment = null;
            //ctrl.editMode = false;
        };

        ctrl.setFocus = function () {
            ctrl.texFocus = false;
            $timeout(function () {
                ctrl.texFocus = true;
            }, 0);
        }
    };

    AdminCommentsItemCtrl.$inject = ['$timeout', 'toaster', 'adminCommentsService'];

    ng.module('adminCommentsItem', [])
    .controller('AdminCommentsItemCtrl', AdminCommentsItemCtrl);

})(window.angular);