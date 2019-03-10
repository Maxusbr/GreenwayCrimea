; (function (ng) {
    'use strict';

    var CatProductRecommendationsCtrl = function($http, toaster) {
        var ctrl = this;

        ctrl.$onInit = function() {
            ctrl.getCategories();
            ctrl.getProperties();
            ctrl.getPropertiesWithValues();
        }

        // get categories
        ctrl.getCategories = function() {
            $http.get('category/getRecomCategories', { params: { categoryId: ctrl.categoryId, type: ctrl.type } }).then(function(response) {
                ctrl.categories = response.data;
            });
        }

        ctrl.deleteCategory = function(type, relCategoryId) {
            $http.post('category/deleteRecomCategory', { categoryId: ctrl.categoryId, relcategoryId: relCategoryId, type: type }).then(function (response) {
                ctrl.getCategories();
                toaster.pop('success', 'Изменения сохранены');
            });
        }

        ctrl.addCategory = function(result) {
            $http.post('category/addRecomCategory', { categoryId: ctrl.categoryId, relcategoryId: result.categoryId, type: ctrl.type }).then(function (response) {
                ctrl.getCategories();
                toaster.pop('success', 'Изменения сохранены');
            });
        }

        // get properties
        ctrl.getProperties = function() {
            $http.get('category/getRecomProperties', { params: { categoryId: ctrl.categoryId, type: ctrl.type } }).then(function(response) {
                ctrl.properties = response.data;
            });
        }

        ctrl.deleteProperty = function (type, propertyId) {
            $http.post('category/deleteRecomProperty', { categoryId: ctrl.categoryId, propertyId: propertyId, type: type }).then(function (response) {
                ctrl.getProperties();
                toaster.pop('success', 'Изменения сохранены');
            });
        }

        ctrl.addProperty = function (propertyId) {
            $http.post('category/addRecomProperty', { categoryId: ctrl.categoryId, propertyId: propertyId, type: ctrl.type }).then(function (response) {
                ctrl.getProperties();
                toaster.pop('success', 'Изменения сохранены');
            });
        }

        // get properties with values
        ctrl.getPropertiesWithValues = function () {
            $http.get('category/getRecomPropertiesWithValues', { params: { categoryId: ctrl.categoryId, type: ctrl.type } }).then(function (response) {
                ctrl.propertiesWithValues = response.data;
            });
        }

        ctrl.deletePropertyWithValue = function (type, propertyValueId) {
            $http.post('category/deleteRecomPropertyWithValue', { categoryId: ctrl.categoryId, propertyValueId: propertyValueId, type: type }).then(function (response) {
                ctrl.getPropertiesWithValues();
                toaster.pop('success', 'Изменения сохранены');
            });
        }

        ctrl.addPropertyWithValue = function (propertyValueId) {
            $http.post('category/addRecomPropertyWithValue', { categoryId: ctrl.categoryId, propertyValueId: propertyValueId, type: ctrl.type }).then(function (response) {
                ctrl.getPropertiesWithValues();
                toaster.pop('success', 'Изменения сохранены');
            });
        }


        ctrl.addPropertyModal = function (result) {

            if (result == null)
                return;

            if (result.type == "0" && result.propertyValueId != null) {
                ctrl.addPropertyWithValue(result.propertyValueId);
            } else if (result.type == "1") {
                ctrl.addProperty(result.propertyId);
            }
        }
    };

    CatProductRecommendationsCtrl.$inject = ['$http', 'toaster'];

    ng.module('catProductRecommendations', [])
        .controller('CatProductRecommendationsCtrl', CatProductRecommendationsCtrl)
        .component('catProductRecommendations', {
            templateUrl: '../areas/admin/content/src/category/components/catProductRecommendations.html',
            controller: CatProductRecommendationsCtrl,
            controllerAs: "ctrl",
            bindings: {
                type: '@',
                categoryId: '@',
            }
      });

})(window.angular);