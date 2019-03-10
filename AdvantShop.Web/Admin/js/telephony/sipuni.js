$(function () {
    // Declare a proxy to reference the hub. 
    var sipuniHub = $.connection.sipuniHub;
    // Create a function that the hub can call to broadcast customerss.
    sipuniHub.client.Notification = function (customers) {

        for (var i = 0; i < customers.length; i++) {
            var html =
                '<div class="call-notify">\
                    <div class="title">' + localize('IncomingCall') + '</div>\
                    <div class="info-row">\
                        <span class="call-notify-name">' + 'Номер:' + '</span>\
                        <span class="call-notify-value">' + customers[i].StandardPhone + '</span>\
                    </div>\
                    <div class="info-row">' +
                    (customers[i].Exist
                        ? '<span class="call-notify-name">' + 'ФИО:' + '</span>\
                            <span class="call-notify-value">' +
                                customers[i].LastName + ' ' + customers[i].FirstName +
                                '<a href="ViewCustomer.aspx?CustomerID=' + customers[i].Id + '" class="btn  btn-confirm btn-small" >' +
                                    localize('ToCustomer') +
                                '</a>\
                            </span>'
                        : '<span class="call-notify-value">\
                                <a href="CreateCustomer.aspx?phone=' + encodeURIComponent(customers[i].StandardPhone) + '" class="btn btn-action btn-small" >' +
                                    localize('CreateCustomer') +
                                '</a>\
                            </span>'
                    ) + 
                       '\
                        \
                    </div>\
                </div>';
            //if (!customers[i].Exist) {
            //    html += '<span class="call-notify-name">' + customers[i].Phone + ' </span>' +
            //        '<a href="CreateCustomer.aspx?phone=' + encodeURIComponent(customers[i].Phone) + '" class="btn call-notify-customer">' + localize('CreateCustomer') + '</a>';
            //} else {
            //    html += '<span class="call-notification-name">' + customers[i].LastName + ' ' + customers[i].FirstName + '</span>' +
            //        '<a href="ViewCustomer.aspx?CustomerID=' + customers[i].Id + '" class="btn call-notify-customer" >' + localize('ToCustomer') + '</a>';
            //}
            //$.sticky(html, { autoclose: 10000, position: "bottom-right" });
            notify(html, notifyType.notify, {timer: 1000000});
        }
        $("#phone_wrapper").removeClass("mini");
    };
    // Start the connection.
    //$.connection.hub.error(function (error) {
    //    console.log('SignalR error: ' + error);
    //});
    //$.connection.hub.logging = true;
    $.connection.hub.start();


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
});
