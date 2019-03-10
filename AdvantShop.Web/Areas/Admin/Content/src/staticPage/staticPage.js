; (function (ng) {
    'use strict';

    var StaticPageCtrl = function ($http, $window, SweetAlert) {

        var ctrl = this;
        
        ctrl.deleteStaticPage = function (id) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('StaticPages/DeleteStaticPage', { staticPageId: id }).then(function (response) {
                        $window.location.assign('staticpages');
                    });
                }
            });
        }

        ctrl.changePage = function (result) {
            ctrl.parentId = result.staticPageId;
            ctrl.parentPageName = result.pageName;
        }
    };

    StaticPageCtrl.$inject = ['$http', '$window', 'SweetAlert'];

    ng.module('staticPage', ['uiGridCustom', 'urlGenerator'])
      .controller('StaticPageCtrl', StaticPageCtrl);

})(window.angular);