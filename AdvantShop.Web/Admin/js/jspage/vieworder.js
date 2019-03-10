
$(function () {

    if ($(".ddlViewOrderStatus").length) {
        var ddl = $(".ddlViewOrderStatus");
        var modal;
        var prevStatus;

        $(".ddlViewOrderStatus").on('focus', function () {
            prevStatus = ddl.val();
        });

        $(".ddlViewOrderStatus").change(function () {

            $("#span-status-name").text(ddl.children("option:selected").text());

             modal = modal || $.advModal({
                title: "Смена статуса",
                htmlContent: $("#modalChangeStatus"),
                clickOut: false,
                funcCross: function() {
                    ddl.val(prevStatus);
                },
                buttons: [{ textBtn: localize('Cancel'), isBtnClose: true, classBtn: 'btn-action', func: function() {
                     ddl.val(prevStatus);
                } }, {
                    textBtn: localize('Ok'), isBtnClose: false, classBtn: 'btn-confirm', func: function () {
                        setOrderStatus(ddl.val(), ddl.attr("data-orderid"), $("#status-basis").val());
                        modal.modalClose();
                    }
                }]
            });

            modal.modalShow();

            //setOrderStatus(dll.val(), dll.attr("data-orderid"));
        });
    }

    if ($(".useIn1c").length) {
        $(".useIn1c").change(function () {
            changeUseIn1C($(".useIn1c input").is(":checked"), $(".useIn1c").attr("data-orderid"));
        });
    }

    if ($(".setManagerConfirm").length) {
        $(".setManagerConfirm").change(function () {
            changeManagerConfirmed($(".setManagerConfirm input").is(":checked"), $(".setManagerConfirm").attr("data-orderid"));
        });
    }

    if ($(".ddlViewOrderManager").length) {
        var dllManager = $(".ddlViewOrderManager");

        $(".ddlViewOrderManager").change(function () {
            setManagerOrder(dllManager.val(), dllManager.attr("data-orderid"));
        });
    }
    if ($(".editableTextBoxInViewOrder").length) {
        var editableTextBox, progress;
        $(".editableTextBoxInViewOrder").focusout(function () {
            editableTextBox = $(this);

            if (editableTextBox.val() == editableTextBox.attr("data-currentvalue")) {
                return;
            }

            progress = new Advantshop.ScriptsManager.Progress.prototype.Init(editableTextBox);
            progress.Show();
            updateOrderField(editableTextBox.attr("data-orderid"), editableTextBox.val(), editableTextBox.attr("data-field-type"));
            progress.Hide();
        });
    }

    $(".multiship-create-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/createordermultiship.ashx",
            success: function (data) {
                if (data != null && data.result != "error") {
                    notify(localize("Admin_ViewOrder_CreateMultishipOrder"), notifyType.notify);
                } else {
                    notify(localize("Admin_ViewOrder_ErrorCreateMultishipOrder"), notifyType.error);
                }
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateMultishipOrder"), notifyType.error);
            }
        });
    });

    $(".send-billing-link").on("click", function () {
        if (!confirm($(this).attr("data-confirm"))) {
            return false;
        }
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/sendbillinglink.ashx",
            success: function (data) {
                if (data != null)
                    notify(data.message, data.result == "success" ? notifyType.notify : notifyType.error);
                else
                    notify(localize("Admin_ViewOrder_SendBillingLinkError"), notifyType.error);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_SendBillingLinkError"), notifyType.error);
            }
        });
    });

    $(".Sdek-send-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value"), tariff: $(this).attr("data-tariff"), pickpoint: $(this).attr("data-checkout-pickpoint") },
            url: "httphandlers/order/Sdeksendorder.ashx",
            success: function (data) {
                notify(data.result.message, data.result.status ? notifyType.notify : notifyType.error);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateSdekOrder"), notifyType.error);
            }
        });
    });

    $(".Sdek-delete-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/Sdekdeleteorder.ashx",
            success: function (data) {
                notify(data.result.message, data.result.status ? notifyType.notify : notifyType.error);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateSdekOrder"), notifyType.error);
            }
        });
    });

    $(".Sdek-callcustomer-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/Sdekcallcustomer.ashx",
            success: function (data) {
                notify(data.result.message, data.result.status ? notifyType.notify : notifyType.error);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateSdekOrder"), notifyType.error);
            }
        });
    });

    $(".Sdek-callcourier-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/Sdekcallcourier.ashx",
            success: function (data) {
                notify(data.result.message, data.result.status ? notifyType.notify : notifyType.error);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateSdekOrder"), notifyType.error);
            }
        });
    });

    $(".checkout-send-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/checkoutsendorder.ashx",
            success: function (data) {
                notify(data.result.message, data.result.error ? notifyType.error : notifyType.notify);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateCheckoutOrder"), notifyType.error);
            }
        });
    });

    $(".yandexdelivery-send-order").on("click", function () {
        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "POST",
            async: false,
            data: { orderid: $(this).attr("data-value") },
            url: "httphandlers/order/yandexdeliverysendorder.ashx",
            success: function (data) {
                notify(data.result.message, data.result.error ? notifyType.error : notifyType.notify);
            },
            error: function () {
                notify(localize("Admin_ViewOrder_ErrorCreateCheckoutOrder"), notifyType.error);
            }
        });
    });

    $(".orderhistory-link").on("click", function() {

        $(".orderhistory-list").toggle();

    });

    //send-letter
    var modalSendLetter;

    $(".send-letter").click(function () {
        
        modalSendLetter = modalSendLetter || $.advModal({
            title: "Письмо пользователю",
            htmlContent: $("#modalSendLetterToCustomer"),
            clickOut: false,
            buttons: [
                { textBtn: localize('Cancel'), isBtnClose: true, classBtn: 'btn-action' },
                { textBtn: localize('Ok'), isBtnClose: false, classBtn: 'btn-confirm', func: function () {
                    sendLetterToCustomer($(".send-letter").attr("data-orderid"), $("#modalSendLetterToCustomer .letter-to-customer-subject").val(), $("#modalSendLetterToCustomer .letter-to-customer-text").val());
                    modalSendLetter.modalClose();
                }
            }]
        });

        modalSendLetter.modalShow();

        //setOrderStatus(dll.val(), dll.attr("data-orderid"));
    });

});

function getSdekOrderPrintform(orderid) {
    window.location = "httphandlers/order/Sdekorderprintform.ashx?orderid=" + orderid;
    return true;
}

function getSdekOrderReportStatus(orderid) {
    window.location = "httphandlers/order/Sdekreportorderstatus.ashx?orderid=" + orderid;
    return true;
}

function getSdekOrderReportInfo(orderid) {
    window.location = "httphandlers/order/Sdekreportorderinfo.ashx?orderid=" + orderid;
    return true;
}

function changeUseIn1C(use, orderid) {

    $.ajax({
        dataType: "text",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, use: use },
        url: "httphandlers/order/changeusein1c.ashx",
        success: function (data) {
            if (data == "true") {
                notify("Изменения сохранены", notifyType.notify);
            } else {
                notify(localize("Admin_ViewOrder_ErrorOnSaveOrderStatus"), notifyType.error);
            }
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveOrderStatus"), notifyType.error);
        }
    });
}


function changeManagerConfirmed(confirm, orderid) {

    $.ajax({
        dataType: "text",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, confirm: confirm },
        url: "httphandlers/order/setmanagerconfirm.ashx",
        success: function (data) {
            if (data == "true") {
                notify("Изменения сохранены", notifyType.notify);
            } else {
                notify(localize("Admin_ViewOrder_ErrorOnSaveOrderStatus"), notifyType.error);
            }
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveOrderStatus"), notifyType.error);
        }
    });
}

function setOrderPaid(paid, orderid) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, paid: paid },
        url: "httphandlers/order/setorderpaid.ashx",
        success: function () {
            if ($(".orders-list-name[data-current-order=1]").length) {
                if (paid == 1) {
                    $(".orders-list-name[data-current-order=1]").removeClass("orders-list").addClass("orders-list-ok");
                } else if (paid == 0) {
                    $(".orders-list-name[data-current-order=1]").removeClass("orders-list-ok").addClass("orders-list");
                }
            }
            notify(localize("Admin_ViewOrder_SaveOrderPayStatus"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveOrderPayStatus"), notifyType.error);
        }
    });
}


function setOrderStatus(statusid, orderid, basis) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, statusid: statusid, basis: basis },
        url: "httphandlers/order/setorderstatus.ashx",
        success: function (data) {
            if ($(".order-main").length) {
                $(".order-main").first().css("border-left-color", "#" + data.Color);
            }
            if ($(".orders-list-row[data-current-order=1]").length) {
                $(".orders-list-row[data-current-order=1]").css("border-left-color", "#" + data.Color);
            }
            notify(localize("Admin_ViewOrder_SaveOrderStatus"), notifyType.notify);

            if (data.IsNotifyUser) {
                $("#lnkSendMail").show();
            }
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveOrderStatus"), notifyType.error);
        },
        complete: function () {
        }
    });
}

function sendLetterToCustomer(orderid, subject, text) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, subject: subject, text: text },
        url: "httphandlers/customer/sendLetterToCustomer.ashx",
        success: function (data) {
            notify("Письмо отослано", notifyType.notify);
        },
        error: function () {
            notify("Не удалось отправить письмо", notifyType.error);
        },
    });
}

function setManagerOrder(managerid, orderid) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, managerid: managerid },
        url: "httphandlers/order/setmanagerorder.ashx",
        success: function () {
            notify(localize("Admin_ViewOrder_SaveManagerOrder"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveManagerOrder"), notifyType.error);
        },
        complete: function () {
        }
    });
}

function updateOrderField(orderid, text, field) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid, text: text, field: field },
        url: "httphandlers/order/updateorderfield.ashx",
        success: function () {
            notify(localize("Admin_ViewOrder_SaveComment"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveComment"), notifyType.error);
        }
    });
}

function sendMailOrderStatus(orderid) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { orderid: orderid },
        url: "httphandlers/order/sendmailorderstatus.ashx",
        success: function (data) {
            if (data.result != "error") {

                if ($(".order-main").length) {
                    $(".order-main").first().css("border-left-color", "#" + data.result);
                }
                if ($(".orders-list-row[data-current-order=1]").length) {
                    $(".orders-list-row[data-current-order=1]").css("border-left-color", "#" + data.result);
                }
                notify(localize("Admin_ViewOrder_SendMailStatusOrder"), notifyType.notify);
            } else {
                notify(localize("Admin_ViewOrder_ErrorOnSendMailStatusOrder"), notifyType.error);
            }

            $("#lnkSendMail").hide();

        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSendMailStatusOrder"), notifyType.error);
        },
        complete: function () {
        }
    });
}