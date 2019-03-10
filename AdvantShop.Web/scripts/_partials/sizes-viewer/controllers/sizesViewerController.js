; (function (ng) {
    'use strict';

    var SizesViewerCtrl = function () {
        var ctrl = this;

        ctrl.sizes = ctrl.sizes();

        ctrl.initSizes({ sizesViewer: ctrl });
    };

    ng.module('sizesViewer')
      .controller('SizesViewerCtrl', SizesViewerCtrl);

    SizesViewerCtrl.$inject = [];

})(window.angular);