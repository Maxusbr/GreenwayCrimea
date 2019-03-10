; (function (ng) {
    'use strict';

    var CountdownCtrl = function ($timeout) {
        var ctrl = this,
            endTime;

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
                days = Math.floor(((timeLeft / 3600000) / 24) % 30);
                hours = Math.floor((timeLeft / 3600000) % 24);
                minutes = Math.floor((timeLeft / 60000) % 60);
                seconds = Math.floor((timeLeft / 1000) % 60);
            } 

            ctrl.update(years, months, days, hours, minutes, seconds);

            return ctrl.dataTime;
        };

        ctrl.tick = function () {

            var timeLeft = ctrl.getTimeleft();

            ctrl.calc(timeLeft);



            if (timeLeft != null) {
                $timeout(ctrl.tick, 1000);
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

        ctrl.init = function (countdownTime) {
             var date = new Date(countdownTime);
             endTime = new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
            ctrl.tick();
        };

    };

    ng.module('countdown')
      .controller('CountdownCtrl', CountdownCtrl);


    CountdownCtrl.$inject = ['$timeout'];

})(window.angular);