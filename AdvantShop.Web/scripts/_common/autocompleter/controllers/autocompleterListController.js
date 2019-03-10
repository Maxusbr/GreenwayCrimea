; (function (ng) {
    'use strict';

    var AutocompleterListCtrl = function () {
        var ctrl = this;

        ctrl.listHover = false;

        ctrl.mouseenter = function () {
            ctrl.listHover = true;
        };

        ctrl.mouseleave = function () {
            ctrl.listHover = false;
        };

        ctrl.getStateHover = function () {
            return ctrl.listHover;
        }
    };

    ng.module('autocompleter')
      .controller('AutocompleterListCtrl',  AutocompleterListCtrl);

})(window.angular);