; (function (ng, body) {
    'use strict';

    var HarmonicaTileCtrl = function (domService) {

        var ctrl = this;

        ctrl.tileActive = function (event) {
            //if (document.body.offsetWidth <= document.body.offsetWidth) {
            //    document.body.style.overflowX = 'hidden';
            //}

            event.stopPropagation();

            ctrl.hoverTileSubmenu = true;
            ctrl.submenuInvert = false;

            //ctrl.checkSubmenuOrientation(submenu);

            ctrl.isVisibleTileSubmenu = true;
        };

        ctrl.tileDeactive = function (event) {

            event.stopPropagation();

            ctrl.hoverTileSubmenu = false;
            ctrl.isVisibleTileSubmenu = false;

            //document.body.style.overflowX = 'auto';
        };


        ctrl.clickOut = function (event) {
            if (domService.closest(event.target, '.js-harmonica-tile') == null) {
                ctrl.hoverTileSubmenu = false;
                ctrl.submenuInvert = false;
                ctrl.isVisibleTileSubmenu = false;
            }
        };


        ctrl.tileClick = function (event) {
            ctrl.isVisibleTileSubmenu === true ? ctrl.tileDeactive(event) : ctrl.tileActive(event);
        };

    }

    ng.module('harmonica')
    .controller('HarmonicaTileCtrl', HarmonicaTileCtrl);

    HarmonicaTileCtrl.$inject = ['domService'];

})(angular, document.body);