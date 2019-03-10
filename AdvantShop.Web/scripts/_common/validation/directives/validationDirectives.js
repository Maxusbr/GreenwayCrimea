; (function (ng) {
    'use strict';
    ng.module('validation')
      .directive('buttonValidation', ['$parse', 'domService', function ($parse, domService) {
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
                          FormCtrl.$setSubmitted();
                          FormCtrl.$setDirty();
                          event.preventDefault();

                          var form = findForm(event, formNames(scope));

                          if (form != null) {
                              var invalidElementFocus = form.querySelector('.ng-invalid:not(form)');

                              if (invalidElementFocus != null) {
                                  invalidElementFocus.focus();
                              }
                          }

                      } else {
                          scope.clickEvent = event;
                          successFunc(scope);
                      }

                      scope.$apply();
                  }

                  function findForm(event, formNames) {
                      var currentFrom;

                      if (formNames != null) {
                          for (var i = 0, len = formNames.length; i < len; i++) {
                              if (document.forms[formNames[i]] != null && document.forms[formNames[i]].classList.contains('ng-invalid')) {
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

})(window.angular);