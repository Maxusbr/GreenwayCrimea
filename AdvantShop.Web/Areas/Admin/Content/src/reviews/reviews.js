; (function (ng) {
    'use strict';

    var ReviewsCtrl = function ($location, $window, uiGridConstants, uiGridCustomConfig, SweetAlert, $q, $uibModal) {

        var ctrl = this,
            columnDefs = [
                {
                    name: '_noopColumnName',
                    visible: false,
                    filter: {
                        placeholder: 'Автор',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Name',
                    }
                },
                {
                    name: '_noopColumnEmail',
                    visible: false,
                    filter: {
                        placeholder: 'Email',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Email',
                    }
                },
                {
                    name: '_noopColumnText',
                    visible: false,
                    filter: {
                        placeholder: 'Текст отзыва',
                        type: uiGridConstants.filter.INPUT,
                        name: 'Text',
                    }
                },
                {
                    name: '_noopColumnArtNo',
                    visible: false,
                    filter: {
                        placeholder: 'Артикул',
                        type: uiGridConstants.filter.INPUT,
                        name: 'ArtNo',
                    }
                },
                {
                    name: 'PhotoName',
                    displayName: 'Изобр.',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div class="ui-grid-custom-flex-center ui-grid-custom-link-for-img">' +
                            '<img ng-src="{{row.entity.PhotoSrc}}" alt="" />' + 
                        '</div></div>',
                    enableSorting: false,
                    filter: {
                        placeholder: 'Изображение',
                        type: uiGridConstants.filter.SELECT,
                        name: 'HasPhoto',
                        selectOptions: [{ label: 'С фотографией', value: true }, { label: 'Без фотографии', value: false }]
                    }
                },
                {
                    name: 'Name',
                    width: 270,
                    displayName: 'Автор',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '{{row.entity.Name}}<br/> ' +
                            '{{row.entity.Email}} ' +
                        '</div>',
                },
                {
                    name: 'Text',
                    displayName: 'Текст отзыва',
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +

                    //        '<ui-modal-trigger size="middle" data-controller="\'ModalAddEditReviewCtrl\'" controller-as="ctrl" ' +
                    //                        'template-url="../areas/admin/content/src/reviews/modal/addEditReview/addEditReview.html" ' +
                    //                        'data-resolve="{\'reviewId\': row.entity.ReviewId}" ' +
                    //                        'data-on-close="grid.appScope.$ctrl.fetchData()"> ' +
                    //            '<a href="">{{ grid.appScope.$ctrl.gridExtendCtrl.truncate(row.entity.Text) }}</a>' +
                    //        '</ui-modal-trigger>' +
                            '{{ grid.appScope.$ctrl.gridExtendCtrl.truncate(row.entity.Text) }}' + 
                        '</div>'
                },
                {
                    name: 'ProductName',
                    displayName: 'Название',
                    enableSorting: false,
                    width: 160,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<a ng-if="row.entity.ProductId != null" href="product/edit/{{row.entity.ProductId}}">{{row.entity.ProductName}}</a> ' +
                            '<span ng-if="row.entity.ProductId == null">{{row.entity.ArtNo}}, {{row.entity.ProductName}}</span>' +
                        '</div>'
                },
                {
                    name: 'AddDateFormatted',
                    displayName: 'Добавлен',
                    width: 90,
                    filter: {
                        placeholder: 'Дата и время',
                        type: 'datetime',
                        term: {
                            from: (new Date()).setMonth((new Date()).getMonth() - 1),
                            to: new Date()
                        },
                        datetimeOptions: {
                            from: {
                                name: 'DateFrom'
                            },
                            to: {
                                name: 'DateTo'
                            }
                        }
                    }
                },
                {
                    name: 'Checked',
                    displayName: 'Проверен',
                    width: 80,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents">' +
                            '<label class="ui-grid-custom-edit-field adv-checkbox-label"> ' +
                                '<input type="checkbox" class="adv-checkbox-input" ng-model="row.entity.Checked" disabled /> ' +
                                '<span class="adv-checkbox-emul"></span> ' +
                            '</label>' +
                        '</div>',
                    filter: {
                        placeholder: 'Проверен',
                        type: uiGridConstants.filter.SELECT,
                        selectOptions: [{ label: 'Да', value: true }, { label: 'Нет', value: false }]
                    }
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 70,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<a href="" class="link-invert ui-grid-custom-service-icon fa fa-pencil" ng-click="grid.appScope.$ctrl.gridExtendCtrl.openModal(row.entity.ReviewId)"></a> ' +
                            '<ui-grid-custom-delete url="reviews/deleteReview" params="{\'reviewId\': row.entity.ReviewId }"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ];

        ctrl.gridOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: columnDefs,
            uiGridCustom: {
                rowClick: function ($event, row) {
                    ctrl.openModal(row.entity.ReviewId);
                },
                selectionOptions: [
                    {
                        text: 'Удалить выделенные',
                        url: 'reviews/deleteReviews',
                        field: 'ReviewId',
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

        ctrl.truncate = function(str) {
            if (str.length > 120) {
                str = str.substring(0, 120) + "..";
            }
            return str;
        }

        ctrl.openModal = function (reviewId) {
            $uibModal.open({
                bindToController: true,
                controller: 'ModalAddEditReviewCtrl',
                controllerAs: 'ctrl',
                size: 'lg',
                templateUrl: '../areas/admin/content/src/reviews/modal/addEditReview/addEditReview.html',
                resolve: {
                    reviewId: function () {
                        return reviewId;
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
        
    };

    ReviewsCtrl.$inject = ['$location', '$window', 'uiGridConstants', 'uiGridCustomConfig', 'SweetAlert', '$q', '$uibModal'];


    ng.module('reviews', ['uiGridCustom', 'urlHelper'])
      .controller('ReviewsCtrl', ReviewsCtrl);

})(window.angular);