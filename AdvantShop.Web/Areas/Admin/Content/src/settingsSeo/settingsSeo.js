; (function (ng) {
    'use strict';

    var SettingsSeoCtrl = function (Upload, $http, toaster, $q, $location, $window, uiGridConstants, uiGridCustomConfig, uiGridCustomParamsConfig, uiGridCustomService, SweetAlert) {

        var ctrl = this;

        ctrl.resetMeta = function (metaType) {
            return SweetAlert.confirm("Вы уверены что хотите сбросить мета информацию?", { title: "Сброс мета информации" }).then(function (result) {
                return result === true
                    ?
                        $http.post('/settingsseo/ResetMetaInfoByType', { 'metaType': metaType }).then(function (response) {
                            if (response.data.result === true) {
                                toaster.pop('success', 'Мета информация успешно сброшенна', "");
                            } else {
                                toaster.pop({
                                    type: 'error',
                                    title: 'Ошибка при сбросе мета информации',
                                    timeout: 0
                                });
                            }
                        })
                        //$q.resolve('sweetAlertConfirm')

                    : $q.reject('sweetAlertCancel');
            });

        };

        ctrl.googleAnalitycsOauth = function (link) {
            var width = 600;
            var hight = 600;
            var left = (screen.width / 2) - (width / 2);
            var top = (screen.height / 2) - (hight / 2);

            var params = 'width = ' + width + ', height = ' + hight + ', left = ' + left + ', top = ' + top;

            var basePath = document.getElementsByTagName('base')[0].getAttribute('href');
            link = basePath.replace("adminv2/", "").replace("admin/", "") + link;
            var oauthWindow = $window.open(link, 'GoogleAnalitics login', params);
            oauthWindow.focus();
        }

        //#region start 301Redirect

        ctrl.upload301 = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Upload.upload({
                    url: '/Redirect301/Import',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.Result === true) {
                        toaster.pop('succes', 'Файл успешно загружен');
                        ctrl.grid.fetchData()
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке файла', data.Error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке файла', 'Файл не соответствует требованиям');
            }
        };

        ctrl.startExport301Red = function () {
            $window.location.assign('redirect301/Export');
        }

        ctrl.setActive301 = function (state) {
            $http.get('redirect301/getactive', { params: { active: state, rnd: Math.random() } }).then(function (response) {
                var active = response.data;
                if (active == null || active == false || active == undefined) {
                    ctrl.active301 = false;
                    ctrl.active301in404 = false;
                }
                else {
                    ctrl.active301 = true;
                    ctrl.active301in404 = true;
                }
            });
        }

        var columnDefs301Red = [
                {
                    name: 'RedirectFrom',
                    displayName: 'Откуда',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Откуда',
                        type: uiGridConstants.filter.INPUT,
                        name: 'RedirectFrom'
                    }
                },
                {
                    name: 'RedirectTo',
                    displayName: 'Куда',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Куда',
                        name: 'RedirectTo',
                        type: uiGridConstants.filter.INPUT
                    },
                    uiGridCustomEdit: { replaceNullable: false }
                },
                {
                    name: 'ProductArtNo',
                    displayName: 'Артикул товара (необязательно)',
                    type: uiGridConstants.filter.INPUT,
                    enableCellEdit: true,
                    width: 150,
                    filter: {
                        placeholder: 'Артикул товара (необязательно)',
                        type: uiGridConstants.filter.INPUT,
                        name: 'ProductArtNo'
                    },
                    uiGridCustomEdit: { replaceNullable : false}
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div class="">' +
                                        '<ui-modal-trigger data-controller="\'ModalAddEdit301RedCtrl\'" controller-as="ctrl" ' +
                                        'template-url="../areas/admin/content/src/settings/modal/addEdit301Red/addEdit301Red.html" ' +
                                        'data-resolve="{\'id\': row.entity.ID}" ' +
                                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                                        '<a ng-href="" class="ui-grid-custom-service-icon fa fa-pencil">{{COL_FIELD}}</a>' +
                                   '</ui-modal-trigger>' +
                                   '<ui-grid-custom-delete url="redirect301/deleteRedirect301" params="{\'Ids\': row.entity.ID}"></ui-grid-custom-delete></div></div>'
                }
        ];

        ctrl.gridOptions301Red = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs301Red,
            uiGridCustom: {
                rowUrl: '',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'redirect301/deleteRedirect301',
                        field: 'ID',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.get301Red = function (params) {
            return $http.post('redirect301/getRedirect301', { params: params, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        ctrl.delete301Red = function (IDs) {
            return $http.post('redirect301/deleteRedirect301', { 'Ids': IDs, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        ctrl.grid301RedOnInit = function (grid301Red) {
            ctrl.grid301Red = grid301Red;
        };

        //#endregion 301Redirect

        //#region start ErrorLog404

        var columnDefsErrorLog404 = [
                {
                    name: 'Url',
                    displayName: '404 страницы',
                    enableCellEdit: false,
                    filter: {
                        placeholder: '404 страницы',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Url'
                    }
                },
                {
                    name: 'UrlReferer',
                    displayName: 'Реферер',
                    enableCellEdit: false,
                    filter: {
                        placeholder: 'Реферер',
                        name: 'UrlReferer',
                        type: uiGridConstants.filter.INPUT
                    }
                },
                {
                    name: 'RedirectTo',
                    displayName: '301 редирект',
                    type: uiGridConstants.filter.INPUT,
                    enableCellEdit: false,
                    width: 130,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div ng-if="row.entity.RedirectTo == null || row.entity.RedirectTo == \'\'">' +
                        '<ui-modal-trigger data-controller="\'ModalAddEdit301RedCtrl\'" controller-as="ctrl" ' +
                        'template-url="../areas/admin/content/src/settings/modal/addEdit301Red/addEdit301Red.html" ' +
                        'data-resolve="{\'from\': { value: row.entity.Url}}" ' +
                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                        '<a ng-href="">Создать редирект</a>' +
                        '</ui-modal-trigger>' +
                        '</div><div ng-if="row.entity.RedirectTo != null && row.entity.RedirectTo != \'\'">{{COL_FIELD}}</div></div>',
                    filter: {
                        placeholder: '301 редирект',
                        type: uiGridConstants.filter.INPUT,
                        name: 'RedirectTo'
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 30,
                    cellTemplate: '<div class="ui-grid-cell-contents"><div class="">' +
                                   '<ui-grid-custom-delete url="ErrorLog404/DeleteItems" params="{\'Ids\': row.entity.Id}"></ui-grid-custom-delete></div></div>'
                }
        ];

        ctrl.gridOptionsErrorLog404 = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefsErrorLog404,
            uiGridCustom: {
                rowUrl: '',
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'ErrorLog404/DeleteItems',
                        field: 'Id',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены что хотите удалить?", { title: "Удаление" }).then(function (result) {
                                return result === true ? $q.resolve('sweetAlertConfirm') : $q.reject('sweetAlertCancel');
                            });
                        }
                    }
                ]
            }
        });

        ctrl.gridErrorLog404OnInit = function (gridErrorLog404) {
            ctrl.gridErrorLog404 = gridErrorLog404;
        };

        //#endregion ErrorLog404
    };

    SettingsSeoCtrl.$inject = ['Upload', '$http', 'toaster', '$q', '$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'uiGridCustomService', 'SweetAlert'];

    ng.module('settingsSeo', ['ngFileUpload', 'toaster', 'as.sortable', 'paymentMethodsList'])
      .controller('SettingsSeoCtrl', SettingsSeoCtrl);

})(window.angular);