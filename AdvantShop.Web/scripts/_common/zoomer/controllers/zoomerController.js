; (function (ng) {

    'use strict';

    var ZoomerCtrl = function ($element, $q, $window) {

        var ctrl = this;

        ctrl.isShowZoom = false;
        ctrl.isProcessing = false;

        ctrl.zoomPos = {
            left: 0,
            top: 0
        };

        ctrl.zoomSizes = {
            top: 0,
            left: 0,
            width: 0,
            height: 0,
            originalWidth: 0,
            originalHeight: 0
        };

        ctrl.zoomerSizes = {
            top: 0,
            left: 0,
            width: 0,
            height: 0
        };

        ctrl.lensSizes = {
            top: 0,
            left: 0,
            width: 0,
            height: 0
        };

        ctrl.getSizePreview = function () {
            return {
                left: $element[0].offsetLeft,
                top: $element[0].offsetTop,
                width: $element[0].offsetWidth,
                height: $element[0].offsetHeight
            };
        };

        ctrl.getSizeOriginal = function (url) {
            return ctrl.getImage(url).then(function (image) {
                return {
                    width: image.naturalWidth,
                    height: image.naturalHeight
                };
            });
        };

        ctrl.getImage = function (url) {

           ctrl.isProcessing = true;

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
                ctrl.isProcessing = false;

                return response;
            });
        };

        ctrl.active = function (event) {

            event.preventDefault();
            event.stopPropagation();

            ctrl.getSizeOriginal(ctrl.originalPath).then(function (imageOriginalSize) {

                var previewSize = ctrl.getSizePreview();

                ctrl.zoomerSizes.left = previewSize.left;
                ctrl.zoomerSizes.top = previewSize.top;
                ctrl.zoomerSizes.width = previewSize.width;
                ctrl.zoomerSizes.height = previewSize.height;

                if (ctrl.type == "inner") {
                    ctrl.zoomSizes.top = previewSize.top;
                    ctrl.zoomSizes.left = previewSize.left;
                    ctrl.zoomSizes.height = previewSize.height;
                    ctrl.zoomSizes.width = previewSize.width;
                } else {

                    switch (ctrl.type) {
                        case 'right':
                            ctrl.zoomSizes.top = previewSize.top;
                            ctrl.zoomSizes.left = previewSize.left + previewSize.width;
                            break;
                        case 'left':
                            ctrl.zoomSizes.top = previewSize.top;
                            ctrl.zoomSizes.left = previewSize.left - previewSize.width;
                            break;
                        default:
                            ctrl.zoomSizes.top = previewSize.top;
                            ctrl.zoomSizes.left = previewSize.left + previewSize.width;
                    }

                    ctrl.zoomSizes.width = ctrl.zoomWidth < imageOriginalSize.width ? ctrl.zoomWidth : imageOriginalSize.width;
                    ctrl.zoomSizes.height = ctrl.zoomHeight < imageOriginalSize.height ? ctrl.zoomHeight : imageOriginalSize.height;
                }

                ctrl.zoomSizes.originalHeight = imageOriginalSize.height;
                ctrl.zoomSizes.originalWidth = imageOriginalSize.width;

                ctrl.isShowZoom = true;
            });
        };

        ctrl.update = function (event) {
            event.preventDefault();
            event.stopPropagation();

            var rect, pointX, pointY;

            rect = $element[0].getBoundingClientRect();

            pointX = event.pageX - (rect.left + $window.pageXOffset);
            pointY = event.pageY - (rect.top + $window.pageYOffset);

            if (ctrl.isShowZoom == true) {
                var scaleOriginal = ctrl.zoomSizes.originalWidth / ctrl.zoomerSizes.width,
                    scalePreview = ctrl.zoomerSizes.width / ctrl.zoomSizes.originalWidth;

                ctrl.lensSizes.width = ctrl.zoomSizes.width * scalePreview;
                ctrl.lensSizes.height = ctrl.zoomSizes.height * scalePreview;

                var lensLeft = pointX - (ctrl.lensSizes.width / 2),
                    lensTop = pointY - (ctrl.lensSizes.height / 2);

                var lensLimit = {
                    left: 0,
                    top: 0,
                    right: ctrl.zoomerSizes.width - ctrl.lensSizes.width,
                    bottom: ctrl.zoomerSizes.height - ctrl.lensSizes.height
                };


                if (lensTop < lensLimit.top) {
                    ctrl.lensSizes.top = 0;
                } else if (lensTop > lensLimit.bottom) {
                    ctrl.lensSizes.top = lensLimit.bottom;
                } else {
                    ctrl.lensSizes.top = lensTop;
                }

                if (lensLeft < lensLimit.left) {
                    ctrl.lensSizes.left = 0;
                } else if (lensLeft > lensLimit.right) {
                    ctrl.lensSizes.left = lensLimit.right;
                } else {
                    ctrl.lensSizes.left = lensLeft;
                }

                var zoomImageLeft = (ctrl.lensSizes.left + ctrl.lensSizes.width) * scaleOriginal,
                    zoomImageTop = (ctrl.lensSizes.top + ctrl.lensSizes.height) * scaleOriginal;

                if (zoomImageLeft >= ctrl.zoomSizes.width) {
                    ctrl.zoomPos.left = -(zoomImageLeft - ctrl.zoomSizes.width);
                }

                if (zoomImageTop >= ctrl.zoomSizes.height) {
                    ctrl.zoomPos.top = -(zoomImageTop - ctrl.zoomSizes.height);
                }
            }
        };

        ctrl.deactive = function () {
            ctrl.isShowZoom = false;
        };

        ctrl.lensMove = ctrl.update;

        ctrl.getZoomerClass = function () {

            var obj = {};
            obj['zoomer-' + ctrl.type] = true;
            obj['zoomer-processing'] = ctrl.isProcessing;
            return obj;
        };
    };

    ng.module('zoomer')
      .controller('ZoomerCtrl', ZoomerCtrl);

    ZoomerCtrl.$inject = ['$element', '$q', '$window'];

})(window.angular);