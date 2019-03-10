; (function (ng, body) {

    'use strict';

    /*убираем BFCache в FF*/
    window.addEventListener('unload', function () { });

    ng.module('app', [
                      //'templatesCache',
                      'pascalprecht.translate',
                      'ngCookies',
                      'angular-cache',
                      'searchPanel',
					  'angular-bind-html-compile',
                      'autocompleter',
                      'catalogFilterMobile',
                      'citySelector',
                      'sidebar',
                      'selectCatalog',
                      'inputSearch',
                      'uiHelper',
                      'ui.mask',
                      //'demo',
                      //from full version
                      'oc.lazyLoad',
                      'sizesViewer',
                      'colorsViewer',
                      'cookiesPolicy',
                      'shipping',
                      'carousel',
                      'bonus',
                      'cart',
                      'dom',
                      'windowExt',
                      'catalogFilter',
                      'rating',
                      'subscribe',
                      'submenu',
                      'countdown',
                      'validation',
                      'videos',
                      'tabs',
                      'urlHelper',
                      //vendors
                      'angular-ladda',
                      'cards',
                      'ngSanitize',
                      'zone',
                      'magnificPopup',
                      'modal',
                      'toaster',
                      'customOptions',
                      'ngFileUpload',
                      'checkout',
                      'checkoutSuccess',
                      'payment',
                      'reviews',
                      'home',
                      'address',
                      'input',
                      'auth',
                      'product',
                      'brand',
                      'preorder',
                      'feedback',
                      'catalogFilter',
                      'popover',
                      'ui-rangeSlider',
                      'toaster',
                      'readmore',
                      'spinbox',
                      'billing'

    ])
        .config(['$provide', '$compileProvider', '$cookiesProvider', '$httpProvider', '$localeProvider', '$translateProvider', '$locationProvider', function ($provide, $compileProvider, $cookiesProvider, $httpProvider, $localeProvider, $translateProvider, $locationProvider) {

            var date = new Date(),
                currentYear = date.getFullYear();

            date.setFullYear(currentYear + 1);

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
            var basePath = document.getElementsByTagName('base')[0].getAttribute('href'),
                regex = new RegExp('^(?:[a-z]+:)?//', 'i');


            $httpProvider.useApplyAsync(true);

            $httpProvider.interceptors.push(function () {
                return {
                    'request': function (config) {

                        var urlOld = config.url,
                            template;

                        if (regex.test(config.url) === false) {

                            if (config.url.charAt(0) === '/') {
                                config.url = config.url.substring(1);
                            }

                            config.url = basePath + config.url;
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
            });

            //#endregion

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
    .run(function () {

        var timerScroll,
            linkWithAnchors = document.querySelectorAll('a[href*="#"]'),
            linkHref,
            linkHashIndex;

        window.addEventListener('scroll', function () {

            clearTimeout(timerScroll);

            if (!body.classList.contains('disable-hover')) {
                body.classList.add('disable-hover');
            }

            timerScroll = setTimeout(function () {
                body.classList.remove('disable-hover');
            }, 500);
        }, false);

        materialClickInit();

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
    })
        .controller('AppCtrl', function () {
        })
        .filter("sanitize", ['$sce', function ($sce) {
            return function (htmlCode) {
                return $sce.trustAsHtml(htmlCode);
            }
        }]);

})(window.angular, document.body);


