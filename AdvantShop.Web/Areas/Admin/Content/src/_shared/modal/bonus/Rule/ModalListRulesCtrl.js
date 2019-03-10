; (function (ng) {
    'use strict';

    var ModalListRulesCtrl = function ($uibModalInstance, $http, $window, toaster, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            var params = ctrl.$resolve.params;
           
            $http.get('rules/GetRuleTypes')
                 .then(function (result) {
                     ctrl.RuleTypes = result.data.obj;
                 },
                 function (err) {
                     toaster.pop('error', '', 'Ошибка получения правил' + err);
                 });

        };
        
        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };

        ctrl.saveRule = function () {
            ctrl.btnLoading = true;
            $http.post('rules/create',
                {
                    RuleType: ctrl.RuleType
                })
                .then(function (result) {
                    var data = result.data.result;
                    if (data === true) {
                        $window.location.assign('rules/edit/' + ctrl.RuleType);
                        toaster.pop('success', '', 'Правило добавлено');
                    } else {
                        toaster.pop('error', '', 'Ошибка при добавлении правила');
                    }
                },
                    function (err) {
                        toaster.pop('error', '', 'Ошибка при добавлении правила');
                    }).finally(function () {
                        ctrl.btnLoading = false;
                    });;
        };
    };

    ModalListRulesCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'toaster', '$q'];

    ng.module('uiModal')
        .controller('ModalListRulesCtrl', ModalListRulesCtrl);

})(window.angular);