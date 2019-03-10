; (function (ng) {
    'use strict';

    var DatetimepickerTriggerCtrl = function ($element) {
        var ctrl = this,
            popupStorage;

        ctrl.getSizes = function () {
            return {
                top: $element[0].offsetTop,
                left: $element[0].offsetLeft,
                height: $element[0].offsetHeight,
                width: $element[0].offsetWidth
            };
        };

        ctrl.addPopup = function (popup) {
            popupStorage = popup;
        };

        ctrl.active = function () {
            if (popupStorage != null) {
                popupStorage.active();
            }
        };
    };

    ng.module('ui.bootstrap.datetimepicker')
      .controller('DatetimepickerTriggerCtrl', DatetimepickerTriggerCtrl);

    DatetimepickerTriggerCtrl.$inject = ['$element']

})(window.angular);