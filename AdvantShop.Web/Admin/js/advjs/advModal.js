(function (jQuery) {

    var modalsTempStorage = [];

    jQuery.extend({
        advModal: function(o) {
            var options = jQuery.extend({
                modalClass: null,
                modalId: null,
                title: null,
                htmlContent: null,
                buttons: null,
                isEnableBackground: true,
                opacityBackground: 0.4,
                opacityModal: 1,
                control: null,
                controlEvent: "click",
                clickOut: true,
                forceShow: false,
                cross: true,
                closeEsc: true,
                enableTouch: true,
                positionShow: true,
                controlBeforeOpen: null,
                controlAfterOpen: null,
                funcCross: null,
                beforeOpen: null,
                afterOpen: null,
                beforeClose: null,
                afterClose: null,
                hideError: true,
                initCallback: null
            }, o);

            var modal = $("<div>").addClass("modal"), modalBackground = $("<div>").addClass("modal-background");

            if (options.isEnableBackground && !$("div.modal-background").length)
                $("#form, #aspnetForm").append(modalBackground);

            if (options.cross)
                modal.append("<div class='modal-close cross'></div>");

            if (options.title)
                modal.append("<div class='title'></div>");

            if (options.htmlContent)
                modal.append("<div class='content'></div>");

            if (options.buttons)
                modal.append("<div class='btns'></div>");

            $("#form, #aspnetForm").append(modal);

            if (options.modalClass) modal.addClass(options.modalClass);
            if (options.modalId) modal.attr("id", options.modalId);

            if (options.isEnableBackground && !modalBackground.length)
                modalBackground = $("div.modal-background");

            if (options.isEnableBackground) {
                modalBackground = modal.prev("div.modal-background");
                modalBackground.css({ opacity: options.opacityBackground });
            }

            var title = modal.children("div.title"),
                content = modal.children("div.content"),
                btnsContainer = modal.children("div.btns");

            //title
            if (title.length)
                title.append(options.title);
            //content
            if (content.length)
                content.append(options.htmlContent);

            //generate btn
            if (btnsContainer.length) {
                var htmlBtns = "";
                $.each(options.buttons, function(idx, el) {
                    htmlBtns += "<a href='javascript:void(0)' class='btn btn-middle btn-modal ";
                    if (el.classBtn)
                        htmlBtns += el.classBtn + "'>";
                    else
                        htmlBtns += "'>";
                    if (el.textBtn)
                        htmlBtns += el.textBtn + "</a>";
                });
                btnsContainer.html(htmlBtns);
            }

            //bindEvent
            if (btnsContainer.length) {
                var btns = btnsContainer.children("a.btn");
                btns.each(function(idx, el) {
                    if (options.buttons[idx].func != null)
                        $(el).on('click', { modal: modal }, options.buttons[idx].func);
                    if (options.buttons[idx].isBtnClose == true)
                        $(el).click(function() {
                            modalClose.apply(modal);
                            return false;
                        });
                });
            }
            if (options.control) {
                var elem = $(options.control);
                var resultControlBeforeOpen = true;


                if (typeof(options.control) === 'string') {
                    $(document.body).on(options.controlEvent, options.control, function(event) {
                        modalControlEvent(event, modal);
                    })
                } else {
                    elem.on(options.controlEvent, function(event) {
                        modalControlEvent(event, modal);
                    });
                }
            }

            if (options.clickOut) {
                $("body").on('click.modalClickOut', function(e) {
                    var isModalOnClick = $(e.target).closest(modal).length;

                    if (!isModalOnClick && modal.is(":visible")) {
                        modalClose.apply(modal);
                    }

                });
            }
            if (options.closeEsc) {
                $(document).keyup(function(e) {
                    if (modal.is(":visible") && e.keyCode == 27) {
                        modalClose.apply(modal);
                    }
                });
            }


            modal.find("div.modal-close").click(function() {
                modalClose.apply(modal);
                if (options.funcCross)
                    (options.funcCross).apply(modal);
                return false;
            });
            $(window).resize(
                function() {
                    if (modal.is(":visible")) modalPosition.apply(modal);
                }
            );

            //position modal	
            modalPosition.apply(modal);

            if (window.jQuery.IsTouchDevice && jQuery.IsTouchDevice()) {
                var modalOffset, docHeight;

                modal.addClass("m-touch");

                if (modalBackground.length > 0)
                    modalBackground.addClass("mb-touch");

                $(window).scroll(function() {
                    modalMobilePos.apply(modal);
                });
            }
            //api
            $.extend(modal, {
                modalShow: function(callback, forceShow) {
                    modalShow.apply(modal, [callback, null, forceShow]);
                },
                modalClose: function(callback) {
                    modalClose.apply(modal, [callback]);
                },
                modalTitle: function(content, callback) {
                    modalTitle.apply(modal, [content, callback]);
                },
                modalContent: function(content, callback) {
                    modalContent.apply(modal, [content, callback]);
                },
                modalBtns: function(content, callback) {
                    modalBtns.apply(modal, [content, callback]);
                },
                modalPosition: function(callback) {
                    modalPosition.apply(modal, [callback]);
                },
                modalSetClickOut: function(isEnabled) {
                    if (isEnabled === false) {
                        $("body").off('click.modalClickOut');
                    } else {
                        $("body").on('click.modalClickOut', function(e) {
                            var isModalOnClick = $(e.target).closest(modal).length;

                            if (!isModalOnClick && modal.is(":visible")) {
                                modalClose.apply(modal);
                            }
                        });
                    }
                }
            }, { options: options });

            if (options.initCallback) {
                options.initCallback();
            }


            modal.data('modal', modal);

            return modal;
            //functions

            function modalClose(callback) {
                var modalPrivate = $(this);
                if (modalPrivate.is(":visible")) {
                    if (options.beforeClose) (options.beforeClose).apply(modalPrivate, [modalPrivate]);


                    var modalsSiblings = $('.modal:visible').not(modalPrivate),
                        isBgShowedSiblings = false;


                    for (var i = 0, il = modalsSiblings.length; i < il; i += 1) {
                        if (modalsSiblings.eq(i).data('modal').options.isEnableBackground) {
                            isBgShowedSiblings = true;
                            break;
                        }
                    }

                    if (options.isEnableBackground && isBgShowedSiblings === false) {
                        modalPrivate.prevAll("div.modal-background").hide();
                    }

                    modal.hide();

                    if (callback) callback.apply(modalPrivate);


                    if (modalsTempStorage.length > 0) {
                        var temp = modalsTempStorage[0];
                        modalsTempStorage.splice(0, 1);

                        temp.data('modal').modalShow();
                    }

                }
            }

            function modalShow(callback, event, forceShow) {
                event = event || window.event;
                if (event) {
                    if (event.stopPropagation) {
                        event.stopPropagation();
                    } else {
                        event.cancelBubble = true;
                    }
                }

                var modalPrivate = $(this);

                if (options.forceShow === false && ($('.modal:visible').not(modalPrivate).length > 0 || $('.js-bubble-zone:visible').length > 0) && (forceShow == false || forceShow == null)) {
                    modalsTempStorage.push(modalPrivate);
                    return;
                }

                if (modalPrivate.is(":hidden")) {
                    if (options.beforeOpen) (options.beforeOpen).apply(modalPrivate, [modalPrivate]);

                    if (options.isEnableBackground)
                        modalPrivate.prevAll("div.modal-background").show();


                    if (options.hideError) {
                        var elems = $("input,select,textrea").not(":submit, :reset, :image, [disabled]").filter(function() {
                            return (this.className.indexOf("valid-") != -1);
                        });
                        elems.nextAll(".error-message").hide();
                    }

                    modalPrivate.show();

                    if (options.positionShow) modalPosition.apply(modalPrivate);

                    if (options.afterOpen) {
                        (options.afterOpen).apply(modalPrivate, [modalPrivate, event]);
                    }

                    if (callback) callback.apply(modalPrivate);

                }
            }

            function modalTitle(newContent, callback) {

                if (!newContent) newContent = " ";

                var modalPrivate = $(this);

                if (!title.length) {
                    modalPrivate.prepend("<div class='title'></div>");
                    title = modalPrivate.children("div.title");
                }

                title.html(newContent);

                if (callback)
                    callback.apply(modalPrivate, [title]);
            }

            function modalContent(newContent, callback) {

                if (!newContent) newContent = " ";

                var modalPrivate = $(this),
                    content = modalPrivate.find('.content'),
                    title = modalPrivate.find('.title');

                if (content.length === 0) {

                    content = $('<div />', { 'class': 'content' });

                    if (title.length > 0) {
                        title.after(content);
                    } else {
                        modalPrivate.prepend(content);
                    }
                }

                content.html(newContent);

                if (callback) callback.apply(modalPrivate, [content]);
            }

            function modalBtns(arrayBtn) {
                var modalPrivate = $(this);
                var btnsContainer = modalPrivate.children("div.btns");

                if (!arrayBtn && btnsContainer.length) btnsContainer.empty();


                if (!btnsContainer.length) {
                    modalPrivate.append("<div class='btns'></div>");
                    btnsContainer = modalPrivate.children("div.btns");
                } else {
                    btnsContainer.empty();
                }

                var htmlBtns = "";

                $.each(arrayBtn, function(idx, el) {
                    htmlBtns += "<a href='javascript:void(0);' class='btn  btn-modal ";
                    if (el.classBtn)
                        htmlBtns += el.classBtn + "'>";
                    else
                        htmlBtns += "'>";
                    if (el.textBtn)
                        htmlBtns += el.textBtn + "</a>";
                });

                btnsContainer.html(htmlBtns);

                var btns = btnsContainer.children("a.btn");
                btns.each(function(idx, el) {
                    if (arrayBtn[idx].func != null)
                        $(el).click(arrayBtn[idx].func);
                    if (arrayBtn[idx].isBtnClose == true)
                        $(el).click(function() { modalClose.apply(modalPrivate); });
                });
            }

            function modalPosition(callback) {
                //reset
                modal.css({
                    left: 0,
                    top: 0,
                });


                var modalPrivate = $(this),
                    widthModal = modalPrivate.outerWidth(),
                    heightModal = modalPrivate.outerHeight(),
                    bodyWidth = $(window).width(),
                    bodyHeight = $(window).height(),
                    left = 0,
                    top = 0;

                //check iframe embed
                if (window != window.top && window.VK != null && window.VK.addCallback != null) {
                    // I'm in a frame!
                    VK.addCallback('onScrollTop', function(vkScrollTop, vkWindowHeight, vkMarginApp, isVkWindowActive) {


                        if (window.jQuery.IsTouchDevice && jQuery.IsTouchDevice()) {
                            modalMobilePos.apply(modalPrivate);
                        } else {
                            left = (bodyWidth - widthModal) / 2;
                            top = (vkWindowHeight - vkMarginApp - heightModal + vkScrollTop) / 2;

                            modal.css({
                                left: left,
                                top: top,
                                opacity: options.opacityModal
                            });
                        }

                        if (callback) callback.apply(modalPrivate);
                    });

                    VK.callMethod('scrollTop');

                } else {


                    if (window.jQuery.IsTouchDevice && jQuery.IsTouchDevice()) {
                        modalMobilePos.apply(modalPrivate);
                    } else {
                        left = (bodyWidth - widthModal) / 2;
                        top = (bodyHeight - heightModal) / 2;

                        modal.css({
                            left: left,
                            top: top,
                            opacity: options.opacityModal
                        });
                    }

                    if (callback) callback.apply(modalPrivate);
                }


            }

            function modalControlEvent(event, modal) {
                if (modal.is(":hidden")) {
                    if (options.controlBeforeOpen) {
                        resultControlBeforeOpen = (options.controlBeforeOpen).apply(this, [modal, event]);

                        if (resultControlBeforeOpen === false) {
                            return;
                        }
                    }

                    modalShow.apply(modal, [null, event]); //null - callback

                    if (options.controlAfterOpen)
                        (options.controlAfterOpen).apply(modal, [modal, event]);
                }
            }

            function modalMobilePos() {
                var modal = $(this),
                    modalBg = options.isEnableBackground === true ? $("div.modal-background") : [],
                    modalOffset = modal.offset(),
                    widthModal = modal.outerWidth(),
                    bodyWidth = $(window).width();

                modal.css({
                    top: $(window).scrollTop(),
                    left: (bodyWidth - widthModal) / 2,
                    opacity: options.opacityModal
                });

                if (modalBg.length > 0) {
                    modalBg.css({ height: $(document).height() });
                }

            }

        }
    });


    jQuery.advModal.reopen = function () {

        if (modalsTempStorage.length > 0 && $('.modal:visible').length === 0) {
            modalsTempStorage[0].data('modal').modalShow();
            modalsTempStorage.splice(0, 1);
        }
    };

    jQuery.advModal.isQueueEmpty = function () {
        return modalsTempStorage.length === 0;
    };

    jQuery.advModal.countModalActive = function () {
        return $('.modal:visible').length;
    };
})(jQuery);


