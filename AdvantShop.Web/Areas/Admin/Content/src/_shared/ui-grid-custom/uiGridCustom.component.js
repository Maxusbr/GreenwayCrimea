; (function (ng) {
    'use strict';

    ng.module('uiGridCustom')
      .directive('uiGridCustom', ['uiGridCustomService', function (uiGridCustomService) {
          return {
              restrict: 'E',
              templateUrl: '../areas/admin/content/src/_shared/ui-grid-custom/templates/ui-grid-custom.html',
              controller: 'UiGridCustomCtrl',
              controllerAs: '$ctrl',
              bindToController: true,
              transclude: {
                  footer: '?uiGridCustomFooter'
              },
              scope: {
                  gridOptions: '<',
                  gridUrl: '<?',
                  gridInplaceUrl: '<?',
                  gridParams: '<?',
                  gridFilterEnabled: '<?',
                  gridSelectionEnabled: '<?',
                  gridPaginationEnabled: '<?',
                  gridTreeViewEnabled: '<?',
                  gridUniqueId: '@',
                  gridOnInplaceBeforeApply: '&',
                  gridOnInplaceApply: '&',
                  gridOnInit: '&',
                  gridSearchPlaceholder: '<?',
                  gridExtendCtrl: '<?',
                  gridEmptyText: '<?',
                  gridSelectionOnInit: '&',
                  gridSelectionOnChange: '&',
                  gridSelectionMassApply: '&',
                  gridOnFetch: '&',
                  gridOnDelete: '&',
                  gridOnPreinit: '&',
                  gridShowExport: '<?',
                  gridOnFilterInit: '&',
                  gridSelectionItemsSelectedFn: '&',
                  gridRowIdentificator: '<?'
              },
              compile: function (cElement, cAttrs) {
                  var uiGridElement = cElement[0].querySelector('[ui-grid]');

                  cAttrs.gridSelectionEnabled == null || cAttrs.gridSelectionEnabled === 'true' ? uiGridElement.setAttribute('ui-grid-selection', '') : uiGridElement.removeAttribute('ui-grid-selection');
                  cAttrs.gridTreeViewEnabled === 'true' ? uiGridElement.setAttribute('ui-grid-tree-view', '') : uiGridElement.removeAttribute('ui-grid-tree-view');

                  return function (scope, element, attrs, ctrl) {
                      scope.$on('modal.closing', function () {
                          ctrl.clearParams();
                          uiGridCustomService.removeFromStorage(ctrl.gridUniqueId);
                      });
                  }
              }
          }
      }])
        .component('uiGridCustomSwitch', {
            require: {
                uiGridCustom: '^uiGridCustom'
            },
            template: '<div class="ui-grid-cell-contents"><div class="js-grid-not-clicked"><switch-on-off checked="$ctrl.row.entity[$ctrl.fieldName || \'Enabled\']" on-change="$ctrl.uiGridCustom.setSwitchEnabled($ctrl.row.entity, checked, $ctrl.fieldName || \'Enabled\')"></switch-on-off></div></div>',
            bindings: {
                row: '<',
                fieldName: '@'
            }
        })
        .component('uiGridCustomDelete', {
            require: {
                uiGridCustom: '^uiGridCustom'
            },
            template: '<a href="" ng-click="$ctrl.delete($ctrl.url, $ctrl.params, $ctrl.confirmText)" class="ui-grid-custom-service-icon fa fa-times link-invert"></a>',
            bindings: {
                url: '@',
                params: '<',
                confirmText: '@'
            },
            controller: ['$http', 'SweetAlert', 'toaster', 'lastStatisticsService', function ($http, SweetAlert, toaster, lastStatisticsService) {

                var ctrl = this;

                ctrl.delete = function (url, params, confirmText) {
                    SweetAlert.confirm(confirmText != null ? confirmText : "Вы уверены, что хотите удалить?", { title: "Удаление" })
                   .then(function (result) {
                       if (result === true) {
                           return $http.post(url, params).then(function (response) {
                               
                               ctrl.uiGridCustom.fetchData();

                               var data = response.data;

                               if (data === true || (data.result != null && data.result === true)) {
                                   toaster.pop('success', '', 'Изменения сохранены');

                                   lastStatisticsService.getLastStatistics();

                                   if (ctrl.uiGridCustom.gridOnDelete != null) {
                                       ctrl.uiGridCustom.gridOnDelete();
                                   }
                               } else if (data.errors != null && data.errors.length > 0) {

                                   data.errors.forEach(function (error) {
                                       toaster.pop('error', '', error);
                                   });
                               }

                               return data;
                           }, function (response) {
                               toaster.pop('success', '', 'Ошибка при удалении записи');
                           });
                       }
                   });
                }
            }]
        });

})(window.angular);