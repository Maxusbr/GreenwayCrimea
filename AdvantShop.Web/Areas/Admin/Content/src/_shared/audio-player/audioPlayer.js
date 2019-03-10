; (function (ng) {
    'use strict';

    var AudioPlayerCtrl = function ($http, $element, $scope) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.timelineWidth = $element[0].querySelector(".audio-progress").offsetWidth;
            ctrl.curPos = '00:00';
            ctrl.barStyle = {'background': 'linear-gradient(to right, #0e90d2 0%, #ababab 0%)'};
            ctrl.isplaying = false;
            ctrl.initAudio(ctrl.src);
        };

        ctrl.initAudio = function (src) {
            if (!src) return;
            ctrl.audio = new Audio();
            ctrl.audio.type = "audio/mpeg";
            ctrl.audio.src = src;
            ctrl.audio.addEventListener('timeupdate', timeupdate);
            ctrl.audio.addEventListener('error', function (e) {
                ctrl.noAudio = true;
                ctrl.isplaying = false;
                ctrl.audio = null;
                $scope.$apply();
            });
        }

        ctrl.play = function (loaded) {
            if (!ctrl.audio && !ctrl.noAudio && ctrl.onLoadSrc && !loaded) {
                ctrl.onLoadSrc().then(function (result) {
                    ctrl.initAudio(result);
                    ctrl.play(true);
                });
                return;
            }
            if (!ctrl.audio) {
                ctrl.noAudio = true;
                return;
            }

            if (ctrl.isplaying) {
                ctrl.audio.pause();
                ctrl.isplaying = false;
            } else {
                ctrl.audio.play();
                ctrl.isplaying = true;
            }
        };

        ctrl.toggleVolume = function () {
            if (!ctrl.audio) return;

            ctrl.audio.muted = !ctrl.audio.muted;
        }

        ctrl.setTime = function ($event) {
            if (!ctrl.audio) return;

            ctrl.audio.removeEventListener('timeupdate', timeupdate, true);
            var position = $event.clientX - $event.target.getBoundingClientRect().left;
            var percent = (position / ctrl.timelineWidth) * 100;
            ctrl.audio.currentTime = (percent * ctrl.audio.duration) / 100;
        };

        ctrl.reset = function () {
            if (!ctrl.audio) return;

            ctrl.audio.addEventListener('timeupdate', timeupdate);
        };

        function formatTime(totalSeconds) {
            if (isNaN(totalSeconds)) return null;

            var minutes = Math.floor(totalSeconds / 60);
            var seconds = totalSeconds - (minutes * 60);
            if (minutes < 10)
                minutes = "0" + minutes;
            if (seconds < 10)
                seconds = "0" + seconds;
            minutes += "";
            seconds += "";

            return minutes + ':' + seconds.substring(0, 2);
        }

        function timeupdate() {
            if (!ctrl.audio) return;

            if (ctrl.audio.currentTime >= ctrl.audio.duration) {
                ctrl.isplaying = false;
                $scope.$apply();
                return;
            }
            ctrl.curPos = formatTime(ctrl.audio.currentTime);
            var percent = Math.ceil((ctrl.audio.currentTime / ctrl.audio.duration) * 100);
            ctrl.barStyle.background = 'linear-gradient(to right, #0e90d2 ' + percent + '%, #ababab ' + percent + '%)';
            if (!ctrl.duration)
                ctrl.duration = formatTime(ctrl.audio.duration);
            $scope.$apply();
        };
    };

    AudioPlayerCtrl.$inject = ['$http', '$element', '$scope'];

    ng.module('audioPlayer', [])
        .controller('AudioPlayerCtrl', AudioPlayerCtrl);

})(window.angular);
