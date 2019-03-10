; (function (ng) {
    'use strict';

    var ModalAddAdditionBonusCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
        var ctrl = this;
        ctrl.isStartExport = false;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            ctrl.cardId = params != null && params.cardId != null ? params.cardId : null;
            ctrl.filter = params != null && params.filter != null ? params.filter : null;
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.addBonus = function () {
            ctrl.btnLoading = true;
            if (ctrl.cardId != null) {
                ctrl.singleAdd();
            } else {
                ctrl.massAdd();
            }
        };

        ctrl.singleAdd = function () {
            $http.post('cards/addAdditionBonus',
            {
                cardId: ctrl.cardId,
                amount: ctrl.amount,
                reason: ctrl.reason,
                name: ctrl.name,
                startDate: ctrl.startDate,
                endDate: ctrl.endDate
            })
                .then(function (result) {
                    var data = result.data.result;
                    if (data === true) {
                        $window.location.assign('cards/edit/' + ctrl.cardId);
                        toaster.pop('success', '', 'Бонусы добавлены');
                    } else {
                        toaster.pop('error', '', 'Ошибка при добавлении бонусов');
                    }
                },
                    function (err) {
                        toaster.pop('error', '', 'Ошибка при добавлении бонусов' + err);
                }).finally(function () {
                    ctrl.btnLoading = false;
                });;
        };

        ctrl.massAdd = function() {
            ctrl.isStartExport = true;
            $http.post('cards/addAdditionBonusmass',
                {
                    amount: ctrl.amount,
                    reason: ctrl.reason,
                    name: ctrl.name,
                    startDate: ctrl.startDate,
                    endDate: ctrl.endDate,
                    FIO: ctrl.FIO,
                    Email: ctrl.Email,
                    MobilePhone: ctrl.MobilePhone,
                    CardNumber: ctrl.CardNumber,
                    GradeId: ctrl.GradeId,
                    BonusAmountFrom: ctrl.BonusAmountFrom,
                    BonusAmountTo: ctrl.BonusAmountTo,
                    SelectMode: ctrl.SelectMode,
                    Ids: ctrl.Ids
                })
                .then(function(result) {
                        var data = result.data.result;
                        if (data === true) {
                            //$window.location.assign('cards/index');
                            toaster.pop('success', '', 'Запущено добавления');
                        } else {
                            toaster.pop('error', '', 'Ошибка при добавлении бонусов');
                        }
                    },
                    function(err) {
                        toaster.pop('error', '', 'Ошибка при добавлении бонусов');
                }).finally(function () {
                    //ctrl.btnLoading = false;
                });;
        };
    };

    ModalAddAdditionBonusCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalAddAdditionBonusCtrl', ModalAddAdditionBonusCtrl);

})(window.angular);