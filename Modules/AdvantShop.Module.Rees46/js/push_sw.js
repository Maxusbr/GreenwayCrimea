'use strict';

self.addEventListener('push', function(event) {
    if( event.data ) {
        var message = JSON.parse(event.data.text());
        fetch('https://api.rees46.com/web_push_subscriptions/received', {
            mode: 'cors',
            method: 'post',
            credentials: 'include',
            headers: {
                "Content-type": "application/x-www-form-urlencoded; charset=UTF-8"
            },
            body: 'url=' + encodeURIComponent(message.url) + '&shop_id=' + encodeURIComponent(message.shop_id)
        });
        event.waitUntil(
            self.registration.showNotification(message.title, {
                body: message.body,
                icon: message.icon,
                tag: 'rees46-push-notification',
                data: {
                    url: message.url
                }
            })
        );
    } else {
    }
});

self.addEventListener('notificationclick', function(event) {
    event.notification.close();

    if (Notification.prototype.hasOwnProperty('data')) {
        var url = event.notification.data.url;
        event.waitUntil(clients.openWindow(url));
    }
});
