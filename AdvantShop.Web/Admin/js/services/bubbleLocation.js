$(function() {
    var bubbleTown = $('.js-bubble-zone'),
        bubbleObj = bubbleTown.data('bubble');

    if (bubbleTown.length > 0 && !localStorage.getItem("cityBubble")) {
        localStorage.setItem("cityBubble", "true");

        bubbleObj.Show($('.js-bubble-town'));

        bubbleTown.find('.js-bubble-town-ok').on('click', function () {
            $.ajax({
                cache: false,
                url: "admin/httphandlers/trial/trackevent.ashx",
                data: { "trialevent": "RightCity", "trialparams":  $(".js-bubble-town").text()}
            });
            bubbleObj.Hide();
            jQuery.advModal.reopen();
        });

        bubbleTown.find('.js-bubble-town-no').on('click', function (e) {
            
            $.ajax({
                cache: false,
                url: "admin/httphandlers/trial/trackevent.ashx",
                data: { "trialevent": "WrongCity", "trialparams": $(".js-bubble-town").text() }
            });

            e.stopPropagation();
            bubbleObj.Hide();

            Advantshop.ScriptsManager.ZoneService.prototype.modal.modalShow(null, true);

            jQuery.advModal.reopen();
        });
    }
});