; (function (ng) {
    'use strict';


    var urlHelperService = function ($window, urlHelperConfig) {

        var service = this,
            tagBaseHref = document.getElementsByTagName('base')[0].getAttribute('href'),
            regexDomain = new RegExp('^(?:[a-z]+:)?//', 'i');

        service.getUrlParam = function (paramName)
        {
            var query = $window.location.search.substring(1);
            var vars = query.split("&");
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split("=");
                if (pair[0] == paramName) { return pair[1]; }
            }
            return null;
        };

        service.getUrlParamByName = function (name) {
            var url = $window.location.href.toLowerCase();
            name = name.toLowerCase().replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        };

        service.getUrlParamsAsObject = function () {
            var search = location.search.substring(1);
            return JSON.parse('{"' + decodeURI(search).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}');
        };

        service.getBaseHref = function () {
            return tagBaseHref;
        };

        service.hasDomain = function (url) {
            return regexDomain.test(url);
        };

        service.getAbsUrl = function (url, excludeAdmin) {

            var base = service.getBaseHref(),
                basePrepare;

            if((excludeAdmin != null && excludeAdmin === true) || urlHelperConfig.isAdmin === false){
                basePrepare = base.replace(urlHelperConfig.adminPath, '');
            } else {
                basePrepare = base;
            }

            if (service.hasDomain(url) === false) {

                //убераем впереди слеш
                if (url.charAt(0) === '/') {
                    url = url.substring(1);
                }

                url = basePrepare + url;
            }

            return url;
        };

        service.getUrl = function (url, excludeAdmin) {

            var base = service.getBaseHref(),
                basePrepare = base.replace(/.*\/\/[^\/]*/, '');

            if ((excludeAdmin != null && excludeAdmin === true) || urlHelperConfig.isAdmin === false) {
                basePrepare = basePrepare.replace(urlHelperConfig.adminPath, '');
            }

            if (service.hasDomain(url) === false) {
                if (url.charAt(0) === '/') {
                    url = url.substring(1);
                }
                url = basePrepare + url;
            }
            return url;
        };

        service.paramsToString = function (object) {
            var result = "";
            for(var key in object)
            {
                if(object.hasOwnProperty(key))
                {
                    result += key + "=" + object[key] + "&";
                }
            }
                       
            return result;
        };

    };

    urlHelperService.$inject = ['$window', 'urlHelperConfig'];

    ng.module('urlHelper', [])
      .service('urlHelper', urlHelperService)
      .constant('urlHelperConfig', {
          isAdmin: false,
          adminPath: 'adminv2/'
      })

})(window.angular);