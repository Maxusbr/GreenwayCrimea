; (function (ng) {

    'use strict';

    ng.module('callback', []);
})(window.angular);

; (function (ng) {

    'use strict';

    var CallbackCtrl = function ($sce, callbackService, toaster, $translate) {

        var ctrl = this, isRender = false;

        ctrl.currentForm = "main";

        ctrl.send = function () {
            if (ctrl.isShowUserAgreementText && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                return;
            }
            callbackService.send(ctrl.name, ctrl.phone, ctrl.comment).then(function (result) {
                if (result == true) {
                    ctrl.currentForm = "final";
                    callbackService.setVisibleFooter(false);

                    $(document).trigger("module_callback");
                }
            });
        }

        ctrl.dialogOpen = function () {

            if (ctrl.currentForm == "final") {
                ctrl.currentForm = "main";
                ctrl.name = "";
                ctrl.phone = "";
                ctrl.comment = "";
                callbackService.setVisibleFooter(true);
            }

            if (isRender) {
                callbackService.dialogOpen();
            } else {
                callbackService.getParams().then(function (result) {                    
                    ctrl.modalText = $sce.trustAsHtml(result.ModalText);
                    ctrl.showCommentField = result.ShowCommentField;
                    ctrl.isShowUserAgreementText = result.IsShowUserAgreementText;
                    ctrl.userAgreementText = result.UserAgreementText;
                    callbackService.dialogRender(result.Title, ctrl);
                });
            }
            isRender = true;
        }
    };

    ng.module('callback')
      .controller('CallbackCtrl', CallbackCtrl);

    CallbackCtrl.$inject = ['$sce', 'callbackService', 'toaster', '$translate'];

})(window.angular);

; (function (ng) {
    'use strict';

    ng.module('callback')
      .directive('callbackStart', ['$compile', function ($compile) {
          return {
              restrict: 'A',
              link: function (scope, element, attrs, ctrl) {
                  var callbacks = document.querySelectorAll('[data-callback]');
                  $compile(callbacks)(scope);
              }
          };
      }]);

    ng.module('callback')
      .directive('callback', function () {
          return {
              restrict: 'A',
              scope: true,
              controller: 'CallbackCtrl',
              controllerAs: 'callback',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  element.on('click', function (event) {
                      event.preventDefault();
                      ctrl.dialogOpen();
                      scope.$apply();
                  });
              }
          };
      });
})(window.angular);

; (function (ng) {
    'use strict';

    var callbackService = function ($http, modalService) {
        var service = this;

        service.send = function (name, phone, comment) {

            return $http.post('callback/addcallback', { name: name, phone: phone, comment: comment, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.getParams = function () {

            return $http.get('callback/getparams').then(function (response) {
                return response.data;
            });
        };

        service.dialogRender = function (title, parentScope) {

            var options = {
                'modalClass': 'callback-dialog',
                'isOpen': true
            };

            modalService.renderModal(
                'modalCallback',
                title,
                '<div data-ng-include="\'/modules/callback/scripts/templates/modal.html\'"></div>',
                '<input type="submit" class="btn btn-middle btn-action" value="Отправить" data-ng-click="callback.send()" data-ng-disabled="modalCallbackForm.$invalid" />',
                options,
                { callback: parentScope });
        };

        service.dialogOpen = function () {
            modalService.open('modalCallback');
        };

        service.dialogClose = function () {
            modalService.close('modalCallback');
        };

        service.setVisibleFooter = function (visible) {
            modalService.setVisibleFooter('modalCallback', visible);
        };
    };

    ng.module('callback')
      .service('callbackService', callbackService);

    callbackService.$inject = ['$http', 'modalService'];

})(window.angular);