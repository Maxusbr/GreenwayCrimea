; (function (ng) {
    'use strict';

    var TabsCtrl = function ($location, $q, $scope, tabsService, urlHelper) {
        var ctrl = this,
            panes = ctrl.panes = {},
            queueHeader = {},
            queueContent = {},
            tabSelected,
            locationUnwatch;


        ctrl.$onInit = function () {
            ctrl.watchLocation();
        };

        ctrl.select = function (tabHeader) {

            var keys = Object.keys(panes);

            if (ctrl.isToggle === false) {
                for (var i = 0, len = keys.length; i < len; i++) {
                    panes[keys[i]].selected = false;
                }

                tabHeader.selected = true;
                tabSelected = tabHeader;
            } else {
                tabHeader.selected = !tabHeader.selected;
                tabSelected = tabHeader.selected === false ? null : tabHeader;
            }
        }

        ctrl.addHeader = function (tabHeader) {
            var defer = $q.defer(),
                search = $location.search();


            panes[tabHeader.id] = tabHeader;

            if (queueHeader[tabHeader.id] != null) {

                queueHeader[tabHeader.id].resolve(tabHeader);

                queueHeader[tabHeader.id].promise.then(function () {

                    tabHeader.isRender = tabContent.isRender;

                    if (tabHeader.isRender === true && (tabHeader.id == search.tab || tabHeader.id == urlHelper.getUrlParam("tab") || tabSelected == null)) {
                        ctrl.select(tabHeader);
                    } else {
                        tabHeader.selected = false;
                    }
                });

            } else {

                if (tabHeader.content == null) {
                    queueContent[tabHeader.id] = defer;
                } else {
                    defer.resolve(header);
                }

                defer.promise.then(function (tabContent) {
                    tabHeader.content = tabContent;
                    tabHeader.isRender = tabContent.isRender;

                    if (tabHeader.isRender === true && (tabHeader.id == search.tab || tabHeader.id == urlHelper.getUrlParam("tab") || tabSelected == null)) {
                        ctrl.select(tabHeader);
                    } else {
                        tabHeader.selected = false;
                    }

                    return tabContent;
                });
            }
        };

        ctrl.addContent = function (tabContent) {
            var header = panes[tabContent.headerId],
                defer = $q.defer();

            //если заголовок ещё не проинициализовался
            if (header == null) {
                queueHeader[tabContent.headerId] = defer;
            } else {
                defer.resolve(header);
            }

            defer.promise.then(function (header) {
                tabContent.header = header;
                return header;
            });

            //проверяем если обещания на получения контента
            if (queueContent[tabContent.headerId] != null) {
                queueContent[tabContent.headerId].resolve(tabContent);
            }
        };

        ctrl.change = function (tabHeader) {

            if (ctrl.allowHideAll === true && tabHeader.selected === true) {
                tabHeader.selected = false;
            } else {
                ctrl.select(tabHeader);

                if (ng.isDefined(tabHeader.id)) {
                    $location.search({ tab: tabHeader.id });
                }
            }
        };

        ctrl.watchLocation = function () {
            $scope.$on('$locationChangeSuccess', function () {
                var params = $location.search();

                if (params != null && params.tab != null && panes[params.tab] != null) {
                    var tabObj = tabsService.findTabByid(params.tab);

                    if (tabObj.pane.selected === false) {
                        ctrl.change(tabObj.pane);
                    }
                }
            });
        }
    };

    ng.module('tabs')
      .controller('TabsCtrl', TabsCtrl);

    TabsCtrl.$inject = ['$location', '$q', '$scope', 'tabsService', 'urlHelper'];

})(window.angular);