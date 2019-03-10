$(document).ready(function () {
    $('#btnFb').on('click', function (e) {
        $.ajax({
            type: 'GET',
            url: '../feedbackslm/getform',
            success: function (data) {
                $('#fbModal').html(data);
                $('#overlay').fadeIn(400,
                   function () {
                       $('#fbmodal_form')
                           .css('display', 'block')
                           .animate({ opacity: 1, top: window.top.window.innerHeight / 2 }, 200);
                   });
            }
        });
    });

    $('#fbModal').on('click', '#fbmodal_close', function () {
        $('#fbmodal_form')
            .animate({ opacity: 0, top: '45%' }, 200,
                function () {
                    $(this).css('display', 'none');
                    $('#fbModal').html('');
                }
            );
    });

    $('#fbModal').on('click', '#fbmodal_send', function () {
        var fData = prepareData();
        $.ajax({
            type: 'POST',
            url: '../feedbackslm/sendform',
            contentType: false,
            processData: false,
            data: fData,
            success: function (data) {                
                if (!data.success) {
                    var err = document.getElementById('#errMes');
                    $(err).text(data.message);
                    $(err).css('color', 'darkred');
                } else {
                    var modal = document.getElementById('fbmodal_form');
                    var close = document.getElementById('fbmodal_close');
                    $(close).val('OK');
                    var close_c = $(close).clone();
                    $(modal).css('width', '300px');
                    $(modal).css('height', '150px');
                    $(modal).css('margin-top', '-75px');
                    $(modal).css('margin-left', '-150px');
                    $(modal).children().css('text-align', 'center');
                    $(modal).children().html(data.message);
                    $(modal).children().append(close_c);
                    //$('#overlay').click();
                }
            }
        })
    });

    $('#fbModal').on('click', '#fbmodal_close, #overlay', function () {
        $('#fbmodal_form')
            .animate({ opacity: 0, top: '45%' }, 200,
                function () {
                    $(this).css('display', 'none');
                    $('#overlay').fadeOut(400);
                }
            );
        return false;
    });

    function prepareData() {
        var name = $('#fbModal #clientName').val();
        var email = $('#fbModal #clientEmail').val();
        var phone = $('#fbModal #clientPhone').val();
        var message = $('#fbModal #message').val();


        var fData = new FormData();
        fData.append("Name", name);
        fData.append("Email", email);
        fData.append("Phone", phone);
        fData.append("Message", message);
        return fData;
    }

});