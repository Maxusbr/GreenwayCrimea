; (function (ng) {
    'use strict';

    var NewsItemCtrl = function ($http, $window, SweetAlert) {

        var ctrl = this;
        ctrl.PhotoId = 0;
        
        ctrl.deleteNewsItem = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('News/DeleteNewsItem', { newsId: id }).then(function (response) {
                        $window.location.assign('news');
                    });
                }
            });
        }

        ctrl.changePhoto = function (result) {
            ctrl.PhotoId = result.pictureId;
        }
    };

    NewsItemCtrl.$inject = ['$http', '$window', 'SweetAlert'];

    ng.module('newsItem', ['uiGridCustom', 'urlGenerator', 'newsProducts'])
      .controller('NewsItemCtrl', NewsItemCtrl);

})(window.angular);