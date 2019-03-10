; (function (ng) {
    'use strict';

    var RulesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, $http, $q, SweetAlert) {

        var ctrl = this,
            columnDefs = [
                 {
                     name: 'RuleTypeStr',
                     displayName: 'Тип'
                 },
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="rules/edit/{{row.entity.RuleType}}">{{COL_FIELD}}</a></div>',
                    enableCellEdit: false
                },
                {
                    name: 'Enabled',
                    displayName: 'Активна',
                    cellTemplate: '<div class="ui-grid-cell-contents"><label class="adv-checkbox-label"><input disabled class="adv-checkbox-input" type="checkbox" ng-model="row.entity.Enabled"><span class="adv-checkbox-emul"></span></label></div>',
                    enableCellEdit: false
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="rules/edit/{{row.entity.RuleType}}" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '<ui-grid-custom-delete url="rules/deleteRule" params="{\'id\': row.entity.RuleType}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            enableSorting:false,
            uiGridCustom: {
                rowUrl: 'rules/edit/{{row.entity.RuleType}}',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'rules/DeleteRuleMass',
                        field: 'RuleType',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });


        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        ctrl.deleteRule = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('rules/deleteRule', { id: id }).then(function (response) {
                        $window.location.assign('rules');
                    });
                }
            });
        }
        
    };

    RulesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', '$http', '$q', 'SweetAlert'];


    ng.module('rules', ['uiGridCustom', 'urlHelper'])
      .controller('RulesCtrl', RulesCtrl);

})(window.angular);