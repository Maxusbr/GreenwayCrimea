; (function (ng) {
    'use strict';

    var ModalChangeGroupsCtrl = function ($uibModalInstance, $http, $window, urlHelper, uiGridCustomConfig) {
        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    enableCellEdit: true
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    width: 80,
                    enableCellEdit: true,
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: []
            }
        });
        
        ctrl.$onInit = function () {
            
        };

        ctrl.close = function () {
            $uibModalInstance.close('');
        };

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
        
    };

    ModalChangeGroupsCtrl.$inject = ['$uibModalInstance', '$http', '$window', 'urlHelper', 'uiGridCustomConfig'];

    ng.module('uiModal')
        .controller('ModalChangeGroupsCtrl', ModalChangeGroupsCtrl);

})(window.angular);