; (function (ng) {
    'use strict';

    var ModalAddMainBonusCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
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
            $http.post('cards/addbonus',
                 {
                     cardId: ctrl.cardId,
                     amount: ctrl.Amount,
                     reason: ctrl.Reason
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
                         toaster.pop('error', '', 'Ошибка при добавлении бонусов');
                     }).finally(function () {
                         ctrl.btnLoading = false;
                     });
        };

        ctrl.massAdd = function () {
            ctrl.isStartExport = true;
            $http.post('cards/addbonusmass',
                 {
                     amount: ctrl.Amount,
                     reason: ctrl.Reason,
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
                 .then(function (result) {
                     var data = result.data.result;
                     if (data === true) {
                         //$window.location.assign('cards/index');
                         toaster.pop('success', '', 'Запущено добавления');
                     } else {
                         toaster.pop('error', '', 'Ошибка при добавлении бонусов');
                     }
                 },
                     function (err) {
                         toaster.pop('error', '', 'Ошибка при добавлении бонусов');
                     }).finally(function() {
                         //ctrl.btnLoading = false;
                     });
        };
    };

    ModalAddMainBonusCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalAddMainBonusCtrl', ModalAddMainBonusCtrl);

})(window.angular);