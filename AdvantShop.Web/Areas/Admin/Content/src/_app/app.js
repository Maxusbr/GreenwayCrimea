; (function (ng) {
    'use strict';

    /*убираем BFCache в FF*/
    window.addEventListener('unload', function () { });

    window.ajaxInProcess = [];

    ng.module('app', [
        'activity',
        //'adminTemplatesCache',
        'adminComments',
        'adminCommentsForm',
        'adminCommentsItem',
        'adminWebNotifications',
        'analytics',
        'analyticsFilter',
        'analyticsReport',
        'angular-bind-html-compile',
        'angular-ladda',
        'angular-web-notification',
        'angularMoment',
        'answersList',
        'audioPlayer',
        'autocompleter',
        'autofocus',
        'autosaveText',
        'brand',
        'brandsList',
        'buttonSave',
        'calls',
        'cards',
        'catalog',
        'carousel',
        'carouselPage',
        'category',
        'catProductRecommendations',
        'certificates',
        'chart.js', 
        'changeAdminShopName',
        'collapseTab',
        'color.picker',
        'colors',
        'coupons',
        'countdown',
        'cmStat',
        'csseditor',
        'customergroups',
        'customer',
        'customers',
        'customerSegment',
        'customerSegments',
        'design',
        'discountsPriceRange',
        'duScroll',
        'exportCategories',
        'exportfeeds',
        'files',
        'fileUploader',
        'grades',
        'helpTrigger',
        'home',
        'iconMove',
        'import',
        'input',
        'inputGhost',
        'internationalPhoneNumber',
        'landings',
        'landingPages',
        'lead',
		'leads',
        'mailSettings',
        'mainpageproducts',
        'menus',
        'module',
        'modules',
        'kanban',
        'ngCkeditor',
        'ngCookies',
        'ngInputModified',
        'ngSanitize',
        'ngTextcomplete',
        'news',
        'newsItem',
        'newsCategory',
        'oc.lazyLoad',
        'order',
        'orders',
        'ordersources',
        'orderstatuses',
        'pascalprecht.translate',
        'paymentMethod',
        'paymentMethodsList',
        'personAvatar',
        'pictureUploader',
        'priceregulation',
        'product',
        'productlists',
        'properties',
        'propertyvalues',
        'rcrumbs',
        'recalc',
        'reviews',
        'rules',
        'saasStat',
        'sidebarUser',
        'shippingMethod',
        'shippingMethodsList',
        'staticBlock',
        'staticPage',
        'staticPages',
        'search',
        'select',
        'settings',
        'settingsBonus',
        'settingsCatalog',
        'settingsCheckout',
        'settingsCrm',
        'settingsCustomers',
        'settingsNews',
        'settingsUsers',
        'settingsSearch',
        'settingsSeo',
        'settingsSocial',
        'settingsSystem',
        'settingsTasks',
        'settingsTelephony',
        //'SignalR',
        'sizes',
        'smstemplates',
        'statistics',
        'sticky',
        'switcherState',
        'submenu',
        'subscription',
        'tags',
        'tariffs',
        'taskgroups',
        'tasks',
        'tasksGrid',
        'toaster',
        'topPanelUser',
        'transformer',
        'ui.bootstrap',
        'ui.bootstrap.datetimepicker',
        'ui.mask',
        'ui.select',
        'uiAceTextarea',
        'uiModal',
        'urlHelper',
        'userInfoPopup',
        'validation',
        'vkMessages',
        'voting',
        'windowExt',
        //'uiGridCustom',
        'modal'
    ])
        .value('duScrollBottomSpy', true)
        .config(['$provide', '$compileProvider', '$cookiesProvider', '$httpProvider', '$translateProvider', 'urlHelperConfig', 'toasterConfig', function ($provide, $compileProvider, $cookiesProvider, $httpProvider, $translateProvider, urlHelperConfig, toasterConfig) { //, $localeProvider, $translateProvider) {
            var date = new Date(),
                currentYear = date.getFullYear();

            //#region Disable comment and css class directives
            $compileProvider.commentDirectivesEnabled(false);
            //$compileProvider.cssClassDirectivesEnabled(false); не использовать так как падает chart.js
            //#endregion

            date.setFullYear(currentYear + 1);

            //#region compile debug
            $compileProvider.debugInfoEnabled(false);
            //#endregion

            // enable if need to use unsave protocols
            //$compileProvider.aHrefSanitizationWhitelist(/^\s*(http|https|ftp|mailto|callto|tel):/);

            //#region set cookie expires
            $cookiesProvider.defaults.expires = date;
            $cookiesProvider.defaults.path = '/';

            if (window.location.hostname !== 'localhost' && window.location.hostname !== 'server' &&
               !/^(?!0)(?!.*\.$)((1?\d?\d|25[0-5]|2[0-4]\d)(\.|$)){4}$/.test(window.location.hostname)) {
                $cookiesProvider.defaults.domain = '.' + window.location.hostname.replace('www.', '');
            }

            //#endregion

            //закоментировал, т.к. в FF не работает валидация input[type="number"]
            //#region ie10 bug validation

            //$provide.decorator('$sniffer', ['$delegate', function ($sniffer) {
            //    var msie = parseInt((/msie (\d+)/.exec(angular.lowercase(navigator.userAgent)) || [])[1], 10);
            //    var _hasEvent = $sniffer.hasEvent;
            //    $sniffer.hasEvent = function (event) {
            //        if (event === 'input' && msie === 10) {
            //            return false;
            //        }
            //        _hasEvent.call(this, event);
            //    }
            //    return $sniffer;
            //}]);

            //#endregion

            //#region prepera ajax url in absolute path

            var basePath = document.getElementsByTagName('base')[0].getAttribute('href'),
                regex = new RegExp('^(?:[a-z]+:)?//', 'i');


            $httpProvider.useApplyAsync(true);

            var tokens = document.getElementsByName("__RequestVerificationToken");
            if (tokens.length > 0) {
                $httpProvider.defaults.headers.post['__RequestVerificationToken'] = tokens[0].value;
            }
            $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';

            $httpProvider.interceptors.push(['$q', 'urlHelper', function ($q, urlHelper) {
                return {
                    'request': function (config) {

                        var urlOld = config.url,
                            template;

                        config.url = urlHelper.getAbsUrl(config.url);

                        //for templates
                        if (urlOld != config.url && ng.isObject(config.cache) && config.cache.get(urlOld) != null) {
                            template = config.cache.get(urlOld);
                            config.cache.remove(urlOld);
                            config.cache.put(config.url, template);
                        }

                        config.executingAjax = {
                            url: config.url,
                            params: config.params
                        };

                        window.ajaxInProcess.push(config.executingAjax);

                        return config;
                    },
                    // optional method
                    'response': function (response) {

                        var index = window.ajaxInProcess.indexOf(response.config.executingAjax);

                        window.ajaxInProcess.splice(index, 1);

                        return response;
                    },

                    // optional method
                    'responseError': function (rejection) {

                        var index = ajaxInProcess.indexOf(rejection.config.executingAjax);

                        ajaxInProcess.splice(index, 1);

                        return $q.reject(rejection);
                    }
                };
            }]);

            //#endregion

            //#region localization

            var localeId = "ru-ru"; //$localeProvider.$get().id;
            $translateProvider
              .translations(localeId, window.AdvantshopResource || {})
              .preferredLanguage(localeId)
              .useSanitizeValueStrategy('sanitizeParameters');

            //#endregion

            urlHelperConfig.isAdmin = true;


            toasterConfig['icon-classes'].call = 'toast-call';

        }])
        .run(['toaster', 'adminWebNotificationsService', 'dateTimePickerConfig', function (toaster, adminWebNotificationsService, dateTimePickerConfig) {

            dateTimePickerConfig.modelType = 'YYYY-MM-DDTHH:mm:ss';

            window.addEventListener('load', function load() {

                window.removeEventListener('load', load);

                var toasterContainer = document.querySelector('[data-toaster-container]'),
                    toasterItems,
                    linkWithAnchors = document.querySelectorAll('a[href*="#"]');

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

                //old style using anchor
                if (linkWithAnchors.length > 0) {
                    for (var i = 0, len = linkWithAnchors.length; i < len; i += 1) {
                        linkWithAnchors[i].setAttribute('target', '_self');
                    }
                }

                sweetAlert.setDefaults({
                    showLoaderOnConfirm: true,
                    cancelButtonText: 'Отмена',
                    confirmButtonText: 'ОK',
                    allowOutsideClick: false,
                    buttonsStyling: false,
                    confirmButtonClass: 'btn btn-sm btn-success',
                    cancelButtonClass: 'btn btn-sm btn-action'
                });

                adminWebNotificationsService.onPageLoad();
            });

        }])
        .filter("sanitize", ['$sce', function ($sce) {
            return function (htmlCode) {
                return $sce.trustAsHtml(htmlCode);
            }
        }])
        .filter('nl2br', ['$sanitize', function ($sanitize) {
            var tag = '<br />';
            return function (msg) {
                if (!msg) return '';
                msg = (msg + '').replace(/(\r\n|\n\r|\r|\n|&#10;&#13;|&#13;&#10;|&#10;|&#13;)/g, tag + '$1');
                return $sanitize(msg);
            };
        }])
        .controller('AppCtrl', function () {

        });

    window.ajaxIsComplete = function () {
        return window.ajaxInProcess.length == 0;
    };

})(window.angular);