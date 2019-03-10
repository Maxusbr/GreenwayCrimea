; (function (ng) {
    'use strict';

    var ModalSubtractMainBonusCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.cardId = params != null && params.cardId != null ? params.cardId : null;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.subtractBonus = function () {
            $http.post('cards/subtractBonus',
                {
                    cardId: ctrl.cardId,
                    amount: ctrl.Amount,
                    reason: ctrl.Reason
                })
                .then(function (result) {
                    var data = result.data.result;
                    if (data === true) {
                        $window.location.assign('cards/edit/' + ctrl.cardId);
                        toaster.pop('success', '', 'Бонусы списаны');
                    } else {
                        toaster.pop('error', '', 'Ошибка при списание бонусов');
                    }
                },
                    function (err) {
                        toaster.pop('error', '', 'Ошибка при списание бонусов');
                    });
        };
    };

    ModalSubtractMainBonusCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalSubtractMainBonusCtrl', ModalSubtractMainBonusCtrl);

})(window.angular);