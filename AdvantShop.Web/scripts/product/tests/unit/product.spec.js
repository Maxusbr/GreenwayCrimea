'use strict';

describe('js.tests.units: product', function () {

    var responseWithColorsAndSizes = { "Offers": [{ "OfferId": 12455, "ProductId": 970, "ArtNo": "970-1", "Color": { "ColorId": 8, "ColorName": "Розовый", "ColorCode": "#ff5c71", "SortOrder": 60, "IconFileName": { "ImageWidthDetails": 18, "ImageHeightDetails": 18, "ImageWidthCatalog": 18, "ImageHeightCatalog": 18, "PhotoId": 0, "ObjId": 0, "Type": 13, "PhotoName": "", "ModifiedDate": "0001-01-01T00:00:00", "Description": null, "PhotoSortOrder": 0, "Main": false, "OriginName": null, "ColorID": null } }, "Size": { "SizeId": 1, "SizeName": "S", "SortOrder": 1000 }, "RoundedPrice": 4400.0, "Discount": 0.0, "Amount": 10.0, "AmountBuy": 1.0, "Main": true, "IsAvailable": true, "Available": "Есть в наличии (<div class=\"details-avalable-text \" >10</div> <div class=\"details-avalable-unit \" ></div>)" }, { "OfferId": 12456, "ProductId": 970, "ArtNo": "970-2", "Color": { "ColorId": 4, "ColorName": "Черный", "ColorCode": "#000000", "SortOrder": 0, "IconFileName": { "ImageWidthDetails": 18, "ImageHeightDetails": 18, "ImageWidthCatalog": 18, "ImageHeightCatalog": 18, "PhotoId": 0, "ObjId": 0, "Type": 13, "PhotoName": "", "ModifiedDate": "0001-01-01T00:00:00", "Description": null, "PhotoSortOrder": 0, "Main": false, "OriginName": null, "ColorID": null } }, "Size": { "SizeId": 1, "SizeName": "S", "SortOrder": 1000 }, "RoundedPrice": 4400.0, "Discount": 0.0, "Amount": 10.0, "AmountBuy": 1.0, "Main": false, "IsAvailable": true, "Available": "Есть в наличии (<div class=\"details-avalable-text \" >10</div> <div class=\"details-avalable-unit \" ></div>)" }, { "OfferId": 12457, "ProductId": 970, "ArtNo": "970-3", "Color": { "ColorId": 8, "ColorName": "Розовый", "ColorCode": "#ff5c71", "SortOrder": 60, "IconFileName": { "ImageWidthDetails": 18, "ImageHeightDetails": 18, "ImageWidthCatalog": 18, "ImageHeightCatalog": 18, "PhotoId": 0, "ObjId": 0, "Type": 13, "PhotoName": "", "ModifiedDate": "0001-01-01T00:00:00", "Description": null, "PhotoSortOrder": 0, "Main": false, "OriginName": null, "ColorID": null } }, "Size": { "SizeId": 2, "SizeName": "M", "SortOrder": 1010 }, "RoundedPrice": 4400.0, "Discount": 0.0, "Amount": 10.0, "AmountBuy": 1.0, "Main": false, "IsAvailable": true, "Available": "Есть в наличии (<div class=\"details-avalable-text \" >10</div> <div class=\"details-avalable-unit \" ></div>)" }, { "OfferId": 12458, "ProductId": 970, "ArtNo": "970-4", "Color": { "ColorId": 4, "ColorName": "Черный", "ColorCode": "#000000", "SortOrder": 0, "IconFileName": { "ImageWidthDetails": 18, "ImageHeightDetails": 18, "ImageWidthCatalog": 18, "ImageHeightCatalog": 18, "PhotoId": 0, "ObjId": 0, "Type": 13, "PhotoName": "", "ModifiedDate": "0001-01-01T00:00:00", "Description": null, "PhotoSortOrder": 0, "Main": false, "OriginName": null, "ColorID": null } }, "Size": { "SizeId": 2, "SizeName": "M", "SortOrder": 1010 }, "RoundedPrice": 4400.0, "Discount": 0.0, "Amount": 10.0, "AmountBuy": 1.0, "Main": false, "IsAvailable": true, "Available": "Есть в наличии (<div class=\"details-avalable-text \" >10</div> <div class=\"details-avalable-unit \" ></div>)" }, { "OfferId": 12459, "ProductId": 970, "ArtNo": "970-5", "Color": { "ColorId": 8, "ColorName": "Розовый", "ColorCode": "#ff5c71", "SortOrder": 60, "IconFileName": { "ImageWidthDetails": 18, "ImageHeightDetails": 18, "ImageWidthCatalog": 18, "ImageHeightCatalog": 18, "PhotoId": 0, "ObjId": 0, "Type": 13, "PhotoName": "", "ModifiedDate": "0001-01-01T00:00:00", "Description": null, "PhotoSortOrder": 0, "Main": false, "OriginName": null, "ColorID": null } }, "Size": { "SizeId": 4, "SizeName": "XL", "SortOrder": 1030 }, "RoundedPrice": 4400.0, "Discount": 0.0, "Amount": 10.0, "AmountBuy": 1.0, "Main": false, "IsAvailable": true, "Available": "Есть в наличии (<div class=\"details-avalable-text \" >10</div> <div class=\"details-avalable-unit \" ></div>)" }], "StartOfferIdSelected": 12455, "Unit": "", "ShowStockAvailability": true, "AllowPreOrder": true },
        productWithColorsAndSizes = {
            productId: 970,
            responseOffers: responseWithColorsAndSizes,
            selectedOffer: responseWithColorsAndSizes.Offers[0],
            colors: [{ "ColorId": 4, "ColorName": "Черный", "ColorCode": "#000000", "PhotoName": "" }, { "ColorId": 8, "ColorName": "Розовый", "ColorCode": "#ff5c71", "PhotoName": "" }],
            sizes: [{ "SizeId": 1, "SizeName": "S" }, { "SizeId": 2, "SizeName": "M" }, { "SizeId": 4, "SizeName": "XL" }],
            responsePrice: { "PriceString": "<div class=\"price-current cs-t-1\"><div class=\"price-number\"> 4 400</div> <div class=\"price-currency\"> руб.</div></div>", "PriceNumber": 4400.0, "PriceOldNumber": 4400.0, "Bonuses": "<div class=\"bonus-price\">+  132  руб. на бонусную карту</div>" },
            responseFirstPaymentPrice: "Без первого взноса"
        },
        customOptionsData = [{ "CustomOptionsId": 479, "Title": "Гарантия", "IsRequired": true, "InputType": 0, "ProductId": 1627, "Options": [{ "OptionId": 2925, "Title": "1 год", "OptionText": "1 год +10%", "ID": 2925 }, { "OptionId": 2926, "Title": "2 года", "OptionText": "2 года +20%", "ID": 2926 }], "ID": 479, "SelectedOptions": { "OptionId": 2925, "Title": "1 год", "OptionText": "1 год +10%", "ID": 2925 } }, { "CustomOptionsId": 480, "Title": "Чехол", "IsRequired": false, "InputType": 0, "ProductId": 1627, "Options": [{ "OptionId": 2927, "Title": "Нет", "OptionText": "Нет", "ID": 2927 }, { "OptionId": 2928, "Title": "Пластиковый", "OptionText": "Пластиковый +  50  руб.", "ID": 2928 }, { "OptionId": 2929, "Title": "Кожанный", "OptionText": "Кожанный +  20  руб.", "ID": 2929 }], "ID": 480, "SelectedOptions": null }, { "CustomOptionsId": 481, "Title": "Крутой", "IsRequired": false, "InputType": 1, "ProductId": 1627, "Options": [{ "OptionId": 2930, "Title": "Не крутой", "OptionText": "Не крутой +  100  руб.", "ID": 2930 }, { "OptionId": 2931, "Title": "Крутой", "OptionText": "Крутой + 10 000  руб.", "ID": 2931 }], "ID": 481, "SelectedOptions": null }, { "CustomOptionsId": 482, "Title": "Хочу", "IsRequired": false, "InputType": 2, "ProductId": 1627, "Options": [{ "OptionId": 2932, "Title": "", "OptionText": " + 5 000  руб.", "ID": 2932 }], "ID": 482, "SelectedOptions": null }, { "CustomOptionsId": 483, "Title": "Своё", "IsRequired": false, "InputType": 3, "ProductId": 1627, "Options": [{ "OptionId": 2933, "Title": " ", "OptionText": "", "ID": 2933 }], "ID": 483, "SelectedOptions": null }, { "CustomOptionsId": 484, "Title": "Коммент", "IsRequired": false, "InputType": 4, "ProductId": 1627, "Options": [{ "OptionId": 2934, "Title": " ", "OptionText": "", "ID": 2934 }], "ID": 484, "SelectedOptions": null }],
        customOptionsXML = "%3cOptions%3e%3cOption%3e%3cCustomOptionId%3e479%3c%2fCustomOptionId%3e%3cCustomOptionTitle%3e%d0%93%d0%b0%d1%80%d0%b0%d0%bd%d1%82%d0%b8%d1%8f%3c%2fCustomOptionTitle%3e%3cOptionId%3e2925%3c%2fOptionId%3e%3cOptionTitle%3e1+%d0%b3%d0%be%d0%b4%3c%2fOptionTitle%3e%3cOptionPriceBC%3e10%3c%2fOptionPriceBC%3e%3cOptionPriceType%3ePercent%3c%2fOptionPriceType%3e%3c%2fOption%3e%3c%2fOptions%3e",
        $controller,
        $httpBackend,
        productService,
        ctrl,
        ctrlCustomOptions,
        ctrlNgForm;


    beforeEach(module('toaster'));
    beforeEach(module('modal'));
    beforeEach(module('pascalprecht.translate'));
    beforeEach(module('product'));
    beforeEach(module('colorsViewer'));
    beforeEach(module('sizesViewer'));
    beforeEach(module('customOptions'));

    beforeEach(inject(function (_$controller_, _$httpBackend_, _productService_) {
        $controller = _$controller_;
        $httpBackend = _$httpBackend_;
        productService = _productService_;
        ctrlCustomOptions = getCustomOptionsControls();
        ctrl = $controller('ProductCtrl', { $scope: {} });
        ctrl.customOptions = ctrlCustomOptions;
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    function getCustomOptionsControls() {

        var _ctrl;

        $httpBackend.whenGET('product/getcustomoptions').respond(customOptionsData);
        $httpBackend.whenGET('product/customoptionsxml?selectedOptions=0_2925').respond(customOptionsXML);

        _ctrl = $controller('CustomOptionsCtrl', { $scope: {} });

        $httpBackend.flush();

        _ctrl.customOptionsForm = {};
        _ctrl.customOptionsForm.$invalid = true;
        _ctrl.customOptionsForm.$setSubmitted = function () { };
        _ctrl.customOptionsForm.$setDirty = function () { };

        return _ctrl;
    }

    it('add and get product to storage', function () {

        productService.addToStorage(ctrl);

        expect(ctrl).toEqual(productService.getProduct());
    });

    it('get price', function () {

        $httpBackend.whenPOST('product/getofferprice').respond(productWithColorsAndSizes.responsePrice);

        ctrl.getPrice(productWithColorsAndSizes.selectedOffer.OfferId, null);

        $httpBackend.flush();

        expect(ctrl.Price.PriceNumber).toEqual(productWithColorsAndSizes.responsePrice.PriceNumber);
    });

    it('get first payment price', function () {

        $httpBackend.expectGET('product/getfirstpaymentprice?price=' + productWithColorsAndSizes.responsePrice.PriceNumber).respond(productWithColorsAndSizes.responseFirstPaymentPrice);

        ctrl.getFirstPaymentPrice(productWithColorsAndSizes.responsePrice.PriceNumber);

        $httpBackend.flush();

        expect(ctrl.visibilityFirstPaymentButton).toEqual(true);
    });

    it('call processCallback on refreshPrice', function () {

        var callbackVarible = false;

        productService.addCallback('refreshPrice', function () {
            callbackVarible = true;
        });

        $httpBackend.expectGET('product/getfirstpaymentprice?price=' + productWithColorsAndSizes.responsePrice.PriceNumber).respond(productWithColorsAndSizes.responseFirstPaymentPrice);
        $httpBackend.whenPOST('product/getofferprice').respond(productWithColorsAndSizes.responsePrice);

        ctrl.refreshPrice();

        $httpBackend.flush();

        expect(callbackVarible).toEqual(true);
    });

    it('compare productId from controller', function () {

        $httpBackend.expectGET('product/getoffers?productId=' + productWithColorsAndSizes.productId).respond(productWithColorsAndSizes.responseOffers);

        ctrl.loadData(productWithColorsAndSizes.productId, undefined, undefined);

        $httpBackend.flush();

        expect(ctrl.productId).toEqual(productWithColorsAndSizes.productId);
    });

    it('correct search of offer selected', function () {

        $httpBackend.expectGET('product/getoffers?productId=' + productWithColorsAndSizes.productId).respond(productWithColorsAndSizes.responseOffers);

        ctrl.loadData(productWithColorsAndSizes.productId, undefined, undefined);

        $httpBackend.flush();

        expect(ctrl.offerSelected.OfferId).toEqual(productWithColorsAndSizes.selectedOffer.OfferId);
    });

    it('function validate', function () {
        expect(ctrl.validate()).toEqual(false);
    });
});
