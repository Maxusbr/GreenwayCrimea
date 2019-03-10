; (function (ng) {
    'use strict';

    var FilesCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'FileName',
                    displayName: 'Имя файла',
                    enableSorting: true,
                    enableCellEdit: true,
                },
                {
                    name: 'FileSizeString',
                    displayName: 'Размер файла',
                    enableSorting: true,
                    enableCellEdit: false,
                    width: 150,

                },
                {
                    name: 'DateCreatedString',
                    displayName: 'Дата создания',
                    enableSorting: true,
                    enableCellEdit: false,
                    width: 150,
                },
                 {
                     name: 'DateModifiedString',
                     displayName: 'Дата изменения',
                     enableSorting: true,
                     enableCellEdit: false,
                     width: 150,
                 },
                {
                    name: 'Link',
                    cellTemplate: '<div class="ui-grid-cell-contents"><a ng-href="{{row.entity.Link}}" download>Скачать</a></div>"',
                    displayName: 'Ссылка',
                    enableSorting: false,
                    enableCellEdit: false,
                    width: 100,
                    
                },

                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="Files/DeleteFile" params="{\'Ids\': row.entity.FileName}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'Files/DeleteFile',
                        field: 'FileName',
                        before: function () {
                            return SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
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

        ctrl.upload = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if (($event.type === 'change' || $event.type === 'drop') && $file != null) {
                Upload.upload({
                    url: '/Files/UploadFile',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.Result == true) {
                        ctrl.ImageSrc = data.Picture;
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке файла', data.error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке файла', 'Файл не соответствует требованиям');
            }
        };
    };

    FilesCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http'];


    ng.module('files', ['uiGridCustom', 'urlHelper'])
      .controller('FilesCtrl', FilesCtrl);

})(window.angular);