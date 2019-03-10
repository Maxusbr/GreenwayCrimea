; (function ($) {
    'use strict';

    var isMyStuffScrolling = function () {

        if (isMyStuffScrolling.doc == null) {
            isMyStuffScrolling.doc = $(document);
        }

        if (isMyStuffScrolling.win == null) {
            isMyStuffScrolling.win = $(window);
        }

        var docHeight = isMyStuffScrolling.doc.height();
        var scroll = isMyStuffScrolling.win.height() + isMyStuffScrolling.win.scrollTop();
        return (docHeight == scroll);
    }

    var checkPos = function (el, parent, container) {

        el.css('top', 0);

        if (el.length === 0) {
            return;
        }

        var maxHeight = $(window).height();

        if (el.outerHeight() > maxHeight) {
            el.css('height', maxHeight);
            el.addClass('achievements-help-inside-scroll');
        }

        var elSize = el.offset().top + el.outerHeight(),
            winSize = container.offset().top + container.outerHeight(),
            dim = winSize - elSize,
            maxNegative = parent.outerHeight() - el.outerHeight(),
            resultTop;

        if (dim < 0) {
            resultTop = dim > maxNegative ? dim : maxNegative;
            el.css('top', resultTop);
        }

        return el;
    };

    $(function () {

        var objInStorage = JSON.parse(localStorage.getItem('achievementsHelp')) || {},
            achievementsList = $('.js-achievements-list'),
            rows = achievementsList.children('.js-achievements-list-row'),
            tabs = $('.tabs-achievements'),
            tab,
            tabIndex,
            rowSelected,
            rowsProcess,
            rowsComplete,
            help;


        if (achievementsList.length === 0) {
            return;
        }

        rows.on('click.achievementsHelp', function () {
            var rowSelected = $(this),
                btnClick = rowSelected.find('.js-achievements-help-call'),
                help = rowSelected.children('.js-achievements-help-inside-static');

            rows.filter('.achievement-active').removeClass('achievement-active');

            rowSelected.addClass('achievement-active');

            checkPos(help, rowSelected, achievementsList.filter(':visible'));

            if (isMyStuffScrolling() === true && help.outerHeight() > ($(window).height() - 58)) {
                var h = help.outerHeight(),
                    t = help.position().top;
                help.css({
                    'height': h - 70,
                    'top': t + 80
                });
            }

            objInStorage.visible = true;
            objInStorage.currentAchievementId = btnClick.attr('data-achievement-help-id');
            objInStorage.currentLevelId = btnClick.attr('data-achievement-level-id');
            objInStorage.scrollTop = 0;

            localStorage.setItem('achievementsHelp', JSON.stringify(objInStorage));
        });

        rowsComplete = rows.filter('.achievements-list-complete');
        rowsProcess = rows.filter('.achievements-list-progress');

        if (rowsProcess.length === 0) {
            return;
        }

        objInStorage.ignore = objInStorage.ignore || [];

        if (objInStorage.currentAchievementId > 0 && objInStorage.ignore.indexOf(objInStorage.currentAchievementId) == -1 && rowsProcess.filter('[data-achievements-id=' + objInStorage.currentAchievementId + ']').length > 0) {
            rowSelected = rowsProcess.filter('[data-achievements-id=' + objInStorage.currentAchievementId + ']');
        } else {

            if (rowsComplete.length === 1) {
                rowSelected = rowsComplete.first();
            }

            if (rowSelected == null) {
                //ищем среди неигнорируемых
                for (var i = 0, il = rowsProcess.length; i < il; i++) {
                    if (objInStorage.ignore.indexOf(rowsProcess.eq(i).attr('data-achievements-id')) == -1) {
                        rowSelected = rowsProcess.eq(i);
                        break;
                    }
                }
            }


            //если не нашли, то берем первую в процессе
            if (rowSelected == null) {
                rowSelected = rowsProcess.first();

                var indexIgnore = objInStorage.ignore.indexOf(rowSelected.attr('data-achievements-id'));

                if (indexIgnore != -1) {
                    objInStorage.ignore.splice(indexIgnore, 1);
                }
            }
        }

        objInStorage.currentAchievementId = rowSelected.attr('data-achievements-id');
        objInStorage.currentLevelId = rowSelected.attr('data-achievement-level-id');
        objInStorage.scrollTop = 0;

        objInStorage.visible = true;
        localStorage.setItem('achievementsHelp', JSON.stringify(objInStorage));

        help = rowSelected.children('.js-achievements-help-inside-static');
        rowSelected.addClass('achievement-active');

        tab = rowSelected.closest('.tab-content');
        tabIndex = tabs.find('.tab-content').index(tab);

        tabs.find('.tab-header').eq(tabIndex).click();

        checkPos(help, rowSelected, achievementsList.filter(':visible'));

        if (rowSelected.offset().top > $(window).height()) {
            $(window).scrollTop(rowSelected.offset().top);
        }

        if (isMyStuffScrolling() === true && help.outerHeight() > ($(window).height() - 58)) {
            var h = help.outerHeight(),
                t = help.position().top;
            help.css({
                'height': h - 70,
                'top': t + 80
            });
        }

        rows.find('.js-achievements-next-step').on('click', function (e) {
            e.stopPropagation();

            var currentRow = $(this).closest(rows),
                indx = rows.index(currentRow),
                nextRow = rows.eq(indx + 1);

            if (nextRow.length > 0) {
                nextRow.click();

                var tabNext = nextRow.closest('.tab-content'),
                    tabNextIndex,
                    tabsSiblings;

                if (tabNext.is(':hidden') === true) {
                    tabsSiblings = tabNext.closest('.tabs-contents').children('.tab-content');
                    tabNextIndex = tabsSiblings.index(tabNext);

                    tabs.find('[data-tabs-header]').eq(tabNextIndex).click();

                }
            }

        });
    });

})(jQuery);