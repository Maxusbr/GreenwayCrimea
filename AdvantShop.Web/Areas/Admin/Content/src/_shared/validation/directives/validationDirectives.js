; (function (ng) {
    'use strict';
    ng.module('validation')
      .directive('buttonValidation', ['$parse', 'domService', 'toaster', function ($parse, domService, toaster) {
          return {
              restrict: 'A',
              require: ['^form'],
              link: function (scope, element, attrs, ctrls) {

                  var FormCtrl = ctrls[0],
                      customValidFunc = $parse(attrs.buttonValidation),
                      startFunc = $parse(attrs.buttonValidationStart),
                      successFunc = $parse(attrs.buttonValidationSuccess),
                      formNames = $parse(attrs.buttonValidationForms);

                  function validate(event) {

                      startFunc(scope);

                      if (FormCtrl.$invalid === true || customValidFunc(scope) === false) {

                          event.preventDefault();

                          FormCtrl.$setSubmitted();
                          FormCtrl.$setDirty();


                          var form = findForm(formNames(scope));

                          if (form != null) {
                              var invalidElementFocus = form.querySelector('.ng-invalid:not(form)');

                              if (invalidElementFocus != null) {
                                  invalidElementFocus.focus();
                              }
                          }

                          toaster.pop({
                              type: 'error',
                              title: 'Ошибка при вводе данных',
                              body: 'validation-output',
                              bodyOutputType: 'directive',
                              directiveData: { errors: ng.copy(FormCtrl.$error) },
                              toasterId: 'toasterContainerAlternative',
                              timeout: 5000
                          });

                      } else {
                          scope.clickEvent = event;
                          successFunc(scope);
                      }

                      scope.$apply();
                  }

                  function findForm(formNames) {
                      var currentFrom;

                      if (formNames != null) {
                          for (var i = 0, len = formNames.length; i < len; i++) {
                              if (document.forms[formNames[i]].classList.contains('ng-invalid')) {
                                  currentFrom = document.forms[formNames[i]];
                                  break;
                              }
                          }
                      } else {
                          currentFrom = document.getElementById(event.target.getAttribute('form')) || domService.closest(event.target, 'ng-form') || domService.closest(event.target, 'form') || document.querySelector('form')
                      }

                      return currentFrom;
                  }

                  element[0].addEventListener('click', validate);
              }
          };
      }]);

    ng.module('validation')
  .directive('validationTabIndex', ['$parse', function ($parse) {
      return {
          controller: ['$attrs', '$parse', '$scope', function ($attrs, $parse, $scope) {
              this.validationTabIndex = $parse($attrs.validationTabIndex)($scope);
          }],
          controllerAs: 'validationTabIndex',
          bindToController: true
      }
  }])

    ng.module('validation')
      .directive('validationInputText', [function () {
          return {
              restrict: 'A',
              bindToController: true,
              controllerAs: 'validationInputText',
              controller: ['$attrs', '$interpolate', '$scope', function ($attrs, $interpolate, $scope) {
                  var ctrl = this;

                  ctrl.$onInit = function () {

                      ctrl.ngModelCtrl.validationInputText = $interpolate($attrs.validationInputText)($scope);

                      ctrl.validationOpenTab = function () {
                          ctrl.uibTabsetCtrl.select(ctrl.validationTabIndexCtrl.validationTabIndex)
                      };
                  };
              }],
              require: {
                  ngModelCtrl: 'ngModel',
                  validationTabIndexCtrl: '?^validationTabIndex',
                  uibTabsetCtrl: '?^uibTabset'
              },
              //link: function (scope, element, attrs, ctrls) {
              //    ctrls[0].validationInputText = attrs.validationInputText;

              //    ctrls[0].validationOpenTab = function () {
              //        ctrls[2].select(ctrls[1].validationTabIndex)
              //    };

              //    if (ctrls[1] != null) {
              //        ctrls[0].validationTabIndex = ctrls[1].validationTabIndex;
              //        //ctrls[0].uibTabsetCtrl = ctrls[2];
              //    }
              //}
          };
      }]);

    ng.module('validation')
      .directive('validationOutput', [function () {
          return {
              templateUrl: '../areas/admin/content/src/_shared/validation/templates/validationOutput.html'
          }
      }]);

    ng.module('validation')
      .component('validationList',
            {
                templateUrl: '../areas/admin/content/src/_shared/validation/templates/validationList.html',
                bindings: {
                    validationType: '<',
                    validationErrors: '<'
                }
            });

    ng.module('validation')
      .component('validationListItem',
          {
              templateUrl: '../areas/admin/content/src/_shared/validation/templates/validationListItem.html',
              bindings: {
                  error: '<?'
              },
              controller: ['domService', function (domService) {
                  var ctrl = this;

                  ctrl.goToElement = function (text, tabIndex, uiTabset) {

                      var el = document.querySelector('[validation-input-text="' + text + '"]'),
                          validationInputTextCtrl;

                      if (el != null) {

                          if (el.type == null || ['input', 'textarea'].indexOf(el.tagName.toLowerCase()) === -1) {
                              el = el.querySelector('input:not([type="button"]), textarea');
                          }

                          validationInputTextCtrl = angular.element(el).controller('validationInputText');

                          if (validationInputTextCtrl.uibTabsetCtrl != null && validationInputTextCtrl.validationTabIndexCtrl != null) {
                              validationInputTextCtrl.uibTabsetCtrl.active = validationInputTextCtrl.validationTabIndexCtrl.validationTabIndex;
                          }


                          setTimeout(function () { el.ckEditorInstance ? el.ckEditorInstance.focus() : el.focus(); }, 0);

                      }
                  };
              }]
          });

    ng.module('validation')
      .directive('validationInputFloat', function () {
          return {
              require: {
                  ngModelCtrl: 'ngModel'
              },
              bindToController: true,
              controller: function () {

                  var ctrl = this;

                  ctrl.$onInit = function () {
                      ctrl.ngModelCtrl.$validators.validInputFloat = function (modelValue, viewValue) {
                          return viewValue == null || (viewValue.length > 0 && /^-?[\s\d,\.]*$/.test(viewValue));
                      };

                      ctrl.ngModelCtrl.$parsers.push(function (value) {
                          return Number(value.replace(/\s*/g, '').replace(/,/g, '.'));
                      });

                  };
              }
          }
      })


})(window.angular);