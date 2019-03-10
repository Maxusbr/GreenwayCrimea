(function ($) {
    var advantshop = Advantshop
    , scriptManager = advantshop.ScriptsManager
    , utilities = advantshop.Utilities
    , storage = {}
    , counter = 0
    , tabStateControl
    , tabIdHidden
    , tabIdLocation = /.*tabid=(.+)/g.exec(window.location.href);

    tabIdLocation = tabIdLocation != null ? '#' + tabIdLocation[1] : null;

    var tabs = function (selector, options) {
        this.$obj = advantshop.GetJQueryObject(selector);
        this.$headers = $('[data-tabs-header]', this.$obj);
        this.$contents = $('[data-tabs-content]', this.$obj);
        this.options = $.extend({}, this.defaultOptions, options);
        return this;
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Tabs = tabs;

    tabs.prototype.InitTotal = function () {

        tabStateControl = $('[data-tabs-hidden]');
        tabIdHidden = tabStateControl.length > 0 ? tabStateControl.val() : null;

        var objects = $('[data-plugin="tabs"]');

        if (objects.length > 0) {
            tabs.prototype.BindEventOne();
        }

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            tabs.prototype.Init(objects.eq(i), utilities.Eval(objects.eq(i).attr('data-tabs-options')) || {});
        }

        var contentPlaceholderLink = $('[data-tabs-links]');

        var row = "<div><a href=\"#tabid={0}\">{1}</a></div>";

        for (var j = 0, arrLengthJ = contentPlaceholderLink.length; j < arrLengthJ; j += 1) {

            var list = "<div class=\"tabs-links\">";

            for (var item in storage) {
                var header;
                for (var headerI = 0, arrHeaders = storage[item].$headers.length; headerI < arrHeaders; headerI += 1) {
                    header = storage[item].$headers.eq(headerI);

                    list += String.Format(row, header.attr('id'), header.text());
                }

            }

            list += "<div>";

            contentPlaceholderLink.eq(j).append(list);
        }
    };

    $(tabs.prototype.InitTotal); // call document.ready

    tabs.prototype.Init = function (selector, options) {
        var tabsObj = new tabs(selector, options);

        tabsObj.GenerateHtml();
        tabsObj.BindEvent();

        var storageId;

        if (tabsObj.$obj.attr('id')) {
            storageId = tabsObj.$obj.attr('id');
        } else {
            storageId = 'tabs-' + (counter += 1);
            tabsObj.$obj.attr('id', storageId);
        }

        storage[storageId] = tabsObj;

        return tabsObj;
    };

    tabs.prototype.GenerateHtml = function () {
        var tabsObj = this;

        if (tabsObj.$headers.length != tabsObj.$contents.length) {
            throw new Error("Tab's headers count not equals tab's  blocks count");
        }

        var countRemove = 0;
        var text, contentItem, headerItem;

       
            for (var i = 0, arrLength = tabsObj.$headers.length; i < arrLength; i++) {
                contentItem = tabsObj.$contents.eq(i);
                headerItem = tabsObj.$headers.eq(i);

                text = $.trim(contentItem.html().replace(/<br\s*[\/]?>/gi, ""));

                if (((text.length === 0) || headerItem.hasClass('tab-hidden')) && !headerItem.hasClass('tab-visible')) {
                    countRemove += 1;
                    contentItem.remove();
                    headerItem.remove();
                }
            }

            if (countRemove > 0) {
                tabsObj.$headers = $('[data-tabs-header]', tabsObj.$obj);
                tabsObj.$contents = $('[data-tabs-content]', tabsObj.$obj);
            }

            if (tabsObj.$headers.length === 0) {
                tabsObj.$obj.remove();
                return;
            }
       

        tabsObj.$headers.addClass('tabs-header');
        tabsObj.$contents.addClass('tabs-content');
    };

    tabs.prototype.BindEvent = function () {
        var tabsObj = this;
        var tabsDom = tabsObj.$obj;
        var tabSelecting = tabIdHidden || tabIdLocation || tabsObj.$headers.filter('.selected');
        var $tab;

        if (tabSelecting != null) {
            $tab = tabsDom.find(tabSelecting);
        } else {
            $tab = tabsObj.$headers.eq(0);
        }

        if ($tab.length === 0) {
            $tab = tabsObj.$headers.eq(0);
        }

        tabsObj.Show($tab);

        tabsDom.on('click', { tabs: tabsObj }, function (event) {
            var target = $(event.target).closest('[data-tabs-header]');
            var tabsObjPrivate = event.data.tabs;

            if (target.length > 0) {

                tabsObjPrivate.Show(target);

                if (tabsObjPrivate.options.callback) {
                    tabsObjPrivate.options.callback(tabsObjPrivate);
                }
            }

        });
    };

    tabs.prototype.BindEventOne = function () {

        $(document.body).on('click', 'a[href^=#tabid]', function (e) {
            var tabid = tabs.prototype.ParseId($(this).attr("href"));
            var container = $(tabid).closest('[data-plugin="tabs"]');
            var containerid = container.length > 0 ? container.attr('id') : null;

            if (containerid == null) {
                return;
            }

            var tabObj = storage[containerid];
            var $tab = $(tabid);
            tabs.prototype.Show.call(tabObj, tabid);

            $(document).scrollTop($tab.offset().top);

            e.preventDefault();
        });

    };

    tabs.prototype.Show = function (selector) {
        var tabsObj = this;
        var $tab, $content;
        var cssClassSelected = tabsObj.options.classSelected;

        if (typeof selector === 'number') {
            $tab = tabsObj.$headers.eq(selector);
            $content = tabsObj.$contents.eq(selector);
        } else if (typeof selector === 'undefined') {
            $tab = tabsObj.$headers.eq(0);
            $content = tabsObj.$contents.eq(0);
        } else {
            $tab = advantshop.GetJQueryObject(selector);
            var idx = tabsObj.$headers.index($tab);
            $content = tabsObj.$contents.eq(idx);
        }

        if ($tab.length === 0) { // || $tab.hasClass(tabsObj.options.classSelected)
            return $tab;
        }

        tabsObj.$headers.removeClass(cssClassSelected);
        $tab.addClass(cssClassSelected);

        tabsObj.$contents.removeClass(cssClassSelected).hide();
        $content.addClass(cssClassSelected).show();


        if (tabsObj.showAnchor === true && $tab.attr("id") != null) {

            window.location.hash = "#tabid=" + $tab.attr("id");

            ///
            // ensure ASP.NET postback comes back to correct tab
            var aspnetForm = $('#form1')[0];

            if (aspnetForm == null) {
                aspnetForm = $('#aspnetForm')[0];
            }

            if (aspnetForm.action.indexOf('#') >= 0) {
                aspnetForm.action = aspnetForm.action.substr(0, aspnetForm.action.indexOf('#'));
            }
            aspnetForm.action += window.location.hash;

            if (tabStateControl.length > 0) {
                tabStateControl.val($tab.attr("id"));
            }
        }

        tabsObj.showAnchor = true;

        var carousel = $content.find(".jcarousel:visible:not(.jcarousel-list)");

        if ($content.length && carousel.length > 0) {
			$(window).load(function(){
			     carousel.jcarousel({ scroll: 1 });
			});
	    }

        if (window.initValidation != null) {

            var inputs = $content.find('input[type="text"],input[type="password"], textarea'),
                isExistValidElems = false;

            for (var i = 0, l = inputs.length; i < l; i += 1) {
                if (inputs[i].className.indexOf('valid-') != -1) {
                    isExistValidElems = true;
                    break;
                }
            }

            if (isExistValidElems === true) {
                initValidation($('form'));
            }
        }

        return tabsObj;
    };

    tabs.prototype.ParseId = function (str) {
        var tabIdArr = /.*tabid=(.+)/g.exec(str);
        var tabId = tabIdArr ? tabIdArr[1] : null;
        return '#' + tabId;
    };

    tabs.prototype.showAnchor = false;

    tabs.prototype.defaultOptions = {
        classSelected: "selected",
        contentPlaceholderLink: null
    };

})(jQuery);