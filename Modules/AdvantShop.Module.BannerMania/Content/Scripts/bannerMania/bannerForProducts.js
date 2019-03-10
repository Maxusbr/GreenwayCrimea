; (function (ng) {
    'use strict';

    var bannerForProductsCtrl = function ($http, toaster, $uibModal, uiGridConstants, uiGridCustomConfig, SweetAlert, Upload, $q) {
        var ctrl = this;

        ctrl.entityId = 0;
        ctrl.entityName = "";
        ctrl.placement = 0;
        ctrl.url = "";
        ctrl.newWindow = false;
        ctrl.enabled = true;
        ctrl.artNo = "";

        ctrl.placementTypes = {};

        ctrl.imageFile = null;

        ctrl.entityTypes = {};
        ctrl.entityType = { Id: 0, Value: "Баннеры для отдельных товаров" };

        ctrl.categories = {};
        ctrl.currentCategory = null;

        ctrl.$onInit = function () {
            ctrl.getPlacementTypes();

            $http.get('../bfpadmin/getentitytypes', { params: { rnd: Math.random() } })
                .then(function (response) {
                    ctrl.entityTypes = response.data;
                    ctrl.entityType = ctrl.entityTypes[0];
                });

            $http.get('../bfpadmin/getcategories', { params: { rnd: Math.random() } })
                .then(function (response) {
                    ctrl.categories = response.data;
                    ctrl.currentCategory = ctrl.categories[0];
                });
        };

        ctrl.getProduct = function () {
            $http.post('../bfpadmin/getproduct', { artNo: ctrl.artNo }).then(function success(response) {
                ctrl.entityId = response.data.EntityId;
                ctrl.entityName = response.data.EntityName;
            });
        };

        ctrl.getPlacementTypes = function () {
            $http.get('../bfpadmin/getplacementtypes', { params: { entityType: ctrl.entityType.Id, rnd: Math.random() } })
                .then(function (response) {
                    ctrl.placementTypes = response.data;
                    ctrl.placement = ctrl.placementTypes[0];
                });
        }

        ctrl.changeEntityType = function () {
            ctrl.reset();
            ctrl.getPlacementTypes();
        };

        ctrl.changeCategory = function () {
            ctrl.entityId = ctrl.currentCategory.Id;
            ctrl.entityName = ctrl.currentCategory.Name.replace(/^\s+|\s+$/g, '');
        };

        ctrl.addBannerEntity = function (form) {
            if (ctrl.entityId == 0) { ctrl.entityId = -1; }

            if (!ctrl.validate(form)) {
                return;
            }

            var req = {
                method: 'POST',
                url: '../bfpadmin/addbannerentity',
                data: { entityId: ctrl.entityId, entityType: ctrl.entityType.Id, entityName: ctrl.entityName, placement: ctrl.placement.Id, url: ctrl.url, newWindow: ctrl.newWindow, enabled: ctrl.enabled }
            }

            SweetAlert.confirm("Вы уверены, что хотите добавить баннер?", { title: "Добавление баннера" }).then(function (result) {
                if (result === true) {
                    $http(req).then(function success(response) {
                        if (response.data === null) {
                            SweetAlert.confirm("Баннер для данного товара и данной позиции уже существует. Заменить баннер?", { title: "Обновление баннера" }).then(function (result) {
                                if (result === true) {
                                    req = {
                                        method: 'POST',
                                        url: '../bfpadmin/addbannerentity',
                                        data: { entityId: ctrl.entityId, entityType: ctrl.entityType.Id, entityName: ctrl.entityName, placement: ctrl.placement.Id, url: ctrl.url, newWindow: ctrl.newWindow, enabled: ctrl.enabled, overwrite: true }
                                    }

                                    $http(req).then(function success(response) {
                                        ctrl.response(response, ctrl.getTextForMessage(ctrl.entityName, true));
                                    })
                                }
                            });
                        } else {
                            ctrl.response(response, ctrl.getTextForMessage(ctrl.entityName, false));
                        }
                    });
                }
            });
        };

        ctrl.getTextForMessage = function (name, update) {
            switch (ctrl.entityType.Id) {
                case 0: return 'Баннер для товара "' + name + '" ' + (update ? 'обновлен' : 'добавлен') + '.';
                case 1: return 'Баннер для категории "' + name + '" ' + (update ? 'обновлен' : 'добавлен') + '.';
                case 2: return 'Баннер для товаров категории "' + name + '" ' + (update ? 'обновлен' : 'добавлен') + '.';
            }
        };

        ctrl.response = function (response, message) {
            if (response.data === true) {
                Upload.upload({
                    url: '../bfpadmin/uploadimage',
                    data: {
                        imageFile: ctrl.imageFile,
                        entityId: ctrl.entityId,
                        entityType: ctrl.entityType.Id,
                        placement: ctrl.placement.Id
                    }
                }).then(function (response) {

                });

                ctrl.reset();

                toaster.pop('success', '', message);
            }
            else {
                toaster.pop('error', 'Ошибка', 'Не удалось добавить баннер');
            }
        };

        ctrl.validate = function (form) {
            if (ctrl.entityId <= 0)
            {
                toaster.pop('error', 'Ошибка', ctrl.entityType.Id == 0 ? 'Выберите товар' : 'Выберите категорию');
                return false;
            }

            if (ctrl.imageFile == null || ctrl.imageFile == '')
            {
                toaster.pop('error', 'Ошибка', 'Выберите изображение');
                return false;
            }

            if (ctrl.placement == null || ctrl.placement.Id < 0)
            {
                toaster.pop('error', 'Ошибка', 'Выберите размещение');
                return false;
            }

            if (!form.$valid) {
                toaster.pop('error', 'Ошибка', 'Заполните необходимые поля');
                return false;
            }

            return true;
        }

        ctrl.reset = function () {
            ctrl.artNo = "";
            ctrl.entityId = 0;
            ctrl.placement = ctrl.placementTypes[0];
            ctrl.currentCategory = ctrl.categories[0];
            ctrl.url = "";
            ctrl.newWindow = false;
            ctrl.enabled = true;
            ctrl.imageFile = null;
            ctrl.grid.update();
        };

        ctrl.selectFile = function ($file) {
            if ($file == null) return false;

            $http.post('../bfpadmin/validateimagefileextenstion', { fileName: $file.name }).then(function success(response) {
                    if (response.data == true) {
                        ctrl.imageFile = $file;
                    } else {
                        toaster.pop('error', 'Ошибка', 'Недопустимый тип файла');
                    }
                })
        };

        var columnDefs = [
            {
                name: 'EntityName',
                displayName: 'Название',
                cellTemplate: '<div class="ui-grid-cell-contents"><a href="{{row.entity.Link}}/edit/{{row.entity.EntityId}}" target="_blank">{{COL_FIELD}}</a></div>',
                filter: {
                    placeholder: 'Название',
                    type: uiGridConstants.filter.INPUT,
                    name: 'EntityName',
                }
            },
            {
                name: 'ImagePath',
                displayName: 'Изображение',
                cellTemplate: '<div class="ui-grid-cell-contents" style="text-align: center;"><a href="../userfiles/modules/bannermania/pictures/{{COL_FIELD}}" target="_blank"><img src="../userfiles/modules/bannermania/pictures/{{COL_FIELD}}" style="width: 150px;"></a></div>',
                width: 150,
                enableSorting: false
            },
            {
                name: 'EntityType',
                displayName: 'Тип баннера',
                filter: {
                    placeholder: 'Тип баннера',
                    name: 'EntityType',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [
                        { label: 'Для отдельных товаров', value: "Products" },
                        { label: 'Для товаров по категориям', value: "ProductsByCategories" },
                        { label: 'Для категорий товаров', value: "Categories" }
                    ]
                }
            },
            {
                name: 'Placement',
                displayName: 'Размещение',
                filter: {
                    placeholder: 'Размещение',
                    name: 'Placement',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [
                        { label: 'Под инфо о доставке', value: "UnderDeliveryInfo" },
                        { label: 'Над инфо о доставке', value: "AboveDeliveryInfo" },
                        { label: 'Над фильтром', value: "AboveFilter" },
                        { label: 'Под фильтром', value: "UnderFilter" },
                        { label: 'Над подвалом', value: "AboveFooter" },
                        { label: 'Под блоком меню', value: "UnderMenu" }
                    ]
                }
            },
            {
                name: 'URL',
                displayName: 'URL',
                enableCellEdit: true,
                filter: {
                    placeholder: 'URL',
                    type: uiGridConstants.filter.INPUT,
                    name: 'URL',
                }
            },
            {
                name: 'NewWindow',
                displayName: 'В новом окне',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" field-name="NewWindow" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 90,
                filter: {
                    placeholder: 'В новом окне',
                    name: 'NewWindow',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Открывать', value: true }, { label: 'Не открывать', value: false }]
                }
            },
            {
                name: 'Enabled',
                displayName: 'Активность',
                enableCellEdit: false,
                cellTemplate: '<ui-grid-custom-switch row="row" field-name="Enabled" class="js-grid-not-clicked"></ui-grid-custom-switch>',
                width: 90,
                filter: {
                    placeholder: 'Активность',
                    name: 'Enabled',
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ label: 'Активен', value: true }, { label: 'Не активен', value: false }]
                }
            },
            {
                name: '_serviceColumn',
                displayName: '',
                width: 80,
                cellTemplate:
                '<div class="ui-grid-cell-contents"><div>' +
                '<ui-grid-custom-delete url="../bfpadmin/deleteBannerEntity" params="{\'bannerId\': row.entity.BannerId}"></ui-grid-custom-delete>' +
                '</div></div>'
            }
        ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: '../bfpadmin/deleteBannerEntities',
                        field: 'BannerId',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    },
                    {
                        text: 'Открывать в новом окне',
                        url: '../bfpadmin/setNewWindow',
                        field: 'BannerId'
                    },
                    {
                        text: 'Открывать в текущем окне',
                        url: '../bfpadmin/setNotNewWindow',
                        field: 'BannerId'
                    },
                    {
                        text: 'Активировать',
                        url: '../bfpadmin/setEnabled',
                        field: 'BannerId'
                    },
                    {
                        text: 'Деактивировать',
                        url: '../bfpadmin/setDisabled',
                        field: 'BannerId'
                    }
                ]
            }
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.grid = grid;
        };
    };

    bannerForProductsCtrl.$inject = ['$http', 'toaster', '$uibModal', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', 'Upload', '$q'];

    ng.module('bannerForProducts', [])
        .controller('bannerForProductsCtrl', bannerForProductsCtrl)
        .component('bannerForProducts', {
            templateUrl: '../modules/BannerMania/content/Scripts/bannerMania/templates/bannerForProducts.html',
            controller: 'bannerForProductsCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);