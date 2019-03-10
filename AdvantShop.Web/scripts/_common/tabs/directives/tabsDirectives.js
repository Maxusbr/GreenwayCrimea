; (function (ng) {
    'use strict';

    ng.module('tabs')
      .directive('tabs', ['tabsService', function (tabsService) {
          return {
              restrict: 'A',
              scope: true,
              //scope: {
              //    type: '@',
              //    classesTabActive: '@',
              //    classesTab: '@',
              //    classesLinkActive: '@',
              //    classesLink: '@'
              //},
              //replace: true,
              //transclude: true,
              //templateUrl: '/scripts/_common/tabs/templates/tabs.html',
              controller: 'TabsCtrl',
              controllerAs: 'tabs',
              bindToController: true,
              compile: function (cElement, cAttrs) {

                  if (cAttrs.type == null) {
                      cAttrs.$set('type', 'horizontal');
                  }

                  if (cAttrs.classesTabActive == null) {
                      cAttrs.$set('classesTabActive', 'tabs-header-active cs-br-1');
                  }

                  if (cAttrs.classesLinkActive == null) {
                      cAttrs.$set('classesLinkActive', 'cs-l-2 link-dotted-invert link-dotted-none');
                  }

                  if (cAttrs.classesLink == null) {
                      cAttrs.$set('classesLink', 'link-dotted-invert');
                  }

                  return function (scope, element, attrs, ctrl) {

                      ctrl.type = attrs.type;
                      ctrl.classesTabActive = attrs.classesTabActive;
                      ctrl.classesTab = attrs.classesTab;
                      ctrl.classesLinkActive = attrs.classesLinkActive;
                      ctrl.classesLink = attrs.classesLink;
                      ctrl.allowHideAll = attrs.allowHideAll != null && attrs.allowHideAll === 'true' ? true : false;
                      ctrl.isToggle = attrs.isToggle != null && attrs.isToggle === 'true' ? true : false;

                      tabsService.addInStorage(ctrl, attrs.id);
                  }
              }
          }
      }]);

    ng.module('tabs')
      .directive('tabHeader', function () {
          return {
              require: ['tabHeader', '^tabs'],
              restrict: 'A',
              scope: true,
              controller: 'TabHeaderCtrl',
              controllerAs: 'tabHeader',
              bindToController: true,
              link: function (scope, element, attrs, ctrls) {
                  ctrls[0].id = attrs.id;
                  ctrls[1].addHeader(ctrls[0]);
              }
          };
      });

    ng.module('tabs')
      .directive('tabContent', function () {
          return {
              require: ['tabContent', '^tabs'],
              restrict: 'A',
              scope: true,
              controller: 'TabContentCtrl',
              controllerAs: 'tabContent',
              bindToController: true,
              link: function (scope, element, attrs, ctrls) {
                  ctrls[0].isRender = element.html().replace(/<br\s*[\/]?>/gi, '').trim().length > 0;
                  ctrls[0].headerId = attrs.tabContent;
                  ctrls[1].addContent(ctrls[0]);
              }
          };
      });


    ng.module('tabs')
      .directive('tabsGoto', ['$window', 'tabsService', function ($window, tabsService) {
          return {
              restrict: 'A',
              link: function (scope, element, attrs, ctrl) {

                  element.on('click', function (event) {

                      event.stopPropagation();
                      event.preventDefault();

                      var tab = document.getElementById(attrs.tabsGoto);

                      if (tab != null) {
                          tabsService.change(attrs.tabsGoto);
                          tab.scrollIntoView();
                          scope.$apply();
                      }
                  });
              }
          }
      }]);

})(window.angular);