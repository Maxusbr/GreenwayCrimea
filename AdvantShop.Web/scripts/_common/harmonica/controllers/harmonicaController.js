; (function (ng, body) {
    'use strict';

    var HarmonicaCtrl = function ($element, $attrs) {

        var ctrl = this,
            tileWidth = null,
            scopeTileVisible = false,
            scopeTile;

        ctrl.harmonicaClassTile = $attrs.harmonicaClassTile;
        ctrl.harmonicaClassTileRow = $attrs.harmonicaClassTileRow;
        ctrl.harmonicaClassTileLink = $attrs.harmonicaClassTileLink;
        ctrl.harmonicaClassTileSubmenu = $attrs.harmonicaClassTileSubmenu;
        ctrl.harmonicaTileOuterWidth = $attrs.harmonicaTileOuterWidth;

        ctrl.links = [];
        ctrl.items = [];

        ctrl.addItem = function (itemElement, itemScope) {
            ctrl.items.push({
                itemElement: itemElement,
                itemScope: itemScope,
                itemWidth: ctrl.getOuterWidth(itemElement)
            });
        };

        ctrl.addLink = function (linkHref, linkText, linkScope) {
            ctrl.links.push({
                linkHref: linkHref,
                linkText: linkText,
                linkScope: linkScope
            });
        };

        ctrl.getLinks = function () {
            return ctrl.links;
        };

        ctrl.saveTileScope = function (scope) {
            scopeTile = scope;
        };

        ctrl.calc = function (containerWidth, items) {

            containerWidth = containerWidth || $element[0].offsetWidth;
            items = items || ctrl.items;

            var sumWidth = 0,
                sumWidthTemp = 0,
                sliceIndex = null,
                dimSumWidth = 0,
                dimItems = [];

            for (var i = 0, l = items.length; i < l; i++) {
                sumWidthTemp = sumWidth + items[i].itemWidth;

                if (containerWidth < sumWidthTemp) {
                    sliceIndex = i;
                    break;
                } else {
                    sumWidth = sumWidthTemp;
                }
            }

            if (sliceIndex === null) {
                sliceIndex = items.length;
            } else {

                dimItems = items.slice(sliceIndex);
                for (var j = 0, jl = dimItems.length ; j < jl; j++) {
                    dimSumWidth = dimSumWidth + dimItems[j].itemWidth;
                }

                tileWidth = tileWidth || angular.isDefined(ctrl.harmonicaTileOuterWidth) ? parseInt(ctrl.harmonicaTileOuterWidth) : 0;

                if (containerWidth < sumWidth + tileWidth) {
                    sliceIndex = sliceIndex - 1;
                }
            }

            return sliceIndex;
        };

        ctrl.setVisible = function (indexHidden) {
            ctrl.setVisibleForItems(indexHidden, ctrl.items);
            ctrl.setVisibleForLinks(indexHidden, ctrl.links);

            scopeTileVisible = indexHidden !== ctrl.items.length;

            if (angular.isDefined(scopeTile)) {
                scopeTile.isVisibleTile = scopeTileVisible;
            }
        };

        ctrl.getVisibleTile = function () {
            return scopeTileVisible;
        };

        ctrl.getCssClassesForTile = function () {
            return {
                harmonicaClassTile: ctrl.harmonicaClassTile,
                harmonicaClassTileRow: ctrl.harmonicaClassTileRow,
                harmonicaClassTileLink: ctrl.harmonicaClassTileLink,
                harmonicaClassTileSubmenu: ctrl.harmonicaClassTileSubmenu
            };
        };

        ctrl.setVisibleForItems = function (indexHidden, items) {
            for (var i = 0, l = items.length; i < l ; i++) {
                items[i].itemScope.isVisibleInMenu = i < indexHidden;
            }
        };

        ctrl.setVisibleForLinks = function (indexHidden, links) {
            for (var i = 0, l = links.length; i < l ; i++) {
                links[i].linkScope.isVisibleInTile = i >= indexHidden;
            }
        };

        ctrl.getOuterWidth = function (element) {

            var el = element[0] != null ? element[0] : element;

            return el.offsetWidth + Math.ceil(parseFloat(window.getComputedStyle(el).marginLeft)) + Math.ceil(parseFloat(window.getComputedStyle(el).marginRight));
        };

        //ctrl.checkSubmenuOrientation = function (element) {
        //    var bodyMaxRight = body.clientWidth,
        //        elemMaxRight = element[0].getBoundingClientRect();

        //    scopeTile.submenuInvert = bodyMaxRight < elemMaxRight.right;
        //};
    }

    ng.module('harmonica')
    .controller('HarmonicaCtrl', HarmonicaCtrl);

    HarmonicaCtrl.$inject = ['$element', '$attrs'];
})(angular, document.body);