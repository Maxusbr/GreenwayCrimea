; (function (ng) {
    'use strict';

    var ModalUserInfoPopupCtrl = function ($uibModalInstance, $http, $window, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
            
            ctrl.q2Options = [
                { text: "Еще не продавали" },
                { text: "Продавали в соц.сетях" },
                { text: "Делали рекламу в интернете" },
                { text: "Был/Есть интернет-магазин" }
            ];

            ctrl.q3Options = [
                { text: "Есть розница" },
                { text: "Есть оптовые" },
                { text: "Оптовые и розничные" },
                { text: "Пока нет" }
            ];

            ctrl.userData = params;
            if (ctrl.userData.Map != null && ctrl.userData.Map.length >= 3) {

                ctrl.Question1 = parseInt(ctrl.userData.Map.filter(function (x) { return x.Name === 'Количество сотрудников в магазине' })[0].Value);
                ctrl.Question2 = ctrl.userData.Map.filter(function (x) { return x.Name === 'Ваш опыт продаж в интернете?' })[0].Value;
                ctrl.Question3 = ctrl.userData.Map.filter(function (x) { return x.Name === 'Есть ли у вас точки продаж в офлайне?' })[0].Value;
            }
        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };


        ctrl.saveUserInfo = function() {
            ctrl.userData.Map = [
                {
                    Name: "Количество сотрудников в магазине",
                    Value: ctrl.Question1,
                },
                {
                    Name: "Ваш опыт продаж в интернете?",
                    Value: ctrl.Question2,
                },
                {
                    Name: "Есть ли у вас точки продаж в офлайне?",
                    Value: ctrl.Question3,
                }
            ];

            $http.post('home/saveUserInformation', ctrl.userData).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.pop('success', '', 'Спасибо за ответы');
                    $uibModalInstance.close({username: data.fio});
                } else {
                    toaster.pop('error', '', data.errors);
                }

                ctrl.btnLoading = false;
            });
        }
    };

    ModalUserInfoPopupCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster'];

    ng.module('uiModal')
        .controller('ModalUserInfoPopupCtrl', ModalUserInfoPopupCtrl);

})(window.angular);