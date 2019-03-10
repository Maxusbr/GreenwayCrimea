; (function (ng) {

    var telephonyService = function ($http, modalService) {
        var service = this,
            modalId = "telephonyModal",
            modalRendered = false;


        service.dialogOpen = function (title, message) {
            if (modalRendered === false) {
                modalService.renderModal(modalId, title, message);
            }

            modalService.getModal(modalId).then(function (modal) {
                modal.modalScope.open();
            });
        };


        service.call = function (phone, check) {
            return $http.post('common/callBack', { phone: phone.replace(/\D+/g, ''), check: check }).then(function (response) {
                return response.data;
            });
        };
    };

    ng.module('telephony')
      .service('telephonyService', telephonyService);

    telephonyService.$inject = ['$http', 'modalService'];

})(window.angular);