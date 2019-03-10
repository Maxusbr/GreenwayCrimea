
function setProductEnabled(enabled, productid) {

    $.ajax({
        dataType: "json",
        traditional: true,
        cache: false,
        type: "POST",
        async: false,
        data: { productid: productid, enabled: enabled },
        url: "httphandlers/product/setproductenabled.ashx",
        success: function () { },
        error: function () { }
    });

};

$(function () {

    if ($(".ckbActiveView360").length) {
        $(".ckbActiveView360").on("click", function () {
            if ($(".ckbActiveView360 input").attr("checked")) {
                $(".photoView360").show();
            } else {
                $(".photoView360").hide();
            }
        });
    }

});
