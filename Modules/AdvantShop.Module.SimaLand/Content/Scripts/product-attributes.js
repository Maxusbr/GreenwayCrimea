$(document).ready(function () {
    $.ajax({
        url: "../comparisoncategory/LoadPAttributes",
        success: function (data) {
            $('#notUpdateName').attr('checked', data.NotUpdateName);
            $('#notUpdateDescr').attr('checked', data.NotUpdateDescription);
            $('#notUpdateUrl').attr('checked', data.NotUpdateUrl);
            $('#notUpdateProperty').attr('checked', data.NotUpdateProperty);
        }
    })

    $('.product-attributes-wrap').on('click', '#savePAttributes', function (e) {
        var nun = $('#notUpdateName').is(':checked');
        var nud = $('#notUpdateDescr').is(':checked');
        var nuu = $('#notUpdateUrl').is(':checked');
        var nup = $('#notUpdateProperty').is(':checked');

        $.ajax({
            type:"POST",
            url: "../comparisoncategory/SavePAttributes",
            data: { nun, nud, nuu, nup },
            //contentType: false,
            //processData: false,
            success: function (data) {
                statusMessage(data);
            }
        })
        

    })

    function statusMessage(obj) {
        $('#saveSettings').removeAttr('disabled');
        $('#saveSettings').val('Сохранить');
        if (obj.Message !== "") {
            $('#errorMessageLabel').show();
            $('.info-labels').css('display', 'inline-block')
                        .animate({ opacity: 1, top: '87px' }, 200);
            $('#errorMessageLabel').text(obj.Message);
            $('.info-labels').css('background', obj.BgColor);
            $('.info-labels').css('border-color', obj.BorderColor);
            setTimeout(function () {
                $('.info-labels')
                .animate({ opacity: 0, top: '40px' }, 200,
                    function () {
                        $('.info-labels').css('display', 'none');
                    }
                );
            }, 3000);
        }
    }
})