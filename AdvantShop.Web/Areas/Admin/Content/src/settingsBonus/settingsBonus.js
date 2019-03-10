; (function (ng) {
    'use strict';

    var SettingsBonusCtrl = function ($http, toaster) {

        var ctrl = this;

        ctrl.uniSenderRegister = function () {
            if (!ctrl.uniSenderRegEmail || !ctrl.uniSenderRegLogin || !ctrl.uniSenderRegPassword) {
                toaster.error('Укажите данные');
                return;
            }
            ctrl.uniSenderRegProgress = true;
            $http.post('settingsbonus/unisenderregister', { email: ctrl.uniSenderRegEmail, login: ctrl.uniSenderRegLogin, password: ctrl.uniSenderRegPassword }).then(function (response) {
                var data = response.data;
                if (data.result === true) {
                    toaster.success("Вы успешно зарегистрировались в UniSender");
                    ctrl.UniSenderApiKey = data.obj.apiKey;
                } else {
                    data.errors.forEach(function (error) {
                        toaster.error('Ошибка', error);
                    });
                }
                ctrl.uniSenderRegEmail = ctrl.uniSenderRegLogin = ctrl.uniSenderRegPassword = '';
                ctrl.uniSenderRegProgress = false;
                ctrl.uniSenderRegistered = false;
            });
        }
    };

    SettingsBonusCtrl.$inject = ['$http', 'toaster'];

    ng.module('settingsBonus', [])
      .controller('SettingsBonusCtrl', SettingsBonusCtrl);

})(window.angular);