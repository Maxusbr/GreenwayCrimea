$(document).ready(function () {

    $.ajax({
        url: "fnpclient/location",
        success: function (data) {
            $('.notify-wrap').appendTo($(data)[0]);
            $('.notify-wrap').css('display', 'inline-block');
        }
    })


})