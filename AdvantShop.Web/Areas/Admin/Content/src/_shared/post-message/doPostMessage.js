; (function () {
    'use strict';


    document.addEventListener('DOMContentLoaded', function () {

        window.addEventListener('message', function (event) {

            try {
                var postData = JSON.parse(event.data);
                //scrollTop

                if (postData.name !== null && postData.name === 'scrollTop') {
                    window.scrollTo(0, 0);
                }


                //Jivosite
                if (postData.name !== null && postData.name === 'openChat') {
                    jivo_api.open();
                    setTimeout(function () {
                        document.querySelector("#jivo-iframe-container").classList.add('shown');
                    }, 30);

                }

                //Модальное окно (затемнение)
                if (postData.name !== null && postData.name === 'removeModalBackground') {
                    var iframeWrap = document.querySelector('.js-iframe-wrap'),
                        iframe = document.querySelector('iframe'),
                        modalBackground = document.querySelector('.post-modal-background');

                    if (iframeWrap !== null && modalBackground !== null && iframe !== null) {
                        iframe.classList.remove('iframe');
                        iframeWrap.removeChild(modalBackground);
                    }

                }

                //Открыть модальное окно
                if (postData.name !== null && postData.name === 'openModal') {

                    var el = document.createElement('div'),
                        iframe = document.querySelector('iframe'),
                        iframeWrap = document.querySelector('.js-iframe-wrap');

                    if (iframeWrap !== null && iframe !== null) {

                        iframe.classList.add('iframe');
                        el.classList.add('post-modal-background');
                        el.addEventListener('click', function removeBackgroundModal(e) {
                            var btn = this;
                            iframe = document.querySelector('iframe');
                            if (iframe !== null) {
                                var iframeContent = iframe.contentWindow;
                                iframeContent.postMessage('closeModal', '*');
                            }
                            iframe.classList.remove('iframe');
                            iframeWrap.removeChild(el);
                            el.removeEventListener('click', removeBackgroundModal);

                        });

                        iframeWrap.appendChild(el);

                    }

                }

                //Высота Iframe
                if (postData.name !== null && postData.name === 'iframeHeight') {

                    window.postMessage('iframeHeight', '*');
                    var iframe = document.querySelector('iframe');

                    if (iframe !== null) {
                        iframe.style.height = (postData.height + 'px');
                    }
                }


            }
            catch (e) {

                //отступ модального окна в support 
                if (event.data === 'openSupportModel') {
                    var iframe = document.querySelector('iframe');

                    if (iframe !== null) {
                        var iframeContent = iframe.contentWindow;
                        iframeContent.postMessage(JSON.stringify({
                            name: 'modalPosition',
                            windowScrollHeight: window.pageYOffset
                        }), '*');
                        //var overlayAdmin = document.querySelector('.js-iframe-modal-overlay');
                        //if (overlayAdmin != null) {
                        //    overlayAdmin.classList.add('show');
                        //}
                    }
                    
                }

                if (event.data !== 'tariffs') {
                    return;
                }
                var iframe = document.querySelector('iframe');
                if (iframe !== null) {
                    iframe.addEventListener('load', function (e) {
                        window.scrollTo(0, 0);
                    });
                }


            }

        }, false);


        doPostMessage();
        
        window.addEventListener('resize', function () {
            doPostMessage();
        });


    });

    function doPostMessage() {
        var iframe = document.querySelector('iframe');

        if (iframe !== null) {
            var iframeContent = iframe.contentWindow;
            iframeContent.postMessage('readyPost', '*');
        }
    }
})();