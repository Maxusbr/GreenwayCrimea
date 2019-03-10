; (function (ng) {
    'use strict';
    var isTouchDevice = 'ontouchstart' in document.documentElement;

    var submenuService = function ($document) {
        var service = this,
            mouseLocs = [];

        service.startSpyMove = function () {
            $document[0].addEventListener('mousemove', memory);
        };

        service.stopSpyMove = function () {
            $document[0].removeEventListener('mousemove', memory);
            mouseLocs.length = 0;
        };

        service.getMouseLocs = function () {
            return mouseLocs;
        };

        function memory(e) {
            mouseLocs.push({ x: e.pageX, y: e.pageY });

            if (mouseLocs.length > 3) {
                mouseLocs.shift();
            }
        };
    };

    ng.module('submenu')
      .service('submenuService', submenuService);

    submenuService.$inject = ['$document'];

})(window.angular);