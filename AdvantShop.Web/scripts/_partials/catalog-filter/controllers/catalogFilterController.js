; (function (ng) {

    'use strict';

    var isIE = /Windows Phone|iemobile|WPDesktop/.test(navigator.userAgent);

    var CatalogFilterCtrl = function ($http, $window, $timeout, popoverService, domService, catalogFilterService) {

        var ctrl = this,
            pageParameters = catalogFilterService.parseSearchString($window.location.search),
            timerPopoverHide,
            timerRange;
            
        ctrl.isIE = isIE;

        ctrl.countVisibleCollapse = ctrl.countVisibleCollapse() || 10;

        ctrl.collapsed = true;
        ctrl.isRenderBlock = false;

        ctrl.itemsOptions = [];

        ctrl.getCssClassForContent = function (controlType) {

            var cssClasses = {};

            cssClasses['catalog-filter-block-content-' + controlType] = true;

            return cssClasses;
        };

        ctrl.inputKeypress = function ($event) {
            if (timerPopoverHide != null) {
                $timeout.cancel(timerPopoverHide);
            }

            timerPopoverHide = $timeout(function () {
                var element = $event.currentTarget.parentNode;

                if (element != null) {
                    ctrl.changeItem(element);
                }
            }, 1200);
        };

        ctrl.clickCheckbox = function ($event) {

            var element = domService.closest($event.target, '.catalog-filter-row');

            if (element != null) {
                ctrl.changeItem(element);
            }
        };

        ctrl.clickSelect = function ($event) {

            var element = $event.currentTarget.parentNode.parentNode;

            if (element != null) {
                ctrl.changeItem(element);
            }
        };

        ctrl.clickRange = function (event) {
            if (timerRange != null) {
                $timeout.cancel(timerRange);
            }

            timerRange = $timeout(function () {
                var element = domService.closest(event.target, '.js-range-slider-block');

                if (element != null) {
                    ctrl.changeItem(element);
                }
            }, 500);
        };

        ctrl.changeColor = function (event) {
            var element = domService.closest(event.target, '.js-color-viewer');

            if (element != null) {
                ctrl.changeItem(element);
            }
        }

        ctrl.changeItem = function (element) {

            var selectedItems, params;

            selectedItems = catalogFilterService.getSelectedData(ctrl.catalogFilterData);

            params = catalogFilterService.buildUrl(selectedItems);

            ctrl.getFilterCount(params).then(function (foundCount) {
                ctrl.foundCount = foundCount;

                popoverService.getPopoverScope('popoverCatalogFilter').then(function (popoverScope) {
                    popoverScope.active(element);

                    if (timerPopoverHide != null) {
                        $timeout.cancel(timerPopoverHide);
                    }

                    timerPopoverHide = $timeout(function () {
                        popoverScope.deactive();
                    }, 5000);
                });
            });
        };

        ctrl.toggleVisible = function (totalItems, index) {
            ctrl.itemsOptions[index].countVisibleItems = ctrl.itemsOptions[index].collapsed === true ? totalItems : ctrl.countVisibleCollapse;
            ctrl.itemsOptions[index].collapsed = !ctrl.itemsOptions[index].collapsed;
        };

        ctrl.reset = function () {
            $window.location.search = '';
        };

        ctrl.submit = function () {
            var selectedItems = catalogFilterService.getSelectedData(ctrl.catalogFilterData);
            $window.location.search = catalogFilterService.buildUrl(selectedItems);
        };

        ctrl.getFilterCount = function (filterString) {
            return $http.get(ctrl.urlCount + '?' + filterString, { params: ng.extend(ctrl.parameters(), { rnd: Math.random() }) }).then(function (response) {
                return response.data;
            });
        };

        ctrl.getFilterData = function () {
            return $http.get(ctrl.url, { params: ng.extend(pageParameters, ctrl.parameters(), { rnd: Math.random() }) }).then(function (response) {
                return response.data;
            });
        };

        ctrl.getFilterData().then(function (catalogFilterData) {

            ctrl.catalogFilterData = catalogFilterData;
            ctrl.isRenderBlock = catalogFilterData != null && catalogFilterData.length > 0;

            //Fill all <select> ng-model by properly <option> on filter initialization
            var selectIndex;
            for (var i = 0, length = catalogFilterData.length; i < length; i++) {
                if (catalogFilterData[i] != null && (catalogFilterData[i].Control == "select" || catalogFilterData[i].Control == "selectSearch")) {
                    for (var j = 0, len = catalogFilterData[i].Values.length; j < len; j++) {
                        if (catalogFilterData[i].Values[j].Selected) {
                            selectIndex = j;
                            break;
                        }
                    }
                    catalogFilterData[i].Selected = catalogFilterData[i].Values[selectIndex];
                    selectIndex = -1;
                } else {
                    ctrl.itemsOptions[i] = {
                        countVisibleItems: ctrl.countVisibleCollapse,
                        collapsed: true
                    };
                }
            }

            catalogFilterService.saveFilterData(catalogFilterData);
        });

    };

    ng.module('catalogFilter')
      .controller('CatalogFilterCtrl', CatalogFilterCtrl);

    CatalogFilterCtrl.$inject = ['$http', '$window', '$timeout', 'popoverService', 'domService', 'catalogFilterService'];

})(window.angular);