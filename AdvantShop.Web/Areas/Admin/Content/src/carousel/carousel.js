; (function (ng) {
    'use strict';

    var CarouselCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, $q, SweetAlert, $http, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: 'ImageSrc',
                    displayName: 'Изобр.',
                    enableSorting: false,
                    width: 80,
                    cellTemplate: '<div class="ui-grid-cell-contents"><a class="ui-grid-custom-flex-center ui-grid-custom-link-for-img"><img class="ui-grid-custom-col-img" ng-src="{{row.entity.ImageSrc}}"></a></div>',
                    enableCellEdit: false,
                },
                {
                    name: 'CaruselUrl',
                    displayName: 'Синоним для URL запроса',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Синоним для URL запроса',
                        type: uiGridConstants.filter.INPUT,
                        name: 'CaruselUrl'
                    }
                },
                {
                    name: 'Description',
                    displayName: 'Alt-тег изображения',
                    enableCellEdit: true,
                    filter: {
                        placeholder: 'Alt-тег изображения',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Description'
                    }
                },

                {
                    name: 'DisplayInOneColumn',
                    displayName: 'Одна колонка',
                    enableCellEdit: true,
                    type: 'checkbox',
                    width: 76,
                    filter: {
                        placeholder: 'Одна колонка',
                        name: 'DisplayInOneColumn',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },

                {
                    name: 'DisplayInTwoColumns',
                    displayName: 'Две колонки',
                    enableCellEdit: true,
                    type: 'checkbox',
                    width: 76,
                    filter: {
                        placeholder: 'Две колонки',
                        name: 'DisplayInTwoColumns',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'DisplayInMobile',
                    displayName: 'Моб. версия',
                    enableCellEdit: true,
                    type: 'checkbox',
                    width: 76,
                    filter: {
                        placeholder: 'Мобильная версия',
                        name: 'DisplayInMobile',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'Blank',
                    displayName: 'В новом окне',
                    enableCellEdit: true,
                    type: 'checkbox',
                    width: 76,
                    filter: {
                        placeholder: 'В новом окне',
                        name: 'Blank',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: 'SortOrder',
                    displayName: 'Сортировка',
                    width: 120,
                    enableCellEdit: true
                },
                {
                    name: 'Enabled',
                    displayName: 'Актив.',
                    enableCellEdit: true,
                    type: 'checkbox',
                    width: 76,
                    filter: {
                        placeholder: 'Актив.',
                        name: 'Enabled',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 60,
                    enableSorting: false,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="Carousel/DeleteCarousel" params="{\'Ids\': row.entity.CarouselId}"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];


        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'Carousel/DeleteCarousel',
                        field: 'CarouselId',
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
                    url: '/Carousel/upload',
                    data: {
                        file: $file,
                        rnd: Math.random(),
                    }
                }).then(function (response) {
                    var data = response.data;
                    if (data.Result == true) {
                        ctrl.ImageSrc = data.Picture;
                    } else {
                        toaster.pop('error', 'Ошибка при загрузке изображения', data.error);
                    }
                })
            } else if ($invalidFiles.length > 0) {
                toaster.pop('error', 'Ошибка при загрузке изображения', 'Файл не соответствует требованиям');
            }
        };
    };

    CarouselCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', '$q', 'SweetAlert', '$http', '$uibModal'];


    ng.module('carouselPage', ['uiGridCustom', 'urlHelper'])
      .controller('CarouselCtrl', CarouselCtrl);

})(window.angular);