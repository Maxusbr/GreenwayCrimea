; (function (ng) {
    'use strict';

    var tabsService = function () {
        var service = this,
            countInStorage = -1,
            storage = {};

        service.addInStorage = function (tabs, id) {
            storage[id || (countInStorage += + 1)] = tabs;
        };

        service.change = function (id) {
            var data = service.findTabByid(id);

            if (data != null) {
                data.tabs.change(data.pane);
            }
        };

        service.findTabByid = function (id) {

            var tabs, pane;

            for (var key in storage) {

                if (pane != null) {
                    break;
                }

                tabs = storage[key];

                if (storage.hasOwnProperty(key)) {
                    pane = tabs.panes[id];
                    break;
                }
            }

            return pane != null ? { tabs: tabs, pane: pane } : null;
        };
    };

    ng.module('tabs')
      .service('tabsService', tabsService);

})(window.angular);