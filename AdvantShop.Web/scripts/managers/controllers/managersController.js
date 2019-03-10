; (function (ng) {

    'use strict';

    var ManagersCtrl = function ($http, managersService, toaster, $translate) {

        var ctrl = this;

        managersService.getModalParams().then(function (data) {
            ctrl.IsShowUserAgreementText = data.IsShowUserAgreementText;
            ctrl.UserAgreementText = data.UserAgreementText;
        });

        managersService.dialogRenderCall(ctrl);
        managersService.dialogRenderEmail(ctrl);

        ctrl.showCall = function (managerId) {
            ctrl.manageridCall = managerId;
            ctrl.currentFormCall = "main";
            managersService.setVisibleFooterCall(true);
            managersService.dialogOpenCall(managerId);
        };

        ctrl.showEmail = function (managerId) {
            ctrl.managerid = managerId;
            ctrl.currentFormEmail = "main";
            managersService.setVisibleFooterEmail(true);
            managersService.dialogOpenEmail(managerId);
        };

        ctrl.sendRequestCall = function () {

            return managersService.requestCall(ctrl.clientname, ctrl.clientphone, ctrl.comment, ctrl.manageridCall).then(function (result) {

                if (result === true) {
                    ctrl.currentFormCall = "final";

                    ctrl.clientname = '';
                    ctrl.clientphone = '';
                    ctrl.comment = '';

                    managersService.setVisibleFooterCall(false);
                    managersService.setPristineFormCall();
                } else {
                    toaster.pop('error', $translate.instant('Js.Managers.ErrorRequestCall'));
                }
            });
        };

        ctrl.sendRequestEmail = function () {

            return managersService.sendEmail(ctrl.clientname, ctrl.emailtext, ctrl.email, ctrl.managerid).then(function (result) {

                if (result === true) {
                    ctrl.currentFormEmail = "final";

                    ctrl.clientname = '';
                    ctrl.emailtext = '';
                    ctrl.emailsubject = '';

                    managersService.setVisibleFooterEmail(false);
                    managersService.setPristineFormEmail();
                } else {
                    toaster.pop('error', $translate.instant('Js.Managers.ErrorRequestEmail'));
                }                   
            });
        };

    };

    ng.module('managers')
      .controller('ManagersCtrl', ManagersCtrl);

    ManagersCtrl.$inject = ['$http', 'managersService', 'toaster', '$translate'];

})(window.angular);
