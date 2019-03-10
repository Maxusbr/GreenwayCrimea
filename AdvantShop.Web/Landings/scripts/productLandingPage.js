; (function () {
    'use strict';

    var init = function () {

        window.removeEventListener('load', init);

        setTimeout(function () {

            var moveblock = document.getElementById('moveblock'),
                containerFirst = document.getElementById('containerFirst'),
                containerSecond = document.getElementById('containerSecond'),
                clentRect = moveblock.getBoundingClientRect(),
                moveblockSize = {
                    top: clentRect.top + window.pageYOffset,
                    bottom: clentRect.bottom + window.pageYOffset,
                    height: clentRect.height
                },
                currentPlace;

            containerFirst.style.height = moveblockSize.height + 'px';
            containerSecond.style.height = moveblockSize.height + 'px';

            window.addEventListener('scroll', function () {
                if (window.pageYOffset >= moveblockSize.bottom) {
                    if (currentPlace !== 'second') {
                        containerSecond.appendChild(moveblock);
                        currentPlace = 'second';
                    }
                } else if (currentPlace !== 'first') {
                    containerFirst.appendChild(moveblock);
                    currentPlace = 'first';
                }
            });
        }, 2000)
    };

    window.addEventListener('load', init);

})();