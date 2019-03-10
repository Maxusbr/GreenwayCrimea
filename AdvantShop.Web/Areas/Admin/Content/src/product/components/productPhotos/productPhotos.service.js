; (function (ng) {
    'use strict';

    var productPhotosService = function ($http) {
        var service = this;
        
        service.getPhotos = function (productId) {
            return $http.get('product/getPhotos', { params: { productId: productId } }).then(function (response) {
                return response.data;
            });
        };

        service.getPhotoColors = function (productId) {
            return $http.get('product/getPhotoColors', { params: { productId: productId } }).then(function (response) {
                return response.data;
            });
        };

        service.deletePhoto = function (photoId) {
            return $http.post('product/deletePhoto', { photoId: photoId }).then(function (response) {
                return response.data;
            });
        };

        service.editPhoto = function (photoId, alt, colorId) {
            return $http.post('product/editPhoto', { photoId: photoId, alt: alt, colorId: colorId }).then(function (response) {
                return response.data;
            });
        };

        service.changePhotoColor = function (photoId, colorId) {
            return $http.post('product/changePhotoColor', { photoId: photoId, colorId: colorId }).then(function (response) {
                return response.data;
            });
        };

        service.changeMainPhoto = function (photoId) {
            return $http.post('product/changeMainPhoto', { photoId: photoId }).then(function (response) {
                return response.data;
            });
        };

        service.changePhotoSortOrder = function (productId, photoId, prevPhotoId, nextPhotoId) {
            return $http.post('product/changePhotoSortOrder', { productId: productId, photoId: photoId, prevPhotoId: prevPhotoId, nextPhotoId: nextPhotoId }).then(function (response) {
                return response.data;
            });
        };

        service.uploadPhoto = function (productId, fileLink) {
            return $http.post('product/uploadPictureByLink', { productId: productId, fileLink: fileLink }).then(function (response) {
                return response.data;
            });
        };

        service.uploadListPhoto = function (productId, fileLinks) {
            return $http.post('product/uploadPicturesByLink', { productId: productId, fileLinks: fileLinks }).then(function (response) {
                return response.data;
            });
        };
    };

    productPhotosService.$inject = ['$http'];

    ng.module('productPhotos')
        .service('productPhotosService', productPhotosService);

})(window.angular);