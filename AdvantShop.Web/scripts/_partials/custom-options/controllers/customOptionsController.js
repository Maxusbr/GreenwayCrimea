; (function (ng) {
    'use strict';

    var CustomOptionsCtrl = function (customOptionsService) {
        var ctrl = this,
            timeoutId;

        customOptionsService.getData(ctrl.productId).then(function (customOptions) {
            ctrl.items = customOptions;

            var query = customOptionsService.convertToQuery(ctrl.items);

            customOptionsService.getXml(ctrl.productId, query).then(function (xml) {
                ctrl.xml = xml;

                if (ctrl.initFn != null) {
                    ctrl.initFn({ customOptions: ctrl });
                }
            });
        });

        ctrl.eventDebounce = function (item) {
            if (timeoutId != null) {
                clearTimeout(timeoutId);
            }

            timeoutId = setTimeout(ctrl.change.bind(ctrl, item), 300);
        };

        ctrl.change = function (item) {

            var query = customOptionsService.convertToQuery(ctrl.items);

            customOptionsService.getXml(ctrl.productId, query).then(function (xml) {
                ctrl.xml = xml;
                ctrl.changeFn({ item: item });
            }); 
        };
    };

    ng.module('customOptions')
      .controller('CustomOptionsCtrl', CustomOptionsCtrl);

    CustomOptionsCtrl.$inject = ['customOptionsService'];

})(window.angular);