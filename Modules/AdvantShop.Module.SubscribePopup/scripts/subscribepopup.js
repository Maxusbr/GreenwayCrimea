//#region module
; (function (ng) {
    'use strict';

    ng.module('subscribePopup', []);

})(window.angular);
//#endregion

//#region controllers
; (function (ng) {

    'use strict';

    var SubscribePopupCtrl = function ($http, subscribePopupService, toaster, $translate) {

        var ctrl = this,
            isRenderDialog = false;

        ctrl.show = function () {
            ctrl.currentForm = "main";
            subscribePopupService.setVisibleFooter(true);
            if (isRenderDialog === false) {

                subscribePopupService.dialogRender(ctrl);

                isRenderDialog = true;

            } else {
                subscribePopupService.dialogOpen();
            }
        };

        ctrl.subscribe = function () {
			if (typeof (ctrl.agreement) != "undefined" && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                return;
            }
            ctrl.currentForm = "final";
            subscribePopupService.setVisibleFooter(false);
            return $http.post('subscribePopup/subscribe', { id: 0, email: ctrl.email });
        };
    };

    ng.module('subscribePopup')
      .controller('SubscribePopupCtrl', SubscribePopupCtrl);

    SubscribePopupCtrl.$inject = ['$http', 'subscribePopupService', 'toaster', '$translate'];

})(window.angular);
//#endregion

//#region services
; (function (ng) {
    'use strict';

    var subscribePopupService = function ($http, $q, modalService) {

        var service = this,
            isRenderDialog = false;


        service.dialogRender = function (parentScope) {

            var options = {
                'modalClass': 'subscribePopup-dialog',
                //'isOpen': true
                startOpenDelay: parentScope.startOpenDelay()
            };

            modalService.renderModal(
                'modalSubscribePopup',
                null,
                '<div data-ng-include="\'/modules/subscribePopup/scripts/templates/modal_old.html\'"></div>',
                null,
                options,
                { subscribePopup: parentScope });
            isRenderDialog = true;
        };

        service.dialogOpen = function () {
            modalService.open('modalSubscribePopup');
        };

        service.dialogClose = function () {
            modalService.close('modalSubscribePopup');
        };

        service.setVisibleFooter = function (visible) {
            modalService.setVisibleFooter('modalSubscribePopup', visible);
        };

    };

    ng.module('subscribePopup')
      .service('subscribePopupService', subscribePopupService);

    subscribePopupService.$inject = ['$http', '$q', 'modalService'];
})(window.angular);
//#endregion

//#region directives 
; (function (ng) {
    'use strict';

    ng.module('subscribePopup')
      .directive('subscribePopup', ['modalService', function (modalService) {
          return {
              restrict: 'A',
              scope: {
                  modalTitle: '@',
                  modalTopText: '@',
                  modalBottomText: '@',
                  modalFinalText: '@',
                  startOpenDelay: '&',
                  isShowUserAgreementText: '@',
                  userAgreementText: '@'
              },
              controller: 'SubscribePopupCtrl',
              controllerAs: 'subscribePopup',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  ctrl.show();
              }
          };
      }]);
})(window.angular);
//#endregion