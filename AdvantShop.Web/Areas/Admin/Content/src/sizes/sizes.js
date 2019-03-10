; (function (ng) {
    'use strict';

    var SizesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, toaster, SweetAlert, $http, $q) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'SizeName',
                    displayName: 'Название',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Название',
                        type: uiGridConstants.filter.INPUT,
                        name: 'SizeName'
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

                            '<ui-modal-trigger data-controller="\'ModalAddEditSizeCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/sizes/modal/addEditSize/addEditSize.html" ' +
                                        'data-resolve="{\'sizeId\': row.entity.SizeId}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil"></a> ' +
                            '</ui-modal-trigger>' +
                            
                            ' <a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.CanBeDeleted, row.entity.SizeId)" ng-class="(!row.entity.CanBeDeleted ? \'ui-grid-custom-service-icon fa fa-times link-disabled\' : \'ui-grid-custom-service-icon fa fa-times link-invert\')"></a> ' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'sizes/deleteSizes',
                        field: 'SizeId',
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
        
        ctrl.delete = function (canBeDeleted, sizeId) {

            if (canBeDeleted) {
                SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                    if (result === true) {
                        $http.post('sizes/deleteSize', { 'sizeId': sizeId }).then(function (response) {
                            ctrl.grid.fetchData();
                        });
                    }
                });
            } else {
                SweetAlert.alert("Размеры, которые используются у товаров, удалять нельзя", { title: "Удаление невозможно" });
            }
        }
    };

    SizesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'toaster', 'SweetAlert', '$http', '$q'];


    ng.module('sizes', ['uiGridCustom', 'urlHelper'])
      .controller('SizesCtrl', SizesCtrl);

})(window.angular);