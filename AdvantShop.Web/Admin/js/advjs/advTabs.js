(function (jQuery) {
    jQuery.fn.advTabs = function (o) {
        var options = jQuery.extend({
            headers: "div.tabs-headers > div.tab-header",
            contents: "div.tabs-contents > div.tab-content",
            classSelected: "selected",
            contentPlaceholderLink: null,
            callbackOpen: null,
            callbackClose: null
        }, o);

        function tabShow(obj, showAnchor, callbackOpen, callbackClose, sender) {

            if (!obj) return;

            var container = $(this).data("tabs");
            var hidden = $("input[type=hidden].tabid");
            if (!container) return;

            var headers = container.headers,
			   contents = container.contents,
			   headerItem,
			   contentItem,
			   carousel;
            
            if (!isNaN(parseInt(obj))) {
                headerItem = $(headers[obj]);
                contentItem = $(contents[obj]);
            }
            else {
                headerItem = $(obj);
                var idx = headers.index(headerItem);
                contentItem = $(contents[idx]);
            }

            if (sender && sender.is("a[href^=#tabid]")) {
                $(document).scrollTop(headerItem.offset().top);
            }

            //if (!headerItem.length || headerItem.hasClass(options.classSelected)) return;

            headers.removeClass(options.classSelected);
            contents.removeClass(options.classSelected);

            if (callbackClose) {
                callbackClose.apply(container);
            }

            headerItem.addClass(options.classSelected);
            contentItem.addClass(options.classSelected);

            if (headerItem.attr("id") && showAnchor) {
                window.location.hash = "#tabid=" + headerItem.attr("id");
            }

            if (hidden.length && showAnchor) {
                hidden.val(headerItem.attr("id"));
            }

            if (callbackOpen) {
                callbackOpen.apply(container);
            }

            carousel = contentItem.find(".jcarousel:visible:not(.jcarousel-list)");

            if (carousel.length) {
                carousel.jcarousel({ scroll: 1 });
            }

            this.trigger('tabs:show', [this, headerItem, contentItem]);
        }

        function parseID(str) {
            var tabId = /.*tabid=(.+)/g.exec(str);

            if (!tabId) return false;

            return tabId[1];
        }

        return this.each(function () {
            var container = $(this),
                headers = container.find(options.headers),
                contents = container.find(options.contents);

            if (headers.length != contents.length) {
                throw new Error("Tab's headers count not equals tab's  blocks count");
            }

            //api
            $.extend(container, {
                tabShow: function (obj, showanchor, callbackOpen, callbackClose) { tabShow.apply(container, [obj, showanchor, callbackOpen, callbackClose]); }
            });

            var countRemove = 0;
            headers.each(function (idx) {
                //remove empty tab
                var contentText = $.trim($(contents[idx]).html().replace(/<br\s*[\/]?>/gi, ""));
                if ($(this).hasClass("tab-hidden") || !contentText.length) {
                    $(this).remove();
                    $(contents[idx]).remove();
                    countRemove++;
                }
            });

            //if items = 0 remove tab-container
            if (countRemove == headers.length) {
                container.remove();
                return;
            }

            if (countRemove > 0) {
                headers = container.find(options.headers),
			    contents = container.find(options.contents);
            }

            container.headers = headers;
            container.contents = contents;
            container.data("tabs", container);

            container.click(function (e) {
                var target = $(e.target).closest(options.headers);
                if (!target.length)
                    return;
                tabShow.apply(container, [target, true, options.callbackOpen, options.callbackClose]);
            });


            var tabId = $("input[type=hidden].tabid").val() || parseID(window.location.href);

            if (tabId && container.find("#" + tabId).length) {
                tabShow.apply(container, ["#" + tabId, false]);
            } else {
                tabShow.apply(container, [$(headers[0]), false]);
            }


            if (options.contentPlaceholderLink && $(options.contentPlaceholderLink).length) {
                var headersLink = headers.filter("[id]");

                if (headersLink.length) {
                    var list = "<div class=\"tabs-links\">";
                    var row = "<div><a href=\"#tabid={0}\">{1}</a></div>";
                    headersLink.each(function () {
                        list += String.Format(row, this.id, $(this).text());
                    });
                    list += "</div>";
                    $(options.contentPlaceholderLink).append(list);
                }
            }

            $("a[href^=#tabid]").click(function (e) {
                var tab = parseID($(this).attr("href"));
                tabShow.apply(container, ["#" + tab, true, null, null, $(e.target)]);
                e.cancelBubble = true;
                return false;
            });
        });
    };
})(jQuery);

