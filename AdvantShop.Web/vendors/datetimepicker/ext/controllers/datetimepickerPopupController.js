; (function (ng) {
    'use strict';

    var DatetimepickerPopupCtrl = function ($scope, domService) {
        var ctrl = this;

        ctrl.isShow = false;

        ctrl.deactive = function () {
            ctrl.isShow = false;
        };

        ctrl.active = function () {
            ctrl.isShow = true;
        };

        ctrl.clickOut = function (event) {
            if (ctrl.isShow === true && domService.closest(event.target, '.js-datetimepicker-popup') == null) {
                ctrl.deactive();
                $scope.$digest();
            }
        };
    };

    ng.module('ui.bootstrap.datetimepicker')
      .controller('DatetimepickerPopupCtrl', DatetimepickerPopupCtrl);

    DatetimepickerPopupCtrl.$inject = ['$scope','domService'];

})(window.angular);