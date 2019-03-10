
window.notifyType = { error: 0, notify: 1 };

function notify(message, type, isDb, options) {
    var opt = {
        speed: 700,
        timer: 10000,
        showContainer: false
    };

    $.extend(opt, options);

    var notify = $("#notify");

    if (!notify.length) {
        notify = $("<div>", { id: "notify" });
        $("form").append(notify);
    }

    var item = $("<div>");
    var close = $("<div>").addClass("close");

    if (message && message.length) {

        item.html(message);
        item.addClass("notify-item " + (type == notifyType.error ? "type-error " : "type-notice "));

        item.append(close);
        notify.append(item);

        if (notify.is(":hidden")) {
            notify.show();
        }

        notifyShow(item);
    }

    if (isDb && type == notifyType.error) {
        $.ajax({
            type: "POST",
            data: { message: message },
            dataType: "text",
            url: "httphandlers/debug/debugjs.ashx"
        });
    }

    $("div.notify-item", notify).bind("mouseleave", function () {
        notifyShow($(this));
    });

    $("div.notify-item", notify).bind("mouseenter", function () {
        notifyFreeze($(this));
    });

    $("div.close", notify).bind("click", function () {
        notifyHide($(this).closest("div.notify-item"));
    });

    var items = notify.children("div.notify-item");

    if (items.text().length && opt.showContainer) {
        notify.slideDown(opt.speed);
        items.show().mouseenter();
    }


    function notifyFreeze(el) {
        if (el.data("timer")) {
            clearTimeout(el.data("timer"));
        }
    }

    function notifyShow(el) {

        el.slideDown(opt.speed);

        if (el.data("timer")) {
            clearTimeout(el.data("timer"));
        }

        var timer = setTimeout(function () { notifyHide(el); }, opt.timer);

        el.data("timer", timer);
    }

    function notifyHide(el) {
        if (el.data("timer")) {
            clearTimeout(el.data("timer"));
        }

        el.slideUp(opt.speed, function () {
            el.remove();
        });
    }
}

