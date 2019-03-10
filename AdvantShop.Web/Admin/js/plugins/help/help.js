(function ($) {

    var advantshop = Advantshop, scriptManager = advantshop.ScriptsManager, utilities = advantshop.Utilities;

    var help = function (selector, options) {
        this.container = advantshop.GetJQueryObject(selector);
        this.$obj = this.container.find('.js-help');
        this.callers = this.container.find('.js-help-icon');
        return window.Bubble.prototype.Init(this.$obj, options, this.callers);
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Help = help;

    help.prototype.InitTotal = function () {
        var objects = $('[data-plugin ="help"]');

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            help.prototype.Init(objects.eq(i), utilities.Eval(objects.eq(i).attr('data-help-options')) || {});
        }
    };

    $(help.prototype.InitTotal); // call document.ready

    help.prototype.Init = function (selector, text, options) {
        var helpObj = new help(selector, $.extend({}, { position: 'right bottom', clickOutClose: false, sizeTile: 0, alignedOnLine: true }, options));
    };




})(jQuery);