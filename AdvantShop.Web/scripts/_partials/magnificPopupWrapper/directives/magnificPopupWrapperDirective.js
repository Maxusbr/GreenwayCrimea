; (function (ng) {
    'use strict';

    //full magnificAPI at: http://dimsemenov.com/plugins/magnific-popup/

    function getType(path) {
        return ['.jpg', '.png', '.gif', '.bmp'].some(function (val) {
            return path.toLowerCase().indexOf(val) !== -1;
        });
    }

    function getIndex(array, item) {
        for (var i = 0, len = array.length; i < len; i++) {
            if (array[i] === item) {
                return i;
            }
        }

        return 0;
    }


    var magnificPopupWrapper = ['magnificPopupDefault', '$parse', function (magnificPopupDefault, $parse) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {

                var delegate = element[0].nodeName === "A" ? null : 'a';

                if (attrs.plugin === "fancybox") { //TODO: Delete this fallback when old fancybox syntax lose relevance
                    if (element[0].nodeName == "A") { //if it's a single href
                        var href = element[0].getAttribute('href'),
                            rel = element[0].getAttribute('rel'),
                            itemsForGroup,
                            itemsObj = [],
                            isImage,
                            options;

                        if (href == null || href.length === 0) {
                            return;
                        }


                        if (rel != null && rel.length > 0) {

                            itemsForGroup = document.querySelectorAll('[rel="' + rel + '"]');

                            Array.prototype.slice.call(itemsForGroup).forEach(function (item) {
                                itemsObj.push({
                                    type: 'image',
                                    src: item.getAttribute('href'),
                                    title: item.getAttribute('title')
                                });
                            });
                        }

                        isImage = getType(href);

                        options = {
                            type: isImage === true ? 'image' : 'inline',
                            mainClass: isImage === true ? 'mfp-custom-image' : 'mfp-custom-inline'
                        }

                        if (itemsObj.length > 0) {
                            options.index = getIndex(itemsForGroup, element[0]);
                            options.items = itemsObj;
                            options.gallery = {
                                enabled: true
                            };
                        }

                        element.magnificPopup(options);
                    }
                    else { //if it's a <div> with <a> to make some galery
                        element.magnificPopup({
                            delegate: delegate,
                            type: magnificPopupDefault.type,
                            mainClass: 'mfp-with-zoom', // this class is for CSS animation below
                            zoom: magnificPopupDefault.zoom,
                            gallery: {
                                enabled: true
                                //preload: [0, 2]
                            }
                        });
                    }
                } else { //TODO: Delete this fallback when old fancybox syntax lose relevance
                    element.magnificPopup(ng.extend({
                        delegate: delegate,
                        type: 'image',
                        mainClass: 'mfp-custom'
                    }, $parse(attrs.magnificPopupOptions)(scope)));
                }

            }
        };
    }];

    ng.module('magnificPopup')
      .directive('plugin', magnificPopupWrapper) //TODO: Delete this fallback when old fancybox syntax lose relevance
      .directive('magnificPopup', magnificPopupWrapper);

})(window.angular);