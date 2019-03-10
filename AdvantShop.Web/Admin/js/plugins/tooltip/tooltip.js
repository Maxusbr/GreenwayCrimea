(function($) {

    var advantshop = Advantshop, scriptManager = advantshop.ScriptsManager, utilities = advantshop.Utilities, cache = {}, counter = 0;

    var tooltip = function(selector, text, options) {
        this.$obj = advantshop.GetJQueryObject(selector);
        this.text = text;

        if (this.text == null || this.text.length == 0) {
            throw Error('undefined "data-tooltip-text" for tooltip');
        }

        this.options = $.extend({}, this.defaultOptions, options);

        return this;
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Tooltip = tooltip;

    tooltip.prototype.InitTotal = function() {
        var objects = $('[data-plugin ="tooltip"]');

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            tooltip.prototype.Init(objects.eq(i), objects.eq(i).attr('data-tooltip-text'), utilities.Eval(objects.eq(i).attr('data-tooltip-options')) || {});
        }
    };

    $(tooltip.prototype.InitTotal); // call document.ready

    tooltip.prototype.Init = function (selector, text, options) {
        var tooltipObj = new tooltip(selector, text, options);

        tooltipObj.GenerateHtml();

        tooltipObj.BindEvent();

        return tooltipObj;
    };

    tooltip.prototype.GenerateHtml = function() {
        var tooltipObj = this,
            tooltipDom = tooltip.prototype.tpl.clone();

        tooltipDom.html(tooltipObj.text);

        $('body').append(tooltipDom);

        tooltipObj.tooltip = tooltipDom;

        return tooltipObj;
    };

    tooltip.prototype.BindEvent = function() {
        var tooltipObj = this,
            timer;

        tooltipObj.$obj.on(tooltipObj.options.eventShow, function () {
            if (timer != null) {
                clearTimeout(timer);
            }

            timer = setTimeout(function () { tooltipObj.Show.call(tooltipObj);}, tooltipObj.options.delay);
        });

        tooltipObj.$obj.on(tooltipObj.options.eventHide, function () {
            if (timer != null) {
                clearTimeout(timer);
            }

            timer = setTimeout(function () { tooltipObj.Hide.call(tooltipObj); }, tooltipObj.options.delay);
        });

        if(tooltipObj.options.isTooltipHover === true){

            tooltipObj.tooltip.on('mouseenter', function () {
                if (timer != null) {
                    clearTimeout(timer);
                }

            })

            tooltipObj.tooltip.on('mouseleave', function () {
                if (timer != null) {
                    clearTimeout(timer);
                }
                timer = setTimeout(function () { tooltipObj.Hide.call(tooltipObj); }, tooltipObj.options.delay);
            })
        }

        var tooltipId = (counter++);

        tooltipObj.$obj.attr('data-tooltip-id', tooltipId);

        cache[tooltipId] = tooltipObj;

        return tooltipObj;
    };

    tooltip.prototype.Show = function() {
        var tooltipObj = this, tooltipDom = tooltipObj.Get(), objOffset = tooltipObj.$obj.offset(), objSize = { width: tooltipObj.$obj.outerWidth(), height: tooltipObj.$obj.outerHeight() };

        var top = objOffset.top + objSize.height;
        var left = objOffset.left - ((tooltipDom.outerWidth() - objSize.width) / 2);

        tooltipDom.css({
            top: top,
            left: left
        });

        tooltipDom.removeClass('tooltip-adv-hide').stop(true, true)[tooltipObj.options.effectShow](tooltipObj.options.effectSpeed);
    };

    tooltip.prototype.Hide = function() {
        var tooltipObj = this, tooltipDom;
        tooltipDom = tooltipObj.Get();

        tooltipDom.stop(true, true)[tooltipObj.options.effectHide](tooltipObj.options.effectSpeed, function () {
            tooltipDom.addClass('tooltip-adv-hide').removeAttr('style').show();
        });
    };

    tooltip.prototype.Get = function() {
        var tooltipObj = this, idx = tooltipObj.$obj.attr('data-tooltip-id'), tooltipDom;

        if (idx == null || idx.length === 0 || cache[idx] == null) {
            tooltipDom = tooltipObj.GenerateHtml().tooltip;
        } else {
            tooltipDom = cache[idx].tooltip;
        }

        return tooltipDom;
    };

    tooltip.prototype.tpl = $("<div />", { 'class': 'tooltip-adv tooltip-adv-hide' });

    tooltip.prototype.defaultOptions = {
        eventShow: 'mouseenter',
        eventHide: 'mouseleave',
        effectShow: 'fadeIn',
        effectHide: 'fadeOut',
        effectSpeed: 100,
        isTooltipHover: false,
        delay: 400
    };

})(jQuery);