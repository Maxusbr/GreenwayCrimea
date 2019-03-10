; (function (ng) {
    'use strict';

    var vkMarketAuthCtrl = function ($http, toaster, vkMarketService) {
        var ctrl = this;

        ctrl.$onInit = function () {
        };


        // Авторизация в vk с правами пользователя, чтобы плучить список групп
        ctrl.authVk = function() {
            var w = 700;
            var h = 525;

            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);

            var url = 'https://oauth.vk.com/authorize?client_id=' + ctrl.clientId +
                '&display=page' +
                '&redirect_uri=' + ctrl.redirectUrl +
                '&scope=offline,market,photos,groups&response_type=token&v=5.64';

            var win = window.open(url, '', 'width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            win.focus();

            var timer = window.setInterval(function () {
                try {
                    if (win.document.URL.indexOf(ctrl.redirectUrl) !== -1) {

                        var accessToken = '';
                        var userId = '';
                        var urlParts = win.document.URL.split('#')[1].split('&');

                        for (var i = 0; i < urlParts.length; i++) {

                            if (urlParts[i].indexOf('access_token') !== -1) {
                                accessToken = urlParts[i].replace('access_token=', '');
                            } else if (urlParts[i].indexOf('user_id') !== -1) {
                                userId = urlParts[i].replace('user_id=', '');
                            }
                        }

                        win.close();
                        window.clearInterval(timer);

                        return vkMarketService
                            .saveAuth({ clientId: ctrl.clientId, accessToken: accessToken, userId: userId })
                            .then(function(data) {

                                ctrl.clienId = data.clientId;
                                ctrl.accessToken = data.accessToken;
                                ctrl.userId = data.userId;

                                vkMarketService.getGroups().then(function (groups) {
                                    ctrl.groups = groups;
                                    if (ctrl.groups != null && ctrl.groups.length > 0) {
                                        ctrl.selectedGroup = ctrl.groups[0];
                                        ctrl.selectGroup();
                                    }
                                });
                            });
                    }

                } catch (e) {
                    console.log(e);
                }

            }, 100);
        }

        ctrl.selectGroup = function () {
            vkMarketService.saveGroup(ctrl.selectedGroup)
                .then(function(data) {
                    ctrl.groupSettings = data;
                });
        }


        ctrl.deleteToken = function() {
            vkMarketService.deleteGroup().then(ctrl.getSettings);
        }

        ctrl.goNextStep = function() {
            ctrl.onUpdate();
        }

    };

    vkMarketAuthCtrl.$inject = ['$http', 'toaster', 'vkMarketService'];

    ng.module('vkMarketAuth', [])
        .controller('vkMarketAuthCtrl', vkMarketAuthCtrl)
        .component('vkMarketAuth', {
            templateUrl: '../modules/vkmarket/content/js/vkMarketAuth/vkMarketAuth.html',
            controller: 'vkMarketAuthCtrl',
            bindings: {
                redirectUrl: '=',
                clientId: '=',
                onUpdate: '&'
            }
        });

})(window.angular);