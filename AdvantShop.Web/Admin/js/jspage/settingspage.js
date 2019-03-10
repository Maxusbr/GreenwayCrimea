
$(function () {
    if ($(".ckbManagerConfirmed").length && $(".divManagerConfirmedValue").length) {

        //displayManagerConfirmedValue();

        $(".ckbManagerConfirmed").change(function () {

            displayManagerConfirmedValue();
        });
    }
});

function displayManagerConfirmedValue() {
    if ($(".ckbManagerConfirmed input").is(":checked")) {
        $(".divManagerConfirmedValue").show();
        $(".divManagerConfirmedValue").css("display", "inline-block");
    } else {
        $(".divManagerConfirmedValue").hide();

    }
}

function resetMeta(metatype) {

    if (!confirm("Вы уверены, что хотите сбросить мета информацию?")) {
        return;
    }

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { metatype: metatype },
        url: "httphandlers/settings/resetmeta.ashx",
        success: function (data) {
            notify(data.result, notifyType.notify);
        },
        error: function (data) {
            notify(data.result, notifyType.error);
        }
    });
}


