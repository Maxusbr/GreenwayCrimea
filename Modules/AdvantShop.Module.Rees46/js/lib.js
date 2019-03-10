
function getRees46Products(type, offerid, itemIds, title, relatedType, visibleItems) {
    if (itemIds == null || itemIds.length == 0) {
        return;
    }

    $.ajax({
        dataType: "text",
        cache: false,
        type: "GET",
        url: "catalog/productsbyofferids",
        data: {
            ids: itemIds.join(','),
            title: title,
            type: relatedType,
            offerId: offerid,
            visibleItems: visibleItems
        },
        success: function (data) {
            if (data != null && data.length > 0) {
                var $targetDom = $('.rees46-recommender.' + type),
                htmlToCompile = data;

                var $injector = angular.element(document).injector();

                $injector.invoke(["$compile", "$rootScope", function ($compile, $rootScope) {

                    $targetDom.html(htmlToCompile);

                    var $scope = $targetDom.scope();

                    $compile($targetDom)($scope || $rootScope);
                    $rootScope.$digest();

                    $targetDom.find("a").each(function (index) {
                        var href = $(this).attr("href") + "?recommended_by=" + type;
                        $(this).attr("href", href);
                    });
                }]);
            }
        },
        error: function (data) {
            console.log(data);
        }
    });
}

window.addEventListener('load', function load(event) {
    window.removeEventListener('load', load, false);

    $(document).on("customer.email", function (e, data) {
        try {
            r46('profile', 'set', { email: data.email }); //REES46.pushAttributes({ email: data.email });
        } catch (e) {
        }
    })
    .on("subscribe.email", function (e, email) {
        try {
            r46('profile', 'set', { email: data.email }); //REES46.pushAttributes({ email: data.email });
        } catch (e) {
        }
    });

}, false);

document.addEventListener('DOMContentLoaded', function () {
    $(document).on("cart.add", function (e, offerId, productId, amount, attributesXml) {
        var s = $(e.target.activeElement).closest('.rees46-recommender');
        var typeRecommend_by = null;
        if (s.length > 0 && s[0].classList.length == 2) {
            typeRecommend_by = s[0].classList[1];
        }
        sendingRees46('cart', offerId, productId, amount, typeRecommend_by);
    });
    $(document).on("cart.remove", function (e, offerId) {
        sendingRees46('remove_from_cart', offerId, null, null, null);
    });
});

function sendingRees46(type, offerId, productId, amount, typeRecommend_by) {
    $.ajax({
        type: 'GET',
        async: false,
        dataType: 'json',
        url: 'rees46/getProductForEvent?offerId=' + offerId + "&productId=" + productId + "&amount=" + amount + "&type=" + type + "&recommend_by=" + typeRecommend_by,
        success: function (data) {
            if (data != null) {
                var s = null;
                if (data.Type == 2) {
                    s = data.RecommendedBy == 'none'
                        ? {
                            id: data.Id,
                            amount: data.Amount,
                            stock: data.Stock,
                            name: data.Name,
                            price: data.Price,
                            categories: data.Categories,
                            image: data.Image,
                            url: data.Url
                        }
                        : {
                            id: data.Id,
                            amount: data.Amount,
                            stock: data.Stock,
                            name: data.Name,
                            price: data.Price,
                            categories: data.Categories,
                            image: data.Image,
                            url: data.Url,
                            recommended_by: data.RecommendedBy
                        };
                } else {
                    s = data.Id;
                }
                r46('track', data.Type == 2 ? 'cart' : 'remove_from_cart', s);
            }
        }
    });
}