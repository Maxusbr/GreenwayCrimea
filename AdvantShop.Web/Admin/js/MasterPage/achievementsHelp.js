; (function ($) {
    'use strict';

    $(function () {
        var blockHelp = $('.js-achievements-help');

        if (blockHelp.length === 0) {
            return;
        }

        var blockHeader = blockHelp.find('.js-achievements-help-title'),
            blockTitle = blockHelp.find('.js-achievements-help-title-text'),
            blockContainer = blockHelp.find('.js-achievements-help-container'),
            blockContentScrollable = blockHelp.find('.js-achievements-help-content-wrap'),
            blockContent = blockHelp.find('.js-achievements-help-content'),
            btnClose = blockHeader.find('.js-achievements-help-close'),
            btnCollapse = blockHeader.find('.js-achievements-help-collapse'),
            btnGetOut = blockHeader.find('.js-achievements-help-getout'),
            btnNext = blockHelp.find('.js-achievements-help-next'),
            btnReady = blockHelp.find('.js-achievements-help-ready'),
            btns = $('.js-achievements-help-call'),
            objInStorage = JSON.parse(localStorage.getItem('achievementsHelp')) || { Expanded: true, scrollTop: 0 },
            isMoving = false,
            mouseStartPos,
            blockPos,
            timer;

        btnClose.add(btnReady).on('click.achievementsHelp', function () {
            if (blockHelp.is(':visible') === true) {

                blockHelp.hide();

                blockHelp.css({
                    top: 'auto',
                    left: 'auto'
                });

                blockHelp.addClass('achievements-help-default-position');

                objInStorage.visible = false;
                objInStorage.currentAchievementId = 0;
                objInStorage.currentLevelId = 0;

                localStorage.setItem('achievementsHelp', JSON.stringify(objInStorage));
            }
        });


        btnNext.on('click.achievementsHelp', function () {

            objInStorage.ignore = objInStorage.ignore || []

            objInStorage.ignore.push(objInStorage.currentAchievementId);

            objInStorage.currentAchievementId = 0;
            objInStorage.currentLevelId = 0;

            localStorage.setItem('achievementsHelp', JSON.stringify(objInStorage));
        });


        btnCollapse.on('click.achievementsHelp', function () {

            if (blockContainer.is(':visible') === true) {

                btnCollapse.removeClass('achievements-help-collapse').addClass('achievements-help-expand');

                blockContainer.hide();

                blockHelp.addClass('achievements-help-block-collapse');

                blockHelp.css({
                    top: 'auto',
                    left: 'auto'
                });

                blockHelp.addClass('achievements-help-default-position');

                objInStorage.Expanded = false;

            } else {
                btnCollapse.removeClass('achievements-help-expand').addClass('achievements-help-collapse');

                blockHelp.removeClass('achievements-help-block-collapse');
                blockContainer.show();

                objInStorage.Expanded = true;
            }

            localStorage.setItem('achievementsHelp', JSON.stringify(objInStorage));
        });

        blockContentScrollable.on('scroll.achievementsHelp', function () {

            var blockScrollable = $(this);

            if (timer != null) {
                clearTimeout(timer);
            }

            timer = setTimeout(function () {
                objInStorage.scrollTop = blockScrollable.scrollTop();
                localStorage.setItem('achievementsHelp', JSON.stringify(objInStorage));
            }, 500);
        });

        //#region draggable
        blockHeader.on('mousedown.achievementsHelp', function (e) {

            isMoving = true;

            mouseStartPos = {
                x: e.pageX,
                y: e.pageY
            };

            var offset = blockHelp.offset();

            blockPos = {
                top: offset.top - $(window).scrollTop(),
                left: offset.left
            };

            e.preventDefault();
        });

        $(document).on('mousemove.achievementsHelp', function (e) {
            if (blockPos != null && mouseStartPos != null) {

                var left = blockPos.left + (e.pageX - mouseStartPos.x),
                    top = blockPos.top + (e.pageY - mouseStartPos.y);

                blockHelp.css({
                    left: left,
                    top: top
                });

                blockHelp.removeClass('achievements-help-default-position');
            }
        });

        blockHeader.on('mouseup.achievementsHelp', function (e) {
            blockPos = null;
            mouseStartPos = null;
            isMoving = false;
        });

        blockHeader.on('mouseout.achievementsHelp', function (e) {

            if (isMoving == false) {
                blockPos = null;
                mouseStartPos = null;
            }

        });
        //#endregion

        if (objInStorage.visible !== false) {

            $.getJSON(blockContent.attr('data-achievement-url-get'), { achievementId: objInStorage.currentAchievementId }).done(function (data) {
                if (data.Ok === true && data.Result != null) {

                    var href = btnGetOut.attr('href') + '#tabid=tabAchievements_' + objInStorage.currentLevelId;

                    btnGetOut.attr('href', href);


                    if (objInStorage.Expanded === false) {
                        blockContainer.hide();
                        btnCollapse.removeClass('achievements-help-collapse').addClass('achievements-help-expand');
                        blockHelp.addClass('achievements-help-block-collapse');
                    }

                    blockContent.html(data.Result.Instructions);
                    blockTitle.html(data.Result.Title);

                    blockHelp.fadeIn(600, function () {
                        if (objInStorage.Expanded === true || objInStorage.Expanded == null) {
                            blockContentScrollable.scrollTop(objInStorage.scrollTop);
                        }
                    });
                }
            });
        }

    });
})(jQuery);