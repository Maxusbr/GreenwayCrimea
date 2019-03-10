; (function (ng) {
    'use strict';

    var ModalSelectStaticPageCtrl = function ($uibModalInstance, uiGridCustomConfig) {
        var ctrl = this;
        
        ctrl.$onInit = function () {

            var params = ctrl.$resolve;
            ctrl.paramert = { 'showRoot': null, 'selected': '0' };
            ctrl.paramert.showRoot = params.showRoot;
            ctrl.paramert.selected = params.selected;
        };

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'PageName',
                    displayName: 'Заголовок',
                },
                {
                    name: 'ModifyDateFormatted',
                    displayName: 'Дата изменения',
                    width: 150,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 100,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" class="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.choose(row.entity.StaticPageId, row.entity.PageName)">Выбрать</a> ' +
                        '</div></div>'
                }
            ]
        });

        if (ctrl.$resolve.multiSelect === false) {
            ng.extend(ctrl.gridOptions, {
                multiSelect: false,
                modifierKeysToMultiSelect: false,
                enableRowSelection: true,
                enableRowHeaderSelection: false
            });
        }

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
        
        ctrl.choose = function (id, pageName) {
            $uibModalInstance.close({ staticPageId: id, pageName: pageName });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalSelectStaticPageCtrl.$inject = ['$uibModalInstance', 'uiGridCustomConfig'];

    ng.module('uiModal')
        .controller('ModalSelectStaticPageCtrl', ModalSelectStaticPageCtrl);

})(window.angular);