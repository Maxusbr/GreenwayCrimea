; (function (ng) {
    'use strict';

    var ColorsViewerCtrl = function () {
        var ctrl = this,
            stopLoop = false,
            isFindedByStart = false,
            findedStartColorById,
            findedStartColorByIdAndMain;

        ctrl.$onInit = function () {
            ctrl.dirty = false;

            ctrl.multiselect = ctrl.multiselect === true;

            if (ctrl.multiselect === true) {
                ctrl.colorSelected = [];

                if (ctrl.colors != null && ctrl.colors.length > 0) {
                    for (var i = 0, len = ctrl.colors.length; i < len; i++) {
                        if (ctrl.colors[i].Selected === true) {
                            ctrl.colorSelected.push(ctrl.colors[i]);
                        }
                    }
                }
            } else {

                if (ctrl.startSelectedColors != null && ctrl.startSelectedColors.length > 0) {
                    for (var s = 0, lenS = ctrl.startSelectedColors.length; s < lenS; s++) {
                        for (var c = 0, lenC = ctrl.colors.length; c < lenC; c++) {
                            if (ctrl.colors[c].ColorId === ctrl.startSelectedColors[s]) {

                                if (ctrl.colors[c].Main === 1) {
                                    findedStartColorByIdAndMain = ctrl.colors[c];
                                    stopLoop = true;
                                } else if (findedStartColorById == null) {
                                    findedStartColorById = ctrl.colors[c];
                                }

                                isFindedByStart = true;
                            }
                        }

                        if (stopLoop === true) {
                            stopLoop = false;
                            break;
                        }
                    }
                }

                if (isFindedByStart === false) {
                    for (var m = 0, l = ctrl.colors.length; m < l; m++) {
                        if (ctrl.colors[m].Main === 1) {
                            ctrl.colorSelected = ctrl.colors[m];
                            break;
                        }
                    }
                } else {
                    ctrl.colorSelected = findedStartColorByIdAndMain || findedStartColorById;
                }
            }

            if (ctrl.initColors != null) {
                ctrl.initColors({ colorsViewer: ctrl });
            }

        };

        ctrl.getImagePath = function (photoName) {
            return 'pictures/color/' + ctrl.imageType.toLowerCase() + '/' + photoName;
        };

        ctrl.selectColor = function (event, color) {

            var indexInSelectedArray;

            if (ctrl.multiselect === true) {

                indexInSelectedArray = ctrl.colorSelected.indexOf(color);

                if (indexInSelectedArray > -1) {
                    ctrl.colorSelected.splice(indexInSelectedArray, 1);
                    //свойство используется в фильтрах
                    color.Selected = false;
                } else {
                    ctrl.colorSelected.push(color);
                    //свойство используется в фильтрах
                    color.Selected = true;
                }
            } else {
                ctrl.colorSelected = color;
            }

            ctrl.dirty = true;

            ctrl.changeColor({ event: event, color: color });
        };

        ctrl.getDirtyState = function () {
            return ctrl.dirty;
        };

    };

    ng.module('colorsViewer')
      .controller('ColorsViewerCtrl', ColorsViewerCtrl);

    ColorsViewerCtrl.$inject = [];

})(window.angular);