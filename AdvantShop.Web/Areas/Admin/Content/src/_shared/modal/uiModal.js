; (function (ng) {
    'use strict';

    var UiModalTriggerCtrl = function ($uibModal) {
        var ctrl = this;

        ctrl.open = function () {

            var options = {
                bindToController: true,
                controller: ctrl.controller,
                controllerAs: ctrl.controllerAs || 'ctrl',
                template: ctrl.template,
                templateUrl: ctrl.templateUrl,
                resolve: ctrl.resolve,
                size: ctrl.size,
                windowClass: ctrl.windowClass,
                backdrop: ctrl.backdrop != null ? (ctrl.backdrop === 'static' ? ctrl.backdrop : ctrl.backdrop === 'true') : true,
                keyboard: ctrl.keyboard != null ? ctrl.keyboard : true
            };

            if (ctrl.onBeforeOpen != null) {
                ctrl.onBeforeOpen();
            }

            $uibModal.open(options).result.then(function (result) {
                if (ctrl.onClose != null) {
                    ctrl.onClose({ result: result });
                }
                return result;
            }, function (result) {
                if (ctrl.onDismiss != null) {
                    ctrl.onDismiss({ result: result });
                }
                return result;
            });
        };
    };

    UiModalTriggerCtrl.$inject = ['$uibModal'];

    ng.module('uiModal', ['ui.bootstrap', 'ngCropper', 'uiGridCustom', 'checklist-model'])
      .controller('UiModalTriggerCtrl', UiModalTriggerCtrl);

})(window.angular);