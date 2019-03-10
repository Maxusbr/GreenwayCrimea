; (function (ng) {

    'use strict';

    var ProductCtrl = function ($q, $scope, $sce, $timeout, productService, modalService, toaster, $translate) {

        var ctrl = this,
            videoElem;

        productService.addToStorage(ctrl);

        ctrl.filterPhotosEnable = true;

        ctrl.productView = "photo";

        ctrl.Price = {};

        ctrl.picture = {};

        ctrl.dirty === false;

        ctrl.offerSelected = {};

        ctrl.carouselHidden = true;

        ctrl.getPrice = function (offerId, customOptions) {
            return productService.getPrice(ctrl.offerSelected.OfferId, ctrl.customOptions != null ? ctrl.customOptions.xml : null)
                  .then(function (price) {
                      ctrl.Price = price;
                      ctrl.Price.PriceString = $sce.trustAsHtml(ctrl.Price.PriceString);
                      ctrl.Price.Bonuses = $sce.trustAsHtml(ctrl.Price.Bonuses);
                      return ctrl.Price;
                  });
        };

        ctrl.getFirstPaymentPrice = function (price, discount, discountAmount) {
            return productService.getFirstPaymentPrice(price, discount, discountAmount).then(function (firstPaymentPrice) {
                ctrl.FirstPaymentPrice = $sce.trustAsHtml(firstPaymentPrice);
                ctrl.visibilityFirstPaymentButton = firstPaymentPrice != null && firstPaymentPrice.length > 0;
            })
        };

        ctrl.refreshPrice = function () {
            return ctrl.getPrice(ctrl.offerSelected.OfferId, ctrl.customOptions != null ? ctrl.customOptions.xml : null)
                  .then(function (price) {
                      return ctrl.getFirstPaymentPrice(price.PriceNumber, ctrl.discount, ctrl.discountAmount);
                  }).then(function () {

                      if (ctrl.shippingVariants != null) {
                          ctrl.shippingVariants.update();
                      }

                      productService.processCallback('refreshPrice');
                  });
        };

        ctrl.prepareOffers = function (data) {
            for (var i = 0, len = data.Offers.length; i < len; i++) {
                if (data.Offers[i].Available != null && ng.isString(data.Offers[i].Available) === true) {
                    data.Offers[i].Available = $sce.trustAsHtml(data.Offers[i].Available);
                }
            }

            return data;
        };

        ctrl.loadData = function (productId, colorId, sizeId) {

            ctrl.productId = productId;

            return productService.getOffers(productId, colorId, sizeId).then(function (data) {
                if (data == null) {
                    return null;
                }
                    
                ctrl.data = ctrl.prepareOffers(data);

                ctrl.offerSelected = productService.findOfferSelected(data.Offers, data.StartOfferIdSelected);

                ctrl.dirty = true;

                ctrl.getColorsViewer()
                    .then(function () {
                        if (ctrl.colorsViewer != null) {
                            ctrl.setColorSelected(ctrl.colorsViewer, ctrl.offerSelected.Color.ColorId);
                        }

                        return ctrl.data;
                    })
                   .then(ctrl.getSizesViewer)
                   .then(function () {
                       if (ctrl.sizesViewer != null) {
                           ctrl.setSizeSelected(ctrl.sizesViewer, ctrl.offerSelected.Size.SizeId);
                       }
                       return ctrl.data;
                   })
                   .then(ctrl.getCarousel)
                   .then(function () {
                       if (ctrl.filterPhotosEnable === true && ctrl.carousel != null) {
                           ctrl.filterPhotos(ctrl.offerSelected.Color != null ? ctrl.offerSelected.Color.ColorId : null, ctrl.carousel);
                       }
                       ctrl.carouselHidden = false;
                   });

                return ctrl.data;
            });
        };

        ctrl.validate = function () {

            var result = true;

            if (ctrl.customOptions != null && ctrl.customOptions.customOptionsForm.$invalid === true) {
                ctrl.customOptions.customOptionsForm.$setSubmitted();
                ctrl.customOptions.customOptionsForm.$setDirty();
                toaster.pop('error', $translate.instant('Js.Product.InvalidCustomOptions'));

                result = false;
            }

            return result;
        };

        //#region compare and wishlist

        ctrl.compareInit = function (compare) {
            ctrl.compare = compare;
        };

        ctrl.wishlistControlInit = function (wishlistControl) {
            ctrl.wishlistControl = wishlistControl;
        };

        //#endregion

        //#region customOptions

        ctrl.customOptionsInitFn = function (customOptions) {
            ctrl.customOptions = customOptions;
        };

        ctrl.customOptionsChange = function () {
            ctrl.refreshPrice();
        };

        //#endregion

        //#region colors

        ctrl.initColors = function (colorsViewer) {
            ctrl.colorsViewer = colorsViewer;

            if (ctrl.colorsViewerDefer != null) {
                ctrl.colorsViewerDefer.resolve();
                delete ctrl.colorsViewerDefer;
            }
        };

        ctrl.getColorsViewer = function () {

            var defer = $q.defer();

            if (ctrl.colorsExist === true && ctrl.colorsViewer == null) {
                ctrl.colorsViewerDefer = defer;
            } else {
                defer.resolve();
            }

            return defer.promise;
        }

        ctrl.changeColor = function (color) {

            ctrl.colorSelected = color;

            if (ctrl.sizesViewer != null) {
                ctrl.sizeSelected = ctrl.getSizeAvalable(ctrl.data.Offers, ctrl.colorSelected.ColorId, ctrl.sizesViewer.sizes, ctrl.data.AllowPreOrder);
            }

            ctrl.offerSelected = productService.getOffer(ctrl.data.Offers, ctrl.colorSelected.ColorId, ctrl.sizeSelected != null && ctrl.sizeSelected.isDisabled === false ? ctrl.sizeSelected.SizeId : null, ctrl.data.AllowPreOrder);

            ctrl.refreshPrice();

            if (ctrl.compare != null) {
                ctrl.compare.checkStatus(ctrl.offerSelected.OfferId);
            }

            if (ctrl.wishlistControl != null) {
                ctrl.wishlistControl.checkStatus(ctrl.offerSelected.OfferId);
            }

            ctrl.setPreviewByColorId(ctrl.colorSelected.ColorId, ctrl.filterPhotosEnable, ctrl.carousel);
        };

        ctrl.setColorSelected = function (colorsViewer, colorId) {
            for (var i = colorsViewer.colors.length - 1; i >= 0; i--) {
                if (colorsViewer.colors[i].ColorId === colorId) {
                    ctrl.colorSelected = colorsViewer.colors[i];
                    break;
                }
            }
        };

        //#endregion

        //#region sizes

        ctrl.initSizes = function (sizesViewer) {
            ctrl.sizesViewer = sizesViewer;

            if (ctrl.sizesViewerDefer != null) {
                ctrl.sizesViewerDefer.resolve();
                delete ctrl.sizesViewerDefer;
            }
        };

        ctrl.getSizesViewer = function () {

            var defer = $q.defer();

            if (ctrl.sizesExist === true && ctrl.sizesViewer == null) {
                ctrl.sizesViewerDefer = defer;
            } else {
                defer.resolve();
            }

            return defer.promise;
        }

        ctrl.changeSize = function (size) {

            ctrl.sizeSelected = size;

            ctrl.offerSelected = productService.getOffer(ctrl.data.Offers, ctrl.colorSelected != null ? ctrl.colorSelected.ColorId : 0, ctrl.sizeSelected.isDisabled ? null : ctrl.sizeSelected.SizeId, ctrl.data.AllowPreOrder);

            ctrl.refreshPrice();

            if (ctrl.compare != null) {
                ctrl.compare.checkStatus(ctrl.offerSelected.OfferId);
            }

            if (ctrl.wishlistControl != null) {
                ctrl.wishlistControl.checkStatus(ctrl.offerSelected.OfferId);
            }
        };

        ctrl.setSizeSelected = function (sizesViewer, sizeId) {

            for (var i = sizesViewer.sizes.length - 1; i >= 0; i--) {
                if (sizesViewer.sizes[i].SizeId === sizeId) {
                    ctrl.sizeSelected = sizesViewer.sizes[i];
                    break;
                }
            }

            ctrl.sizeSelected = ctrl.getSizeAvalable(ctrl.data.Offers, ctrl.colorSelected != null ? ctrl.colorSelected.ColorId : 0, ctrl.sizesViewer.sizes, ctrl.data.AllowPreOrder);
        };

        ctrl.getSizeAvalable = function (offers, colorId, sizes, allowPreorder) {

            var offerItem, sizeItem, sizeSelected, loopCheckStart;

            sizes.forEach(function (item) {
                item.isDisabled = true;
            });

            for (var i = offers.length - 1; i >= 0; i--) {

                offerItem = offers[i];

                if (colorId == null || offerItem.Color == null) {
                    loopCheckStart = true;
                } else {
                    loopCheckStart = offerItem.Color != null && offerItem.Color.ColorId === colorId;
                };

                if (loopCheckStart === true) {
                    for (var s = sizes.length - 1; s >= 0; s--) {
                        if (offerItem.Size.SizeId == sizes[s].SizeId && (allowPreorder === true || offerItem.Amount > 0)) {
                            sizes[s].isDisabled = false;
                            break;
                        }
                    };
                }
            }

            if (ctrl.sizeSelected == null || ctrl.sizeSelected.isDisabled === true) {
                for (var j = 0, l = sizes.length ; j < l; j++) {
                    if (sizes[j].isDisabled == null || sizes[j].isDisabled == false) {
                        sizeSelected = sizes[j];
                        break;
                    }
                }
            } else {
                sizeSelected = ctrl.sizeSelected;
            }

            return sizeSelected;
        };

        //#endregion

        //#region carousels

        ctrl.addCarousel = function (carousel) {
            ctrl.carousel = carousel;


            if (ctrl.carouselDefer != null) {
                ctrl.carouselDefer.resolve();
                delete ctrl.carouselDefer;
            }
        };

        ctrl.getCarousel = function () {
            var defer = $q.defer();

            if (ctrl.carouselExist === true && ctrl.carousel == null) {
                ctrl.carouselDefer = defer;
            } else {
                defer.resolve();
            }

            return defer.promise;
        };

        ctrl.carouselItemSelect = function (carousel, item, index) {
            ctrl.setPreview(item.parameters);

            ctrl.updateModalPreview(item.parameters.originalPath);

            if (carousel != null && ctrl.carousel != null && carousel != ctrl.carousel) {
                ctrl.carousel.setItemSelect(index);
            } else if (ctrl.carouselPreview != null && carousel != ctrl.carouselPreview) {
                ctrl.carouselPreview.setItemSelect(index);
            }
        };


        //#endregion

        //#region modal preview

        ctrl.carouselPreviewNext = function () {
            ctrl.carouselPreview.next();

            var items = ctrl.carouselPreview.getItems(),
                itemSelected = ctrl.carouselPreview.getSelectedItem() || (items != null ? items[0] : null),
                itemSelectedNew;

            if (itemSelected != null) {
                itemSelectedNew = items[itemSelected.carouselItemData.index + 1];

                if (itemSelectedNew) {
                    ctrl.carouselPreview.setItemSelect(itemSelectedNew);
                    ctrl.setPreview(itemSelectedNew.carouselItemData.parameters);
                    ctrl.updateModalPreview(itemSelectedNew.carouselItemData.parameters.originalPath);
                }
            }
        };

        ctrl.carouselPreviewPrev = function () {
            ctrl.carouselPreview.prev();

            var items = ctrl.carouselPreview.getItems(),
                itemSelected = ctrl.carouselPreview.getSelectedItem() || (items != null ? items[0] : null),
                itemSelectedNew;

            if (itemSelected != null) {
                itemSelectedNew = items[itemSelected.carouselItemData.index - 1];

                if (itemSelectedNew) {
                    ctrl.carouselPreview.setItemSelect(itemSelectedNew);
                    ctrl.setPreview(itemSelectedNew.carouselItemData.parameters);
                    ctrl.updateModalPreview(itemSelectedNew.carouselItemData.parameters.originalPath);
                }
            }
        };

        ctrl.addModalPictureCarousel = function (carouselPreview) {
            ctrl.carouselPreview = carouselPreview;
        };

        ctrl.carouselPreviewUpdate = function () {
            if (ctrl.carouselPreview != null) {
                ctrl.carouselPreview.update();
            }
        };

        ctrl.updateModalPreview = function (imgSrc) {

            productService.getPhoto(imgSrc).then(function (img) {
                $timeout(function () {
                    ctrl.maxHeightModalPreview = ctrl.getMaxHeightModalPreview();
                    ctrl.modalPreviewHeight = img.naturalHeight > ctrl.maxHeightModalPreview ? ctrl.maxHeightModalPreview : img.naturalHeight;
                }, 0);
            });
        };

        ctrl.modalPreviewCallbackOpen = function (modal) {

            ctrl.setPreviewByColorId(ctrl.offerSelected.Color != null ? ctrl.offerSelected.Color.ColorId : null, ctrl.filterPhotosEnable, ctrl.carouselPreview);

            $timeout(function () {
                ctrl.carouselPreviewUpdate();
            }, 100);
        };

        ctrl.modalPreviewOpen = function (event, picture) {

            event.preventDefault();
            event.stopPropagation();

            ctrl.modalPreviewState = 'load';

            ctrl.dialogOpen()
                .then(function () {
                    productService.getPhoto(picture == null ? ctrl.picture.originalPath : picture.originalPath).then(function (img) {
                        $timeout(function () {
                            ctrl.maxHeightModalPreview = ctrl.getMaxHeightModalPreview();
                            ctrl.modalPreviewHeight = img.naturalHeight > ctrl.maxHeightModalPreview ? ctrl.maxHeightModalPreview : img.naturalHeight;
                            //ctrl.carouselPreviewUpdate();
                            ctrl.modalPreviewState = 'complete';
                            if (ctrl.filterPhotosEnable === true && ctrl.carousel != null && picture != null) {
                                ctrl.filterPhotos(ctrl.offerSelected.Color != null ? ctrl.offerSelected.Color.ColorId : null, ctrl.carousel, picture.PhotoId);
                            }
                        }, 0);
                    });
                });
        };

        ctrl.getMaxHeightModalPreview = function () {
            var result = 0,
                height,
                modalElement,
                modaPreview = document.getElementById('modalPreview_' + ctrl.productId);

            if (modaPreview != null) {
                modalElement = modaPreview.querySelector('.modal-content');
            }
            if (modalElement != null) {
                height = parseFloat(getComputedStyle(modalElement).height);
                result = isNaN(height) === false ? height : 0;
            }

            return result;
        };

        ctrl.dialogOpen = function () {
            return modalService.getModal('modalPreview_' + ctrl.productId).then(function (modal) {
                modal.modalScope.open();
            });
        }

        ctrl.resizeModalPreview = function () {
            $scope.$apply(function () {
                ctrl.updateModalPreview(ctrl.picture.originalPath);
                ctrl.carouselPreviewUpdate();
            });
        };

        //#endregion

        //#region productViewChange



        ctrl.showVideo = function (visible) {
            ctrl.visibleVideo = visible;
        }

        ctrl.showRotate = function (visible) {
            ctrl.visibleRotate = visible;
        }


        //#endregion

        //#region shippingVariants
        ctrl.addShippingVariants = function (shippingVariants) {
            ctrl.shippingVariants = shippingVariants;
        };
        //#endregion

        //#region spinbox amount
        ctrl.updateAmount = function (value, proxy) {
            if (ctrl.shippingVariants != null) {
                ctrl.shippingVariants.update();
            }
        }
        //#endregion

        ctrl.filterPhotosFunction = function (item, index) {
            return item != null && (item.carouselItemData.parameters.colorId == null || ctrl.offerSelected.Color == null || item.carouselItemData.parameters.colorId == ctrl.offerSelected.Color.ColorId);
        };

        ctrl.setPreviewByColorId = function (colorId, filterEnabled, carousel) {

            var findArray;

            if (ctrl.carousel) {
                if (filterEnabled === true) {
                    ctrl.filterPhotos(colorId, carousel, ctrl.picture.PhotoId);
                } else {
                    findArray = ctrl.carousel.items.filter(ctrl.filterPhotosFunction);

                    if (findArray != null && findArray.length > 0) {
                        ctrl.setPreview(findArray[0].carouselItemData.parameters);
                    }
                }
            }
        };

        ctrl.filterPhotos = function (colorId, carousel, selectedPhotoId) {
            var selectedItem, findedSelected = false, items;

            if (carousel) {
                carousel.filterItems(ctrl.filterPhotosFunction);

                if (selectedPhotoId != null) {
                    items = carousel.getItems();

                    for (var i = 0, len = items.length; i < len; i++) {
                        if (items[i].carouselItemData.parameters.PhotoId === selectedPhotoId) {
                            carousel.setItemSelect(items[i]);
                            findedSelected = true;
                            break;
                        }
                    }
                }

                if (findedSelected === false) {
                    selectedItem = carousel.getActiveItem();

                    if (selectedItem != null) {
                        carousel.setItemSelect(selectedItem);
                        ctrl.setPreview(selectedItem.carouselItemData.parameters);
                    }
                }

            }
        };

        ctrl.setView = function (viewName) {
            ctrl.productView = viewName;

            ctrl.stopVideo();
        };

        ctrl.setPreview = function (picture) {
            ctrl.picture = picture;
        };

        ctrl.getUrl = function (url) {
            var result = url,
                params = [];

            if (ctrl.colorsViewer != null && ctrl.colorsViewer.colorSelected != null) {
                params.push("color=" + ctrl.colorsViewer.colorSelected.ColorId);
            }

            if (ctrl.sizesViewer != null && ctrl.sizesViewer.sizeSelected != null) {
                params.push("size=" + ctrl.sizesViewer.sizeSelected.SizeId);
            }

            if (params.length > 0) {
                result = result + '?' + params.join('&');
            }

            return result;
        };
    };

    ng.module('product')
      .controller('ProductCtrl', ProductCtrl);


    ProductCtrl.$inject = ['$q', '$scope', '$sce', '$timeout', 'productService', 'modalService', 'toaster', '$translate'];

})(window.angular);