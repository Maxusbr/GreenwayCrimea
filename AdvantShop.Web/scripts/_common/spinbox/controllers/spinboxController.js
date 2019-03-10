; (function (ng) {
    'use strict';

    var SpinboxCtrl = function ($timeout, spinboxKeyCodeAllow) {
        var ctrl = this,
            callbackTimerId;

        //ctrl.min = ctrl.min() || 0;
        //ctrl.max = ctrl.max() || Number.POSITIVE_INFINITY;
        //ctrl.step = ctrl.step() || 1;

        var callbackCall = function () {

            if (callbackTimerId != null) {
                $timeout.cancel(callbackTimerId);
            }

            callbackTimerId = $timeout(function () {
                if (ctrl.updateFn != null) {
                    ctrl.updateFn({ value: ctrl.value, proxy: ctrl.proxy });
                }
            }, 700);
        };

        ctrl.$onInit = function () {
            ctrl.checkButtons(ctrl.value);
        };

        ctrl.less = function () {
            var newValue = ctrl.value - ctrl.step;

            newValue = ctrl.checkRange(newValue) === true ? newValue : ctrl.min != null ? ctrl.min : newValue;

            ctrl.value = ctrl.numberRound(newValue);

            ctrl.checkButtons(ctrl.value);

            callbackCall();
        };

        ctrl.more = function () {
            var newValue = ctrl.value + ctrl.step;

            newValue = ctrl.checkRange(newValue) === true ? newValue : ctrl.max != null ? ctrl.max : newValue;

            ctrl.value = ctrl.numberRound(newValue);

            ctrl.checkButtons(ctrl.value);

            callbackCall();
        };

        ctrl.checkRange = function (newValue) {
            return newValue <= ctrl.max && newValue >= ctrl.min;
        };

        ctrl.checkRegex = function (char) {
            return /\d/g.test(char);
        };

        ctrl.keydown = function (event) {
            var symbol;

            if (event.altKey || event.ctrlKey || event.shiftKey) {
                event.preventDefault();
                return;
            }

            var code = ctrl.prepareNumpad(event.keyCode);

            if (!ctrl.isExistKeyCodeAllow(code)) {
                symbol = Number(String.fromCharCode(code));

                if (isNaN(symbol) === true) {
                    event.preventDefault();
                }
            } else {
                switch (code) {
                    case 40:
                        // down arrow
                        ctrl.less();
                        break;
                    case 38:
                        // up arrow
                        ctrl.more();
                        break;
                }
            }
        };

        ctrl.keyup = function (event) {
            var code = ctrl.prepareNumpad(event.keyCode),
                symbol = Number(String.fromCharCode(code));

            //update if number 
            if (isNaN(symbol) === false) {
                callbackCall();
            }
        };

        ctrl.prepareNumpad = function (keycode) {
            return keycode > 95 && keycode < 106 ? keycode - 48 : keycode;
        };

        ctrl.isExistKeyCodeAllow = function (keycode) {
            var result = false;

            for (var key in spinboxKeyCodeAllow) {
                if (spinboxKeyCodeAllow[key] == keycode) {
                    result = true;
                    break;
                }
            }

            return result;
        };

        ctrl.checkButtons = function (newValue) {
            ctrl.lessBtnDisabled = newValue === ctrl.min;
            ctrl.moreBtnDisabled = newValue === ctrl.max;
        };

        ctrl.valueFoldStep = function () {
            var remainder = ctrl.value % ctrl.step,
                whole,
                newValue;

            if (remainder !== 0 || ctrl.value === 0) {

                whole = ctrl.value - remainder;

                newValue = whole + ctrl.step;

                newValue = ctrl.checkRange(newValue) === true ? newValue : ctrl.max != null ? ctrl.max : newValue;

                ctrl.value = ctrl.numberRound(newValue);

                ctrl.checkButtons(ctrl.value);

                callbackCall();
            }
        };

        ctrl.numberRound = function (newValue) {
            //http://0.30000000000000004.com/
            //http://stackoverflow.com/questions/588004/is-floating-point-math-broken/588014#588014
            return parseFloat(newValue.toPrecision(12));
        };
    };

    ng.module('spinbox')
      .controller('SpinboxCtrl', SpinboxCtrl);

    SpinboxCtrl.$inject = ['$timeout', 'spinboxKeyCodeAllow'];

})(window.angular);