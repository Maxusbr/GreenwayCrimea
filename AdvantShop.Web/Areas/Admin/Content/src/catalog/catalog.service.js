; (function (ng) {
    'use strict';

    var catalogService = function ($http) {
        var service = this;

        service.getCatalog = function (params) {
            return $http.get('catalog/getcatalog', { params: params }).then(function (response) {
                return response.data;
            });
        };

        service.getCategories = function (categoryId, categorysearch) {
            return $http.get('catalog/categorylistjson', { params: { categoryId: categoryId, categorysearch: categorysearch } }).then(function (response) {
                return response.data;
            });
        };

        service.deleteCategories = function (categoryIds) {
            return $http.post('catalog/deletecategories', { categoryIds: categoryIds }).then(function (response) {
                return response.data;
            });
        };

        service.changeCategorySortOrder = function (categoryId, prevCategoryId, nextCategoryId, parentCategoryId) {
            return $http.post('catalog/changecategorysortorder', { categoryId: categoryId, prevCategoryId: prevCategoryId, nextCategoryId: nextCategoryId, parentCategoryId: parentCategoryId }).then(function (response) {
                return response.data;
            });
        };

        service.getDataProducts = function () {
            return $http.get('catalog/getdataproducts', { params: { rnd: Math.random() } }).then(function (response) {
                return response.data;
            });
        };

        //service.deleteCategory = function (categoryId, prevCategoryId, nextCategoryId, parentCategoryId) {
        //    return $http.post('catalog/changecategorysortorder', { categoryId: categoryId, prevCategoryId: prevCategoryId, nextCategoryId: nextCategoryId, parentCategoryId: parentCategoryId }).then(function (response) {
        //        return response.data;
        //    });
        //};
    };

    catalogService.$inject = ['$http'];

    ng.module('catalog')
        .service('catalogService', catalogService);

})(window.angular);