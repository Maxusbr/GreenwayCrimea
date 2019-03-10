; (function ($) {
    'use strict';

    $(function () {
        var doc = $(document),
            toolbarBottom = $('.js-toolbar-bottom'),
            toolbarBottomCompare = $('.js-toolbar-bottom-compare', toolbarBottom),
            toolbarBottomWishlist = $('.js-toolbar-bottom-wishlist', toolbarBottom),
            toolbarBottomCart = $('.js-toolbar-bottom-cart', toolbarBottom),
            toolbarBottomOrderConfirmation = $('.js-toolbar-bottom-confirm', toolbarBottom),
            toolbarBottomInplace = $('.js-toolbar-bottom-inplace', toolbarBottom);

        doc.on('compare.add compare.remove', function (e, compareData) {
            toolbarBottomCompare.find('.js-toolbar-bottom-count').html(compareData.compare.length);
        });

        doc.on('wishlist.add', function (e, wishlistData) {
            toolbarBottomWishlist.find('.js-toolbar-bottom-count').html(wishlistData.Count);
        });

        doc.on('cart.add cart.update cart.clear cart.remove', function (e, cartData) {

            toolbarBottomCart.find('.js-toolbar-bottom-count').html(cartData.TotalItems);

            if (cartData.TotalItems === 0) {
                toolbarBottomOrderConfirmation.addClass('btn-disabled');
            } else if (cartData.TotalItems > 0) {
                toolbarBottomOrderConfirmation.removeClass('btn-disabled');
            }
        });

        toolbarBottomOrderConfirmation.on('click.toolbarBottomConfirm', function (e) {
            if (toolbarBottomOrderConfirmation.hasClass('btn-disabled') === true) {
                e.preventDefault();
            }
        });

        if (toolbarBottomInplace.length > 0) {

            var htmlTag = $('html');

            toolbarBottomInplace.on('click.toolbarBottomInplace', function () {

                if (toolbarBottomInplace.is(':disabled') === true) {
                    return;
                }

                var progress = Advantshop.ScriptsManager.Progress.prototype.Init(htmlTag);

                progress.Show();

                toolbarBottomInplace.is(':checked') ? htmlTag.addClass('inplace-enabled').removeClass('inplace-disabled') : htmlTag.addClass('inplace-disabled').removeClass('inplace-enabled');

                toolbarBottomInplace.attr('disabled', 'disabled');

                $.ajax({
                    type: 'POST',
                    url: 'admin/httphandlers/inplaceeditor/inplaceenabled.ashx',
                    dataType: 'json',
                    cashe: false,
                    data: { inplaceEnabled: toolbarBottomInplace.is(':checked') },
                    success: function (response) {

                        if ($('[data-page="details"]').length > 0) {
                            window.location.reload(true);
                        } else {
                            response.Enabled === true ? Advantshop.ScriptsManager.Inplace.prototype.InitTotal() : Advantshop.ScriptsManager.Inplace.prototype.DestroyTotal();
                        }
                    },
                    complete: function (error) {
                        progress.Hide();
                        toolbarBottomInplace.removeAttr('disabled');
                    }
                });
            });
        }
    });
})(jQuery);