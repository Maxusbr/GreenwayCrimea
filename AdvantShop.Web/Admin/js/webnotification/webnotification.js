; (function (window) {

    window.webNotificationType = { 
        call: 0
    };

    'use strict';

    var WebNotifyService = (function () {
        var instance;

        function init() {

            var NotifyEngine = window.Notification || window.webkitNotifications,
                isGranted;

            //просим доступ
            if ($("body").hasClass("webnotifications") && NotifyEngine != null) {
                NotifyEngine.requestPermission(function (result) {
                    isGranted = result === 'granted';
                });
            }


            function check() {
                return NotifyEngine != null && isGranted === true;
            }

            //#region public api
            return {

                send: function (title, body, tag, type) {

                    if (check() === false) {
                        return;
                    }

                    var icon;
                    switch (type) {
                        case webNotificationType.call:
                            icon = 'js/webnotification/images/incoming_call.jpg';
                            break;
                        default:
                            icon = '';
                    }

                    var n = new NotifyEngine(title, {
                        tag: tag,
                        body: body,
                        icon: icon
                    });
                    n.onclick = function (x) {
                        window.focus();
                        this.close();
                    };

                    return n;
                }
            };
            //#endregion
        };

        return function () {

            if (!instance) {
                instance = init();
            }

            return instance;
        };

    })();

    window.WebNotifyService = WebNotifyService();

})(window);