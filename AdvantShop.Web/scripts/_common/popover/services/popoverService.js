; (function (ng, body) {

    'use strict';

    var popoverService = function ($cacheFactory, $q, $rootScope, $compile, $cookieStore, $window) {
        var service = this,
            cache = $cacheFactory('cachePopover'),
            deferOverlay,
            defersPopover = {},
            defersPopoverControl = {},
            tileTriangle = 8,
            tileSize;

        service.addStorage = function (popoverId, obj) {

            var item = cache.get(popoverId),
                objForSave = {};

            if (item != null) {
                ng.extend(objForSave, item, obj);
            } else {
                objForSave = obj;
            }

            var popover = cache.put(popoverId, objForSave)

            if (defersPopover[popoverId] != null) {
                defersPopover[popoverId].resolve(popover);
            }

            return popover;
        };

        service.addControl = function (popoverId, control) {
            return service.getPopoverScope(popoverId).then(function (popoverScope) {
                popoverScope.controlElement = control;

                if (defersPopoverControl[popoverId] != null) {
                    defersPopoverControl[popoverId].resolve(control);
                }
                return popoverScope;
            });
        };

        service.getControl = function (popoverId) {
            return service.getPopoverScope(popoverId).then(function (popoverScope) {

                var defer = $q.defer();

                if (popoverScope.controlElement == null) {
                    defersPopoverControl[popoverId] = defer;
                } else {
                    defer.resolve(popoverScope.controlElement);
                }

                return defer.promise;
            });
        };

        service.getPopoverScope = function (popoverId) {

            var popover = cache.get(popoverId),
                defer = $q.defer(),
                promise = defer.promise;

            if (popover == null) {
                defersPopover[popoverId] = defer;
            } else {
                defer.resolve(popover);
            }

            return promise;
        };

        service.addPopoverOverlay = function (overlayScope) {
            service.addStorage('popoverOverlay', overlayScope);
            if (deferOverlay != null) {
                deferOverlay.resolve(overlayScope);
            }
        };

        service.getPopoverOverlay = function () {
            var overlay = cache.get('popoverOverlay'),
                defer = $q.defer(),
                promise = defer.promise;

            if (overlay == null) {
                deferOverlay = defer;
                service.renderOverlay();
            } else {
                defer.resolve(overlay);
            }

            return promise;
        };

        service.showOverlay = function (popoverId) {
            return service.getPopoverOverlay().then(function (overlayScope) {
                overlayScope.isVisibleOverlay = true;
                overlayScope.popoverId = popoverId;
                return overlayScope;
            });
        };

        service.renderOverlay = function () {

            var overlay = ng.element('<div class="popover-overlay" data-popover-overlay></div>'),
                overlayStorage;

            body.appendChild(overlay[0]);

            $compile(overlay)($rootScope.$new(true));
        };

        service.getPosition = function (popoverElement, popoverControlElement, position, isFixed) {
            var popoverControlSize,
                popoverControlPos,
                popoverControlPosAbs,
                popoverSize,
                pos,
                positionData,
                tileSize,
                controlRect;

            popoverControlSize = {
                width: popoverControlElement.offsetWidth,
                height: popoverControlElement.offsetHeight
            };

            popoverControlPos = {
                top: popoverControlElement.offsetTop,
                left: popoverControlElement.offsetLeft
            };

            controlRect = popoverControlElement.getBoundingClientRect();

            popoverControlPosAbs = {
                top: controlRect.top,
                bottom: controlRect.bottom,
                left: controlRect.left,
                right: controlRect.right
            };


            if (!(controlRect.top >= 0 && controlRect.bottom <= (window.innerHeight || document.documentElement.clientHeight))) {
                popoverControlPosAbs.top += $window.pageYOffset;
                popoverControlPosAbs.bottom += $window.pageYOffset;
            }

            popoverSize = {
                width: popoverElement.offsetWidth,
                height: popoverElement.offsetHeight
            };

            pos = {
                top: 0,
                left: 0,
                leftTile: 0,
                position: position
            };

            positionData = isFixed === true ? popoverControlPosAbs : popoverControlPos;

            switch (position) {
                case 'top':
                    tileSize = tileSize || popoverElement.querySelector('.js-popover-tile').offsetHeight; //5 - для нахлеста
                    pos.top = positionData.top - popoverSize.height;
                    pos.left = positionData.left + (popoverControlSize.width - popoverSize.width) / 2;
                    break;
                case 'right':
                    tileSize = tileSize || popoverElement.querySelector('.js-popover-tile').offsetWidth + tileTriangle; //5 - для нахлеста
                    pos.top = positionData.top + (popoverControlSize.height - popoverSize.height) / 2;
                    pos.left = positionData.left + popoverControlSize.width + tileSize;
                    break;
                case 'bottom':
                    tileSize = tileSize || popoverElement.querySelector('.js-popover-tile').offsetHeight; //5 - для нахлеста
                    pos.top = positionData.top + popoverControlSize.height + tileSize;
                    pos.left = positionData.left + (popoverControlSize.width - popoverSize.width) / 2;
                    break;
                case 'left':
                    tileSize = tileSize || popoverElement.querySelector('.js-popover-tile').offsetWidth + tileTriangle; //5 - для нахлеста
                    pos.top = positionData.top + (popoverControlSize.height - popoverSize.height) / 2;

                    if (isFixed === true) {
                        pos.left = positionData.left - popoverSize.width;
                    } else {
                        pos.left = 'auto';
                        pos.right = '100%';
                    }

                    if (popoverControlPosAbs.left <= popoverSize.width) {
                        tileSize = tileSize || popoverElement.querySelector('.js-popover-tile').offsetWidth + tileTriangle; //5 - для нахлеста
                        pos.left = positionData.left + popoverControlSize.width + tileSize;
                        pos.position = 'right';
                        pos.right = 'auto';
                    }
                    break;
                default:
                    throw new Error('Not register position:' + position);
                    break;
            }

            return pos;
        };


    };

    ng.module('popover')
      .service('popoverService', popoverService);

    popoverService.$inject = ['$cacheFactory', '$q', '$rootScope', '$compile', '$cookieStore', '$window'];

})(window.angular, document.body);