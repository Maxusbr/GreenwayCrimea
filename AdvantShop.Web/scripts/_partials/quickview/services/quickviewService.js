; (function (ng) {
    'use strict';

    var quickviewService = function (modalService) {
        var service = this,
            isRender = false,
            data = {};

        service.dialogRender = function (parentScope) {
            modalService.renderModal('modalQuickView',
                                    null,
                                    '<div data-ng-include="\'/scripts/_partials/quickview/templates/quckviewModal.html\'"></div>',
                                    null,
                                    { 'isOpen': true, 'modalClass': 'modal-quickview', 'backgroundEnable': true },
                                    { quickview: parentScope });
        };

        service.getUrl = function (productId, colorId) {
            return 'product/productquickview?productId=' + productId + (colorId != null ? '&color=' + colorId : '');
        };

        service.dialogOpen = function (itemData, productId, colorId) {

            data.url = service.getUrl(productId, colorId);
            data.itemData = itemData;
            data.productId = productId;
            data.next = service.next;
            data.prev = service.prev;

            if (isRender === false) {
                service.dialogRender(data);
                isRender = true;
            } else {
                modalService.open('modalQuickView');
            }
        };

        service.dialogClose = function () {
            modalService.close('modalQuickView');
        }

        service.prev = function () {

            var index, indexPrev;

            index = data.itemData.siblings.indexOf(data.productId);
            indexPrev = index - 1;

            if (data.itemData.siblings[indexPrev] != null) {
                data.productId = data.itemData.siblings[indexPrev];
                data.url = service.getUrl(data.productId);
            }

        };

        service.next = function () {

            var index, indexNext;

            index = data.itemData.siblings.indexOf(data.productId);

            indexNext = index + 1;

            if (data.itemData.siblings[indexNext] != null) {
                data.productId = data.itemData.siblings[indexNext];
                data.url = service.getUrl(data.productId);
            }

        };
    };


    ng.module('quickview')
      .service('quickviewService', quickviewService);

    quickviewService.$inject = ['modalService'];

})(window.angular);