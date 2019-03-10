; (function (ng) {
    'use strict';

    var extService = function () {
        var service = this;

        service.defaultOptionsFill = function (defaultOptions, attrs, element) {
            var list = {};

            for (var key in defaultOptions) {
                if (defaultOptions.hasOwnProperty(key) && ng.isUndefined(attrs[key]) && ng.isDefined(defaultOptions[key])) {
                    list[key] = defaultOptions[key];

                    if (ng.isDefined(element)) {
                        attrs.$set(key, defaultOptions[key]);
                    }
                }
            }
        };
    };


    ng.module('ext')
      .service('extService', extService);

})(window.angular);