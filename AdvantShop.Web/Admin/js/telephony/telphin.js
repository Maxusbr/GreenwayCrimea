$(function () {
    // Declare a proxy to reference the hub. 
    var telphinHub = $.connection.telphinHub;
    // Create a function that the hub can call to broadcast customerss.
    telphinHub.client.Notification = function (customers) {

        for (var i = 0; i < customers.length; i++) {
            var html = '<div class="telphin">';
            html += '<div class="telphin__title">' + localize('IncomingCall') + '</div>';
            if (!customers[i].Exist) {
                html +=
                    '<span class="call-notification-name">' + customers[i].StandardPhone + ' </span>' +
                    '<div>';

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
                            '(' + orders.lastLead.status + ')</span> от ' + orders.lastLead.date +
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
            //$.sticky(html, { autoclose: 10000, position: "bottom-right" });
            notify(html, notifyType.telphin);
        }
    };
    // Start the connection.
    //$.connection.hub.error(function (error) {
    //    console.log('SignalR error: ' + error);
    //});
    //$.connection.hub.logging = true;
    $.connection.hub.start();
});