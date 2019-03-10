; (function (ng) {
    'use strict';

    var AdminCommentsFormCtrl = function ($scope, $window) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.parent.form = ctrl;
        };

        ctrl.addCommentKeydown = function (e) {
            if (e.keyCode === 13 && e.ctrlKey) {
                ctrl.submit();
            }
        };


        ctrl.submit = function () {
            if ($scope.addAdminCommentForm.$pristine === true || ctrl.text == null) {
                ctrl.setFocus();
                return;
            }
            ctrl.submitFn({ form: ctrl });
        };

        ctrl.onReply = function (comment) {
            ctrl.setFocus();
            if ($scope.addAdminCommentForm.$pristine === true || ctrl.text == null) {
                ctrl.text = comment.Name + ', ';
            }
        }

        ctrl.reset = function () {
            ctrl.text = '';
            $scope.addAdminCommentForm.$setPristine();
        };

        ctrl.setFocus = function () {
            var element = $window.document.getElementById('adminCommentsFormText');
            if (element)
                element.focus();
        };

    };

    AdminCommentsFormCtrl.$inject = ['$scope', '$window'];

    ng.module('adminCommentsForm', [])
    .controller('AdminCommentsFormCtrl', AdminCommentsFormCtrl);

})(window.angular);