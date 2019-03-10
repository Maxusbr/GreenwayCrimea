; (function (ng) {

    'use strict';

    /*убираем BFCache в FF*/
    window.addEventListener('unload', function () { });

    var dependencyService = ng.injector(['dependency']).get('dependencyService');

    ng.module('app', dependencyService.get())
        .config(['$provide', '$compileProvider', '$cookiesProvider', '$httpProvider', '$localeProvider', '$translateProvider', '$locationProvider', function ($provide, $compileProvider, $cookiesProvider, $httpProvider, $localeProvider, $translateProvider, $locationProvider) {
            var date = new Date(),
                currentYear = date.getFullYear();

            date.setFullYear(currentYear + 1);

            

            //Turn off URL manipulation in AngularJS
            //this code breaking preventDefault for anchors with empty href
            //$provide.decorator('$browser', ['$delegate', function ($delegate) {
            //    $delegate.onUrlChange = function () { };
            //    $delegate.url = function () { return "" };
            //    return $delegate;
            //}]);

            //#region compile debug
            $compileProvider.debugInfoEnabled(false);
            //#endregion

            //#region set cookie expires
            $cookiesProvider.defaults.expires = date;
            $cookiesProvider.defaults.path = '/';

            if (window.location.hostname !== 'localhost' && window.location.hostname !== 'server' &&
               !/^(?!0)(?!.*\.$)((1?\d?\d|25[0-5]|2[0-4]\d)(\.|$)){4}$/.test(window.location.hostname)) {
                $cookiesProvider.defaults.domain = '.' + window.location.hostname.replace('www.', '');
            }

            //#endregion

            //#region ie10 bug validation

            $provide.decorator('$sniffer', ['$delegate', function ($sniffer) {
                var msie = parseInt((/msie (\d+)/.exec(angular.lowercase(navigator.userAgent)) || [])[1], 10);
                var _hasEvent = $sniffer.hasEvent;
                $sniffer.hasEvent = function (event) {
                    if (event === 'input' && msie === 10) {
                        return false;
                    }
                    _hasEvent.call(this, event);
                }
                return $sniffer;
            }]);

            //#endregion

            //#region prepera ajax url in absolute path

            $httpProvider.useApplyAsync(true);

            $httpProvider.interceptors.push(['urlHelper', function (urlHelper) {
                return {
                    'request': function (config) {

                        var urlOld = config.url,
                            template;

                        config.url = urlHelper.getAbsUrl(config.url);

                        if (config.url.indexOf('.html') !== -1 || config.url.indexOf('.htm') !== -1) {
                            config.url += (config.url.indexOf('?') === -1 ? '?' : '&') + 'v=6';
                        }

                        //for templates
                        if (urlOld != config.url && ng.isObject(config.cache) && config.cache.get(urlOld) != null) {
                            template = config.cache.get(urlOld);
                            config.cache.remove(urlOld);
                            config.cache.put(config.url, template);
                        }

                        //config.headers['Pragma'] = 'no-cache';
                        //config.headers['Expires'] = '-1';
                        //config.headers['Cache-Control'] = 'no-cache, no-store';

                        return config;
                    }
                };
            }]);

            //#endregion

            /* Прописано для # в URL вместо /#/ */
            $locationProvider.html5Mode({
                enabled: true,
                requireBase: true,
                rewriteLinks: false
            });

            $locationProvider.hashPrefix('#');

            //#region localization

            var localeId = $localeProvider.$get().id;
            $translateProvider
              .translations(localeId, window.AdvantshopResource)
              .preferredLanguage(localeId)
              .useSanitizeValueStrategy('sanitizeParameters');
            //#endregion
        }])
        .run(['$cookies', '$timeout', 'toaster', 'modalService', function ($cookies, $timeout, toaster, modalService) {

            //var timerScroll;

            //window.addEventListener('scroll', function () {

            //    clearTimeout(timerScroll);

            //    if (!document.body.classList.contains('disable-hover')) {
            //        document.body.classList.add('disable-hover');
            //    }

            //    timerScroll = setTimeout(function () {
            //        document.body.classList.remove('disable-hover');
            //    }, 500);
            //}, false);

            //#endregion

            if ($cookies.get('zonePopoverVisible') == null || $cookies.get('zonePopoverVisible').length === 0) {
                modalService.stopWorking();
            }

            //window.addEventListener('load', function load() {

            //window.removeEventListener('load', load);

            var toasterContainer = document.querySelector('[data-toaster-container]'),
                toasterItems,
                linkWithAnchors = document.querySelectorAll('a[href*="#"]'),
                linkHref,
                linkHashIndex;


            $timeout(function () {
                if (toasterContainer != null) {
                    toasterItems = document.querySelectorAll('[data-toaster-type]');
                    if (toasterItems != null) {
                        for (var i = 0, len = toasterItems.length; i < len; i++) {
                            toaster.pop({
                                type: toasterItems[i].getAttribute('data-toaster-type'),
                                body: toasterItems[i].innerHTML,
                                bodyOutputType: 'trustedHtml'
                            });
                        }
                    }
                }
            })

            //old style using anchor
            //if (linkWithAnchors.length > 0) {
            //    for (var i = 0, len = linkWithAnchors.length; i < len; i += 1) {

            //        linkWithAnchors[i].setAttribute('target', '_self');
            //        linkHref = linkWithAnchors[i].getAttribute('href');
            //        linkHashIndex = linkHref.indexOf('#');

            //        //if (linkHref.charAt(linkHashIndex + 1) !== '#' && linkHref.charAt(linkHashIndex + 1) !== '?') {
            //        //    linkWithAnchors[i].setAttribute('href', [linkHref.slice(0, linkHashIndex), '#', linkHref.slice(linkHashIndex)].join(''));
            //        //}

            //        if (linkHashIndex !== -1) {
            //            linkWithAnchors[i].addEventListener('click', function () {
            //                var href = this.href,
            //                    index = href.indexOf('#'),
            //                    name = href.substring(index + 1),
            //                    goalByName = document.querySelector('[name="' + name + '"]'),
            //                    goalById = document.getElementById(name);

            //                if (goalByName != null) {
            //                    goalByName.scrollIntoView();
            //                } else if (goalById != null) {
            //                    goalById.scrollIntoView();
            //                }
            //            });
            //        }
            //    }
            //}

            //})
        }])
        .controller('AppCtrl', function () {
        })
        .filter("sanitize", ['$sce', function ($sce) {
            return function (htmlCode) {
                return $sce.trustAsHtml(htmlCode);
            }
        }]);

})(window.angular);