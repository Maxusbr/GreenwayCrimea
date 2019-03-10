; (function (ng) {
    'use strict';

    angular.module('ui.bootstrap.tabs').config(['$provide', function ($provide) {

        $provide.decorator('uibTabsetDirective', ['$delegate', '$location', '$compile', function ($delegate, $location, $compile) {

            var directive = $delegate[0];

            directive.bindToController.uid = '@';

            directive.bindToController.onSelectBatch = '&';

            directive.templateUrl = function (element, attrs) {
                return attrs.templateUrl || '../areas/admin/content/vendors/ui-bootstrap/angular-tab-decorator/tabset.html';
            };

            var originalLink = directive.link;

            var originalCompile = directive.compile;

            var link = function myLinkFnOverride(scope, element, attrs, tabsCtrl) {


                if (tabsCtrl.uid == null) {
                    throw new Error('Not defined uid the tab');
                }

                var localStorageData = JSON.parse(localStorage.getItem('admin-URLtab')),
                    urlData = $location.search(),
                    activeTab,
                    collapseTabEl = document.createElement('div'),
                    tabs = tabsCtrl.tabs;


                collapseTabEl.setAttribute('collapse-tab', '');
                element[0].appendChild(collapseTabEl);
                $compile(collapseTabEl)(scope);


                if (urlData[tabsCtrl.uid]) { // если есть url
                    activeTab = urlData[tabsCtrl.uid];

                } else { // взять из localstorage

                    if (localStorageData != null ) {  
                    
                        if (((new Date(localStorageData.date).getTime() + 300000) - new Date().getTime()) < 0) { // 300000 - 5 минут

                            localStorage.removeItem('admin-URLtab');
                        }
                        if (localStorageData[tabsCtrl.uid]) {
                            activeTab = localStorageData[tabsCtrl.uid];
                        }

                    }    

                }

                if (activeTab != null) {

                    tabsCtrl.active = parseFloat(activeTab);

                    if (isNaN(tabsCtrl.active)) {
                        tabsCtrl.active = activeTab;
                    }

                }

                var originalSelect = tabsCtrl.select;
                var isInit = false;

                tabsCtrl.select = function (index, evt) {

                    if (isInit && tabsCtrl.uid.length != 0 && index != null) {
                        $location.search(tabsCtrl.uid, tabs[index].index);
                        $location.search()
                        var dataTabs = $location.search();
                        dataTabs.date = new Date();
                        if (!dataTabs[tabsCtrl.uid]) {
                            dataTabs[tabsCtrl.uid] = urlData[tabsCtrl.uid];
                        }
                        localStorage.setItem('admin-URLtab', JSON.stringify(dataTabs));
                    }

                    isInit = true;

                    if (tabsCtrl.onSelectBatch !== null) {
                        tabsCtrl.onSelectBatch({
                            index: index,
                            event: evt,
                            tab: tabsCtrl.tabs[index].tab
                        });
                    }

                    return originalSelect.apply(this, arguments);
                }



                return originalLink.apply($delegate, arguments);
            }

            directive.compile = function (cElement, cAttrs) {
                return function (scope, element, attrs, tabsCtrl) {
                    link.apply(this, arguments);
                };
            };

            return $delegate;
        }]);

        $provide.decorator('uibTabDirective', ['$delegate', function ($delegate) {
            var directive = $delegate[0];

            directive.scope.removable = '<?';

            return $delegate;
        }]);

    }]);
})(window.angular);