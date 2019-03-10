; (function (ng) {
    'use strict';

    var vkAuthCtrl = function ($http, toaster, SweetAlert, vkAuthService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.getSettings();
        };

        ctrl.getSettings = function() {
            vkAuthService.getVkSettings().then(function (data) {
                ctrl.clientId = data.clientId;
                ctrl.groups = data.groups;
                if (ctrl.groups != null && ctrl.groups.length > 0) {
                    ctrl.selectedGroup = ctrl.groups[0];
                }

                ctrl.groupId = data.group != null ? data.group.Id : null;
                ctrl.groupName = data.group != null ? data.group.Name : null;
                ctrl.groupScreenName = data.group != null ? data.group.ScreenName : null;
            });
        }

        // Авторизация в vk с правами пользователя, чтобы плучить список групп
        ctrl.authVk = function() {
            var w = 700;
            var h = 525;

            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);

            var url = 'https://oauth.vk.com/authorize?client_id=' + ctrl.clientId +
                '&display=page' +
                '&redirect_uri=' + ctrl.redirectUrl +
                '&scope=offline,groups&response_type=token&v=5.64';

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

                        return vkAuthService
                            .saveAuthVkUser({ clientId: ctrl.clientId, accessToken: accessToken, userId: userId })
                            .then(function(data) {

                                ctrl.clienId = data.clientId;
                                ctrl.accessToken = data.accessToken;
                                ctrl.userId = data.userId;

                                vkAuthService.getGroups().then(function (groups) {
                                    ctrl.groups = groups;
                                    if (ctrl.groups != null && ctrl.groups.length > 0) {
                                        ctrl.selectedGroup = ctrl.groups[0];
                                    }
                                });
                            });
                    }

                } catch (e) {
                    console.log(e);
                }

            }, 100);
        }

        // Авторизация в vk с правами пользователя, чтобы плучить список групп
        ctrl.authGroup = function () {

            if (ctrl.selectedGroup == null) {
                return;
            }

            var group = ctrl.selectedGroup;

            var w = 700;
            var h = 525;

            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);

            var url = 'https://oauth.vk.com/authorize?client_id=' + ctrl.clientId +
                '&group_ids=' + group.Id +
                '&display=page' +
                '&redirect_uri=' + ctrl.redirectUrl +
                '&scope=messages,manage&response_type=token&v=5.64';

            var win = window.open(url, '', 'width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            win.focus();

            var timer = window.setInterval(function () {
                try {
                    if (win.document.URL.indexOf(ctrl.redirectUrl) !== -1) {

                        var accessToken = '';
                        var urlParts = win.document.URL.split('#')[1].split('&');

                        for (var i = 0; i < urlParts.length; i++) {
                            if (urlParts[i].indexOf('access_token') !== -1) {
                                accessToken = urlParts[i].split('=')[1];
                            }
                        }

                        win.close();
                        window.clearInterval(timer);

                        return vkAuthService
                            .saveAuthVkGroup({ group: group, accessToken: accessToken })
                            .then(function (data) {
                                if (data.result === true) {
                                    ctrl.groupId = group.Id;
                                    ctrl.groupName = group.Name;
                                    ctrl.groupScreenName = group.ScreenName;
                                    toaster.pop('success', '', 'Группа ' + group.Name + ' подключена. Настройка завершена.');
                                }
                            });
                    }

                } catch (e) {
                    console.log(e);
                }

            }, 100);
        }

        ctrl.deleteGroup = function() {
            vkAuthService.deleteGroup().then(ctrl.getSettings);
        }

    };

    vkAuthCtrl.$inject = ['$http', 'toaster', 'SweetAlert', 'vkAuthService'];

    ng.module('vkAuth', [])
        .controller('vkAuthCtrl', vkAuthCtrl)
        .component('vkAuth', {
            templateUrl: '../areas/admin/content/src/settingsCrm/components/vkAuth/vkAuth.html',
            controller: 'vkAuthCtrl',
            bindings: {
                redirectUrl: '='
            }
        });

})(window.angular);