$(document).ready(function () {
    var topBlock = $(".toolbar-top");
    var bannerBlock = $("#bannerInTop");

    if (topBlock.length > 0 && bannerBlock.length > 0) {
        bannerBlock.insertAfter(topBlock);
        bannerBlock.css('margin-top', '0px');
        bannerBlock.css('display', 'inline-block');
    }

    var bannerEntities = $('[data-bannermania-entity-id]');

    bannerEntities.each(function () {

        var placementType = $(this).attr('data-bannermania-entity-placement');
        var placementBlock = null;

        switch (placementType) {
            case 'UnderDeliveryInfo':
            case 'AboveDeliveryInfo':
                placementBlock = $('.details-aside');
                if (placementBlock.length > 0) {

                    if (placementType == "UnderDeliveryInfo") {
                        $(this).insertAfter(placementBlock);
                    }
                    else {
                        $(this).insertBefore(placementBlock);
                    }
                }
                break;

            case 'UnderFilter':
            case 'AboveFilter':
                placementBlock = $('[data-catalog-filter]');
                if (placementBlock.length > 0) {
                    if (placementType == "UnderFilter") {
                        $(this).insertAfter(placementBlock);
                    }
                    else {
                        $(this).insertBefore(placementBlock);
                    }
                }
                else {
                    placementBlock = $('.menu-dropdown.menu-dropdown-modern.menu-dropdown-expanded');
                    if (placementBlock.length > 0) {
                        $(this).insertAfter(placementBlock);
                    }
                }
                break;

            case 'AboveFooter':
                placementBlock = $('.site-footer');
                if (placementBlock.length > 0) {
                    $(this).insertBefore(placementBlock);
                }
                break;

            case 'UnderMenu':
                placementBlock = $('.menu-block');
                if (placementBlock.length > 0) {
                    $(this).insertAfter(placementBlock);
                }
                break;
        }

        if (placementBlock.length > 0) {
            $(this).removeAttr('style');
            $(this).addClass('bm-banner');
        }
    });
})