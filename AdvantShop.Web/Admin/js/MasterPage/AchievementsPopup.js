; (function ($) {
    'use strict';

    $(function () {
        var achievementPopup = $('.js-achievements-popup');

        if (achievementPopup.length > 0) {
            $.advModal({
                htmlContent: achievementPopup,
                buttons: [
                    { textBtn: 'Начать', classBtn: 'btn-submit', func: function () { window.location.assign('Achievements.aspx') } },
                    { textBtn: 'Отмена', classBtn: 'btn-action', isBtnClose: true }
                ]
            }).modalShow();
        }
    });

})(jQuery);