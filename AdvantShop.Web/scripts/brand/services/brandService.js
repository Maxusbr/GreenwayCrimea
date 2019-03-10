; (function (ng) {

    'use strict';

    var brandService = function ($window) {
        var service = this;

        service.getUrlParam = function (str) {
            var index = str.indexOf('?'),
                strNormalize = str,
                parameters = {},
                arrayKeyValues,
                temp;

            if (index > -1) {
                strNormalize = strNormalize.substring(index + 1);
            }

            arrayKeyValues = strNormalize.split('&');

            for (var i = arrayKeyValues.length - 1; i >= 0; i--) {
                temp = arrayKeyValues[i].split('=');

                if (temp.length === 2) {
                    parameters[decodeURIComponent(temp[0])] = decodeURIComponent(temp[1]);
                }
            }

            return parameters;

        }

        service.buildUrlParams = function (obj, paramName, paramValue) {
            return "?" + paramName + "=" + paramValue;
            //var result = [],
            //    buildedData;

            //obj = service.getUrlParam(obj);

            //obj[paramName] = paramValue;

            //for (var item in obj) {
            //    buildedData = item + '=' + obj[item];
            //    result.push(buildedData);
            //}

            //return "?"+result.join('&');
        }

        service.filterRefresh = function (str) {
            var base = document.getElementsByTagName('base')[0].href + "manufacturers";
            $window.location.replace(base + str);
        }

    };

    ng.module('brand')
      .service('brandService', brandService);

    brandService.$inject = ['$window'];

})(window.angular);