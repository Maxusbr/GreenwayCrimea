; (function (ng) {
    'use strict';

    var CategoriesBlockCtrl = function (SweetAlert, catalogService, toaster) {
        var ctrl = this;

        ctrl.$onInit = function () {

            ctrl.categoriesSelected = [];

            ctrl.fetch();
        };

        ctrl.fetch = function () {
           return catalogService.getCategories(ctrl.categoryId, ctrl.categorysearch).then(function (result) {
               return ctrl.categories = result;
            });
        };

        ctrl.toggleSelectedAll = function (selectAll) {
            if (selectAll === true) {
                ctrl.categoriesSelected = ctrl.categories.map(function (item) { return item.CategoryId; });
            } else {
                ctrl.categoriesSelected = [];
            }
        };

        ctrl.deleteCategories = function (ids) {
            SweetAlert.confirm("Вы уверены, что хотите удалить выбранные категории?", { title: "Удаление категорий", showLoaderOnConfirm: true })
               .then(function (result) {
                   if (result === true) {
                       catalogService.deleteCategories(ids).then(function () {
                           ctrl.categoriesSelected = [];
                       })
                       .then(ctrl.fetch)
                       .then(function () {
                           if (ctrl.onDelete != null) {
                               ctrl.onDelete();
                           }
                       });
                   }
               });
        };

        ctrl.deleteCategory = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить категорию?", { title: "Удаление категории", showLoaderOnConfirm: true, confirmButtonColor: "#2d9cee", cancelButton: "#ffffff" })
               .then(function (result) {
                   if (result === true) {
                       catalogService.deleteCategories([id]).then(function () {
                           ctrl.categoriesSelected = [];
                       })
                       .then(ctrl.fetch)
                       .then(function () {
                           if (ctrl.onDelete != null) {
                               ctrl.onDelete();
                           }
                       });
                   }
               });
        }

        ctrl.sortableOptions = {
            orderChanged: function (event) {
                var categoryId = event.source.itemScope.category.CategoryId,
                    prevCategory = ctrl.categories[event.dest.index - 1],
                    nextCategory = ctrl.categories[event.dest.index + 1];

                catalogService.changeCategorySortOrder(categoryId, prevCategory != null ? prevCategory.CategoryId : null, nextCategory != null ? nextCategory.CategoryId : null).then(function () {
                    toaster.pop("success", "", "Изменения сохранены");
                });
            }
        };
    };


    CategoriesBlockCtrl.$inject = ['SweetAlert', 'catalogService', 'toaster'];

    ng.module('categoriesBlock', ['ng-sweet-alert', 'checklist-model', 'as.sortable'])
        .controller('CategoriesBlockCtrl', CategoriesBlockCtrl);

})(window.angular);