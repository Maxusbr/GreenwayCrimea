$(document).ready(function () {
    var a = $('.order-item-info');
    $(a).each(function (i, v) {
        var val = $(v).children();
        renderArtNo(val[1]);
    })
})
function renderArtNo(elem) {
    var id = elem.innerText;
    $.ajax({
        url: '../comparisoncategory/renderhrefartno',
        data: { slProductId: id },
        success: function (data) {
            if (data.ok)
            elem.innerHTML = data.artno;
        }
    })
}