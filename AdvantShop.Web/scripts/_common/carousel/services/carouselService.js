; (function (ng) {
    'use strict';

    var carouselService = function ($q) {
        var service = this;

        service.waitLoadImages = function (images) {
            var deferMain = $q.defer(),
                promises = [],
                img;

            for (var i = images.length - 1; i >= 0; i--) {

                img = images[i];

                if (!img.complete || (typeof (img.naturalWidth) !== "undefined" || img.naturalWidth === 0)) {

                    var deferItem = $q.defer(),
                        imageFake = new Image();

                    promises.push(deferItem.promise);

                    (function (defer) {

                        imageFake.onload = function () {
                            defer.resolve(true);
                        };

                        imageFake.onerror = function () {
                            defer.resolve();
                        };
                    })(deferItem);

                    imageFake.src = img.src;
                }
            }

            if (promises.length === 0) {
                promises.push(deferMain.promise);
                deferMain.resolve();
            }

            return $q.all(promises);
        };
    };

    ng.module('carousel')
      .service('carouselService', carouselService);

    carouselService.$inject = ['$q'];

})(window.angular);