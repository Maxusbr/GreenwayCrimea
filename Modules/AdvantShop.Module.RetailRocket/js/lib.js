window.addEventListener('load', function load(event) {
    window.removeEventListener('load', load, false);

    if ($(".rr-products").length > 0) {

        var blocks = $(".rr-products");

        for (var i = 0; i < blocks.length; i++) {

            var block = $(blocks[i]),
                ids = block.attr("data-ids"),
                title = block.attr("data-title"),
                type = block.attr("data-type"),
                visibleItems = block.attr("data-visibleitems");

            getProducts(block, ids, title, type, visibleItems);
        }
    }

    function getProducts(block, ids, title, type, visibleItems) {
        $.ajax({
            dataType: "text",
            cache: false,
            type: "GET",
            url: "catalog/productsbyids",
            data: {
                ids: ids,
                title: title,
                visibleItems: visibleItems,
            },
            success: function (data) {
                if (data != null && data.length > 0) {

                    var $targetDom = block,
                    htmlToCompile = data;

                    var $injector = angular.element(document).injector();

                    $injector.invoke(["$compile", "$rootScope", function ($compile, $rootScope) {

                        $targetDom.html(htmlToCompile);

                        var $scope = $targetDom.scope();

                        $compile($targetDom)($scope || $rootScope);
                        $rootScope.$digest();

                        $targetDom.find("a").each(function (index) {
                            var offerId = $(this).parents(".products-view-item").attr("data-offer-id");

                            if (offerId != null && offerId != "undefined") {
                                var attr = "try { rrApi.recomMouseDown(" + offerId + ", {methodName: '" + type + "'}) } catch(e) {}";
                                $(this).attr("onmousedown", attr);
                            }
                        });

                        $targetDom.find("[data-cart-add]").each(function (index) {
                            var offerId = $(this).parents(".products-view-item").attr("data-offer-id");

                            if (offerId != null && offerId != "undefined") {
                                var attr = "try { rrApi.recomAddToCart(" + offerId + ", {methodName: '" + type + "'}) } catch(e) {}";
                                $(this).attr("onmousedown", attr);
                            }
                        });
                    }]);
                }
            }
        });
    }

    // add events on product page
    if (document.location.pathname.indexOf("/products/") == 0 && typeof (rrApi) != "undefined") {

        $(".products-view-block").each(function () {

            var targetDom = $(this);

            targetDom.find("a").each(function () {
                var offerId = $(this).parents(".products-view-item").attr("data-offer-id");

                if (offerId != null && offerId != "undefined") {
                    var attr = "try { rrApi.recomMouseDown(" + offerId + ") } catch(e) {}";
                    $(this).attr("onmousedown", attr);
                }
            });

            targetDom.find("[data-cart-add]").each(function () {
                var offerId = $(this).parents(".products-view-item").attr("data-offer-id");

                if (offerId != null && offerId != "undefined") {
                    var attr = "try { rrApi.recomAddToCart(" + offerId + ") } catch(e) {}";
                    $(this).attr("onmousedown", attr);
                }
            });
        });
    }

    $(document).on("cart.add", function(e, offerId, productId, amount, attributesXml) {
            try {
                if (offerId != 0) {
                    rrApi.addToBasket(offerId);
                }
            } catch (e) {
            }
        })
        .on("customer.email", function(e, data) {
            try {
                rrApiOnReady.push(function() { rrApi.setEmail(data.email); });
            } catch (e) {
            }
        })
        .on("subscribe.email", function (e, email) {
            try {
                rrApiOnReady.push(function() { rrApi.setEmail(email); });
            } catch (e) {
            }
        });

}, false);



