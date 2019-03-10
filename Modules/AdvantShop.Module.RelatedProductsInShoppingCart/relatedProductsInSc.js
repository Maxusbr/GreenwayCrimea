$(document).ready(function() {

});

$(document).on("update_related_products", function() {
    $.ajax({
        dataType: 'json',
        cache: false,
        type: 'POST',
        url: 'modules/RelatedProductsInShoppingCart/GetRelatedProducts.ashx',
        success: function(data) {
            if ($('#divRelatedProductsInSc').length) {
                $('#divRelatedProductsInSc').html("");
                if (data.RelatedProducts.length > 0) {
                    var html = new EJS({ url: 'Modules/RelatedProductsInShoppingCart/relatedProductsInSc.tpl' }).render(data);
                    $('#divRelatedProductsInSc').html(html);

                    var carousel = $(".jcarousel:visible:not(.jcarousel-list)");

                    if (carousel.length) {
                        carousel.jcarousel({ scroll: 1 });
                    }
                }
            }
        },
        error: function() {

        }
    });
});