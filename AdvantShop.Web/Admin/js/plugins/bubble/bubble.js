; (function ($, $win, $doc) {
    'use strict';

    var objs = $([]);

    var Bubble = function (selector, options, callers) {

        this.$obj = $(selector);

        if (this.$obj.length === 0) {
            return null;
        }


        this.options = $.extend({}, this.defaultOptions, options);
        this._position = this.options.position;


        this.callers = $('[data-bubble-call="' + this.$obj.attr('id') + '"]').add(callers || $([]));

        objs = objs.add(this.$obj);

        document.body.appendChild(this.$obj[0]);

        return this;
    };

    Bubble.prototype.InitTotal = function () {
        var objects = $('[data-plugin="bubble"]:not("[data-autobind="false"]")');

        for (var i = 0, il = objects.length; i < il; i += 1) {
            Bubble.prototype.Init(objects.eq(i), (new Function("return " + objects.eq(i).attr('data-bubble-options')))() || {});
        }
    };

    Bubble.prototype.Init = function (selector, options, callers) {
        var BubbleObj = new Bubble(selector, options, callers);

        if (BubbleObj == null || BubbleObj.$obj.length === 0) {
            return;
        }

        BubbleObj.GenerateHtml();

        BubbleObj.BindEvent();

        BubbleObj.$obj.data('bubble', BubbleObj);
    };

    Bubble.prototype.GenerateHtml = function () {

        var BubbleObj = this;

        if (BubbleObj.options.isBackgroundEnabled && Bubble.prototype.bg == null) {
            Bubble.prototype.renderBg();
        }
    };

    Bubble.prototype.BindEvent = function () {
        var BubbleObj = this;


        if (BubbleObj.options.canHover === true) {

            BubbleObj.$obj.off('mouseenter.Bubble');
            BubbleObj.$obj.on('mouseenter.Bubble', function () {
                BubbleObj.Show();
            });

            BubbleObj.$obj.off('mouseleave.Bubble');
            BubbleObj.$obj.on('mouseleave.Bubble', function () {
                BubbleObj.Hide();
            });
        }

        if (BubbleObj.options.eventTypeShow == BubbleObj.options.eventTypeHide) {

            BubbleObj.callers.off(BubbleObj.options.eventTypeShow);

            BubbleObj.callers.on(BubbleObj.options.eventTypeShow, function () {
                if (BubbleObj.$obj.is(':visible') === true) {
                    BubbleObj.Hide(BubbleObj.$obj);
                } else {
                    BubbleObj.Show($(this));
                }
            });

        } else {
            BubbleObj.callers.off(BubbleObj.options.eventTypeShow);

            BubbleObj.callers.on(BubbleObj.options.eventTypeShow, function () {
                BubbleObj.Show($(this));
            });

            BubbleObj.callers.off(BubbleObj.options.eventTypeHide);

            BubbleObj.callers.on(BubbleObj.options.eventTypeHide, function () {
                BubbleObj.Hide(BubbleObj.$obj);
            });
        }


        $('body').off('click.Bubble');
        $('body').on('click.Bubble touchend.Bubble', function (e) {
            var target = $(e.target),
                itemObj;
            if (target.closest('[data-plugin="bubble"]').length === 0 && target.closest('[data-bubble-call]').length === 0 && objs.filter(':visible').length > 0) {

                for (var i = 0, l = objs.length; i < l; i += 1) {
                    itemObj = objs.eq(i).data('bubble');

                    if (itemObj.options.clickOutClose === true) {
                        itemObj.Hide();
                    }
                }
            }
        });

    };

    Bubble.prototype.Show = function (caller) {

        var BubbleObj = this;

        BubbleObj.$obj.show();

        if (caller != null && caller.length > 0) {
            BubbleObj.Pos(caller);
        }

        if (BubbleObj.options.isBackgroundEnabled === true) {
            BubbleObj.showBg();
        }

        BubbleObj.$obj.trigger('show.bubble', BubbleObj);
    };

    Bubble.prototype.Hide = function () {

        var BubbleObj = this;

        BubbleObj.$obj.hide();

        if (BubbleObj.options.isBackgroundEnabled == true) {
            Bubble.prototype.hideBg();
        }

        BubbleObj.$obj.trigger('hide.bubble', BubbleObj);
    };

    Bubble.prototype.Pos = function (caller) {
        var BubbleObj = this,
            objSize = { width: BubbleObj.$obj.outerWidth(), height: BubbleObj.$obj.outerHeight() },
            callerOffset = caller.offset(),
            callerSize = { width: caller.outerWidth(), height: caller.outerHeight() },
            pos = BubbleObj._position.split(' '),
            posHorizontal = pos[0],
            posVertical = pos[1],
            sizeTile = BubbleObj.options.sizeTile,
            isAlignedOnLine = BubbleObj.options.alignedOnLine,
            left, top;

        BubbleObj.$obj.removeClass('bubble-' + BubbleObj.options.position.replace(' ', '-') + (isAlignedOnLine === true ? '-align-on-line' : ''));

        BubbleObj.options.position = BubbleObj._position;

        switch (posHorizontal) {
            case 'left':
                left = callerOffset.left - objSize.width - sizeTile;
                if (left < 0) {
                    left = callerOffset.left + callerSize.width + sizeTile;
                    BubbleObj.options.position = 'right' + ' ' + BubbleObj.options.position.split(' ')[1];
                }
                break;
            case 'center':
                left = (callerOffset.left) + ((callerSize.width - objSize.width) / 2);
                break;
            case 'right':
                left = callerOffset.left + callerSize.width + sizeTile;
                if (left + objSize.width > document.body.offsetWidth) {
                    left = callerOffset.left - objSize.width - sizeTile;
                    BubbleObj.options.position = 'left' + ' ' + BubbleObj.options.position.split(' ')[1];
                }
                break;
        }

        switch (posVertical) {
            case 'top':
                top = callerOffset.top - objSize.height - sizeTile + (isAlignedOnLine === true ? callerSize.height : 0);
                if (top < Math.max(document.documentElement.scrollTop, document.body.scrollTop)) {
                    top = callerOffset.top + (isAlignedOnLine === false ? callerSize.height + sizeTile : 0);
                    BubbleObj.options.position = BubbleObj.options.position.split(' ')[0] + ' ' + 'bottom';
                }
                break;
            case 'middle':
                top = (callerOffset.top) + ((callerSize.height - objSize.height) / 2);
                break;
            case 'bottom':
                top = callerOffset.top + (isAlignedOnLine === false ? callerSize.height + sizeTile : 0);
                if (top + objSize.height > document.documentElement.clientHeight + window.pageYOffset) {
                    top = callerOffset.top - objSize.height - sizeTile + (isAlignedOnLine === true ? callerSize.height : 0);
                    BubbleObj.options.isChangedPosition = true;
                    BubbleObj.options.position = BubbleObj.options.position.split(' ')[0] + ' ' + 'top';
                }
                break;
        }

        BubbleObj.$obj.addClass('bubble-' + BubbleObj.options.position.replace(' ', '-') + (isAlignedOnLine === true ? '-align-on-line': ''));

        BubbleObj.$obj.css({
            'left': left,
            'top': top
        });
    };

    Bubble.prototype.renderBg = function () {
        if (Bubble.prototype.bg != null) {
            return Bubble.prototype.bg;
        }

        var bg = $('<div />', { 'class': 'js-bubble-bg bubble-bg' });

        $(document.body).append(bg);

        Bubble.prototype.bg = bg;

        return bg;
    };

    Bubble.prototype.showBg = function () {
        var bg = Bubble.prototype.bg || Bubble.prototype.renderBg();

        bg.addClass('bubble-bg-active');
    };

    Bubble.prototype.hideBg = function () {

        if (Bubble.prototype.bg == null || Bubble.prototype.bg.hasClass('bubble-bg-active') === false) {
            return;
        } else {
            Bubble.prototype.bg.removeClass('bubble-bg-active');
        }

    };

    Bubble.prototype.defaultOptions = {
        position: 'right middle',
        eventTypeShow: 'mouseenter.Bubble',
        eventTypeHide: 'mouseleave.Bubble',
        isBackgroundEnabled: false,
        clickOutClose: true,
        sizeTile: 14,
        canHover: true,
        alignedOnLine: false
    };

    $(Bubble.prototype.InitTotal);
    window.Bubble = Bubble;

})(jQuery, jQuery(window), jQuery(document));