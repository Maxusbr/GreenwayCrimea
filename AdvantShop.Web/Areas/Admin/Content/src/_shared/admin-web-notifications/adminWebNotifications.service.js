; (function (ng) {
    'use strict';

    var adminWebNotificationsService = function ($rootScope, $cookies, $http, $window, webNotification, toaster, SweetAlert, urlHelper, $uibModalStack, adminWebNotificationsEvents) {
        var service = this,
            listeners = {};

        var notifyHub = SJ.iwc.SignalR.getHubProxy('notifyHub', {
            client: {
                showNotification: function (notification) {
                    // from only one browser tab
                    if (!SJ.iwc.SignalR.isConnectionOwner())
                        return;
                    service.showNotification(notification);
                },
                showNotifications: function (notifications) {
                    // from only one browser tab
                    if (!SJ.iwc.SignalR.isConnectionOwner())
                        return;
                    if (notifications == null)
                        return;
                    // firefox is blocking concurrent notifications
                    //var isFF = navigator.userAgent.toLowerCase().indexOf('firefox') > -1,
                    //    timeout = isFF ? 200 : 0,
                    var showTimer,
                        i = 0;
                    showTimer = setInterval(function () {
                        service.showNotification(notifications[i]);
                        i++;
                        if (i >= notifications.length)
                            clearInterval(showTimer);
                    }, 200);
                },
                popNotification: function (notification) {
                    service.popNotification(notification);
                },
                closeToaster: function (toastId) {
                    //setTimeout(function () {
                    toaster.clear({ toastId: toastId });
                    //}, 10000);
                },
                updateOrders: function () {
                    //console.log('updateOrders');
                    service.executeCallbacks(adminWebNotificationsEvents.updateOrders);
                },
                updateLeads: function () {
                    //console.log('updateLeads');
                    service.executeCallbacks(adminWebNotificationsEvents.updateLeads);
                },
                updateTasks: function () {
                    //console.log('updateTasks');
                    service.executeCallbacks(adminWebNotificationsEvents.updateTasks);
                },
            }
        });
        SJ.iwc.SignalR.start();

        service.onPageLoad = function () {
            setTimeout(function () {
                if ($uibModalStack.getTop() == null) { // если нет открытых модальных окон
                    service.checkPermission();
                }
            }, 5000);
            service.getToasterNotifications();
        }

        service.webNotificationAvailable = function () {
            return typeof (webNotification.lib) != 'undefined';
        }

        service.permissionDenied = function () {
            return service.webNotificationAvailable() && webNotification.lib.permission == 'denied';
        }

        service.showPermissionDeniedMsg = function () {
            SweetAlert.alert('Необходимо разрешить текущему сайту показывать оповещенния в настройках браузера', {
                title: 'Уведомления заблокированы',
                type: null,
                showLoaderOnConfirm: false,
                customClass: 'sa-small sa-custom',
                padding: 10,
                confirmButtonClass: 'btn btn-success btn-sm',
                width: '320px',
                input: 'checkbox',
                inputPlaceholder: 'Не напоминать'
            }).then(function (result) { // result is 1 if checkbox checked else 0
                if (result === 1) {
                    $cookies.put('dontDisturbByNotify', 'true');
                }
            });
        }

        service.checkPermission = function (step) {
            var isChrome = /Chrome/.test(navigator.userAgent) && /Google Inc/.test(navigator.vendor);
            if (window.location.hostname !== 'localhost' && window.location.hostname !== 'server' &&
                isChrome && window.location.protocol != "https:") {
                return;
            }
            if (!service.webNotificationAvailable() || $cookies.get('dontDisturbByNotify') === 'true' || webNotification.permissionGranted || step > 10) {
                return;
            }
            if (!step)
                step = 0;
            if (service.permissionDenied()) {
                service.showPermissionDeniedMsg();
                return;
            }
            SweetAlert.confirm('Включите уведомления <br> Для того чтобы получать уведомления о новых звонках, задачах, комментариях и др. ', {
                title: 'Внимание',
                confirmButtonText: 'Включить',
                cancelButtonText: 'Не сейчас',
                type: null,
                customClass: 'sa-small sa-custom',
                padding: 10,
                confirmButtonClass: 'btn btn-danger btn-sm',
                cancelButtonClass: 'btn btn-action btn-sm',
                width: '320px'
            }).then(function (result) {
                webNotification.lib.requestPermission().then(function (permission) {
                    if (permission == 'granted') {
                        SweetAlert.alert('', {
                            title: 'Уведомления включены',
                            type: null,
                            showLoaderOnConfirm: false,
                            customClass: 'sa-small sa-custom',
                            padding: 10,
                            confirmButtonClass: 'btn btn-success btn-md',
                            width: '320px',
                        });
                    } else if (permission == 'default') {
                        service.checkPermission(++step);
                    } else if (permission == 'denied') {
                        service.showPermissionDeniedMsg();
                    }
                });
            }, function (result) {
                var date = new Date();
                date.setDate(date.getDate() + 1);
                $cookies.put('dontDisturbByNotify', 'true', { expires: date });
            });
        }

        service.showNotification = function (notification) {
            if (notification.IconPath == null || !notification.IconPath.length) {
                switch (notification.Type) {
                    case 1: //'Notify'
                        notification.IconPath = '';
                        break;
                    case 2: //'Call'
                        notification.IconPath = urlHelper.getUrl('/areas/admin/content/images/notifications/incoming_call.jpg', true);
                        break;
                    default:
                        notification.IconPath = '';
                }
            }
            webNotification.showNotification(notification.Title, {
                body: notification.Body,
                icon: notification.IconPath,
                tag: notification.Tag,
                onClick: function onNotificationClicked() {
                    if (notification.Data != null && notification.Data.Url != null) {
                        if (notification.InNewTab === true) {
                            var w = $window.open(notification.Data.Url, '_blank');
                            if (w != null) {
                                w.focus();
                            }
                        } else {
                            $window.location.assign(notification.Data.Url);
                            $window.focus();
                        }
                    } else {
                        $window.focus();
                    }
                    this.close();
                }
                //autoClose: 4000 //auto close the notification after 4 seconds (you can manually close it via hide function)
            }, function onShow(error, hide) {
                if (error) {
                    console.log('Unable to show notification: ' + error.message);
                } else {
                    //setTimeout(function hideNotification() {
                    //    hide();
                    //}, 5000);
                }
            });
            //$rootScope.$apply();
        };

        service.popNotification = function (notification) {
            switch (notification.Type) {
                case 1: //'Notify'
                    break;
                case 2: //'Call'
                    service.popCallNotification(notification);
                    break;
            }
            $rootScope.$apply();
        };

        service.popCallNotification = function (notification) {
            if (notification.Data == null)
                return;
            var params = {
                leadId: notification.Data.leadId,
                lastOrderId: notification.Data.lastOrder != null ? notification.Data.lastOrder.orderId : null,
                lastLeadId: notification.Data.lastLead != null ? notification.Data.lastLead.id : null,
            };
            $http.post('calls/checkDataAccess', params).then(function (response) {
                var result = response.data;
                if (result == null)
                    return;
                notification.Data.hasOrdersAccess = result.hasOrdersAccess;
                notification.Data.hasCRMAccess = result.hasCRMAccess;
                notification.Data.hasCustomersAccess = result.hasCustomersAccess;
                notification.Data.leadId = result.leadId;
                if (result.lastLeadId == null)
                    notification.Data.lastLead = null;
                if (result.lastOrderId == null)
                    notification.Data.lastOrder = null;
                toaster.pop({
                    type: 'call',
                    title: notification.Title,
                    body: 'call-notification',
                    directiveData: ng.copy(notification.Data),
                    bodyOutputType: 'directive',
                    timeout: 0,
                    toastId: notification.Tag
                });
            });
        };

        service.getToasterNotifications = function () {
            return $http.post('calls/getNotifications').then(function (response) {
                if (response.data != null && response.data.notifications && response.data.notifications.length > 0) {
                    var notifications = response.data.notifications;
                    var showTimer,
                        i = 0;
                    showTimer = setInterval(function () {
                        service.popNotification(notifications[i]);
                        i++;
                        if (i >= notifications.length) {
                            clearInterval(showTimer);
                        }
                    }, 10);
                }
            });
        }

        service.addListener = function (eventName, callback) {

            if (adminWebNotificationsEvents[eventName] == null) {
                throw Error('Event name "' + eventName + '" not exist');
            }

            listeners[eventName] = listeners[eventName] || [];
            listeners[eventName].push(callback);

        };

        service.executeCallbacks = function (eventName, data) {
            if (listeners[eventName] != null && listeners[eventName].length > 0) {
                listeners[eventName].forEach(function (callback) {
                    callback(data);
                });
            }
        }
    };

    adminWebNotificationsService.$inject = ['$rootScope', '$cookies', '$http', '$window', 'webNotification', 'toaster', 'SweetAlert', 'urlHelper', '$uibModalStack', 'adminWebNotificationsEvents'];

    ng.module('adminWebNotifications', [])
        .constant('adminWebNotificationsEvents', {
            updateOrders: 'updateOrders',
            updateLeads: 'updateLeads',
            updateTasks: 'updateTasks'
        })
        .service('adminWebNotificationsService', adminWebNotificationsService);

})(window.angular);