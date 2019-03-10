; (function (ng) {
    'use strict';

    var VideosCtrl = function($http, $sce) {

        var ctrl = this;

        ctrl.getVideos = function() {
            return $http.get('product/getvideos', { params: { productId: ctrl.productId } }).then(function(response) {
                ctrl.videos = response.data;

                for (var i = 0; i < ctrl.videos.length; i++) {
                    ctrl.videos[i].PlayerCode = $sce.trustAsHtml(ctrl.videos[i].PlayerCode);
                }

                return response.data;
            });
        };
        ctrl.getVideos();
    };

    ng.module('videos')
      .controller('VideosCtrl', VideosCtrl);

    VideosCtrl.$inject = ['$http', '$sce'];

})(window.angular);