; (function (ng) {
    'use strict';

    var uiGridCustomService = function ($http, $window, $location, uiGridCustomDataScheme, uiGridCustomConfig, urlHelper) {
        var service = this,
            uiGridCustomDataSchemeRevert,
            STORAGE_ID = 'uiGrid',
            storageIds = [];

        service.getData = function (url, params) {
            return $http.get(url, { params: params }).then(function (response) {
                return service.convertToClientParams(response.data);
            });
        };

        service.applyInplaceEditing = function (url, params) {
            return $http.post(url, params).then(function (response) {
                return response.data;
            });
        };

        service.convertToClientParams = function (resultFromServer, onlyEqual) {
            return service.mapping(resultFromServer, 'client', onlyEqual);
        };

        service.convertToServerParams = function (resultFromClient, onlyEqual) {
            return service.mapping(resultFromClient, 'server', onlyEqual);
        };

        service.mapping = function (data, convertTo, onlyEqual) {

            var result, dictionary;

            uiGridCustomDataSchemeRevert = uiGridCustomDataSchemeRevert || service.revertDictionary(uiGridCustomDataScheme);

            dictionary = convertTo === 'server' ? uiGridCustomDataSchemeRevert : uiGridCustomDataScheme;

            for (var key in data) {
                if (data.hasOwnProperty(key) === true) {
                    if (dictionary[key] != null) {
                        result = result || {};
                        result[dictionary[key]] = data[key];
                    } else if (onlyEqual == null || onlyEqual == false) {
                        result = result || {};
                        result[key] = data[key];
                    }
                }
            }

            if (convertTo === 'client' && result != null && result.data == null) {
                result.data = [];
            }

            return result;
        };

        service.revertDictionary = function (dictionary) {
            var result = {};

            for (var key in dictionary) {
                if (dictionary.hasOwnProperty(key) === true) {
                    result[dictionary[key]] = key;
                }
            }

            return result;
        }

        service.getParamsByUrl = function (uniqueId) {
            return JSON.parse($location.search()[uniqueId] || null);
        };

        service.setParamsByUrl = function (uniqueId, params) {
            return $location.search(uniqueId, JSON.stringify(params));
        };


        service.clearParams = function (uniqueId) {
            return $location.search(uniqueId, null);
        };

        //service.calcOptions = function (options) {
        //    var step, tempStep, isTotalItemsLess;

        //    options.paginationPageSizes.length = 0;

        //    isTotalItemsLess = options.totalItems <= uiGridCustomConfig.paginationPageSize;

        //    options.paginationPageSize = isTotalItemsLess ? options.totalItems : uiGridCustomConfig.paginationPageSize;

        //    step = options.totalItems / 4;

        //    if (isTotalItemsLess === false || step < 0) {

        //        for (var i = 1, len = step + 1; i < len; i++) {
        //            tempStep = i * uiGridCustomConfig.paginationPageSize;

        //            if (tempStep <= options.totalItems) {
        //                options.paginationPageSizes.push(tempStep);
        //            } else {
        //                options.paginationPageSizes.push(tempStep);
        //                break;
        //            }
        //        }
        //    } else {
        //        options.paginationPageSizes.push(options.totalItems);
        //    }
        //}

        service.export = function (url, params) {
            //get all items
            params.ItemsPerPage = 1000000;
            params.Page = 1;
            params.OutputDataType = "Csv";
            $window.location.assign(url + "?" + urlHelper.paramsToString(params));

            //return $http.get(url, { params: params }).then(function (response) {
            //    //return response.data;
            //});
        };

        service.addInStorage = function (uniqueId) {
            storageIds.push(uniqueId);
        };

        service.removeFromStorage = function (uniqueId) {
            var index = storageIds.indexOf(uniqueId);

            if (uniqueId !== -1) {
                storageIds.splice(index, 1);
            }
        };

        service.validateId = function (uniqueId) {
            return uniqueId != null && uniqueId.length > 0 && storageIds.indexOf(uniqueId) === -1;
        };

        service.saveDataInStorage = function (key, data) {

            var storage = service.getDataInStorage() || {};

            storage[key] = data;

            $window.localStorage.setItem(STORAGE_ID, JSON.stringify(storage));

            return storage[key];
        };

        service.getDataItimFromStorageByKey = function (key) {
            var data = service.getDataInStorage();

            return data != null ? data[key] : null;
        };

        service.getDataInStorage = function () {
            var data = $window.localStorage.getItem(STORAGE_ID);

            return data != null && data.length > 0 ? JSON.parse(data) : null;
        };
    };

    uiGridCustomService.$inject = ['$http', '$window', '$location', 'uiGridCustomDataScheme', 'uiGridCustomConfig', 'urlHelper'];

    ng.module('uiGridCustom')
      .service('uiGridCustomService', uiGridCustomService);

})(window.angular);
