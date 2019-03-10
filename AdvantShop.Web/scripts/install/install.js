$(function () {


    if ($(".install-payment").length > 0) {
        

        $(".block-options label input").on("click", function () {
            var className = $(this).val();

            $(this).parents(".block-options").find(".list-methods > div").hide();
            $(this).parents(".block-options").find(".list-methods ." + className.toLowerCase()).show();
            initValidation($("form"));
        });

    }

    
    $("fieldset.group > legend input[type='checkbox']").each(function () {
        ShowDivVisible.call(this, 0);
    });

    $("fieldset.group > legend input[type='checkbox']").click(function () {
        ShowDivVisible.call(this, "slow");
    });

    if ($("#check_folders").length) {
        $("#check_folders").click(function () {
            getAccessInfo();
        });
    }
});

function getAccessInfo() {
    var content = $("#access_info");
    $.ajax({
        cache: false,
        type: "POST",
        url: "./httphandlers/getdirectoryaccessinfo.ashx",
        beforeSend: function () {
            $("#load_content").show();
        },
        success: function (data) {
            content.html(data);
        },
        complete: function () {
            $("#load_content").hide();
        }
    });
};

function ShowDivVisible(speed) {

    var sender = $(this);
    var parentBlock = sender.closest("fieldset.group");
    var block = parentBlock.find(".block-options");

    if (parentBlock.hasClass("simple")) return false;

    if (parentBlock.length && !sender.is(":checked")) {
        block.slideUp(speed);
        parentBlock.addClass("collapsed-block");
    }
    else {
        block.slideDown(speed);
        parentBlock.removeClass("collapsed-block");
    }
}

