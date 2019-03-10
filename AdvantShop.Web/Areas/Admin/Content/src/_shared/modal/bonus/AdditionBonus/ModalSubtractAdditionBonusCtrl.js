; (function (ng) {
    'use strict';

    var ModalSubtractAdditionBonusCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.cardId = params != null && params.cardId != null ? params.cardId : null;

            $http.get('cards/getAdditionBonus?cardId='+ ctrl.cardId )
                 .then(function (result) {
                     ctrl.additionBonuses = result.data.obj;
                 },
                 function (err) {
                     toaster.pop('error', '', 'Ошибка получения бонусов' + err);
                 });

        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.subctractBonus = function () {
            $http.post('cards/subtractAdditionBonus',
            {
                cardId: ctrl.cardId,
                amount: ctrl.amount,
                reason: ctrl.reason,
                additionId: ctrl.additionId
            })
                .then(function (result) {
                    var data = result.data.result;
                    if (data === true) {
                        $window.location.assign('cards/edit/' + ctrl.cardId);
                        toaster.pop('success', '', 'Бонусы списаны');
                    } else {
                        toaster.pop('error', '', 'Ошибка при списывание бонусов');
                    }
                },
                    function (err) {
                        toaster.pop('error', '', 'Ошибка при списывание бонусов' + err);
                    });
        };
    };

    ModalSubtractAdditionBonusCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalSubtractAdditionBonusCtrl', ModalSubtractAdditionBonusCtrl);

})(window.angular);