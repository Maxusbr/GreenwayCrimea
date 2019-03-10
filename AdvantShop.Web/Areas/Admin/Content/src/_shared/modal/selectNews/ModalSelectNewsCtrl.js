; (function (ng) {
    'use strict';

    var ModalSelectNewsCtrl = function ($uibModalInstance, uiGridCustomConfig) {
        var ctrl = this;

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Title',
                    displayName: 'Заголовок',
                },
                {
                    name: 'AddingDateFormatted',
                    displayName: 'Дата добавления',
                    width: 150,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 100,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" class="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.choose(row.entity.NewsId, row.entity.Title)">Выбрать</a> ' +
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
        
        ctrl.choose = function (newsId, title) {
            $uibModalInstance.close({ newsId: newsId, title: title });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalSelectNewsCtrl.$inject = ['$uibModalInstance', 'uiGridCustomConfig'];

    ng.module('uiModal')
        .controller('ModalSelectNewsCtrl', ModalSelectNewsCtrl);

})(window.angular);