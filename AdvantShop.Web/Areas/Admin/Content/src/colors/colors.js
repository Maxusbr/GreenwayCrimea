; (function (ng) {
    'use strict';

    var ColorsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, SweetAlert, $http, $q) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'ColorIcon',
                    displayName: 'Иконка',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<span ng-if="row.entity.ColorIcon != \'\'"><img ng-src="{{row.entity.ColorIcon}}" /></span> ' +
                            '<span ng-if="row.entity.ColorIcon == \'\'"><i class="fa fa-square color-square" style="color:{{row.entity.ColorCode}}" /></span> ' +
                        '</div>',
                },
                {
                    name: 'ColorName',
                    displayName: 'Название',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<ui-modal-trigger data-controller="\'ModalAddEditColorCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/colors/modal/addEditColor/addEditColor.html" ' +
                                        'data-resolve="{\'colorId\': row.entity.ColorId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="">{{COL_FIELD}}</a> ' +
                            '</ui-modal-trigger>' +
                        '</div>',
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'ColorName'
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Поряд.',
                    width: 100,
                    enableCellEdit: true,
                },
                {
                    name: 'ProductsCount',
                    displayName: 'Используется у товаров',
                    width: 90,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +

                            '<ui-modal-trigger data-controller="\'ModalAddEditColorCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/colors/modal/addEditColor/addEditColor.html" ' +
                                        'data-resolve="{\'colorId\': row.entity.ColorId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +
                            
                            ' <a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.CanBeDeleted, row.entity.ColorId)" ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'colors/deleteColors',
                        field: 'ColorId',
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

        ctrl.delete = function (canBeDeleted, colorId) {

            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function(result) {
                    if (result === true) {
                        $http.post('colors/deleteColor', { 'colorId': colorId }).then(function (response) {
                            ctrl.grid.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("Цвета, которые используются у товаров, удалять нельзя", { title: "Удаление невозможно" });
            }
        }
        
    };

    ColorsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', 'SweetAlert', '$http', '$q'];


    ng.module('colors', ['uiGridCustom', 'urlHelper'])
      .controller('ColorsCtrl', ColorsCtrl);

})(window.angular);