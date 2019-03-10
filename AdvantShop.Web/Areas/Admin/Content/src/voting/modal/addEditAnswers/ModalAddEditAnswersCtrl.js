; (function (ng) {
    'use strict';

    var ModalAddEditAnswersCtrl = function ($uibModalInstance, $http, toaster) {
        var ctrl = this;
        
        ctrl.$onInit = function () {
            var params = ctrl.$resolve;
                ctrl.ID = params.ID != null ? params.ID : 0;
                ctrl.ThemeId = params.ThemeId != null ? params.ThemeId : 0;
                ctrl.mode = ctrl.ID != 0 ? "edit" : "add";
                ctrl.getAnswers(ctrl.ID, ctrl.ThemeId);
        };

        ctrl.getAnswers = function (ID, ThemeId) {
            $http.get('Voting/GetAnswersItem', { params: { Id: ID, ThemeId: ThemeId, rnd: Math.random() } }).then(function (response) {
                if (response.data.result == null) {
                    var answers = response.data.answers;
                    ctrl.theme = response.data.theme;
                    if (answers != null) {

                        ctrl.Name = answers.Name;
                        ctrl.SortOrder = answers.Sort;
                        ctrl.IsVisible = answers.IsVisible;
                    }
                }
                else {
                    toaster.pop("error", "Ошибка", "Не удалось загрузить данные");
                    $uibModalInstance.close('getAnswers');
                }
            });
        }
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
        
        ctrl.saveAnswer = function () {

            ctrl.btnSleep = true;

            var params = {
                ID: ctrl.ID,
                Name: ctrl.Name,
                SortOrder: ctrl.SortOrder,
                IsVisible: ctrl.IsVisible,
                ThemeId: ctrl.ThemeId,
                rnd: Math.random()
            };

            var url = ctrl.mode == "add" ? 'Voting/AddAnswers' : 'Voting/EditAnswers';

            $http.post(url, params).then(function (response) {
                var data = response.data;
                if (data.result == true) {
                    toaster.pop("success", "", "Изменения сохранены");
                    $uibModalInstance.close('saveAnswer');
                } else {
                    toaster.pop("error", "Ошибка", "Ошибка при создании/редактировании варианта ответа");
                    ctrl.btnSleep = false;
                }
            });
        }
    };

    ModalAddEditAnswersCtrl.$inject = ['$uibModalInstance', '$http', 'toaster'];

    ng.module('uiModal')
        .controller('ModalAddEditAnswersCtrl', ModalAddEditAnswersCtrl);

})(window.angular);