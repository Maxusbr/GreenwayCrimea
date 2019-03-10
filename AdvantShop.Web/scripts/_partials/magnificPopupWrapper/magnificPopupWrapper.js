; (function (ng) {
    'use strict';

    ng.module('magnificPopup', [])
        .constant('magnificPopupDefault', {
            type: "image", //There is no any “auto-detection” of type based on URL, so you should define it manually
            zoom: {
                enabled: true,
                duration: 300,
                easing: 'ease-in-out', // CSS transition easing function 
                // The "opener" function should return the element from which popup will be zoomed in
                // and to which popup will be scaled down
                // By defailt it looks for an image tag:
                opener: function (openerElement) {
                    // openerElement is the element on which popup was initialized, in this case its <a> tag
                    // you don't need to add "opener" option if this code matches your needs, it's default one.
                    return openerElement.is('img') ? openerElement : openerElement.find('img');
                }
            },

        });

})(window.angular);