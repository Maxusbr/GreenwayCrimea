; (function (ng) {
    'use strict';

    var russianPostPrintBlankService = function ($http) {
        var service = this;

        service.getOrders = function (page, status, payed, shipping, orderNumber) {
            return $http.get('../rppbadmin/getorders', { params: { page: page, status: status, payed: payed, shipping: shipping, orderNumber: orderNumber, rnd: Math.random() } })
                .then(function (response) {
                    return response.data;
                });
        };

        service.getPagesCount = function () {
            return $http.get('../rppbadmin/getOrdersCount')
                .then(function (response) {
                    return response.data;
                });
        };

        service.getTemplates = function () {
            return $http.get('../rppbtemplates/templateslist', { params: { rnd: Math.random() } })
                .then(function (response) {
                    return response.data;
                });
        };

        service.deleteTemplate = function (templateId) {
            return $http.post('../rppbtemplates/deletetemplate', { templateId: templateId })
                .then(function (response) {
                    return response.data;
                });
        };

        service.getTemplate = function (templateId) {
            return $http.get('../rppbtemplates/gettemplate', { params: { templateId: templateId } })
                .then(function (response) {
                    return response.data;
                });
        };

        service.editTemplate = function (templateId, name, content) {
            return $http.post('../rppbtemplates/edittemplate', { templateId: templateId, name: name, content: JSON.stringify(content) })
                .then(function (response) {
                    return response.data;
                });
        };
        
        service.getAvailableTemplateTypes = function () {
            return $http.get('../rppbtemplates/getavailabletemplatetypes', { params: { rnd: Math.random() } })
                .then(function (response) {
                    return response.data;
                });
        };
        
        service.getTemplateType = function (templateType) {
            return $http.get('../rppbtemplates/gettemplatetype', { params: { templateType: templateType } })
                .then(function (response) {
                    return response.data;
                });
        };

        service.createTemplate = function (templateType, name, content) {
            return $http.post('../rppbtemplates/addtemplate', { templateType: templateType, name: name, content: JSON.stringify(content) })
                .then(function (response) {
                    return response.data;
                });
        };

        service.checkActive = function () {
            return $http.post('../rppbadmin/checkActive', {})
                .then(function (response) {
                    return response.data;
                });
        };
    };

    ng.module('module')
        .service('russianPostPrintBlankService', russianPostPrintBlankService);

    russianPostPrintBlankService.$inject = ['$http', '$q'];

})(window.angular);