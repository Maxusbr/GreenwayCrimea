; (function (ng) {
    'use strict';

    ng.module('uiGridCustomEdit')
        .directive('uiGridCustomEdit', function () {
            return {
                replace: true,
                priority: 0,
                require: '^uiGrid',
                scope: false,
                compile: function () {
                    return {
                        pre: function (scope, element, attrs, uiGridCtrl) {
                            uiGridCtrl.grid.api.registerEventsFromObject({
                                uiGridEditCustom: {
                                    change: null
                                }
                            });
                        },
                        post: function ($scope, $elm, $attrs, uiGridCtrl) {
                        }
                    };
                }
            };
        });

    ng.module('uiGridCustomEdit')
      .directive('uiGridCell', ['$compile', '$parse', '$q', '$templateRequest', 'uiGridConstants', function ($compile, $parse, $q, $templateRequest, uiGridConstants) {
          return {
              priority: -100, // run after default uiGridCell directive
              restrict: 'A',
              require: ['^uiGrid'],
              scope: false,
              compile: function () {
                  return {
                      pre: function (scope, element, attrs) {

                      },
                      post: function (scope, element, attrs) {
                          var uiGridCustomEditOptions = scope.col.colDef.uiGridCustomEdit || {},
                              uiGridCustomEditAttributes = uiGridCustomEditOptions != null ? uiGridCustomEditOptions.attributes : null,
                              cellName;

                          var getModelValue =  function() {
                              var result;
                              if (uiGridCustomEditOptions.customModel != null) {
                                  result = scope.uiGridEditCustom[uiGridCustomEditOptions.customModel];
                              } else {
                                  result = scope.row.entity[scope.col.colDef.name];
                              }

                              return result;
                          };

                          // resolves a string path against the given object
                          // shamelessly borrowed from
                          // http://stackoverflow.com/questions/6491463/accessing-nested-javascript-objects-with-string-key
                          var resolveObjectFromPath = function (object, path) {
                              path = path.replace(/\[(\w+)\]/g, '.$1'); // convert indexes to properties
                              path = path.replace(/^\./, '');           // strip a leading dot
                              var a = path.split('.');
                              while (a.length) {
                                  var n = a.shift();
                                  if (n in object) {
                                      object = object[n];
                                  } else {
                                      return;
                                  }
                              }
                              return object;
                          }


                          if (uiGridCustomEditOptions.customModel != null) {
                              cellName = 'uiGridEditCustom.' + uiGridCustomEditOptions.customModel;
                          }else if(uiGridCustomEditOptions.modelFromCol != null){
                              cellName = "row.entity['" + uiGridCustomEditOptions.modelFromCol + "']"; //кавычки местами поменял, чтобы в верстке не дублировались двойные кавычки
                          }else{
                              cellName =  scope.row.getQualifiedColField(scope.col);
                          }

                          scope.uiGridEditCustom = {};

                          scope.uiGridEditCustom.shouldEdit = function (col, row) {
                              return !scope.row.isSaving && scope.col.colDef.enableCellEdit && (ng.isFunction(scope.col.colDef.cellEditableCondition) ? scope.col.colDef.cellEditableCondition(scope) : true);
                          };

                          if (scope.uiGridEditCustom.shouldEdit(scope.col, scope.row)) {

                              scope.isFocus = false;

                              if (uiGridCustomEditOptions.customModel != null) {
                                  scope.uiGridEditCustom[uiGridCustomEditOptions.customModel] = null;
                              }

                              scope.uiGridEditCustom.focus = function ($event, value) {

                                  scope.isFocus = true;

                                  scope.uiGridEditCustom.editOldValue = value;

                                  if (uiGridCustomEditOptions.onActive != null && scope.uiGridEditCustom.form.$valid === true) {
                                      uiGridCustomEditOptions.onActive(scope.row.entity, scope.col.colDef, value, scope.uiGridEditCustom);
                                  }
                              };

                              scope.uiGridEditCustom.change = function (rowEntity, colDef, newVal) {

                                  scope.isFocus = false;

                                  if (newVal !== scope.uiGridEditCustom.editOldValue) {

                                      if ((newVal == null || newVal.length === 0) && (uiGridCustomEditOptions.replaceNullable === true || uiGridCustomEditOptions.replaceNullable == null)) {
                                          //rowEntity[colDef.name] = scope.uiGridEditCustom.editOldValue;
                                          uiGridCustomEditOptions.customModel != null ? scope.uiGridEditCustom[uiGridCustomEditOptions.customModel] = scope.uiGridEditCustom.editOldValue : rowEntity[colDef.name] = scope.uiGridEditCustom.editOldValue;

                                          if (uiGridCustomEditOptions.onDeactive != null) {
                                              uiGridCustomEditOptions.onDeactive(rowEntity, colDef, scope.uiGridEditCustom.editOldValue, scope.uiGridEditCustom);
                                          }

                                      } else {
                                          //rowEntity[colDef.name] = newVal;

                                          uiGridCustomEditOptions.customModel != null ? scope.uiGridEditCustom[uiGridCustomEditOptions.customModel] = newVal : rowEntity[colDef.name] = newVal;

                                          if (uiGridCustomEditOptions.onChange != null) {
                                              uiGridCustomEditOptions.onChange(rowEntity, colDef, newVal, scope.uiGridEditCustom);
                                          }

                                          scope.grid.api.uiGridEditCustom.raise.change(rowEntity, colDef, newVal, scope.uiGridEditCustom.editOldValue, function (rowEntity, colDef, newVal, oldValue) {

                                              if (uiGridCustomEditOptions.onDeactive != null) {
                                                  uiGridCustomEditOptions.onDeactive(rowEntity, colDef, newVal, scope.uiGridEditCustom);
                                              }
                                          });
                                      }
                                  } else {
                                      if (uiGridCustomEditOptions.onDeactive != null) {
                                          uiGridCustomEditOptions.onDeactive(rowEntity, colDef, newVal, scope.uiGridEditCustom);
                                      }
                                  }

                                  //scope.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
                              };

                              scope.uiGridEditCustom.keyup = function (rowEntity, colDef, newVal, event) {

                                  switch (event.keyCode) {
                                      case 13:
                                          document.activeElement.blur();
                                          break;
                                      case 27:
                                          newVal = scope.uiGridEditCustom.editOldValue;
                                          break;
                                  }
                              };

                              scope.uiGridEditCustom.getSelectOptions = function () {
                                  if (scope.col.colDef.uiGridCustomEdit.editDropdownOptionsFunction) {
                                      $q.when(scope.col.colDef.uiGridCustomEdit.editDropdownOptionsFunction(scope.row.entity, scope.col.colDef)).then(function (result) {
                                          scope.uiGridEditCustom.editDropdownOptionsArray = result;
                                      });
                                  } else if (scope.col.colDef.uiGridCustomEdit.editDropdownRowEntityOptionsArrayPath) {
                                      scope.uiGridEditCustom.editDropdownOptionsArray = resolveObjectFromPath(scope.row.entity, scope.col.colDef.uiGridCustomEdit.editDropdownRowEntityOptionsArrayPath)
                                  } else {
                                      scope.uiGridEditCustom.editDropdownOptionsArray = scope.col.colDef.uiGridCustomEdit.editDropdownOptionsArray;
                                  }
                              };

                              scope.uiGridEditCustom.selectToggle = function ($event, isOpen, value) {
                                  if (isOpen === true) {

                                      if (uiGridCustomEditOptions.editDropdownLazyLoad != null) {
                                          scope.uiGridEditCustom.getSelectOptions();
                                      }

                                      scope.uiGridEditCustom.focus($event, value);
                                  }
                              }

                              if (uiGridCustomEditOptions.onActive != null) {
                                  scope.$on('uiGridCustomRowMouseenter', function ($event) {
                                      if (scope.isFocus === false && scope.uiGridEditCustom.form != null && scope.uiGridEditCustom.form.$valid === true) {
                                          uiGridCustomEditOptions.onActive(scope.row.entity, scope.col.colDef, getModelValue(), scope.uiGridEditCustom);
                                      }
                                  });
                              }

                              if (uiGridCustomEditOptions.onDeactive != null) {
                                  scope.$on('uiGridCustomRowMouseleave', function ($event) {
                                      if (scope.isFocus === false && scope.uiGridEditCustom.form != null && scope.uiGridEditCustom.form.$valid === true) {
                                          uiGridCustomEditOptions.onDeactive(scope.row.entity, scope.col.colDef, getModelValue(), scope.uiGridEditCustom);
                                      }
                                  });
                              }

                              $templateRequest('../areas/admin/content/src/_shared/ui-grid-custom/templates/ui-grid-custom-edit.html').then(function (template) {
                                  var tpl = template.replace(/MODEL_CUSTOM_EDIT/g, cellName).replace(/INPUT_ATTRIBUTES/g, attributesToString(uiGridCustomEditAttributes));
                                  var cellElement = ng.element(tpl);
                                  var dropdownValueLazyLoad, dropdownLabelLazyLoad;

                                  if (scope.col.colDef.type === 'select' || scope.col.colDef.type === 'ui-select') {

                                      scope.uiGridEditCustom.editDropdownIdLabel = scope.col.colDef.uiGridCustomEdit.editDropdownIdLabel || 'value';
                                      scope.uiGridEditCustom.editDropdownValueLabel = scope.col.colDef.uiGridCustomEdit.editDropdownValueLabel || 'label';

                                      if (uiGridCustomEditOptions.editDropdownLazyLoad == null) {
                                          scope.uiGridEditCustom.getSelectOptions();
                                      } else {


                                          if (ng.isString(uiGridCustomEditOptions.editDropdownLazyLoad.value)) {
                                              dropdownValueLazyLoad = uiGridCustomEditOptions.editDropdownLazyLoad.value
                                          } else if (ng.isFunction(uiGridCustomEditOptions.editDropdownLazyLoad.value)) {
                                              dropdownValueLazyLoad = uiGridCustomEditOptions.editDropdownLazyLoad.value(scope.col, scope.row);
                                          }

                                          if (ng.isString(uiGridCustomEditOptions.editDropdownLazyLoad.label)) {
                                              dropdownLabelLazyLoad = uiGridCustomEditOptions.editDropdownLazyLoad.label
                                          } else if (ng.isFunction(uiGridCustomEditOptions.editDropdownLazyLoad.label)) {
                                              dropdownLabelLazyLoad = uiGridCustomEditOptions.editDropdownLazyLoad.label(scope.col, scope.row);
                                          }

                                          $q.all([$q.when(dropdownValueLazyLoad), $q.when(dropdownLabelLazyLoad)]).then(function (results) {
                                              scope.uiGridEditCustom.editDropdownOptionsArray = [{
                                                  value: results[0],
                                                  label: results[1]
                                              }];
                                          })
                                      }
                                  }

                                  element.html(cellElement);
                                  $compile(cellElement)(scope);

                                  if (uiGridCustomEditOptions.onInit != null) {

                                      scope.$watch('row', function (newValue, oldValue) {
                                          uiGridCustomEditOptions.onInit(scope.row.entity, scope.col.colDef, getModelValue(), scope.uiGridEditCustom);
                                      });

                                      uiGridCustomEditOptions.onInit(scope.row.entity, scope.col.colDef, getModelValue(), scope.uiGridEditCustom);
                                  }
                              });
                          }
                      }
                  };
              }
          };
      }]);

    function attributesToString(attributes) {
        var result = '';

        if (attributes != null) {
            for (var key in attributes) {
                if (attributes.hasOwnProperty(key) === true) {
                    result += [' ', key, '=', attributes[key]].join('');
                }
            }
        }

        return result;
    }

})(window.angular);
