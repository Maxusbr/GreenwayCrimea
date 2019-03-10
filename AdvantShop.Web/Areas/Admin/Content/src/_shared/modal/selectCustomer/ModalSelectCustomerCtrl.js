; (function (ng) {
    'use strict';

    var ModalSelectCustomerCtrl = function ($uibModalInstance, $http, uiGridConstants, uiGridCustomConfig) {

        var ctrl = this;
        
        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            multiSelect: false,
            modifierKeysToMultiSelect: false,
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            columnDefs: [
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.selectCustomer(row.entity.CustomerId)">Выбрать</a>' +
                        '</div></div>'
                },
                {
                    name: 'Name',
                    displayName: 'Покупатель',
                    filter: {
                        placeholder: 'Имя',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: 'Phone',
                    displayName: 'Телефон',
                    filter: {
                        placeholder: 'Телефон',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Phone',
                    }
                },
                {
                    name: 'Email',
                    displayName: 'Email',
                    filter: {
                        placeholder: 'Email',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Email',
                    }
                }
            ]
        });

        ctrl.$onInit = function () {
            ctrl.btnChangeDisabled = true;
        };

        ctrl.selectCustomer = function (customerId) {
            $uibModalInstance.close({ customerId: customerId });
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalSelectCustomerCtrl.$inject = ['$uibModalInstance', '$http', 'uiGridConstants', 'uiGridCustomConfig'];

    ng.module('uiModal')
        .controller('ModalSelectCustomerCtrl', ModalSelectCustomerCtrl);

})(window.angular);