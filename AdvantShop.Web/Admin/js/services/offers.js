(function ($) {
    var storage = {};

    var offers = function (productId) {
        if (productId == null || productId.length === 0) {
            throw Error('Undefined productId');
        }

        if (productId in storage) {
            return storage[productId];
        }

        this.productId = productId;

        this.GetOffers();
    };

    offers.prototype.GetOffers = function () {
        var offersObj = this;
        $.ajax({
            type: 'GET',
            dataType: 'json',
            async: false,
            cache: false,
            url: 'httphandlers/details/offers.ashx',
            data: {
                productId: offersObj.productId
            },
            success: function (data) {

                offersObj.storageOffers = $.extend(offersObj.storageOffers, data);

                storage[offersObj.productId] = offersObj;
            },
            error: function (data) {
                throw Error(data.responseText);
            }
        });

        return storage[offersObj.productId];
    };

    offers.prototype.GetPrice = function (params) {
        var offersObj = this;
        var cOptions = document.getElementById('customOptionsHidden_' + offersObj.productId);
        var result, parameters;

        if (offersObj.storageOffers.offerSelected == null) {
            return '';
        }

        parameters = $.extend({
            attributesXml: cOptions != null ? cOptions.value : null,
            offerId: offersObj.storageOffers.offerSelected.OfferId
        }, params || {});

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: 'httphandlers/details/offerprice.ashx',
            data: parameters,
            async: false,
            success: function (data) {
                result = data;
            },
            error: function (data) {
                throw Error(data.responseText);
            }
        });

        return result;
    };

    offers.prototype.GetFirstPaymentPrice = function (minPrice) {
        var offersObj = this;
        var result;

        if (offersObj.storageOffers.offerSelected == null) {
            return '';
        }

        var cOptions = document.getElementById('customOptionsHidden_' + offersObj.productId);

        var fPPercent = 0;
        if (document.getElementById('hfFirstPaymentPercent') != null) {
            fPPercent = document.getElementById('hfFirstPaymentPercent').value;
        }

        params = {
            price: offersObj.storageOffers.offerSelected.Price,
            discount: offersObj.storageOffers.offerSelected.Discount,
            attributesXml: cOptions != null ? cOptions.value : null,
            productId: offersObj.productId,
            firstPaymentPercent: fPPercent,
            minPrice: minPrice
        };


        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: 'httphandlers/details/offerfirstpaymentPrice.ashx',
            data: params,
            async: false,
            success: function (data) {
                result = data.Price;
            },
            error: function (data) {
                throw Error(data.responseText);
            }
        });

        return result;
    };

    offers.prototype.UpdateProduct = function (productId, params) {

        var offersObj;

        var isInplaceEnabled = Advantshop.ScriptsManager.Inplace != null ? Advantshop.ScriptsManager.Inplace.prototype.checkEnabled() : false;

        if (productId != null) {
            offersObj = storage[productId];
        } else {
            offersObj = this;
        }

        var storageOffersCurrent = offersObj.storageOffers;

        if (storageOffersCurrent.offerSelected == null && storageOffersCurrent.Offers.length > 0) {
            storageOffersCurrent.offerSelected = storageOffersCurrent.Offers[0];
        }

        var priceContainer = document.getElementById('priceWrap'),
            firstPaymentPercentContainer = document.getElementById('lblFirstPayment'),
            bonusPriceContainer = document.getElementById('lblProductBonus'),
            firstPaymentNote = document.getElementById('lblFirstPaymentNote'),
            sku = document.getElementById('skuValue'),
            btn = document.getElementById('btnAdd'),
            btnBuyInOneClick = document.getElementById('lBtnBuyInOneClick'),
            btnOrderByRequest = document.getElementById('btnOrderByRequest'),
            btnAddCredit = document.getElementById('btnAddCredit'),
            spanAvailability = document.getElementById('availability');

        //get new price
        var priceUpdated = offers.prototype.GetPrice.call(offersObj, params);

        storageOffersCurrent.offerSelected.Price = priceUpdated.PriceNumber;

        //price update
        if (priceContainer != null) {

            $(priceContainer).html(priceUpdated.PriceString);

            if (isInplaceEnabled === true) {

                var pricesNumInplace = $(priceContainer).find('[data-plugin="inplace"]');
                var priceNumInplace, inplaceParams, inplaceOptions, inplaceCustomOptionsInp, inplaceCustomOptions;

                for (var p = 0, pl = pricesNumInplace.length; p < pl; p += 1) {
                    priceNumInplace = pricesNumInplace.eq(p);

                    inplaceParams = priceNumInplace.attr('data-inplace-params');

                    if (inplaceParams != null) {

                        inplaceParams = inplaceParams.replace(/'/g, '"');
                        inplaceParams = JSON.parse(inplaceParams);

                        inplaceParams.id = storageOffersCurrent.offerSelected.OfferId;

                        inplaceCustomOptionsInp = document.getElementById('customOptionsHidden_' + storageOffersCurrent.offerSelected.ProductId);
                        inplaceCustomOptions = inplaceCustomOptionsInp != null && inplaceCustomOptionsInp.value.length > 0 ? inplaceCustomOptionsInp.value : null;

                        if (inplaceCustomOptions != null) {
                            inplaceParams.customOptions = inplaceCustomOptions;
                        } else {
                            delete inplaceParams.customOptions;
                        }

                        inplaceParams = JSON.stringify(inplaceParams);

                        inplaceParams = inplaceParams.replace(/"/g, '\'');

                        priceNumInplace.attr('data-inplace-params', inplaceParams);

                        inplaceParams = Advantshop.Utilities.Eval(priceNumInplace.attr('data-inplace-params')) || {};
                        inplaceOptions = Advantshop.Utilities.Eval(priceNumInplace.attr('data-inplace-options')) || {};


                        if (Advantshop.ScriptsManager.Inplace != null) {
                            Advantshop.ScriptsManager.Inplace.prototype.Init(priceNumInplace, inplaceParams, inplaceOptions);
                        }

                    }
                }
            }


        }

        if (bonusPriceContainer != null) {
            $(bonusPriceContainer).html(priceUpdated.Bonuses);
        }

        if (firstPaymentPercentContainer != null) {

            var minPrice;

            if (btnAddCredit != null) {
                minPrice = parseFloat(btnAddCredit.getAttribute('data-cart-minprice'));
            };

            if (isNaN(minPrice) === false) {
                firstPaymentPercentContainer.innerHTML = offers.prototype.GetFirstPaymentPrice.call(offersObj, minPrice);
                firstPaymentPercentContainer.style.display = firstPaymentPercentContainer.innerHTML.length > 0 ? 'inline-block' : 'none';

                if (firstPaymentNote != null) {
                    firstPaymentNote.style.display = firstPaymentPercentContainer.innerHTML.length > 0 ? 'inline-block' : 'none';
                }
            }
        }

        if (sku != null && storageOffersCurrent.offerSelected != null) {
            sku.innerHTML = storageOffersCurrent.offerSelected.ArtNo;
        }

        var isAvalable = storageOffersCurrent.offerSelected.Amount > 0,
            isCanBuy = storageOffersCurrent.offerSelected.Price > 0,
            isCanPreorder = offersObj.storageOffers.AllowPreOrder;

        if (spanAvailability != null) {
            if (storageOffersCurrent.offerSelected.Amount > 0) {
                if (storageOffersCurrent.ShowStockAvailability) {
                    spanAvailability.innerHTML = localize("detailsAvailable") +
                        " (<span class=\"js-details-offer-amount\">" + storageOffersCurrent.offerSelected.Amount + "</span>" + (storageOffersCurrent.Unit != "" ? " " + storageOffersCurrent.Unit : "") + ")";
                } else {
                    spanAvailability.innerHTML = localize("detailsAvailable");
                }
                spanAvailability.setAttribute("class", "available");
            } else {
                spanAvailability.innerHTML = localize("detailsNotAvailable");
                spanAvailability.setAttribute("class", "not-available");
            }


            //if (storageOffersCurrent.ShowStockAvailability) {
            //    var formatStr = storageOffersCurrent.Unit == "" ? "{0} <span>{1}{2}</span>" : "{0} <span>({1} {2})</span>";
            //    spanAvalable.innerHTML = String.Format(formatStr, localize("productAvailable"), storageOffersCurrent.offerSelected.Amount, storageOffersCurrent.Unit);
            //}
        }

        ///if price  === 0
        if (isCanBuy === false || isAvalable === false) {
            if (btn != null) {
                btn.style.display = 'none';
            }

            if (btnBuyInOneClick != null) {
                btnBuyInOneClick.style.display = 'none';
            }
            if (btnAddCredit != null) {
                btnAddCredit.style.display = 'none';
            }
            if (isCanPreorder == true) {
                if (btnOrderByRequest != null) {
                    btnOrderByRequest.setAttribute('data-offerid', storageOffersCurrent.offerSelected.OfferId); // for codebehaind
                    btnOrderByRequest.style.display = 'inline-block';
                }
            } else {
                if (btnOrderByRequest != null) {
                    btnOrderByRequest.style.display = 'none';
                }
            }
        } else {
            if (btn != null) {
                btn.setAttribute('data-cart-add-offerid', storageOffersCurrent.offerSelected.OfferId);
                btn.setAttribute('data-offerid', storageOffersCurrent.offerSelected.OfferId); // for codebehaind
                btn.style.display = isAvalable === true ? 'inline-block' : 'none';
            }

            if (btnBuyInOneClick != null) {
                btnBuyInOneClick.setAttribute('data-buyoneclick-offerid', storageOffersCurrent.offerSelected.OfferId);
                btnBuyInOneClick.setAttribute('data-offerid', storageOffersCurrent.offerSelected.OfferId); // for codebehaind
                btnBuyInOneClick.style.display = isAvalable === true ? 'inline-block' : 'none';
            }

            if (btnAddCredit != null) {
                btnAddCredit.setAttribute('data-cart-add-offerid', storageOffersCurrent.offerSelected.OfferId);
                btnAddCredit.setAttribute('data-offerid', storageOffersCurrent.offerSelected.OfferId); // for codebehaind
                btnAddCredit.style.display = isAvalable === true && priceUpdated.PriceNumber > parseFloat(btnAddCredit.getAttribute('data-cart-minprice')) ? 'inline-block' : 'none';
            }

            if (btnOrderByRequest != null) {
                btnOrderByRequest.style.display = 'none';
            }

        }

    };

    offers.prototype.GetStorage = function (productId) {
        return storage[productId] || storage;
    };

    Advantshop.Offers = offers;

})(jQuery);