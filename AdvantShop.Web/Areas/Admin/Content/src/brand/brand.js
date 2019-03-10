; (function (ng) {
    'use strict';

    var BrandCtrl = function ($http, $window, SweetAlert) {

        var ctrl = this;
        ctrl.PhotoId = 0;

        ctrl.updateImage = function (result) {
            ctrl.PhotoId = result.pictureId;
        };

        ctrl.deleteBrand = function (brandId) {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result === true) {
                    $http.post('brands/deleteBrand', { brandId: brandId }).then(function (response) {
                        $window.location.assign('brands');
                    });
                }
            });
        }
    };

    BrandCtrl.$inject = ['$http', '$window', 'SweetAlert'];

    ng.module('brand', ['uiGridCustom', 'urlGenerator'])
      .controller('BrandCtrl', BrandCtrl);

})(window.angular);