function ready() {
    var prevNextProductId = -1;
    var counter = 0;
    var productViews = document.getElementsByClassName('products-view-item');

    if (productViews.length > 0) {
        var productIds = [];
        [].forEach.call(productViews, function (val) { productIds.push(val.dataset.productId) });
    }

    var elems = document.getElementsByClassName('products-view-quickview');

    [].forEach.call(elems, function (elem) {
        elem.onclick = function (e) {
            setEvent();
            quickMarkers(e.target.parentElement.dataset.productId);
        }
    })

    function setEvent() {
        var next = document.getElementsByClassName('quickview-arrows-next');
        var prev = document.getElementsByClassName('quickview-arrows-prev');
        if (next.length > 0) {
            next[0].onclick = prevNextProduct;
        } else {
            setTimeout(setEvent, 300);
        }
        if (prev.length > 0) {
            prev[0].onclick = prevNextProduct;
        } else {
            setTimeout(setEvent, 300);
        }
    }

    function prevNextProduct() {
        var modals = document.getElementsByClassName('modal-content');
        var i = -1;
        [].forEach.call(modals, function (val, index) {
            var len = val.querySelectorAll('[data-ng-include="quickview.url"]');
            if (len.length > 0) {
                i = index;
            }
        });
        var p = true;

        if (i >= 0) {
            var btn = modals.length === 0 ? undefined : modals[i].getElementsByClassName('btn');
            var loadBtn = btn !== undefined ? btn[0].dataset.productId : 0;
            p = loadBtn === prevNextProductId;
        }
        if (counter === 10) {
            counter = 0;
            return;
        }
        if (p) {
            counter++;
            setTimeout(prevNextProduct, 300);
        } else {
            prevNextProductId = loadBtn;
            var markersWrap = document.getElementById('markers-wrapper');
            if (markersWrap !== null) {
                var markersGallery = document.getElementsByClassName('gallery-picture-labels');

                while (markersWrap.children.length > 0) {
                    markersGallery[0].appendChild(markersWrap.children[0]);
                }

                markersWrap.parentNode.removeChild(markersWrap);
            }
        }
    }

    function quickMarkers(productId) {
        prevNextProductId = productId;
        var modals = document.getElementsByClassName('modal-content');
        var i = -1;

        [].forEach.call(modals, function (val, index) {
            var len = val.querySelectorAll('[data-ng-include="quickview.url"]');
            if (len.length > 0) {
                i = index;
            }
        });

        var p = true;

        if (i >= 0) {
            var btn = modals.length === 0 ? undefined : modals[i].getElementsByClassName('btn');
            var loadBtn = btn !== undefined ? btn[0].dataset.productId : 0;
            p = loadBtn !== productId;
        }

        if (modals.length === 0 || p) {
            setTimeout(quickMarkers, 300, productId);
        } else {
            var markersWrap = document.getElementById('markers-wrapper');
            if (markersWrap !== null) {
                var markersGallery = document.getElementsByClassName('gallery-picture-labels');

                while (markersWrap.children.length > 0) {
                    markersGallery[0].appendChild(markersWrap.children[0]);
                }

                markersWrap.parentNode.removeChild(markersWrap);
            }
        }
    }

    if (location.pathname.indexOf('/products/') == -1) {
        return;
    }

    var markersWrap = document.getElementById('markers-wrapper');
    var markersGallery = document.getElementsByClassName('gallery-picture-labels');

    while (markersWrap.children.length > 0) {
        markersGallery[0].appendChild(markersWrap.children[0]);
    }

    markersWrap.parentNode.removeChild(markersWrap);

}

document.addEventListener("DOMContentLoaded", ready);

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function setCookie(name, value, options) {
    options = options || {};

    var expires = options.expires;

    if (typeof expires == "number" && expires) {
        var d = new Date();
        d.setTime(d.getTime() + expires * 1000);
        expires = options.expires = d;
    }
    if (expires && expires.toUTCString) {
        options.expires = expires.toUTCString();
    }

    value = encodeURIComponent(value);

    var updatedCookie = name + "=" + value;

    for (var propName in options) {
        updatedCookie += "; " + propName;
        var propValue = options[propName];
        if (propValue !== true) {
            updatedCookie += "=" + propValue;
        }
    }

    document.cookie = updatedCookie;
}