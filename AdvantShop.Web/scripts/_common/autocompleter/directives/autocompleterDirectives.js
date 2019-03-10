; (function (ng) {
    'use strict';

    ng.module('autocompleter')
      .directive('autocompleter', ['$compile', 'autocompleterConfig', function ($compile, autocompleterConfig) {
          return {
              restrict: 'A',
              scope: {
                  requestUrl: '@',
                  minLength: '&',
                  templatePath: '@',
                  field: '@',
                  linkAll: '@',
                  applyFn: '&',
                  params: '=?',
                  showEmptyResultMessage: '&'
              },
              controller: 'AutocompleterCtrl',
              controllerAs: 'autocompleter',
              bindToController: true,
              link: function (scope, element, attrs, ctrl) {
                  var minLength = ctrl.minLength();

                  ctrl.minLength = minLength != null ? minLength : autocompleterConfig.minLength;

                  ctrl.autocompleterElement = element;
              }
          }
      }]);

    ng.module('autocompleter')
      .directive('autocompleterInput', ['$compile', '$timeout', function ($compile, $timeout) {
          return {
              require: ['autocompleterInput', 'ngModel', '^autocompleter'],
              restrict: 'A',
              controller: 'AutocompleterInputCtrl',
              controllerAs: 'autocompleterInput',
              bindToController: true,
              scope: true,
              link: function (scope, element, attrs, ctrls) {

                  var ctrl = ctrls[0],
                      ngModel = ctrls[1],
                      parentCtrl = ctrls[2],
                      pos;

                  ctrl.listRendered = false;

                  element[0].addEventListener('keyup', function (event) {
                      scope.$apply(function () {

                          if (ctrl.listRendered === false) {

                              ctrl.listRendered = true;

                              var list = ng.element('<div data-autocompleter-list></div>');

                              parentCtrl.autocompleterElement.append(list);

                              $compile(list)(scope);
                          }

                          parentCtrl.autocompleteKeyup(event, element[0].value);

                          if (parentCtrl.listPositional == null) {
                              parentCtrl.setListPosition({ left: element[0].offsetLeft, top: element[0].offsetTop + element[0].offsetHeight });
                          }
                      });
                  });

                  element[0].addEventListener('blur', function () {
                      if (parentCtrl.listCtrl != null && parentCtrl.listCtrl.getStateHover() === false && parentCtrl.isDirty === true) {
                          parentCtrl.applyFn({ value: ngModel.$modelValue, obj: parentCtrl.activeItem != null ? parentCtrl.activeItem.item : null });
                      }
                  });

                  parentCtrl.model = ngModel;
              }
          }
      }]);

    ng.module('autocompleter')
      .directive('autocompleterList', ['domService', function (domService) {
          return {
              require: ['autocompleterList', '^autocompleter'],
              restrict: 'A',
              controller: 'AutocompleterListCtrl',
              controllerAs: 'autocompleterList',
              bindToController: true,
              scope: true,
              replace: true,
              templateUrl: '/scripts/_common/autocompleter/templates/_list.html',
              link: function (scope, element, attrs, ctrls) {

                  var ctrl = ctrls[0],
                      parentCtrl = ctrls[1];

                  ctrl.parentScope = parentCtrl;

                  parentCtrl.addList(element[0], ctrl);

              }
          }
      }]);

    ng.module('autocompleter')
      .directive('autocompleterItem', [function () {
          return {
              require: ['autocompleterItem', '^autocompleter'],
              controller: 'AutocompleterItemCtrl',
              controllerAs: 'autocompleterItem',
              bindToController: true,
              restrict: 'A',
              scope: {
                  item: '=',
                  itemTemplatePath: '=?',
                  index: '=?',
                  groupIndex: '=?'
              },
              templateUrl: '/scripts/_common/autocompleter/templates/_item.html',
              replace: true,
              link: function (scope, element, attrs, ctrls) {

                  var ctrl = ctrls[0],
                      parentCtrl = ctrls[1];

                  ctrl.parentScope = parentCtrl;

                  ctrl.itemDOM = element[0];

                  parentCtrl.addItem(ctrl);

                  scope.$on('$destroy', function (destroyEvent) {

                      var currentScope = destroyEvent.currentScope.autocompleterItem;

                      if (currentScope != null) {
                          currentScope.parentScope.items[currentScope.groupIndex].splice(currentScope.index, 1);
                      }
                  });
              }
          }
      }]);

})(window.angular);

