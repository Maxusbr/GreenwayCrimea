; (function (ng) {
    'use strict';

    var ProductViewItemCtrl = function ($q, $timeout, productViewService, $translate) {

        var ctrl = this, controls = {}, needCarouselUpdate = false, photosStorage;
        ctrl.photosVisible = false;
        ctrl.photos = [];
        ctrl.picture = {};
        ctrl.colorSelected = null;

        ctrl.getPhotos = function (cache) {
            var defer = $q.defer(),
                promise;

            cache = cache != null ? cache : true;

            if (photosStorage == null || needCarouselUpdate === true) {
                promise = productViewService.getPhotos(ctrl.productId).then(function (photos) {
                    return photosStorage = photos;
                });
            } else {
                promise = defer.promise;
                defer.resolve(photosStorage);
            }

            return promise;
        };

        ctrl.numberals = function (num) {
            if (num <= 0) return ctrl.textNumberals = num + ' ' + $translate.instant('Js.ProductView.Photos0');
            num = num % 100;
            var nums = num % 10;
            if (num > 10 && num < 20) return ctrl.textNumberals = num + ' ' + $translate.instant('Js.ProductView.Photos5');
            if (nums > 1 && nums < 5) return ctrl.textNumberals = num + ' ' + $translate.instant('Js.ProductView.Photos2');
            return nums == 1 ? ctrl.textNumberals = num + ' ' + $translate.instant('Js.ProductView.Photos1')
            : ctrl.textNumberals = num + ' ' + $translate.instant('Js.ProductView.Photos5');
        };

        ctrl.fill = function (photos) {
            if (ctrl.getControl('colorsViewer') != null) {
                ctrl.photos = ctrl.filterPhotos(photosStorage, ctrl.getControl('colorsViewer').colorSelected.ColorId);
            } else {
                ctrl.photos = photos;
            }
            ctrl.numberals(ctrl.photos.length);
            return photos;
        };

        ctrl.process = function () {

            return ctrl.getPhotos().then(function (photos) {

                ctrl.fill(photos);


                $timeout(function () {

                    ctrl.carouselInit = true;

                    if (needCarouselUpdate === true && ctrl.getControl('photosCarousel') != null && ctrl.getControl('photosCarousel').carousel != null) {
                        ctrl.getControl('photosCarousel').carousel.update();
                        needCarouselUpdate = false;
                    }
                }, 0);

                return photos;
            });
        };

        ctrl.clearPhotos = function () {
            photosStorage = null;
            needCarouselUpdate = true;
        };

        ctrl.enter = function () {

            ctrl.photosVisible = true;

            ctrl.process();
        };

        ctrl.leave = function () {
            ctrl.photosVisible = false;
            ctrl.carouselInit = false;
        };

        ctrl.changePhoto = function (photo) {
            ctrl.picture = photo;
        }

        ctrl.initColors = function (colorsViewer) {
            ctrl.addControl('colorsViewer', colorsViewer);
        };

        ctrl.getSelectedColorId = function () {
            var colorsViewer = ctrl.getControl('colorsViewer'),
                colorId;

            if (colorsViewer != null && colorsViewer.colorSelected != null && colorsViewer.getDirtyState() === true) {
                colorId = colorsViewer.colorSelected.ColorId;
            }

            return colorId;
        };

        ctrl.initColorsCarousel = function (carousel) {
            ctrl.addControl('colorsViewerCarousel', carousel);
        };

        ctrl.changeColor = function (color) {

            ctrl.photos = ctrl.filterPhotos(photosStorage, color.ColorId);
            ctrl.picture = ctrl.photos[0];
            ctrl.numberals(ctrl.photos.length);
        };

        ctrl.addControl = function (name, scope) {
            controls[name] = scope;
        };

        ctrl.getControl = function (name) {
            return controls[name];
        };

        ctrl.filterPhotos = function (photos, colorId) {
            return photos.filter(function (item) { return item.ColorID === colorId || item.ColorID == null });
        };

        ctrl.getUrl = function (url) {
            var result = url,
                colorId = ctrl.getSelectedColorId();

            if (colorId != null) {
                result = result + '?color=' + colorId;
            }

            return result;
        };
    };

    ng.module('productView')
      .controller('ProductViewItemCtrl', ProductViewItemCtrl);

    ProductViewItemCtrl.$inject = ['$q', '$timeout', 'productViewService', '$translate'];

})(window.angular);