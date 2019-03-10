; (function (ng) {
    'use strict';

    var ModalAddEditVotingCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.ID = params.ID != null ? params.ID : 0;
            ctrl.mode = ctrl.ID != 0 ? "edit" : "add";

            if (ctrl.mode == "edit") {
                ctrl.getVoting(ctrl.ID);
            }
            else {
                ctrl.ID = 0;
            }
        };

        ctrl.getVoting = function (ID) {
            $http.get('Voting/GetVotingItem', { params: { ID: ID, rnd: Math.random() } }).then(function (response) {
                var data = response.data;
                if (data != null) {

                    ctrl.Name = data.Name;
                    ctrl.IsDefault = data.IsDefault;
                    ctrl.IsHaveNullVoice = data.IsHaveNullVoice;
                    ctrl.IsClose = data.IsClose;
                    ctrl.PsyID = data.PsyID;
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.saveVoting = function () {

            ctrl.btnSleep = true;

            var params = {
                ID: ctrl.ID,
                Name: ctrl.Name,
                IsDefault: ctrl.IsDefault,
                IsHaveNullVoice: ctrl.IsHaveNullVoice,
                IsClose: ctrl.IsClose,
                PsyID: ctrl.PsyID,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'Voting/AddVoting' : 'Voting/EditVoting';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close('saveVoting');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании темы голосования");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditVotingCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditVotingCtrl', ModalAddEditVotingCtrl);

})(window.angular);