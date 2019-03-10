; (function (window) {
    'use strict';

    var qazy = {},
        windowLoaded = false,
        inProgress = false,
        resizeFn,
        scrollFn;

    qazy.qazy_image = 'data:image/svg+xml;utf8,<?xml version="1.0" encoding="utf-8"?><svg width="44px" height="44px" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid" class="uil-ring"><rect x="0" y="0" width="100" height="100" fill="none" class="bk"></rect><defs><filter id="uil-ring-shadow" x="-100%" y="-100%" width="300%" height="300%"><feOffset result="offOut" in="SourceGraphic" dx="0" dy="0"></feOffset><feGaussianBlur result="blurOut" in="offOut" stdDeviation="0"></feGaussianBlur><feBlend in="SourceGraphic" in2="blurOut" mode="normal"></feBlend></filter></defs><path d="M10,50c0,0,0,0.5,0.1,1.4c0,0.5,0.1,1,0.2,1.7c0,0.3,0.1,0.7,0.1,1.1c0.1,0.4,0.1,0.8,0.2,1.2c0.2,0.8,0.3,1.8,0.5,2.8 c0.3,1,0.6,2.1,0.9,3.2c0.3,1.1,0.9,2.3,1.4,3.5c0.5,1.2,1.2,2.4,1.8,3.7c0.3,0.6,0.8,1.2,1.2,1.9c0.4,0.6,0.8,1.3,1.3,1.9 c1,1.2,1.9,2.6,3.1,3.7c2.2,2.5,5,4.7,7.9,6.7c3,2,6.5,3.4,10.1,4.6c3.6,1.1,7.5,1.5,11.2,1.6c4-0.1,7.7-0.6,11.3-1.6 c3.6-1.2,7-2.6,10-4.6c3-2,5.8-4.2,7.9-6.7c1.2-1.2,2.1-2.5,3.1-3.7c0.5-0.6,0.9-1.3,1.3-1.9c0.4-0.6,0.8-1.3,1.2-1.9 c0.6-1.3,1.3-2.5,1.8-3.7c0.5-1.2,1-2.4,1.4-3.5c0.3-1.1,0.6-2.2,0.9-3.2c0.2-1,0.4-1.9,0.5-2.8c0.1-0.4,0.1-0.8,0.2-1.2 c0-0.4,0.1-0.7,0.1-1.1c0.1-0.7,0.1-1.2,0.2-1.7C90,50.5,90,50,90,50s0,0.5,0,1.4c0,0.5,0,1,0,1.7c0,0.3,0,0.7,0,1.1 c0,0.4-0.1,0.8-0.1,1.2c-0.1,0.9-0.2,1.8-0.4,2.8c-0.2,1-0.5,2.1-0.7,3.3c-0.3,1.2-0.8,2.4-1.2,3.7c-0.2,0.7-0.5,1.3-0.8,1.9 c-0.3,0.7-0.6,1.3-0.9,2c-0.3,0.7-0.7,1.3-1.1,2c-0.4,0.7-0.7,1.4-1.2,2c-1,1.3-1.9,2.7-3.1,4c-2.2,2.7-5,5-8.1,7.1 c-0.8,0.5-1.6,1-2.4,1.5c-0.8,0.5-1.7,0.9-2.6,1.3L66,87.7l-1.4,0.5c-0.9,0.3-1.8,0.7-2.8,1c-3.8,1.1-7.9,1.7-11.8,1.8L47,90.8 c-1,0-2-0.2-3-0.3l-1.5-0.2l-0.7-0.1L41.1,90c-1-0.3-1.9-0.5-2.9-0.7c-0.9-0.3-1.9-0.7-2.8-1L34,87.7l-1.3-0.6 c-0.9-0.4-1.8-0.8-2.6-1.3c-0.8-0.5-1.6-1-2.4-1.5c-3.1-2.1-5.9-4.5-8.1-7.1c-1.2-1.2-2.1-2.7-3.1-4c-0.5-0.6-0.8-1.4-1.2-2 c-0.4-0.7-0.8-1.3-1.1-2c-0.3-0.7-0.6-1.3-0.9-2c-0.3-0.7-0.6-1.3-0.8-1.9c-0.4-1.3-0.9-2.5-1.2-3.7c-0.3-1.2-0.5-2.3-0.7-3.3 c-0.2-1-0.3-2-0.4-2.8c-0.1-0.4-0.1-0.8-0.1-1.2c0-0.4,0-0.7,0-1.1c0-0.7,0-1.2,0-1.7C10,50.5,10,50,10,50z" fill="#8b9091" filter="url(#uil-ring-shadow)"><animateTransform attributeName="transform" type="rotate" from="0 50 50" to="360 50 50" repeatCount="indefinite" dur="1s"></animateTransform></path></svg>';

    qazy.view_elements = [];

    qazy.reveal = function () {

        var len = qazy.view_elements.length,
            count = len,
            offsetParentTop,
            pageYOffset = window.pageYOffset,
            viewportHeight = window.innerHeight,
            offsetParentLeft,
            pageXOffset = window.pageXOffset,
            viewportWidth = window.innerWidth,
            indexesForRemove = [],
            temp;

        for (var i = 0; i < len; i++) {

            temp = qazy.view_elements[i].obj.getBoundingClientRect();

            offsetParentTop = temp.top + pageYOffset;
            offsetParentLeft = temp.left + pageXOffset;

            if (offsetParentTop > pageYOffset && offsetParentTop < pageYOffset + viewportHeight) { // && offsetParentLeft > pageXOffset && offsetParentLeft < pageXOffset + viewportWidth
                qazy.view_elements[i].obj.src = qazy.view_elements[i].val;
                qazy.view_elements[i] = undefined;
            }
        }

        qazy.view_elements = qazy.view_elements.filter(function (item) {
            return item !== undefined;
        });

        if (qazy.view_elements.length === 0 && windowLoaded === true) {
            window.removeEventListener('resize', qazy.reveal);
            window.removeEventListener('scroll', qazy.reveal);
        }
    };

    qazy.checkLoaded = function (img) {
        // During the onload event, IE correctly identifies any images that
        // weren't downloaded as not complete. Others should too. Gecko-based
        // browsers act like NS4 in that they report this incorrectly.
        if (!img.complete) {
            return false;
        }

        // However, they do have two very useful properties: naturalWidth and
        // naturalHeight. These give the true size of the image. If it failed
        // to load, either of these should be zero.
        if (typeof img.naturalWidth != 'undefined' && img.naturalWidth == 0) {
            return false;
        }

        // No other way of checking: assume it's ok.
        return true;
    };

    //responsible for stopping img loading the image from server and also for displaying lazy loading image.
    qazy.qazy_list_maker = function () {
        var elements = document.querySelectorAll('img[data-qazy]'),
            item;

        for (var i = 0, len = elements.length; i < len; i++) {

            if (qazy.checkLoaded(elements[i]) === true) {
                continue;
            }

            item = {
                obj: elements[i],
                val: elements[i].src
            };

            qazy.view_elements.push(item);

            item.obj.src = item.obj.getAttribute('data-qazy-placeholder') || qazy.qazy_image;
        }
    };


    window.addEventListener('DOMContentLoaded', function domContentLoaded() {
        window.removeEventListener('DOMContentLoaded', domContentLoaded);
        qazy.qazy_list_maker();
    });

    window.addEventListener('load', function load() {
        window.removeEventListener('load', load);
        windowLoaded = true;

        qazy.reveal();

        window.addEventListener('resize', qazy.reveal);
        window.addEventListener('scroll', qazy.reveal);
    });

})(window);