(function($, win) {

    var advantshop = window.Advantshop
    , $body = $(document.body)
    , $win = $(win)
    ,progressCollection = {}
    ,counter = -1;

    var progress = function(selector, options) {
        var $obj = advantshop.GetJQueryObject(selector);
        var reference = $obj.attr('data-progress-reference') || '';

        if (progress.prototype.inCollection(reference)) {
            return progress.prototype.GetProgressInCollection(reference);
        }

        this.$obj = $obj;
        this.options = $.extend({}, this.defaultOptions, options);

        return this;
    };


    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    advantshop.ScriptsManager.Progress = progress;

    progress.prototype.Init = function(selector, options) {
        var $obj = advantshop.GetJQueryObject(selector);
        var reference = $obj.attr('data-progress-reference') || '';

        if (progress.prototype.inCollection(reference)) {
            return progress.prototype.GetProgressInCollection(reference);
        }

        var progressObj = new progress($obj, options);

        progressObj.GenerateHtml();

        return progressObj;
    };

    progress.prototype.GenerateHtml = function() {
        var progressObj = this;

        if (progressObj.options.type = progress.prototype.type.Over) {
            progressObj.GenerateHtmlOver();
        } else {
            // to do inside progress
        }
    };

    progress.prototype.GenerateHtmlOver = function() {
        var progressObj = this;
        var progressName = 'progress-' + (counter += 1);

        var $progress = $('<div>', {
            'class': 'progress progress-over',
            'data-progress-id': progressName
        });

        $body.append($progress);

        progressObj.$progress = $progress;

        progressCollection[progressName] = progressObj;

        progressObj.$obj.attr('data-progress-reference', progressName);
    };

    progress.prototype.SetPosition = function() {
        var progressObj = this;
        var $progress = progressObj.$progress;
        var offset = progressObj.$obj.offset();
        var size = { width: progressObj.$obj.outerWidth(), height: progressObj.$obj.outerHeight() };

        if (progressObj.$obj.is('body') || progressObj.$obj.is('html') || progressObj.$obj.is($win)) {
            size.height = $win.height();
            offset.top = $win.scrollTop();
        }

        $progress.css({ top: offset.top, left: offset.left, width: size.width, height: size.height });
    };

    progress.prototype.Show = function() {
        var progressObj = this;
        var options = progressObj.options;
        var $progress = progressObj.$progress;

        progressObj.SetPosition();

        $progress.stop(true, true);

        if (progressObj.$obj.is('body') || progressObj.$obj.is('html') || progressObj.$obj.is($win)) {
            $win.off('scroll.progressScroll');
            $win.on('scroll.progressScroll', function () {
                progressObj.SetPosition();
            });
            $('html, body').css('overflow-x', 'hidden');
        }

        $progress[options.animationShow](options.animationSpeed, function() {
            progressObj.interval = setInterval(function() { progressObj.SetPosition(); }, options.tick);
        });
    };

    progress.prototype.Hide = function() {
        var progressObj = this;
        var options = progressObj.options;
        var $progress = progressObj.$progress;

        if (progressObj.interval) {
            clearInterval(progressObj.interval);
        }

        if (progressObj.$obj.is('body') || progressObj.$obj.is('html') || progressObj.$obj.is($win)) {
            $win.off('scroll.progressScroll');
            $('html, body').css('overflow-x', '');
        }

        $progress.stop(true, true);
        $progress[options.animationHide](options.animationSpeed);
    };

    progress.prototype.type = { Inside: 'inside', Over: 'over' };

    progress.prototype.inCollection = function(reference) {
        var result = false;

        if (!reference && this.$obj && this.$obj.attr('data-progress-reference')) {
            reference = this.$obj.attr('data-progress-reference');
        }

        if (progressCollection[reference]) {
            result = true;
        }

        return result;
    };

    progress.prototype.GetProgressInCollection = function(reference) {
        var progressObj = this;
        var result = null;

        if (!reference && this.$obj && this.$obj.attr('data-progress-reference')) {
            reference = this.$obj.attr('data-progress-reference');
        }

        if (progressObj.inCollection(reference)) {
            result = progressCollection[reference];
        }

        return result;
    };


    progress.prototype.defaultOptions = {
        tick: 50,
        animationShow: "fadeIn",
        animationHide: "fadeOut",
        animationSpeed: 0,
        opacity: 0.5,
        type: progress.prototype.Inside
    };

})(jQuery, window);