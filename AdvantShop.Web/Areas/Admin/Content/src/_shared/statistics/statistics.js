; (function (ng) {
    'use strict';

    var lastStatisticsCtrl = function ($http, $timeout, $sce, lastStatisticsService) {

        var ctrl = this;
        
        ctrl.getValue = function () {
            var count = 0;
            ctrl.data = lastStatisticsService.getData();

            if (ctrl.data != null) {
                switch (ctrl.type) {
                    case 'orders':
                        count = ctrl.data.LastOrdersCount;
                        break;
                    case 'reviews':
                        count = ctrl.data.LastReviews;
                        break;
                    case 'leads':
                        count = ctrl.data.LastLeadsCount;
                        break;
                    case 'tasks':
                        count = ctrl.data.LastTasksCount;
                        break;
                    case 'academy':
                        count = ctrl.data.RemainingLessons;
                        break;
                }
            }
            return count != 0 && count != "undefined" ? $sce.trustAsHtml('<span class="new-item">' + (count <= 99 ? count : '99+') + '</span>') : '';
        };
    };

    lastStatisticsCtrl.$inject = ['$http', '$timeout', '$sce', 'lastStatisticsService'];

    ng.module('statistics', [])
      .controller('lastStatisticsCtrl', lastStatisticsCtrl);


    ng.module('statistics')
    .directive('statisticsCount', ['$sce', function ($sce) {
        return {
            restrict: 'A',
            scope: true,
            controller: 'lastStatisticsCtrl',
            controllerAs: 'statisticsCount',
            bindToController: true,
            link: function (scope, element, attrs, ctrl, transclude) {
                ctrl.type = attrs.type;
            }
        };
    }]);

})(window.angular);