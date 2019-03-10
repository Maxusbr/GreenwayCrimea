; (function (ng) {
    'use strict';
    
    var lastStatisticsService = function ($document, $http) {
        var service = this,
            timer,
            data = null,
            documentIsVisible = $document[0].visibilityState === 'visible';

        service.getLastStatistics = function () {

            if (documentIsVisible === true) {
                $http.get('common/getlaststatistics', { params: { rnd: Math.random() } }).then(function (response) {

                    data = response.data;
                    if (timer != null) {
                        clearTimeout(timer);
                    }
                    timer = setTimeout(function () {
                        service.getLastStatistics();
                    }, 30 * 1000);

                });
            }
        }

        //setTimeout(function () {
            service.getLastStatistics();
       // }, 1000);

        service.getData = function() {
            return data;
        };


        $document.on("visibilitychange", function () {
            documentIsVisible = $document[0].visibilityState === 'visible';

            if (documentIsVisible === true) {
                service.getLastStatistics();
            }
        });
    };

    lastStatisticsService.$inject = ['$document', '$http'];

    ng.module('statistics')
        .service('lastStatisticsService', lastStatisticsService);



})(window.angular);