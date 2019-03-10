$(document).ready(function () {
    if ($("div").is(".products-view-item")) {
        var ids = "";
        $.each($('.products-view-item'), function (index, value) {
            ids += value.dataset.productId + ','
        });
        ids = ids.substring(0, ids.length - 1)
        $.ajax({
            url: "client/getlabels",
            data: { ids: ids },
            success: function (data) {
                if (data.have) {
                    $.each(data.labels, function (index, value) {
                        if (value != "") {
                            if ($('.products-view-item[data-product-id=' + value.ProdctId + '] .products-view-labels').length == 0)
                            {
                                $('.products-view-item[data-product-id=' + value.ProdctId + '] .products-view-info').after('<div class="products-view-labels">' + value.Labels + '</div>');
                            } else {
                                var elem = $('.products-view-item[data-product-id=' + value.ProdctId + '] .products-view-labels').append(value.Labels);
                            }
                        }
                    })
                }
            }
        })
    }
})