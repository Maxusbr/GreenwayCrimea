; (function (ng) {
    'use strict';

    var RatingCtrl = function ($http) {
        var ctrl = this;

        ctrl.items = [];

        ctrl.change = function (index) {
            if (ctrl.readonly === false) {
                for (var i = 0, l = ctrl.items.length; i < l; i++) {
                    ctrl.items[i].isHover = i <= index;
                }
            }
        };

        ctrl.select = function (val) {

            if (ctrl.readonly === false) {

                ctrl.current = val;

                for (var i = 0; i < val; i++) {
                    ctrl.items[i].isSelected = true;
                }

                if (ctrl.url) {
                    return $http.post(ctrl.url, { objId: ctrl.objId, rating: ctrl.current }).then(function (response) {
                        ctrl.current = response.data;
                        ctrl.readonly = true;
                    });
                }
            }
        };
    };

    ng.module('rating')
      .controller('RatingCtrl', RatingCtrl);

    RatingCtrl.$inject = ['$http'];

})(window.angular);