$(document).ready(function () {
    $.ajax({
        url: "../slapiv5/LoadPriceBalanceSettings",
        success: function (data) {
            $('#auto-update-balance').attr('checked', data.au);
            $('#timePriod').val(data.h)
        }
    })

    $('.tabContainer').on('click', '#btnSave', function (e) {
        var au = $('#auto-update-balance').is(':checked');
        var h = $('#timePriod').val();

        $.ajax({
            url: "../slapiv5/SavePriceBalanceSettings",
            data: { au, h },
            success: function (data) {
                if (data.error == true) {
                    $('#statusMessages div').removeClass();
                    $('#statusMessage div').addClass('error');
                } else {
                    $('#statusMessage div').removeClass();
                    $('#statusMessage div').addClass('success');
                    $("#statusMessage").css("visibility", "visible");
                    setTimeout('$("#statusMessage").css("visibility","hidden")', 3000);
                }
                $('#statusMessage div').text(data.message)
            }
        })
    })

    loadMarkups();

    function loadMarkups() {
        $.ajax({
            url: "../slapiv5/getmarkups",
            success: function (data) {
                $("#range-list").html(data);
            }
        })
    }

    $('#sl-login').click(function (e) {
        var fData = new FormData();
        fData.append("email", $("#sl-email").val());
        fData.append("password", $("#sl-password").val());
        fData.append("phone", $("#sl-tel").val());
        fData.append("regulation", $("#sl-regulation").is(":checked"));

        $('#authorize-status div').removeClass();
        $('#authorize-status div').addClass('info');
        $('#authorize-status div').text('Идет обработка данных...')

        $.ajax({
            type: "POST",
            url: "../slapiv5/sllogin",
            contentType: false,
            processData: false,
            data: fData,
            success: function (data) {
                if (data.error == true) {
                    $('#authorize-status div').removeClass();
                    $('#authorize-status div').addClass('error');
                } else {
                    $('#authorize-status div').removeClass();
                    $('#authorize-status div').addClass('success');
                }
                $('#authorize-status div').text(data.message)
            }
        })
    })

    $('#add-markup').click(function (e) {
        $.ajax({
            url: "../slapiv5/addmarkup",
            success: function (data) {
                $("#range-list").html(data);
            }
        })
    })

    $(".tabContainer").on('keypress', '.input-markup', function (e) {
        e = e || event;

        if (e.ctrlKey || e.altKey || e.metaKey) return;

        var chr = getChar(e);

        if (chr == null) return;

        if (chr < '0' || chr > '9') {
            return false;
        }
    });

    function getChar(event) {
        if (event.which == null) {
            if (event.keyCode < 32) return null;
            return String.fromCharCode(event.keyCode)
        }

        if (event.which != 0 && event.charCode != 0) {
            if (event.which < 32) return null;
            return String.fromCharCode(event.which) 
        }

        return null;
    }

    function validate() {
        var minPrice = parseInt($('#minPrice').val());
        var maxPrice = parseInt($('#maxPrice').val());
        var markup = parseInt($('#markup').val());
        if (isNaN(markup) || isNaN(maxPrice) || isNaN(minPrice))
        {
            console.log('Ни одно из полей не должен быть пустым');
            return false;
        }

        if (minPrice >= maxPrice) {
            console.log('Начальная цена не может быть больше конечной цены в диапазоне');
            return false;
        }

        if (markup < 0 || markup > 100)
        {
            console.log('наценка не может быть меньше 0 или больше 100');
            return false;
        }

        if (minPrice < 0 || maxPrice < 0)
        {
            console.log('цена не может быть отрицательной');
            return false;
        }

        return true;
    }

    $('.tabContainer').on('click', '#added', function (e) {

        var minPrice = $('#minPrice').val();
        var maxPrice = $('#maxPrice').val();
        var markup = $('#markup').val();

        if (!validate())
        {
            return false;
        }

        var fData = new FormData();
        fData.append("Id", 0);
        fData.append("MinPrice", minPrice);
        fData.append("MaxPrice", maxPrice);
        fData.append("Markup", markup);

        $.ajax({
            type: "POST",
            url: "../slapiv5/addmarkup",
            data: fData,
            processData: false,
            contentType: false,
            success: function (data) {
                $("#range-list").html(data);
            }
        })

        return false;
    })

    $('.tabContainer').on('click', '#editMarkup', function (e) {
        var Id = e.target.dataset.rangeid;

        $.ajax({
            url: "../slapiv5/editmarkup",
            data: { Id},
            success: function (data) {
                $("#range-list").html(data);
            }
        })
        return false;
    })

    $('.tabContainer').on('click', '#update', function (e) {
        var id = e.target.dataset.rangeid;
        var minPrice = $('#minPrice').val();
        var maxPrice = $('#maxPrice').val();
        var markup = $('#markup').val();

        if (!validate())
        {
            return false;
        }

        var fData = new FormData();
        fData.append("Id", id);
        fData.append("MinPrice", minPrice);
        fData.append("MaxPrice", maxPrice);
        fData.append("Markup", markup);

        $.ajax({
            type: "POST",
            url: "../slapiv5/updatemarkup",
            processData: false,
            contentType: false,
            data: fData,
            success: function (data) {
                $("#range-list").html(data);
            }
        })

        return false;
    })

    $('.tabContainer').on('click', '#cancel', function (e) {
        loadMarkups();
        return false;
    })

    $('.tabContainer').on('click', '#deleteMarkup', function (e) {
        var Id = e.target.dataset.rangeid;

        $.ajax({
            url: "../slapiv5/deletemarkup",
            data: { Id},
            success: function (data) {
                $("#range-list").html(data);
            }
        })

        return false;
    })

    $.ajax({
        url: "../slapiv5/getvalidpriceandbalance",
        success: function (data) {
            $('#toUpdate').html(data.toUpdate);
            if (data.process)
            {
                $('#manualStart').attr('disabled', 'disabled');
            }
        }
    })

    $('.tabContainer').on('click', '#manualStart', function (e) {
        $.ajax({
            url: "../slapiv5/updatebalance",
            success: function (data) {

            }
        })
    })
})