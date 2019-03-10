; (function (ng) {
    'use strict';

    angular.module('urlGenerator', [])
        .directive('urlGenerator', ['$http', function ($http) {
            return {
                restrict: 'A',
                scope: {
                    urlPath: '=',
                    urlGeneratorEnabled: '<?'
                },
                link: function (scope, element, attrs) {
                    
                    var timer;

                    if (scope.urlGeneratorEnabled) {
                        element.bind('keyup', function () {
                            var url = scope.urlPath;
                            var name = element[0].value;

                            if (timer != null) {
                                clearTimeout(timer);
                            }

                            setTimeout(function () {
                                $http.get('common/generateUrl?name=' + name).then(function (response) {
                                    scope.urlPath = response.data;
                                });
                            }, 500)
                        });
                    }

                }
            };
        }]);

})(window.angular);