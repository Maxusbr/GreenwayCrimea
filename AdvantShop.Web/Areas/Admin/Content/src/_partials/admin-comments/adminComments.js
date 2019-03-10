; (function (ng) {
    'use strict';

    var AdminCommentsCtrl = function ($location, $window, $anchorScroll, $timeout, toaster, SweetAlert, adminCommentsService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.fetch();
        };

        ctrl.fetch = function () {
            adminCommentsService.getComments(ctrl.objId, ctrl.type).then(function (result) {
                ctrl.comments = result.Comments;
            });
        };

        ctrl.reply = function (comment) {
            ctrl.adminCommentIdActive = comment.Id;
            ctrl.visibleFormCancelButton = true;
            ctrl.formVisible = true;
            ctrl.form.onReply(comment);
            ctrl.editingComment = null;
        };

        ctrl.showComment = function ($event, id) {
            $event.preventDefault();
            var elId = 'admin-comment-' + id,
                element = $window.document.getElementById(elId);
            if (element) {
                $anchorScroll(elId);
                element.classList.add('highlighted');
                $timeout(function () {
                    element.classList.remove('highlighted');
                }, 2000);
            }
        };

        ctrl.clearForm = function () {
            ctrl.form.reset();
            ctrl.adminCommentIdActive = null;
            ctrl.visibleFormCancelButton = false;
        };

        ctrl.submit = function (form, actionUrl) {
            adminCommentsService.addComment(ctrl.objId, ctrl.type, form.adminCommentId, form.text, $location.absUrl()).then(function (response) {
                if (response.Result === true) {
                    ctrl.fetch();
                    //ctrl.comments.push(response.Comment);
                    toaster.success('Комментарий успешно добавлен');
                }
                else {
                    toaster.error('Ошибка', response.Error);
                }
            });
            ctrl.clearForm();
        };

        ctrl.cancel = function () {
            ctrl.clearForm();
        };

        ctrl.delete = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    adminCommentsService.deleteComment(id).then(function (response) {
                        if (response.Result === true)
                            ctrl.fetch();
                    });
                }
            });
        };

    };

    AdminCommentsCtrl.$inject = ['$location', '$window', '$anchorScroll', '$timeout', 'toaster', 'SweetAlert', 'adminCommentsService'];

    ng.module('adminComments', [])
    .controller('AdminCommentsCtrl', AdminCommentsCtrl);

})(window.angular);