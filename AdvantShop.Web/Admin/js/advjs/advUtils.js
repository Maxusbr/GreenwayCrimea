function CheckOrder(ordernum) {
    var status;
    $.ajax({
        async: false,
        dataType: "json",
        cache: false,
        url: "httphandlers/checkorder.ashx",
        data: { OrderNum: ordernum },
        success: function (data) {
            status = data;
        },
        error: function () {
            status = null;
        }
    });
    return status;
}

function VoteRating(productId, rating) {
    var result;
    $.ajax({
        async: false,
        dataType: "json",
        cache: false,
        url: "httphandlers/addrating.ashx",
        data: { productId: productId, rating: rating },
        success: function (data) {
            result = data;
        },
        error: function () {
            result = null;
        }
    });
    return result;
}

function VoteRatingCustomer(id, rating) {
    var result;
    $.ajax({
        async: false,
        dataType: "json",
        cache: false,
        url: "httphandlers/customer/setcustomerrating.ashx",
        data: { id: id, rating: rating },
        success: function (data) {
            result = data;
        },
        error: function () {
            result = null;
        }
    });
    return result;
}

function htmlEncode(value) {
    return $('<div/>').text(value || "").html();
}

function htmlDecode(value) {
    return $('<div/>').html(value || "").text();
}

function encodeFormData() {
    $('.toencode').each(function () {
        $(this).val(htmlEncode($(this).val()));
    });
}
function decodeFormData() {
    $('.toencode').each(function () {
        $(this).val(htmlDecode($(this).val()));
    });
}

var timeout;
function ApplyFilter(sender, needRedirect, showPopup, refreshPrices) {

    var currentFilter = window.location.href;
    currentFilter = ChangeQueryParam(currentFilter, "page", "");

    if ($("#filter-price .slider").length) {
        var curMin = $("div.slider").find(".min").val();
        var curMax = $("div.slider").find(".max").val();

        if (curMin !== $("div.slider").attr('data-current-min') || curMax !== $("div.slider").attr('data-current-max')) {
            currentFilter = ChangeQueryParam(currentFilter, "pricefrom", curMin);
            currentFilter = ChangeQueryParam(currentFilter, "priceto", curMax);
        }
    }

    if ($("#property-filter").length) {

        $(".slider", '#property-filter').each(function () {

            var min = $(this).find(".min").val(),
                max = $(this).find(".max").val(),
                prop = $(this).parents(".content-price").attr("id").replace("filter-pricerange-", "");

            if (min === $(this).attr('data-current-min') && max === $(this).attr('data-current-max')) {
                return;
            }

            currentFilter = ChangeQueryParam(currentFilter, "prop_" + prop + "_min", min);
            currentFilter = ChangeQueryParam(currentFilter, "prop_" + prop + "_max", max);
        });

        var properties = "";
        var name = "";
        $("input:checked", '#property-filter').each(function () {
            if (!name.length) {
                name = $(this).attr("name");
            }
            if (name != $(this).attr("name")) {
                properties = properties.slice(0, -1);
                properties += "-" + $(this).val() + ",";
                name = $(this).attr("name");
            } else {
                properties += $(this).val() + ",";
            }
        });

        $("select", '#property-filter').each(function () {
            if ($(this).val() != "0") {
                if (!name.length) {
                    name = $(this).attr("name");
                }
                if (name != $(this).attr("name")) {
                    properties = properties.slice(0, -1);
                    properties += "-" + $(this).val() + ",";
                    name = $(this).attr("name");
                } else {
                    properties += $(this).val() + ",";
                }
            }
        });

        if (properties.length) properties = properties.slice(0, -1);
        currentFilter = ChangeQueryParam(currentFilter, "prop", properties);
    }

    if ($("#brand-filter").length) {
        var brands = "";
        $("input:checked", '#brand-filter').each(function () {
            brands += $(this).val() + ",";
        });

        if (brands.length) brands = brands.slice(0, -1);

        currentFilter = ChangeQueryParam(currentFilter, "brand", brands);
    }

    if ($("#size-filter").length) {
        var sizes = "";
        $("input:checked", '#size-filter').each(function () {
            sizes += $(this).val() + ",";
        });

        if (sizes.length) sizes = sizes.slice(0, -1);

        currentFilter = ChangeQueryParam(currentFilter, "size", sizes);
    }

    if ($("#color-filter").length) {
        var colors = "";
        $("input:checked", '#color-filter').each(function () {
            colors += $(this).val() + ",";
        });

        if (colors.length) colors = colors.slice(0, -1);

        currentFilter = ChangeQueryParam(currentFilter, "color", colors);
    }


    if ($("#ddlSort").length) {
        var sorting = $("#ddlSort").val();
        currentFilter = ChangeQueryParam(currentFilter, "sort", sorting != "NoSorting" ? sorting : null);
    }

    if ($("#ddlCategory").length) {
        var category = $("#ddlCategory").val();
        currentFilter = ChangeQueryParam(currentFilter, "category", category != "0" ? category : null);
    }

    if ($("#txtName").length) {
        var searchName = $("#txtName").val();
        currentFilter = ChangeQueryParam(currentFilter, "name", searchName != "" ? encodeURIComponent(searchName) : null);
    }

    if ($("#cbAvailable").length) {
        currentFilter = ChangeQueryParam(currentFilter, "available", $("#cbAvailable").is(":checked") ? "1" : "0");
    }

    if ($("#cbPreorder").length) {
        currentFilter = ChangeQueryParam(currentFilter, "preorder", $("#cbPreorder").is(":checked") ? "1" : "0");
    }
    

    if (needRedirect) {
        if (currentFilter.length) window.location = currentFilter;
    } else {
        currentFilter = ChangeQueryParam(currentFilter, "ajax", "1");

        $.ajax({
            dataType: "json",
            cache: false,
            url: currentFilter,
            async: false,
            success: function (data) {
                if (data.ProductsCount > 0) {
                    $("#filter-price .slider").each(function () {
                        if (refreshPrices) {
                            $(this).data("prices", { from: data.AvaliblePriceFrom, to: data.AvaliblePriceTo });
                        }
                        makeMagic($(this));
                    });
                }

                if (showPopup) {
                    var productsFound = $("#productsFound");

                    if (!productsFound.length) {
                        productsFound = $("<div />", {
                            id: "productsFound",
                        }).addClass("producsFound");

                        $("body").append(productsFound);
                    }

                    productsFound.html(localize("catalogItemsFound") + " <span id=\"productsFoundCount\">" +
                            data.ProductsCount + "</span>" +
                            (data.ProductsCount > 0 ? "<div class='btn-wrap-products-found'><a id='aShowFilter' class='btn btn-buy btn-small' href='" + currentFilter + "'>" + localize("catalogShow") + "</a></div>" : ""));

                    $('#aShowFilter', productsFound).attr("href", ChangeQueryParam(currentFilter, "ajax", null));

                    var senderTop = $(sender).offset().top;
                    var filterWidth = $('#filter').outerWidth();
                    var filterLeft = $('#filter').offset().left;

                    var productsFoundSize = { width: productsFound.outerWidth(true), height: productsFound.outerHeight() };

                    var productsFoundPosition = {};

                    if (filterLeft > productsFoundSize.width) {
                        productsFoundPosition.left = filterLeft - productsFoundSize.width;
                    } else {
                        productsFoundPosition.left = filterLeft + filterWidth;
                    }

                    productsFoundPosition.top = senderTop - (productsFoundSize.height / 2);

                    productsFound.css(productsFoundPosition);

                    productsFound.stop(true, true).fadeIn(500);

                    if (timeout) {
                        clearTimeout(timeout);
                    }

                    timeout = setTimeout(function () {
                        $(".producsFound").stop(true, true).fadeOut(500);
                    }, 5000);


                }
            },
            error: function (data) {
                notify("Error in filter", notifyType.error, true);
            }
        });

    }
}

function makeMagic(divSlider) {

    var minPrice = divSlider.slider("option", "min");
    var maxPrice = divSlider.slider("option", "max");
    var currentMin = (divSlider.slider("option", "values")[0] - minPrice) / ((maxPrice - minPrice) / 100);
    var currentMax = (divSlider.slider("option", "values")[1] - minPrice) / ((maxPrice - minPrice) / 100);

    var limeLeft = (divSlider.data("prices").from - minPrice) / maxPrice * 100;
    var limeWidth = (divSlider.data("prices").to - minPrice) / ((maxPrice - minPrice) / 100) - limeLeft;

    var greenMinLeft = limeLeft;
    var greenMinWidth = currentMin < limeLeft ? 0 : currentMin > limeLeft + limeWidth ? limeLeft + limeWidth : currentMin - limeLeft;

    var greenMaxLeft = currentMax > limeLeft + limeWidth ? limeLeft + limeWidth : currentMax > limeLeft ? currentMax : limeLeft;
    var greenMaxWidth = currentMax > limeLeft + limeWidth ? 0 : limeLeft + limeWidth - (currentMax > limeLeft ? currentMax : limeLeft);

    $(".sliderCustom").remove();

    $(".ui-slider-range.ui-widget-header").after("<div class=\"sliderCustom color-background\" style='left:" + greenMinLeft + "%; width:" + greenMinWidth + "%;'></div>");
    $(".ui-slider-range.ui-widget-header").after("<div class=\"sliderCustom color-background\" style='left:" + greenMaxLeft + "%; width:" + greenMaxWidth + "%;'></div>");
    $(".ui-slider-range.ui-widget-header").after("<div class=\"sliderCustom color-background-selected\" style='left:" + limeLeft + "%; width: " + limeWidth + "%;'></div>");
}



function ClearFilter() {
    //    var currentFilter = window.location.href;
    //    currentFilter = ChangeQueryParam(currentFilter, "pricefrom", null);
    //    currentFilter = ChangeQueryParam(currentFilter, "priceto", null);
    //    currentFilter = ChangeQueryParam(currentFilter, "prop", null);
    //    currentFilter = ChangeQueryParam(currentFilter, "brand", null);
    //    currentFilter = ChangeQueryParam(currentFilter, "color", null);
    //    currentFilter = ChangeQueryParam(currentFilter, "size", null);
    //    if (currentFilter.length) window.location = currentFilter;
    window.location = window.location.href.split('?')[0];
}

function ApplySearch() {
    var currentSearch = window.location.href;
    currentSearch = ChangeQueryParam(currentSearch, "page", "");

    var slider = $("#filter-price .slider");
    if (slider.length) {
        var curMin = parseInt(slider.find(".min").val());
        var curMax = parseInt(slider.find(".max").val());
        
        if (curMin !== parseInt(slider.attr('data-current-min')) || curMax !== parseInt(slider.attr('data-current-max'))) {
            currentSearch = ChangeQueryParam(currentSearch, "pricefrom", curMin);
            currentSearch = ChangeQueryParam(currentSearch, "priceto", curMax);
        }

        
    }

    if ($("#ddlSort").length) {
        var sorting = $("#ddlSort").val();
        currentSearch = ChangeQueryParam(currentSearch, "sort", sorting != "NoSorting" ? sorting : null);
    }

    if ($("#txtName").length) {
        var name = $("#txtName").val();
        currentSearch = ChangeQueryParam(currentSearch, "name", name != "" ? encodeURIComponent(name) : null);
    }

    if ($("#txtSKU").length) {
        var sku = $("#txtSKU").val();
        currentSearch = ChangeQueryParam(currentSearch, "sku", sku != "" ? encodeURIComponent(sku) : null);
    }

    if ($("#ddlCategory").length) {
        var category = $("#ddlCategory").val();
        currentSearch = ChangeQueryParam(currentSearch, "category", category != "0" ? category : null);
    }

    if (currentSearch.length) window.location = currentSearch;
}

function searchNow() {
    var searchtext = $("#txtSearch").val();
    if (searchtext && searchtext != $("#txtSearch").attr("placeholder")) {
        var url = String.Format("{0}search.aspx?name={1}", $("base").attr("href"), encodeURIComponent(searchtext));
        window.location.href = url;
    }
}

function redirectBrandCountry() {
    if ($("#ddlCountry option:selected").val() != 0) {
        window.location = String.Format("{0}manufacturers?country={1}", $("base").attr("href"), encodeURIComponent($("#ddlCountry option:selected").text()));
    } else {
        window.location = String.Format("{0}manufacturers", $("base").attr("href"));
    }
}

function ChangeQueryParam(query, name, value) {
    query = query.toString();
    //parse query and to object
    var params = {};

    var anchoridx = query.indexOf("#");
    var anchor = "";

    if (anchoridx != -1) {
        anchor = query.substring(anchoridx);
        query = query.replace(anchor, "");
    }

    var queryNew = query;
    queryNew.replace(/(\w+)=(.+?)(&|$)/g, function (substr, key, val) {
        params[key] = val;
    });

    if (!params || !name.length) return false;

    //manupulation param
    if (value && value.toString().length) {
        params[name.toString()] = value;
    } else {
        delete params[name.toString()];
    }

    //object to string
    queryNew = "?";
    $.each(params, function (key, item) {
        queryNew += key + "=" + item + "&";
    });

    queryNew = queryNew.slice(0, -1);

    return query.split("?")[0] + queryNew + anchor;
}

function ChangeCurrency(ddl) {
    $.ajax({
        async: false,
        dataType: "json",
        cache: false,
        url: "httphandlers/shoppingcart/setcurrency.ashx",
        data: { ISO3: $(ddl).val() },
        success: function () {
            window.location.reload(true);
        },
        error: function () {
            alert("Currency error");
        }
    });
}

function DisplayFilter(visibleBlocks, visibleLines) {
    var btn = $("<div>", { html: "<a class=\"link-nice\" href=\"javascript:void(0)\">" + localize("utilsShowAll") + "</a>" }).addClass("btn-all-look");
    var blocks = $(".propList, .brandList, .size-filter, .color-filter", "#filter");
    blocks.each(function (idx) {

        var items = $(this).children("div, label");
        var itemsInput = items.find("input");
        var itemsInputSelected = itemsInput.filter(":checked:last");
        var slice;

        if (itemsInputSelected.length || idx == 0) {
            slice = idx == 0 ? visibleLines : itemsInput.index(itemsInputSelected) + 1;
            $(this).closest(".content").show().prev(".expander-control").removeClass("expander-collapse").addClass("expander-expand");
        } else {
            slice = visibleLines;
        }

        if (items.length > slice) {
            var wrapper = $("<div>", { html: items.slice(slice) }).addClass("prop-hidden");
            $(this).append(wrapper);
            $(this).append(btn.clone());
        }
    });

    var propertyBlock = $("#property-filter", "#filter");
    var properties = propertyBlock.children("div");

    if (properties.length > visibleBlocks) {
        var wrapperProperties = $("<div>", { html: properties.slice(visibleBlocks) }).addClass("prop-hidden");
        propertyBlock.append(wrapperProperties);
        propertyBlock.append(btn.clone());
    }

    $("#filter").click(function (e) {
        var target = $(e.target);

        if (target.is("a.link-nice") && target.closest("div.btn-all-look").length) {
            var content = target.closest("div.btn-all-look").prev("div.prop-hidden");

            if (content.is(":visible")) {
                content.slideUp("fast", function () { target.text(localize("utilsShowAll")); });
            } else {
                content.slideDown("fast", function () { target.text(localize("utilsHide")); });
            }
            return false;
        }
        return true;
    });




    if (document.getElementById('color-filter') != null) {
        var colorContainer = $('#color-filter'),
         colorLabel = colorContainer.find('.filter-color'),
         colorInput = colorLabel.find('input:checkbox');

        for (var i = 0, arrLength = colorLabel.length; i < arrLength; i += 1) {
            if (colorInput.eq(i).is(':checked') === true) {
                colorLabel.eq(i).addClass('selected');
            }
        }

        colorContainer.on('click', { items: $('input:checkbox', '#color-filter'), parents: $('.filter-color', '#color-filter') }, function (e) {
            var target = $(e.target);
            var dataFilter = e.data;
            var elIndex = dataFilter.items.index(target);

            if (elIndex != -1) {

                if (target.is(':checked') === true) {
                    dataFilter.parents.eq(elIndex).addClass('selected');
                } else {
                    dataFilter.parents.eq(elIndex).removeClass('selected');
                }
            }

        });
    }



}

function defaultButtonClick(buttonId, event) {
    var btn = $("#" + buttonId);
    event = event || window.event;


    event.stopPropagation ? event.stopPropagation() : event.cancelBubble = true;

    if (!btn.length || getKeyCode(event) != 13) {
        return;
    }

    //btn.trigger("focus");

    //btn.trigger("click");
    
    var href = btn.attr("href") || "";

    if (href.indexOf("doPostBack") != -1) {
        window.location = href;
    }
    btn.click();
    return false;
}

function getKeyCode(e) {
    var keycode = null;

    if (window.event) keycode = window.event.keyCode;
    else if (e) keycode = e.which;

    return keycode;
}

function enableButton(btnId, enabled) {
    var btn = $("#" + btnId);
    if (enabled) {
        btn.removeClass("btn-disabled");
        if (btn.attr("onClickOld")) {
            btn.attr("onClick", btn.attr("onClickOld")).removeAttr("onClickOld");
        } else {
            btn.removeAttr("onClick");
        }
    } else {
        btn.addClass("btn-disabled");
        if (btn.attr("onClick")) {
            btn.attr("onClickOld", btn.attr("onClick")).attr("onClick", "return false;");
        } else {
            btn.attr("onClick", "return false;");
        }
    }
}

function open_printable_version(link) {
    var win = "menubar=no,location=no,resizable=yes,scrollbars=yes";
    var newWin = window.open(link, 'printableWin', win);
    newWin.focus();
}

function getStingPrice(price, func) {
    $.ajax({
        type: "POST",
        async: false,
        url: "httphandlers/getstringprice.ashx",
        data: { price: price },
        dataType: "html",
        success: function (data) {
            if (func) {
                func(data);
            }
            return data;
        },
        error: function (data) {
            notify(localize("giftcertificate") + " status text:" + data.statusText, notifyType.error, true);
        }
    });
}
