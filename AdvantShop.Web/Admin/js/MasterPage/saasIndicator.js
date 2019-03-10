(function ($) {
    $(function () {

        var place = $('#battery');

        if (place.length === 0) {
            return;
        }

        var saasContent = place.find('#batteryContent');

        Advantshop.ScriptsManager.Tooltip.prototype.Init(place, saasContent.show(), { isTooltipHover: true });

    })
})(jQuery);