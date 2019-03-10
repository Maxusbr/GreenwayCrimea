; (function (ng) {
    'use strict';

    var dependencyList = [
                    //'templatesCache',
                    'pascalprecht.translate',
                    'ngCookies',
                    'ngFileUpload',
                    'ui.mask',
                    'ngSanitize',
                    'angular-bind-html-compile',
                    'angular-cache',
                    'angular-ladda',
                    'address',
                    'autocompleter',
                    'autofocus',
                    'bonus',
                    'builder',
                    'buyOneClick',
                    'cards',
                    'cart',
                    'carousel',
                    'catalogFilter',
                    'checkOrder',
                    'cookiesPolicy',
                    'colorsViewer',
                    'compare',
                    'countdown',
                    'currency',
                    'customOptions',
                    'defaultButton',
                    'demo',
                    'dom',
                    'ext',
                    'harmonica',
                    'input',
                    'urlHelper',
                    'mobileOverlap',
                    'modal',
                    'module',
                    'oc.lazyLoad',
                    'order',
                    'payment',
                    'popover',
                    'productView',
                    'productsCarousel',
                    'rotate',
                    'quickview',
                    'rating',
                    'readmore',
                    'reviews',
                    'videos',
                    'shipping',
                    'sizesViewer',
                    'rootMenu',
                    'select',
                    'submenu',
                    'scrollToTop',
                    'spinbox',
                    'subscribe',
                    'tabs',
                    'telephony',
                    'toaster',
                    'transformer',
                    'ui-rangeSlider',
                    'ui.bootstrap.datetimepicker',
                    'voting',
                    'windowExt',
                    'wishlist',
                    'zone',
                    'zoomer',
                    'magnificPopup',
                    'mouseoverClassToggler',
                    'validation',
                    'internationalPhoneNumber',
                    
                    //controllers of pages
                    'auth',
                    'catalog',
                    'checkout',
                    'checkoutSuccess',
                    'product',
                    'feedback',
                    'home',
                    'billing',
                    'brand',
                    'managers',
                    'myaccount',
                    'preorder',
                    'search',
                    'giftcertificate',
                    'wishlistPage',
                    'ui.bootstrap.popover',
                    'ui.bootstrap'
                    ];

    var dependencyService = function () {
        var service = this;

        //service.add = function (items) {
        //    if (ng.isArray(items) === true) {
        //        dependencyList = dependencyList.concat(items);
        //    } else {
        //        dependencyList.push(items);
        //    }
        //};

        service.get = function () {
            return dependencyList;
        };
    };


    ng.module('dependency', [])
      .service('dependencyService', dependencyService);


})(window.angular);