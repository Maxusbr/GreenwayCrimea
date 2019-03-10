//#region module
; (function (ng) {
    'use strict';

    ng.module('findCheaper', []);

})(window.angular);
//#endregion

//#region controllers
; (function (ng) {

    'use strict';

    var FindCheaperCtrl = function ($http, findCheaperService, toaster, $translate) {

        var ctrl = this,
            isRenderDialog = false;

        ctrl.productName = ctrl.productName();

        ctrl.show = function () {
            ctrl.currentForm = "main";
            findCheaperService.setVisibleFooter(true);
            if (isRenderDialog === false) {

                findCheaperService.dialogRender(ctrl.modalTitle, ctrl);

                isRenderDialog = true;

            } else {
                findCheaperService.dialogOpen();
            }
        };

        ctrl.sendRequest = function () {
            if (ctrl.isShowUserAgreementText && !ctrl.agreement) {
                toaster.pop('error', $translate.instant('Js.Subscribe.ErrorAgreement'));
                return;
            }
            ctrl.currentForm = "final";
            //return $http.post('findCheaper/addrequest', { id: 0, name: ctrl.clentname, phone: ctrl.clientphone, wishprice: ctrl.wishprice, whereCheaper: ctrl.wherecheaper, price: ctrl.price, offerArtNo: ctrl.offerArtNo, productName: ctrl.productName });
            findCheaperService.setVisibleFooter(false);
            return $http.post('findCheaper/addrequest', { id: 0, clientName: ctrl.clientname, clientPhone: ctrl.clientphone, wishprice: ctrl.wishprice, whereCheaper: ctrl.wherecheaper, price: ctrl.productPrice, offerArtNo: ctrl.productOfferId, productName: ctrl.productName });
        };
    };

    ng.module('findCheaper')
      .controller('FindCheaperCtrl', FindCheaperCtrl);

    FindCheaperCtrl.$inject = ['$http', 'findCheaperService', 'toaster', '$translate'];

})(window.angular);
//#endregion

//#region services
; (function (ng) {
    'use strict';

    var findCheaperService = function ($http, $q, modalService) {

        var service = this,
            isRenderDialog = false;


        service.dialogRender = function (title, parentScope) {

            var options = {
                'modalClass': 'findCheaper-dialog',
                'isOpen': true
            };

            modalService.renderModal(
                'modalFindCheaper',
                title,
                '<div data-ng-include="\'/modules/findCheaperModule/scripts/templates/modal.html?v=2\'"></div>',
                '<input type="submit" class="btn btn-middle btn-action" value="Отправить заявку" data-button-validation data-button-validation-success="findCheaper.sendRequest()"/>',
                options,
                { findCheaper: parentScope });
            isRenderDialog = true;
        };

        service.dialogOpen = function () {
            modalService.open('modalFindCheaper');
        };

        service.dialogClose = function () {
            modalService.close('modalFindCheaper');
        };

        service.setVisibleFooter = function (visible) {
            modalService.setVisibleFooter('modalFindCheaper', visible);
        };

    };

    ng.module('findCheaper')
      .service('findCheaperService', findCheaperService);

    findCheaperService.$inject = ['$http', '$q', 'modalService'];
})(window.angular);
//#endregion

//#region directives 
; (function (ng) {
    'use strict';

    ng.module('findCheaper')
      .directive('findCheaper', function () {
          return {
              restrict: 'A',
              scope: {
                  productName: '&',
                  productPrice: '=',
                  productOfferId: '=',
                  modalTitle: '@',
                  modalTopText: '@',
                  modalFinalText: '@',
                  isShowUserAgreementText: '=?',
                  userAgreementText: '@',
              },
              controller: 'FindCheaperCtrl',
              controllerAs: 'findCheaper',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  element[0].addEventListener('click', function (event) {

                      scope.$apply(ctrl.show);

                      event.preventDefault();
                  });
              }
          };
      });
})(window.angular);
//#endregion