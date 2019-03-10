(function ($, $window) {
    var storage = []
    , advantshop = Advantshop
    , scriptManager = advantshop.ScriptsManager
    , utilities = advantshop.Utilities;

    var transformer = function (selector, options) {

        var h = 0;

        this.$obj = advantshop.GetJQueryObject(selector);
        this.options = this.options = $.extend({}, this.defaultOptions, options);

        h = this.$obj.outerHeight();

        this.fullPosTop = this.$obj.offset().top + (this.options.point === 'bottom' ? h : 0);
        this.$obj.parent().css('minHeight', h);

        storage.push(this);
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Transformer = transformer;

    transformer.prototype.InitTotal = function () {

        var objs = $('[data-plugin="transformer"]');

        objs.each(function () {
            transformer.prototype.Init($(this), utilities.Eval($(this).attr('data-transformer-options')) || {});
        });


        $window.on('scroll', function () {
            var scrollTop = $window.scrollTop(),
                item;

            for (var i = storage.length - 1; i >= 0; i--) {
                item = storage[i];
                scrollTop > item.fullPosTop ? item.compact() : item.full();
            }
        });
    };

    transformer.prototype.Init = function (element, options) {
        var obj = new transformer(element, options);
    };

    transformer.prototype.compact = function () {

        var transformerObj = this,
            classes = transformerObj.options.classes,
            $obj = transformerObj.$obj;

        $obj.removeClass('transformer-full').addClass('transformer-compact');

        if (classes != null) {
            if (classes[0] != null && classes[0].length > 0) {
                $obj.removeClass(classes[0]);
            }

            if (classes[1] != null && classes[1].length > 0) {
                $obj.addClass(classes[1]);
            }
        }
    };

    transformer.prototype.full = function () {
        var transformerObj = this,
            classes = transformerObj.options.classes,
            $obj = transformerObj.$obj;

        $obj.removeClass('transformer-compact').addClass('transformer-full');

        if (classes != null) {
            if (classes[1] != null && classes[1].length > 0) {
                $obj.removeClass(classes[1]);
            }

            if (classes[0] != null && classes[0].length > 0) {
                $obj.addClass(classes[0]);
            }
        }
    };

    transformer.prototype.defaultOptions = {
        point: 'bottom',
        classes: [null, null]
    };

    $window.load(transformer.prototype.InitTotal); // call document.ready

})(jQuery, jQuery(window));