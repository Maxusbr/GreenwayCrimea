$(document).ready(function () {
    var labels = $('#simalandLabels').html();
    $('.gallery-picture-labels .products-view-label:last').after(labels);
    $('#simalandLabels').remove();
    $('.details-block .details-row:first').before($('#simalandButtons'))
    $('#simalandButtons').css('display', 'block');
    var checkArtno;
    var elem = $('.details-sku .details-param-value');
    var hrefartno = "";
    var id = elem[0].innerHTML;
    if (id) {
        $.ajax({
            url: '/comparisoncategory/renderhrefartno',
            data: { slProductId: id },
            success: function (data) {
                if (data.ok) {
                    checkArtno = setInterval(artno, 100);
                    hrefartno = data.artno;
                }
            }
        })
    }

    function artno() {
        var c = elem[0].innerHTML;
        if (c.length != hrefartno.length) {
            elem[0].innerHTML = hrefartno;
        } else {
            clearInterval(checkArtno);
        }
    }

})