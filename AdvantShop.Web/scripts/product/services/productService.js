; (function (ng) {
    'use strict';

    var productService = function ($http, $q, modalService) {
        var service = this,
            _product,
            callbacks = {};

        service.getOffers = function (productId, colorId, sizeId) {
            return $http.get('product/getoffers', { params: { productId: productId, colorId: colorId, sizeId: sizeId } }).then(function (response) {
                return response.data;
            });
        }

        service.findOfferSelected = function (offers, offerIdSelected) {
            var offer;

            for (var i = offers.length - 1; i >= 0; i--) {
                if (offers[i].OfferId === offerIdSelected) {
                    offer = offers[i];
                    break;
                }
            }

            return offer;
        };

        service.findOffersByColorId = function (offers, colorId) {
            return offers.filter(function (item) {
                return colorId != null && item.Color != null && item.Color.ColorId === colorId;
            });
        };

        service.findOffersBySizeId = function (offers, sizeId) {
            return offers.filter(function (item) {
                return sizeId != null && item.Size != null && item.Size.SizeId === sizeId;
            });
        };

        service.getOffer = function (offers, colorId, sizeId, allowPreOrder) {
            var arrayOffers = offers.slice(),
                arrayOffersByColor = [],
                arrayOffersBySize = [],
                stopLoop = false,
                offer;

            arrayOffersByColor = service.findOffersByColorId(arrayOffers, colorId);
            arrayOffersBySize = service.findOffersBySizeId(arrayOffers, sizeId);

            if (arrayOffersByColor.length > 0 && arrayOffersBySize.length > 0) {

                for (var i = 0, lenC = arrayOffersByColor.length; i < lenC; i++){
                    for (var j = 0, lenS = arrayOffersBySize.length; j < lenS; j++){
                        if (arrayOffersByColor[i].OfferId === arrayOffersBySize[j].OfferId) {
                            offer = arrayOffersByColor[i];
                            stopLoop = true;
                            break;
                        }
                    }

                    if (stopLoop === true) {
                        break;
                    }
                }
            }

            if (offer == null && arrayOffersByColor.length > 0) {
                offer = arrayOffersByColor[0];
            }
            if (offer == null && arrayOffersBySize.length > 0) {
                offer = arrayOffersBySize[0];
            }

            return offer;
        };

        service.getPrice = function (offerId, attributesXml) {
            return $http.post('product/getofferprice', { offerId: offerId, attributesXml: attributesXml }).then(function (response) {
                return response.data;
            });
        }

        service.getFirstPaymentPrice = function (price, discount, discountAmount) {
            return $http.get('product/getfirstpaymentprice', { params: { price: price, discount: discount, discountAmount: discountAmount } }).then(function (response) {
                return response.data;
            });
        };

        service.getShippings = function (offerId) {
            return $http.get('product/getshippings', { params: { offerId: offerId } }).then(function (response) {
                return response.data;
            });
        };

        service.addCallback = function (name, func) {
            callbacks[name] = callbacks[name] || [];
            callbacks[name].push(func);
        };

        service.processCallback = function (name, data) {

            var arrFunc = callbacks[name];

            if (arrFunc != null && arrFunc.length > 0) {
                for (var i = 0, len = arrFunc.length; i < len; i++) {
                    arrFunc[i](data);
                }
            };
        };

        service.getPhoto = function (url) {
            var defered = $q.defer(),
                img = new Image();

            img.src = url;

            if (img.complete == true || typeof img.naturalWidth !== "undefined" && img.naturalWidth > 0) {
                defered.resolve(img);
            } else {
                img.onload = function (e) {
                    defered.resolve(img);
                };
            }

            return defered.promise.then(function (response) {
                return response;
            });

        };

        service.addToStorage = function (product) {
            _product = product;
        };

        service.getProduct = function () {
            return _product;
        };
    };

    ng.module('product')
      .service('productService', productService);

    productService.$inject = ['$http', '$q', 'modalService'];

})(window.angular);