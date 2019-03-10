$(document).ready(function () {

    $.ajax({
        url: "COMPClient/location",
        success: function (data) {
            var sliderMainBlock = $('.slider-main-block');

            var categoriesBlock = $('.product-categories-module.product-categories-slim');
            categoriesBlock.removeAttr('style');

            var rmBlock = $("#categoriesOnMainPageRM");
            if (rmBlock.length > 0) {
                rmBlock.append(categoriesBlock);
            }
            else {
                if (data === "True") {
                    categoriesBlock.insertAfter(sliderMainBlock);
                }
                else {
                    sliderMainBlock.before(categoriesBlock);
                }
            }
        }
    })
})