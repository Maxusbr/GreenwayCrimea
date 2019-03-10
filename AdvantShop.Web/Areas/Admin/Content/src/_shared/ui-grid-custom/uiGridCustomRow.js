; (function (ng) {
    'use strict';

    var UiGridCustomRowCtrl = function ($scope) {
        var ctrl = this;

        ctrl.mouseenterRow = function ($event) {
            ctrl.isHover = true;

            $scope.$broadcast('uiGridCustomRowMouseenter');
        };

        ctrl.mouseleaveRow = function ($event) {
            ctrl.isHover = false;

            $scope.$broadcast('uiGridCustomRowMouseleave');
        };
    };

    UiGridCustomRowCtrl.$inject = ['$scope'];

    ng.module('uiGridCustom')
      .controller('UiGridCustomRowCtrl', UiGridCustomRowCtrl);

})(window.angular);