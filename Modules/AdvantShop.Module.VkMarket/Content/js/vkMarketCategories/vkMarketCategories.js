; (function (ng) {
    'use strict';

    var vkMarketCategoriesCtrl = function ($http, toaster, vkMarketService, uiGridConstants, uiGridCustomConfig, SweetAlert, $q, $uibModal) {
        var ctrl = this,
            columnDefs = [
                {
                    name: 'Name',
                    displayName: 'Название',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.openModal(row.entity.Id)">{{COL_FIELD}}</a> ' +
                        '</div>',
                },
                {
                    name: 'Categories',
                    displayName: 'Категории магазина',
                    width: 300,
                },
                {
                    name: 'VkCategoryName',
                    displayName: 'Категория ВКонтакте',
                    width: 200,
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    enableCellEdit: true,
                    width: 100,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.openModal(row.entity.Id)"></a> ' +
                            //'<ui-grid-custom-delete url="../vkMarketSettings/deleteCategory" params="{\'Id\': row.entity.Id, \'vkId\': row.entity.VkId}"></ui-grid-custom-delete>' +
                            '<a href="" ng-click="grid.appScope.$ctrl.gridExtendCtrl.delete(row.entity.Id, row.entity.VkId)" class="ui-grid-custom-service-icon fa fa-times link-invert"></a> ' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.openModal(row.entity.Id);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: '../vkMarketSettings/deleteCategories',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить? Категории и товары в них будут удалены из Вконтакте тоже.", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };

        
        ctrl.openModal = function (id) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditVkCategoryCtrl',
                controllerAs: 'ctrl',
                templateUrl: '../modules/vkMarket/content/js/vkMarketCategories/modals/ModalAddEditVkCategory.html',
                resolve: {
                    id: function () {
                        return id;
                    }
                }
            }).result.then(function (result) {
                ctrl.grid.fetchData();
                return result;
            }, function (result) {
                ctrl.grid.fetchData();
                return result;
            });
        };

        ctrl.delete = function (id, vkId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить? При удалении категории удалится подборка в ВКонтакте и товары в ней.", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    toaster.pop('success', '', 'Удаление категории и товаров началось');
                    $http.post('../vkMarketSettings/deleteCategory', { 'Id': id, 'VkId': vkId }).then(function (response) {
                        ctrl.grid.fetchData();
                    });
                }
            });
        }
        
    };

    vkMarketCategoriesCtrl.$inject = ['$http', 'toaster', 'vkMarketService', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', '$q', '$uibModal'];

    ng.module('vkMarketCategories', ['uiGridCustom'])
        .controller('vkMarketCategoriesCtrl', vkMarketCategoriesCtrl)
        .component('vkMarketCategories', {
            templateUrl: '../modules/vkmarket/content/js/vkMarketCategories/vkMarketCategories.html',
            controller: 'vkMarketCategoriesCtrl'
        });

})(window.angular);