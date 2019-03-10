$(function() {
        $("#share a").on("click", function() {
            $.ajax({
                cache: false,
                url: "httphandlers/trial/trackevent.ashx",
                data: { "trialevent": $(this).parent("div").attr("data-event"), "trialparams": $(this).attr("data-network") }
            });
        });
    }
);