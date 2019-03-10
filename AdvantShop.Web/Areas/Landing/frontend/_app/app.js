; (function (ng) {
    'use strict';

    ng.module('app',
        [
            'ngSanitize',
            'blocksConstructor',
            'buyOneClick',
            'countdown',
            'dom',
            'inplaceLanding',
            'modal',
            'ngFileUpload',
            'oc.lazyLoad',
            'pascalprecht.translate',
            'subblockInplace',
            'toaster',
            'validation',
            'tabs',
            'urlHelper',
            'spinbox'
        ])

        .config(['$localeProvider', '$translateProvider', function ($localeProvider, $translateProvider) {

            var localeId = $localeProvider.$get().id;

            $translateProvider
                .translations(localeId, window.AdvantshopResource)
                .preferredLanguage(localeId)
                .useSanitizeValueStrategy('sanitizeParameters');
        }
        ])
    .filter('sanitize', ['$sce', function ($sce) {
        return function (htmlCode) {
            return $sce.trustAsHtml(htmlCode);
        }
    }]);

})(window.angular);