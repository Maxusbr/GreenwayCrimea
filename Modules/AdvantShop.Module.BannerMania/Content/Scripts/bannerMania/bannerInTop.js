; (function (ng) {
    'use strict';

    var bannerInTopCtrl = function ($http, toaster, $uibModal, SweetAlert) {
        var ctrl = this;

        ctrl.OnlyOnMainPage = false;
        ctrl.TargetBlank = true;
        ctrl.URL = "";
        ctrl.ImagePath = "";

        $http.get('../bitadmin/getsettings').then(function success(response) {
            ctrl.OnlyOnMainPage = response.data.OnlyOnMainPage;
            ctrl.TargetBlank = response.data.TargetBlank;
            ctrl.URL = response.data.URL;
            ctrl.ImagePath = response.data.ImagePath;
        });

        ctrl.saveChanges = function (obj) {
            $http.post('../bitadmin/saveChanges',
                { OnlyOnMainPage: ctrl.OnlyOnMainPage, TargetBlank: ctrl.TargetBlank, URL: ctrl.URL })
                .then(function success(response) {
                    if (response.data.success) {
                        toaster.pop('success', '', response.data.msg);
                    } else {
                        toaster.pop('error', '', response.data.msg);
                    }
                })
        }

        ctrl.selectFile = function ($files, $file, $newFiles, $duplicateFiles, $invalidFiles, $event) {
            if ($file == null) return false;
            var fData = new FormData();
            fData.append("imageFile", $file);
            var req = {
                method: 'POST',
                url: '../bitadmin/uploadimage',
                headers: {
                    'Content-Type': undefined
                },
                data: fData
            }

            $http(req).then(function success(response) {
                if (!response.data.success) {
                    toaster.pop('error', '', response.data.msg);
                    return false;
                }

                ctrl.ImagePath = response.data.newImagePath;
                toaster.pop('success', '', response.data.msg);
                return true;
            })
        }

        ctrl.deleteImage = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить изображение?", { title: "Удаление изображения" }).then(function (result) {
                if (result === true) {
                    $http.post('../bitadmin/deleteImage')
                        .then(function success(response) {
                            if (response.data.success) {
                                toaster.pop('success', '', response.data.msg);
                                ctrl.ImagePath = response.data.newImagePath;
                            } else {
                                toaster.pop('error', '', response.data.msg);
                            }
                        })
                }
            });
        }
    };

    bannerInTopCtrl.$inject = ['$http', 'toaster', '$uibModal', 'SweetAlert'];

    ng.module('bannerInTop', [])
        .controller('bannerInTopCtrl', bannerInTopCtrl)
        .component('bannerInTop', {
            templateUrl: '../modules/BannerMania/content/Scripts/bannerMania/templates/bannerInTop.html',
            controller: 'bannerInTopCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);