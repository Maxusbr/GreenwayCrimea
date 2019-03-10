; (function (ng) {
    'use strict';

    var InplaceRichCtrl = function (inplaceService) {
        var ctrl = this;

        //ctrl.inplaceParams = ctrl.inplaceParams();

        ctrl.active = function () {
            ctrl.isShow = true;
            ctrl.startContent = ctrl.editor.getData();
        };

        ctrl.save = function () {

            var content = ctrl.editor.getData(), params;

            if (ctrl.startContent === content) {
                return;
            }

            params = ng.extend(ctrl.getParams(), { content: content });

            inplaceService.save(ctrl.inplaceUrl, params).finally(function () {
                ctrl.isShow = false;
            });
        };

        ctrl.cancel = function () {
            ctrl.isShow = false;
            ctrl.editor.setData(ctrl.startContent);
        };
    };

    ng.module('inplace')
      .controller('InplaceRichCtrl', InplaceRichCtrl);

    InplaceRichCtrl.$inject = ['inplaceService'];

})(window.angular);