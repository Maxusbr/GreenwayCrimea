; (function (ng) {
    'use strict';

    var adminCommentsService = function ($http) {
        var service = this;

        service.getComments = function (objId, type) {
            return $http.get('adminComments/getComments', { params: { objId: objId, type: type } }).then(function (response) {
                return response.data;
            });
        }

        service.deleteComment = function (id) {
            return $http.post('adminComments/delete', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.addComment = function (objId, type, parentId, text, objUrl) {
            return $http.post('adminComments/add', { objId: objId, type: type, text: text, parentId: parentId, objUrl: objUrl }).then(function (response) {
                return response.data;
            });
        }

        service.updateComment = function (id, text) {
            return $http.post('adminComments/update', { id: id, text: text }).then(function (response) {
                return response.data;
            });
        }
    };

    adminCommentsService.$inject = ['$http'];

    ng.module('adminComments')
        .service('adminCommentsService', adminCommentsService);

})(window.angular);