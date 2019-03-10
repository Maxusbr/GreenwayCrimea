; (function (ng) {
    'use strict';

    var SwitcherStateCtrl = function () {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.checked = ctrl.checked || false;
            ctrl.invert = ctrl.invert || false;
            ctrl.textOn = ctrl.textOn || 'Активен';
            ctrl.textOff = ctrl.textOff || 'Скрыт';
        };

        ctrl.changeState = function (checked) {

            ctrl.switcher(checked);
        };

        ctrl.switcher = function (checked) {

            if (checked !== ctrl.checked) {
                ctrl.checked = checked;

                if (ctrl.onChange != null) {
                    ctrl.onChange({ checked: checked });
                }
            } 
        };
    }

    SwitcherStateCtrl.$inject = [];

    ng.module('switcherState', [])
      .controller('SwitcherStateCtrl', SwitcherStateCtrl);

})(window.angular);