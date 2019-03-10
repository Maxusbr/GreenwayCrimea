; (function (ng) {
    'use strict';

    ng.module('searchPanel')
      .directive('searchIcon', function () {
          return {
              restrict: 'A',
              //replace: true,
              template: "<div class='searchBtn inked ink-light' ng-class='{\"cs-bg-13\": spCtrl.active}' ng-mouseleave='spCtrl.hidePanel()'><a href='javascript:void(0);' class='search-link icon-search-before icon-margin-drop cs-t-8' ng-click='spCtrl.togglePanel()'></a></div>",
              controller: 'searchPanelCtrl',
              controllerAs: 'spCtrl',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) { }
          }
      });

    ng.module('searchPanel')
      .directive('searchPanel', function () {
          return {
              restrict: 'A',
              replace: true,
              controller: 'searchPanelCtrl',
              controllerAs: 'spCtrl',
              bindToController: true,
              templateUrl: "/Areas/Mobile/scripts/_common/searchPanel/templates/searchPanel.html",
              link: function (scope, element, attrs, ctrl) { }
          }
      });

})(window.angular);