var mpAddProduct;
var mpAddProductCopy;

$(function () {

    if ($("#addProductForm").length) {

        mpAddProduct = $.advModal({
            control: "[data-add-product-call]",
            htmlContent: $("#addProductForm"),
            title: localize("Admin_AddProduct_Title"),
            afterOpen: function () {
                resetAddForm();
                $("[data-add-product-name]").focus();
            },
            beforeClose: function () {
                //resetAddForm();
            }
        });

        if ($("[data-add-product-btn]").length) {

            $("[data-add-product-btn]").click(function () {
                if (!window.Page_ClientValidate("AddProduct"))
                    return;
                $.ajax({
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    async: true,
                    data: {
                        name: htmlEncode($("[data-add-product-name]").val()),
                        artno: htmlEncode($("[data-add-product-artno]").val()),
                        url: htmlEncode($("[data-add-product-url]").val()),
                        categoryId: htmlEncode($(this).attr("data-add-product-category"))
                    },
                    url: "httphandlers/catalog/addproduct.ashx",
                    beforeSend: function () {
                        showAddProgress(true);
                    },
                    success: function (data) {
                        showAddProgress(false);
                        if (data != null && data.result == "success") {
                            mpAddProduct.modalClose();
                            window.location = $("base").attr("href") + "product.aspx?productid=" + data.productId;
                        } else if (data != null && data.message.length) {
                            $("#errAddProduct").text(data.message);
                        } else {
                            mpAddProduct.modalClose();
                        }
                    },
                    error: function () {
                        mpAddProduct.modalClose();
                        showAddProgress(false);
                    }
                });
            });
        }

        function showAddProgress(show) {
            if (show) {
                $("#waitAddProduct").show();
                $("[data-add-product-btn]").attr("disabled", "disabled");
            } else {
                $("#waitAddProduct").hide();
                $("[data-add-product-btn]").removeAttr("disabled");
            }
        }
        function resetAddForm() {
            $("[data-add-product-name]").val("");
            $("[data-add-product-artno]").val("");
            $("[data-add-product-url]").val("");
            $("#errAddProduct").text("");
            $("#addProductForm .err-icon").hide();
            showAddProgress(false);
        }
    }

    if ($("#addProductCopyForm").length) {

        mpAddProductCopy = $.advModal({
            control: "[data-add-productcopy-call]",
            htmlContent: $("#addProductCopyForm"),
            title: localize("Admin_AddProductCopy_Title"),
            afterOpen: function () {
                $("[data-add-productcopy-name]").focus();
            },
            beforeClose: function () {
                resetAddCopyForm();
            }
        });

        if ($("[data-add-productcopy-btn]").length) {
            $("[data-add-productcopy-btn]").click(function () {
                if (!window.Page_ClientValidate("AddProductCopy"))
                    return;
                $.ajax({
                    dataType: "json",
                    cache: false,
                    type: "POST",
                    async: true,
                    data: {
                        name: htmlEncode($("[data-add-productcopy-name]").val()),
                        artno: htmlEncode($("[data-add-productcopy-artno]").val()),
                        url: htmlEncode($("[data-add-productcopy-url]").val()),
                        productId: htmlEncode($(this).attr("data-add-productcopy-sourceproduct"))
                    },
                    url: "httphandlers/catalog/copyproduct.ashx",
                    beforeSend: function () {
                        showAddCopyProgress(true);
                    },
                    success: function (data) {
                        showAddCopyProgress(false);
                        if (data != null && data.result == "success") {
                            mpAddProductCopy.modalClose();
                            window.location = $("base").attr("href") + "product.aspx?productid=" + data.productId;
                        } else if (data != null && data.message.length) {
                            $("#errAddProductCopy").text(data.message);
                            mpAddProductCopy.modalClose();
                        } else {
                            mpAddProductCopy.modalClose();
                        }
                    },
                    error: function () {
                        showAddCopyProgress(false);
                        mpAddProductCopy.modalClose();
                    }
                });
            });
        }

        var showAddCopyProgress = function (show) {
            if (show) {
                $("#waitAddProductCopy").show();
                $("[data-add-productcopy-btn]").attr("disabled", "disabled");
            } else {
                $("#waitAddProductCopy").hide();
                $("[data-add-productcopy-btn]").removeAttr("disabled");
            }
        }
        function resetAddCopyForm() {
            $("#errAddProductCopy").text("");
            $("#addProductCopyForm .err-icon").hide();
            showAddCopyProgress(false);
        }
    }
});