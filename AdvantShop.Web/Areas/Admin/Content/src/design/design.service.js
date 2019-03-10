; (function (ng) {
    'use strict';

    var designService = function ($http, Upload) {
        var service = this;

        service.getDesigns = function (params) {
            return $http.get(typeof (params["stringid"]) === "undefined" ? 'design/getdesigns' : 'design/getdesigns?stringId=' + params["stringid"]).then(function (response) {
                return response.data;
            });
        };

        service.saveDesign = function (designType, name) {
            return $http.post('design/savedesign', { designType: designType, name: name }).then(function (response) {
                return response.data;
            });
        };

        service.uploadDesign = function (file, designType) {
            return Upload.upload({
                url: 'design/uploaddesign',
                data: {
                    file: file,
                    designType: designType,
                    rnd: Math.random(),
                }
            }).then(function (response) {
                return response.data;
            })
        };

        service.deleteDesign = function (name, designType) {
            return $http.post('design/deletedesign', { name: name, designType: designType }).then(function (response) {
                return response.data;
            });
        };

        service.previewTemplate = function (id, previewTemplateId) {
            return $http.post('design/previewtemplate', { id: id, previewTemplateId: previewTemplateId }).then(function (response) {
                return response.data;
            });
        };

        service.checkPage = function (url) {
            return $http.get(url, { rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        };

        service.resizePictures = function () {
            return $http.post('design/resizePictures').then(function (response) {
                return response.data;
            });
        };

        service.installTemplate = function (stringId, id, version) {
            return $http.post('design/installTemplate', { stringId: stringId, id: id, version: version }).then(function (response) {
                return response.data;
            });
        };       

        service.updateTemplate = function (id) {
            return $http.post('design/updateTemplate', { id: id }).then(function (response) {
                return response.data;
            });
        };

        service.deleteTemplate = function (stringid) {
            return $http.post('design/deleteTemplate', { stringid: stringid }).then(function (response) {
                return response.data;
            });
        };

    };

    designService.$inject = ['$http', 'Upload'];

    ng.module('design')
        .service('designService', designService);

})(window.angular);