; (function (ng) {
    'use strict';

    var vkMarketCtrl = function ($http, toaster, vkMarketService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.getAuthSettings();
        };

        ctrl.getAuthSettings = function () {
            vkMarketService.getAuthSettings().then(function (data) {
                ctrl.isActive = data.IsActive;
                ctrl.clientId = data.ApplicationId;
            });
        }

        ctrl.export = function () {
            vkMarketService.export().then(function (data) {
                if (data.result === true) {
                    toaster.pop('success', '', 'Начался перенос товаров в ВКонткате. Длительность переноса зависит от кол-ва товаров и фотографий.');
                } else {
                    data.errors.forEach(function (e) {
                        toaster.pop('error', '', e);
                    });
                }
            });
        }
        
    };

    vkMarketCtrl.$inject = ['$http', 'toaster', 'vkMarketService'];

    ng.module('vkMarket', [])
        .controller('vkMarketCtrl', vkMarketCtrl)
        .component('vkMarket', {
            templateUrl: '../modules/vkmarket/content/js/vkMarket/vkMarket.html',
            controller: 'vkMarketCtrl',
            bindings: {
                redirectUrl: '=',
            }
        });

})(window.angular);