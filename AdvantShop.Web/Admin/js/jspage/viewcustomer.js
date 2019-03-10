
$(function () {

    if ($(".ddlCustomerGroup").length) {
        var dll = $(".ddlCustomerGroup");

        $(".ddlCustomerGroup").on("change", function () {
            setCustomerGroup(dll.attr("data-customerid"), dll.val());
        });
    }

    if ($(".ddlViewCustomerManager").length) {
        var dllManager = $(".ddlViewCustomerManager");

        $(".ddlViewCustomerManager").on("change", function () {
            SetCustomerManager(dllManager.attr("data-customerid"), dllManager.val());
        });
    }

    if ($(".ckbSubscribed4News").length) {
        var ckb = $(".ckbSubscribed4News");

        $(".ckbSubscribed4News").on("change", function () {
            setCustomerSubscribe(ckb.attr("data-email"), ckb.is(":checked"));
        });
    }

    if ($(".editableTextBoxInViewCustomer").length) {
        var editableTextBox, progress;
        $(".editableTextBoxInViewCustomer").focusout(function () {
            editableTextBox = $(this);

            if (editableTextBox.val() == editableTextBox.attr("data-currentvalue")) {
                return;
            }

            progress = new Advantshop.ScriptsManager.Progress.prototype.Init(editableTextBox);
            progress.Show();
            updateCustomerField(editableTextBox.attr("data-customerid"), editableTextBox.val(), editableTextBox.attr("data-field-type"));
            editableTextBox.attr("data-currentvalue", editableTextBox.val());
            progress.Hide();
        });
    }

});

function setCustomerSubscribe(email, subscribe) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { subscribe: subscribe, email: email },
        url: "httphandlers/customer/setcustomersubscribe.ashx",
        success: function () {
            //if ($(".orders-list-name[data-current-order=1]").length) {
            //    if (paid == 1) {
            //        $(".orders-list-name[data-current-order=1]").removeClass("orders-list").addClass("orders-list-ok");
            //    } else if (paid == 0) {
            //        $(".orders-list-name[data-current-order=1]").removeClass("orders-list-ok").addClass("orders-list");
            //    }
            //}
            notify(localize("Admin_ViewCustomer_SaveCustomerSubscribe"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewCustomer_ErrorOnSaveCustomerSubscribe"), notifyType.error);
        }
    });
}

function setCustomerGroup(customerId, customerGroupId) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { customerId: customerId, customerGroupId: customerGroupId },
        url: "httphandlers/customer/setcustomergroup.ashx",
        success: function () {
            //if ($(".orders-list-name[data-current-order=1]").length) {
            //    if (paid == 1) {
            //        $(".orders-list-name[data-current-order=1]").removeClass("orders-list").addClass("orders-list-ok");
            //    } else if (paid == 0) {
            //        $(".orders-list-name[data-current-order=1]").removeClass("orders-list-ok").addClass("orders-list");
            //    }
            //}
            notify(localize("Admin_ViewCustomer_SaveCustomerGroup"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewCustomer_ErrorOnSaveCustomerGroup"), notifyType.error);
        }
    });
}

function SetCustomerManager(customerId, managerId) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { customerId: customerId, managerId: managerId },
        url: "httphandlers/customer/setcustomermanager.ashx",
        success: function () {
            //if ($(".orders-list-name[data-current-order=1]").length) {
            //    if (paid == 1) {
            //        $(".orders-list-name[data-current-order=1]").removeClass("orders-list").addClass("orders-list-ok");
            //    } else if (paid == 0) {
            //        $(".orders-list-name[data-current-order=1]").removeClass("orders-list-ok").addClass("orders-list");
            //    }
            //}
            notify(localize("Admin_ViewCustomer_SaveCustomerManager"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewCustomer_ErrorOnSaveCustomerManager"), notifyType.error);
        }
    });
}

function updateCustomerField(customerid, text, field) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { customerid: customerid, text: text, field: field },
        url: "httphandlers/customer/updatecustomerfield.ashx",
        success: function () {
            notify(localize("Admin_ViewOrder_SaveComment"), notifyType.notify);
        },
        error: function () {
            notify(localize("Admin_ViewOrder_ErrorOnSaveComment"), notifyType.error);
        }
    });
}