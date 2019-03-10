;(function ($) {

    $(function () {

        var objects = $('[data-plugin="radiolist"]');

        objects.addClass('radiolist');
        objects.find('input[type="radio"]').filter(':checked').closest('label').addClass('radiolist-checked');

        objects.on('click', function (e) {
            var lbl = $(e.target).closest('label');
            var div = lbl.parent();

            if (lbl.is('label') === true && lbl.hasClass('radiolist-checked') === false) {

                div.find("label").removeClass('radiolist-checked');
                div.find("input").prop("checked", false);

                lbl.find("input").prop("checked", true)
                                 .closest('label').addClass('radiolist-checked');
            }
        });
    });
})(jQuery);