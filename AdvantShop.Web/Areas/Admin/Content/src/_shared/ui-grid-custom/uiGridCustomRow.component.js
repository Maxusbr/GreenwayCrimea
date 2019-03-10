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
      .directive('uiGridCustomRow', function () {
          return {
              transclude: true,
              controller: 'UiGridCustomRowCtrl',
              controllerAs: '$ctrl',
              bindToController: true,
              template: '<div ng-class="{\'ui-grid-custom-row-hover\': $ctrl.isHover}" ng-mouseenter="$ctrl.mouseenterRow($event)" ng-mouseleave="$ctrl.mouseleaveRow($event)" ng-transclude></div>'
          };
      });

})(window.angular);