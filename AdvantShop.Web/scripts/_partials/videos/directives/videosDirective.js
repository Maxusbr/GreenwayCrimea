; (function (ng) {
    'use strict';

    ng.module('videos')
      .directive('videos', function () {
          return {
              restrict: 'A',
              controller: 'VideosCtrl',
              controllerAs: 'videos',
              scope: {
                  productId: '@',
              },
              bindToController: true,
              replace: true,
              templateUrl: '/scripts/_partials/videos/templates/videosTemplate.html',
          };
      });

})(window.angular);