; (function (ng) {
    'use strict';

    var CssEditorCtrl = function ($http, $q, toaster) {

        var ctrl = this;

        ctrl.save = function() {
            $http.post("design/cssEditor", { value: ctrl.text }).then(function (response) {
                if (response.data.result) {
                    toaster.pop('success', '', 'Изменения сохранены');
                } else {
                    toaster.pop('error', '', 'Изменения не сохранены');
                }
            });
        }


        $(window).bind('keydown', function (event) {
            if (event.ctrlKey || event.metaKey) {
                switch (String.fromCharCode(event.which).toLowerCase()) {
                    case 's':
                        event.preventDefault();
                        //$("form[name='form']").submit();
                        ctrl.save();
                        break;
                }
            }
        });

    };

    CssEditorCtrl.$inject = ['$http', '$q', 'toaster'];

    ng.module('csseditor', [])
      .controller('CssEditorCtrl', CssEditorCtrl);

})(window.angular);