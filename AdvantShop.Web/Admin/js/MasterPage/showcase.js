; (function ($) {
    'use strict';

    var isShowBubbleOnLoad = localStorage.getItem('isShowBubbleOnLoad') != null;

    var pinger = function (bubble, timer) {

        if (bubble == null || isShowBubbleOnLoad === true) {
            return;
        }

        setTimeout(function () {
            if (jQuery.advModal.isQueueEmpty() === true && jQuery.advModal.countModalActive() === 0) {
                bubble.Show(bubble.callers.eq(0));
                isShowBubbleOnLoad = true;
                localStorage.setItem('isShowBubbleOnLoad', 'true');
            } else {
                pinger(bubble, timer);
            }
        }, timer);
    };

    $(window).load(function () {

        var $bubble = $('#showcase');

        if (isShowBubbleOnLoad === true) {
            Bubble.prototype.Init($bubble, { eventTypeShow: 'mouseenter', eventTypeHide: 'mouseleave', isBackgroundEnabled: false, position: 'center bottom' });
        } else {

            Bubble.prototype.Init($bubble, { eventTypeShow: null, eventTypeHide: 'click', isBackgroundEnabled: true });

            $bubble.one('hide.bubble', function (e, data) {
                Bubble.prototype.Init($bubble, { eventTypeShow: 'mouseenter', eventTypeHide: 'mouseleave', isBackgroundEnabled: false, position: 'center bottom' });
            });
        }

        pinger($bubble.data('bubble'), 300);

    });

})(jQuery);