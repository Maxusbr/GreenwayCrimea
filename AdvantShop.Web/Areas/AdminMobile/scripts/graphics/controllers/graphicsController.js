; (function (ng) {

    'use strict';

    var GraphicsCtrl = function ($http) {
        var ctrl = this;

        ctrl.updateGraph = function (newDate, oldDate) {
            $http.get("/adminmobile/getgraphics", {
                params:
                {
                    dateFrom: ctrl.dateFrom,
                    dateTo: ctrl.dateTo,
                    paid: ctrl.paid
                }
            }).then(function (response) {
                if (response.data != null) {
                    $("#chartWeek").attr("data-chart", response.data.DataChart);
                    $("#chartWeek").attr("data-chart-options", "{xaxis : {mode: 'time',timeformat: '%d %b',min: " + response.data.Min + ",max: " + response.data.Max + "}}");

                    //var date = new Date();

                    //ctrl.dateFrom = date.setTime(response.data.Min);
                    //ctrl.dateTo = date.setTime(response.data.Max);

                    window.chart.prototype.InitTotal();
                }
            });
        };

    };

    ng.module('graphics')
      .controller('GraphicsCtrl', GraphicsCtrl);

    GraphicsCtrl.$inject = ['$http'];

})(window.angular);