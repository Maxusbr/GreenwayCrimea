; (function (ng) {
    'use strict';

    ng.module('audioPlayer')
        .component('audioPlayer', {
            templateUrl: '../areas/admin/content/src/_shared/audio-player/templates/audio-player.html',
            controller: 'AudioPlayerCtrl',
            bindings: {
                src: '<',
                loading: '<',
                onLoadSrc: '&'
            }
        });
})(window.angular);