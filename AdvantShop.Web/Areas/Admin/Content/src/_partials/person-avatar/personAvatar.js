; (function (ng) {
    'use strict';

    var PersonAvatarCtrl = function ($http, $rootScope, $timeout) {

        var ctrl = this, popoverTimer;

        ctrl.$onInit = function () {
            //ctrl.updateAvatar(ctrl.startValue);

            $rootScope.$on('avatarupdated', function (event, data) {
                if (data != null && data.customerId == ctrl.customerId) {
                    ctrl.fetch();
                }
            });
        };

        ctrl.updateAvatar = function (src) {
            if (src != null && src !== ctrl.noAvatarSrc) {
                var index = src.indexOf('?rnd');
                ctrl.avatarSrc = src.substring(0, index > 0 ? index : src.length) + "?rnd=" + (Math.random());
            } else {
                ctrl.avatarSrc = ctrl.noAvatarSrc;
            }

            if (ctrl.img != null) {
                ctrl.img.src = ctrl.avatarSrc;
            }
        };

        ctrl.addImgElement = function (img) {
            ctrl.img = img;
        }

        ctrl.fetch = function () {
            $http.post('common/getAvatar', { customerId: ctrl.customerId }).then(function (response) {
                ctrl.updateAvatar(response.data);
            });
        }

        ctrl.popoverOpen = function () {

            if (popoverTimer != null) {
                $timeout.cancel(popoverTimer);
            }

            ctrl.popoverIsOpen = true;
        };

        ctrl.popoverClose = function () {

            popoverTimer = $timeout(function () {
                ctrl.popoverIsOpen = false;
            }, 500);
        };
    };

    PersonAvatarCtrl.$inject = ['$http', '$rootScope', '$timeout'];

    ng.module('personAvatar', ['uiModal','ui.bootstrap'])
      .controller('PersonAvatarCtrl', PersonAvatarCtrl);

})(window.angular);