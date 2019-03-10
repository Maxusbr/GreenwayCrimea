$(document).ready(function () {
    var orderInfoPrintBlock = $("#orderInfoTemplatesBlock");
    if (orderInfoPrintBlock.length > 0) {
        var additionalParametersBlock = $(".ibox.category-block.block-additional-parameters").first();
        if (additionalParametersBlock.length > 0) {
            orderInfoPrintBlock.removeAttr('style');
            orderInfoPrintBlock.insertAfter(additionalParametersBlock);
        }
    }

    $("body").on("change", "#ddlOrderInfoTemplatesList", function () {
        var selectedTemplate = $("#ddlOrderInfoTemplatesList").val();

        var buttonPrint = $("#orderInfoTemplatesBlockPrint");
        if (buttonPrint.length > 0) {
            if (selectedTemplate > 0) {
                buttonPrint.removeAttr("disabled");
            } else {
                buttonPrint.attr("disabled", "disabled");
            }
        }
    });

    $("body").on("click", "#orderInfoTemplatesBlockPrint", function () {
        var formType = $("#ddlOrderInfoTemplatesList").val();
        var orderId = $("[data-order-info-templates-order-id]").attr("data-order-info-templates-order-id");

        var templateId = Number.parseInt(formType);

        if (isNaN(templateId) || templateId == -1) return;

        var prepayment = 0;
        var prepaymentField = $("#txtOrderInfoPrepayment");
        if (prepaymentField.length > 0) {
            prepayment = Number.parseFloat(prepaymentField.val());
            if (isNaN(prepayment) || prepayment < 0) {
                prepayment = 0;
            }
        }

        var href = "../RussianPostPrintFormByTemplate/?formType=" + formType + "&orderId=" + orderId + "&prepayment=" + prepayment;

        var width = 600;
        var height = 400;
        var popup = window.open(href, "popup", "height=" + height + ", width=" + width + "");
    });
})