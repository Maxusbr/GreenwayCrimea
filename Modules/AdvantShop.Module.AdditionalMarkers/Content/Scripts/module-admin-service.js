; (function (ng) {
    'use strict';

    var moduleAdminService = function ($http) {
        var service = this;

        service.getAllMarkers = function () {
            return $http.get('../amadmin/getallmarkers').then(function (response) {
                return response.data;
            });
        }

        service.actionMarker = function (marker, editIdentity, oldMarkerId) {
            return $http.post('../amadmin/actionmarker', { marker, editIdentity, oldMarkerId }).then(function (response) {
                return response.data;
            });
        }

        service.getCurrSoring = function () {
            return $http.get('../amadmin/getSorting').then(function (response) {
                return response.data;
            });
        }

        service.link = function (productId, markerId) {
            return $http.post('../amadmin/link', { productId, markerId }).then(function (response) {
                return response.data;
            });
        }
        service.getMarkers = function (productId) {
            return $http.post('../amadmin/getmarkers', { productId }).then(function (response) {
                return response.data;
            });
        }
    }

    moduleAdminService.$inject = ['$http'];

    ng.module('module').service('moduleAdminService', moduleAdminService);

})(window.angular)