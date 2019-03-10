; (function (ng) {
    'use strict';

    var CategoryCtrl = function ($http, uiGridCustomConfig, toaster, Upload, SweetAlert) {

        var ctrl = this;

        ctrl.$onInit = function() {
            ctrl.showGridPropertyGroups = true;
        }

        ctrl.gridPropertyGroupsOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Name',
                    displayName: 'Группы свойств для данной категории:'
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 40,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div><ui-grid-custom-delete url="category/deleteGroupFromCategory" params="{\'groupId\': row.entity.PropertyGroupId, \'categoryId\': row.entity.CategoryId }"></ui-grid-custom-delete></div></div>'
                }
            ],
        });

        ctrl.gridOnInit = function (grid) {
            ctrl.gridPropertyGroups = grid;
            ctrl.showGridPropertyGroups = grid.gridOptions.data.length > 0;
        };

        ctrl.gridOnFetch = function (grid) {
            ctrl.showGridPropertyGroups = grid.gridOptions.data.length > 0;
        }


        ctrl.changeCategory = function (result) {
            ctrl.ParentCategoryId = result.categoryId;
            ctrl.ParentCategoryName = result.categoryName;
        }


        ctrl.PictureId = 0;
        ctrl.IconId = 0;
        ctrl.MiniPictureId = 0;

        ctrl.updateMiniImage = function (result) {
            ctrl.MiniPictureId = result.pictureId;
        };

        ctrl.updateIconImage = function (result) {
            ctrl.IconId = result.pictureId;
        };

        ctrl.updateImage = function (result) {
            ctrl.PictureId = result.pictureId;
        };

        // load tags
        ctrl.loadTags = function (categoryId) {
            $http.get('category/getTags?categoryId=' + categoryId).then(function (response) {
                ctrl.tags = response.data.tags;
                ctrl.selectedTags = response.data.selectedTags;
            });
        }

        ctrl.tagTransform = function (newTag) {
            return { value: newTag };
        };

        ctrl.deleteCategory = function (categoryId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result) {
                    $http.post('category/delete', { id: categoryId }).then(function (response) {
                        if (response.data.result === true) {
                            if (response.data.needRedirect) {
                                window.location = 'catalog?categoryid=' + response.data.id;
                            }
                        } else {
                            toaster.pop('error', '', 'Ошибка при удалении', "");
                        }
                    });
                }
            });
        }
    };

    CategoryCtrl.$inject = ['$http', 'uiGridCustomConfig', 'toaster', 'Upload', 'SweetAlert'];

    ng.module('category', ['angular-inview','uiGridCustom', 'urlGenerator', 'uiModal'])
      .controller('CategoryCtrl', CategoryCtrl);

})(window.angular);