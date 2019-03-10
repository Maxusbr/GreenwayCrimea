; (function (ng) {
    'use strict';

    var AutoSaveTextCtrl = function ($attrs, $http, toaster) {
        var ctrl = this;

        ctrl.save = function (paramName, parameters) {
            
            var params = {};
            params[paramName] = ctrl.ngModel.$viewValue;
            
            $http.post($attrs.url, ng.extend(params, parameters || {})).then(function (response) {
                var data = response.data;
                if (data.result) {
                    toaster.pop('success', '', 'Изменения успешно сохранены');
                
                } else if (data.errors != null) {
                    data.errors.forEach(function(err) {
                        toaster.pop('success', '', err);
                    });
                }
            });
        }
    };

    AutoSaveTextCtrl.$inject = ['$attrs', '$http', 'toaster'];

    ng.module('autosaveText', [])
        .controller('AutoSaveTextCtrl', AutoSaveTextCtrl)
        .directive('autosaveText', ['$parse', function ($parse) {
            return {
                require: {
                    ngModel: 'ngModel'
                },
                scope: true,
                controller: AutoSaveTextCtrl,
                controllerAs: 'autosaveText',
                bindToController: true
            }
        }]);

})(window.angular);