; (function (ng) {
    'use strict';

    var giftcertificateService = function ($http, modalService) {
        var service = this;
        
        service.dialogOpen = function () {
            modalService.open('giftcertificatePreview');
        };

        service.dialogClose = function () {
            modalService.close('giftcertificatePreview');
        };
    };

    ng.module('giftcertificate')
      .service('giftcertificateService', giftcertificateService);

    giftcertificateService.$inject = ['$http', 'modalService'];

})(window.angular);