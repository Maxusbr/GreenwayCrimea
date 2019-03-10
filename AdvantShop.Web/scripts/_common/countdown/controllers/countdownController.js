; (function (ng) {
    'use strict';

    var CountdownCtrl = function ($scope) {
        var ctrl = this,
            endTime;

        ctrl.$onInit = function () {
            ctrl.init(ctrl.endTime, ctrl.endTimeUtc);
        };

        ctrl.dataTime = {};

        ctrl.getTimeleft = function () {
            var dimTime = endTime.getTime() - (new Date()).getTime();

            return dimTime > 0 ? dimTime : null;
        };

        ctrl.calc = function (timeLeft) {

            var years = 0, months = 0, days = 0, hours = 0, minutes = 0, seconds = 0;

            if (timeLeft != null) {
                years = Math.floor((timeLeft / 3600000) / 24 / 365);
                months = Math.floor(((timeLeft / 3600000) / 24 / 30) % 12);
                days = Math.floor((timeLeft / 3600000) / 24);
                hours = Math.floor((timeLeft / 3600000) % 24);
                minutes = Math.floor((timeLeft / 60000) % 60);
                seconds = Math.floor((timeLeft / 1000) % 60);

                if ( ctrl.isShowDays === false) {
                    hours = Math.floor(((timeLeft + ((timeLeft / 3600000) / 24)) / 3600000));
                    ctrl.isShowDays = false;
                }
            }

            ctrl.update(years, months, days, hours, minutes, seconds);

            return ctrl.dataTime;
        };

        ctrl.tick = function () {

            var timeLeft = ctrl.getTimeleft();

            ctrl.calc(timeLeft);

            if (timeLeft != null) {
                setTimeout(function () {
                    ctrl.tick();
                    $scope.$digest();
                }, 1000);
            }
        };

        ctrl.update = function (years, months, days, hours, minutes, seconds) {
            ctrl.dataTime.years = years;
            ctrl.dataTime.months = months;
            ctrl.dataTime.days = days;
            ctrl.dataTime.hours = hours;
            ctrl.dataTime.minutes = minutes;
            ctrl.dataTime.seconds = seconds;
        };

        ctrl.init = function (countdownTime, endTimeUtc) {
            if (endTimeUtc != null) {
                endTime = new Date(endTimeUtc);
            } else {
                var date = new Date(countdownTime);
                endTime = new Date(date.getUTCFullYear(),
                                    date.getUTCMonth(),
                                    date.getUTCDate(),
                                    date.getUTCHours(),
                                    date.getUTCMinutes(),
                                    date.getUTCSeconds());
            }
            ctrl.tick();
        };

    };

    ng.module('countdown')
      .controller('CountdownCtrl', CountdownCtrl);


    CountdownCtrl.$inject = ['$scope'];

})(window.angular);