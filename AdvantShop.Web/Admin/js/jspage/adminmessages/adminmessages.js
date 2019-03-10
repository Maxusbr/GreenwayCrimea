var mpAdminMessage;

var mpListAdminMessages;

$(function () {
    if ($("#modalAdminMessage").length) {
        mpAdminMessage = $.advModal({
            title: localize('AdminMessagesMessage'),
            htmlContent: $("#modalAdminMessage"),
            control: $(".modalShowRow"),
            afterClose: function () { initValidation($("form")); },
            controlBeforeOpen: function () {
                getMessage($(this).parent().find(".adminMessageCheckBox").attr("data-amid"));
            },

            clickOut: false,
            buttons: [{ textBtn: localize('Close'), isBtnClose: true, classBtn: 'btn-action' }]
        });
    }
});

function getMessage(amId) {
    $.ajax({
        dataType: "json",
        cache: false,
        type: "POST",
        async: false,
        data: {
            amId: htmlEncode(amId)
        },
        url: "httphandlers/adminmessage/getadminmessage.ashx",
        success: function (data) {

            if (data.result === 'error') {
                $("#modalAdminMessage").html("error");
                return;
            }


            new EJS({ url: 'js/jspage/adminmessages/templates/modal.tpl' }).update(document.getElementById('modalAdminMessage'), data);

            setViewedAdminMessage(amId);

        },
        error: function (data) {
            $("#modalAdminMessage").html("error");
        }
    });
}

function setViewedAdminMessage(amid) {

    var ids = [];
    ids.push(amid);
    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { ids: ids },
        url: "httphandlers/adminmessage/SetViewedAdminMessage.ashx",
        success: function () {
            $(".adminMessageCheckBox[data-amid=" + amid + "]").closest("tr").attr("class", "adminMessageRowViewed");
        },
        error: function () {
        }
    });
}

function setViewedCheckedAdminMessages() {
    var checkBoxes = $(".adminMessageCheckBox:checked");
    var ids = [];
    for (var i = 0, arlength = checkBoxes.length; i < arlength; i += 1) {
        ids.push(checkBoxes.eq(i).attr("data-amid"));
    }

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { ids: ids },
        url: "httphandlers/adminmessage/setviewedadminmessage.ashx",
        success: function () {
            for (var i = 0, arlength = ids.length; i < arlength; i += 1) {
                $(".adminMessageCheckBox[data-amid=" + ids[i] + "]").closest("tr").attr("class", "adminMessageRowViewed");
            }
            $(".adminMessageCheckBox").removeAttr("checked");
            $("#checkAll").removeAttr("checked");
        },
        error: function () {
        }
    });
}

function setNotViewedCheckedAdminMessages() {
    var checkBoxes = $(".adminMessageCheckBox:checked");
    var ids = [];
    for (var i = 0, arlength = checkBoxes.length; i < arlength; i += 1) {
        ids.push(checkBoxes.eq(i).attr("data-amid"));
    }

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { ids: ids },
        url: "httphandlers/adminmessage/setnotviewedadminmessage.ashx",
        success: function () {
            for (var i = 0, arlength = ids.length; i < arlength; i += 1) {
                $(".adminMessageCheckBox[data-amid=" + ids[i] + "]").closest("tr").attr("class", "adminMessageRowNotViewed");
            }
            $(".adminMessageCheckBox").removeAttr("checked");
            $("#checkAll").removeAttr("checked");
        },
        error: function () {
        }
    });
}

function checkAllAdminMessages(control) {
    $(".adminMessageCheckBox").attr("checked", $(control).is(":checked"));
}