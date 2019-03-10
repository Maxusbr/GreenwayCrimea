$(function () {
    // Declare a proxy to reference the hub. 
    var callHub = $.connection.callHub;

    // Create a function that the hub can call to broadcast customers.
    callHub.client.Notification = getNotifications;
    // Start the connection.
    //$.connection.hub.error(function (error) {
    //    console.log('SignalR error: ' + error);
    //});
    //$.connection.hub.logging = true;
    //$.connection.hub.start();

    localStorage.removeItem("windowsActivity");
    localStorage.removeItem("hubActivity");

    var isConnectionStarted = false,
        tabId = Math.floor(Math.random() * 1000),
        windowsActivity = {},
        hubActivity = {};

    setInterval(function () {
        windowsActivity = JSON.parse(localStorage["windowsActivity"] || "{}");
        hubActivity = JSON.parse(localStorage["hubActivity"] || "{}");

        // 
        var hidden = typeof document.hidden !== "undefined" && document.hidden;
        if (!hidden && isConnectionStarted === false) {
            $.connection.hub.start();
            isConnectionStarted = true;
            hubActivity[tabId] = true;
        } else if (hidden && isConnectionStarted === true) {
            $.connection.hub.stop();
            isConnectionStarted = false;
            hubActivity[tabId] = false;
        }

        windowsActivity[tabId] = !hidden;

        var hasActiveWindow = false;
        for (var key in windowsActivity) {
            if (windowsActivity[key]) {
                hasActiveWindow = true;
                break;
            }
        }
        if (!hasActiveWindow) {
            var hubAlreadyStarted = false;
            for (var key in hubActivity) {
                if (hubActivity[key] && key != tabId) {
                    hubAlreadyStarted = true;
                    break;
                }
                hubActivity[key] = false;
            }
            if (!hubAlreadyStarted) {
                $.connection.hub.start();
                hubActivity[tabId] = true;
            }
        }
        localStorage["windowsActivity"] = JSON.stringify(windowsActivity);
        localStorage["hubActivity"] = JSON.stringify(hubActivity);
    }, 1000);

    window.onbeforeunload = function () {
        delete windowsActivity[tabId];
        delete hubActivity[tabId];
        localStorage["windowsActivity"] = JSON.stringify(windowsActivity);
        localStorage["hubActivity"] = JSON.stringify(hubActivity);
    }

    function getNotifications(customers) {

        for (var i = 0; i < customers.length; i++) {
            var text = '',
                html = '<div class="telphin">';
            html += '<div class="telphin__title">' + localize('IncomingCall') + '</div>';
            if (!customers[i].Exist) {
                html +=
                    '<span class="call-notification-name">' + customers[i].StandardPhone + ' </span>' +
                    '<div>';
                text += customers[i].StandardPhone;

                var orders = customers[i].Orders;

                if (orders != null) {
                    html += '<div class="telphin__info">';

                    if (orders.lastOrder != null) {
                        html +=
                            '<div><span class="telphin__last-order-name" style="font-style: italic;">Последний заказ: </span><span class="bold"><a class="telphin__href" target="_blank" href="ViewOrder.aspx?orderId=' + orders.lastOrder.orderId + '">' + orders.lastOrder.orderNumber + '</a> ' +
                            '(' + orders.lastOrder.status + ')</span> от ' + orders.lastOrder.date +
                            '</div>';
                    }

                    if (orders.lastLead != null) {
                        html +=
                            '<div><span class="telphin__last-lead" style="font-style: italic;">Последний лид: </span><span class="bold"><a target="_blank" class="telphin__href" href="EditLead.aspx?id=' + orders.lastLead.id + '">' + orders.lastLead.id + '</a> ' +
                            '(' + orders.lastLead.status + ')</span> от ' + orders.lastLead.date +
                            '</div>';
                    }

                    html += '<div style="padding:15px 0 0 0"><span class="telphin__total-price">Всего заказов на сумму:</span> <b>' + orders.totalOrderPrice + ' (' + orders.totalOrderCount + 'шт)</b> - оплаченных</div>';

                    html += '</div> ';
                }

                html +=
                    '<a target="_blank" class="telphin__href" href="EditLead.aspx?phone=' + customers[i].StandardPhone + '">Создать лид</a> ' +
                    '<a target="_blank" class="telphin__href" href="EditOrder.aspx?orderId=addnew&phone=' + customers[i].StandardPhone + '">Создать заказ</a> ' +
                    '</div>';
            } else {
                html +=
                    '<a target="_blank" class="telphin__customer" href="ViewCustomer.aspx?CustomerID=' + customers[i].Id + '">' + customers[i].LastName + ' ' + customers[i].FirstName + '</a>';

                text += customers[i].LastName + ' ' + customers[i].FirstName + '\n' + customers[i].StandardPhone;

                var orders = customers[i].Orders;

                if (orders != null) {
                    html += '<div class="telphin__info"> ';

                    if (orders.lastOrder != null) {
                        html +=
                            '<div class="telphin__info-item"><span class="telphin__last-order-name">Последний заказ: </span><span class="bold"><a target="_blank" class="telphin__href" href="ViewOrder.aspx?orderId=' + orders.lastOrder.orderId + '">' + orders.lastOrder.orderNumber + '</a> ' +
                            '(' + orders.lastOrder.status + ')</span> от ' + orders.lastOrder.date +
                            '</div>';
                    }

                    if (orders.lastLead != null) {
                        html +=
                            '<div class="telphin__info-item"><span class="telphin__last-lead" style="font-style: italic;">Последний лид: </span><span class="bold"><a target="_blank" class="telphin__href" href="EditLead.aspx?id=' + orders.lastLead.id + '">' + orders.lastLead.id + '</a> ' +
                            '(' + lastLead.orders.status + ')</span> от ' + orders.lastLead.date +
                            '</div>';
                    }

                    html += '<div class="telphin__info-item"><span class="telphin__total-price">Всего заказов на сумму:</span> <b>' + orders.totalOrderPrice + ' (' + orders.totalOrderCount + 'шт)</b> - оплаченных</div>';

                    var manager = customers[i].Manager;
                    if (manager != null) {
                        html += '<div class="telphin__info-manager">Менеджер: <span>' + manager.Name + '</span> </div>';
                    }

                    html += '</div> ';
                }
            }
            html += '</div>';

            notify(html, notifyType.telphin);

            var hours = new Date().getHours(),
                minutes = new Date().getMinutes(),
                timeFormatted = (hours < 10 ? '0' + hours : hours) + ':' + (minutes < 10 ? '0' + minutes : minutes)
            
            var n = WebNotifyService.send(
                localize('IncomingCall') + ' - ' + timeFormatted,
                text, customers[i].callId, webNotificationType.call);
        }
    };


    // webphone
    if ($("#phone_wrapper").length) {
        $("#phone_wrapper").append("<span class=\"telephony\">Sipuni</span><span class=\"toggle-phone\"></span>");

        $("#phone_wrapper").on("click", ".toggle-phone", function () {
            $("#phone_wrapper").toggleClass("mini");
        });
    }

    $("body").on("click", "[data-call-number]", function () {
        var phone = $(this).attr("data-call-number");
        if (phone.length) {
            $("#phone_wrapper").removeClass("mini");
            $("#phone_wrapper input.phone-screen__field").val(phone).focus();
            $("#phone_wrapper button:disabled:not(.phone__key)").removeAttr("disabled");
        }
    });

    $("body").on("click", "[data-recordlink-callid]", function () {
        var obj = $(this);
        $.ajax({
            type: 'GET',
            dataType: 'json',
            async: false,
            cache: false,
            url: 'httphandlers/calls/getrecordlink.ashx',
            data: {
                type: obj.attr("data-recordlink-type"),
                callId: obj.attr("data-recordlink-callid")
            },
            success: function (data) {
                obj.hide();
                if (data != null && data.Link != null && data.Link.length > 0) {
                    obj.after("<audio src=\"" + data.Link + "\" controls />");
                    
                } else {
                    obj.after("<span>No record</span>");
                }
            },
            error: function (data) {
                throw Error(data.responseText);
            }
        });
    });
});