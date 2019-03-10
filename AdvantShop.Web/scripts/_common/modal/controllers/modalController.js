; (function (ng) {
    'use strict';

    var isTouchDevice = 'ontouchstart' in document.documentElement;

    var ModalController = function ($element, $attrs, $location, $scope, modalDefaultOptions, modalService) {

        var ctrl = this,
            urlSearch = $location.search();

        ctrl.isShowFooter = true;

        if (ng.isDefined(urlSearch.modal) && urlSearch.modal === $attrs.id) {
            ctrl.isOpen = true;
        } else {
            ctrl.isOpen = ng.isDefined($attrs.isOpen) ? $attrs.isOpen === 'true' : modalDefaultOptions.isOpen;
        }

        ctrl.modalClickOut = function (event) {
            if (ctrl.closeOut === true && event.currentTarget == event.target && ctrl.isFloating === false) {
                ctrl.close();
            }
        };

        ctrl.setVisibleFooter = function (visible) {
            ctrl.isShowFooter = visible;
        };

        ctrl.close = function (skipRemove) {

            ctrl.isOpen = false;

            if (isTouchDevice === true) {
                document.body.style.width = 'auto';
                document.body.classList.remove('adv-body-fixed-touch');
            }

            document.body.classList.remove('bodyNotScroll');

            modalService.removeItemQueue(ctrl);

            if (ctrl.callbackClose != null) {
                ctrl.callbackClose($scope);
            }

            if (ctrl.destroyOnClose === true && !skipRemove) {
                $element.remove();
                modalService.removeFromStorage(ctrl.id);
            }
        };

        ctrl.destroy = function () {

            ctrl.close();

            $element.remove();

            modalService.removeFromStorage(ctrl.id);
        };

        ctrl.open = function (skipQueue) {
            var queue, lastItem;

            if (modalService.isWorking() === true || (modalService.checkQueue(ctrl) === true || skipQueue === true)) {

                ctrl.isOpen = true;

                if (isTouchDevice === true) {
                    document.body.style.width = document.body.offsetWidth + 'px';
                    document.body.classList.add('adv-body-fixed-touch');
                }

                document.body.classList.add('bodyNotScroll');

                if (ctrl.callbackOpen != null) {
                    ctrl.callbackOpen($scope);
                }
            }

            if (modalService.existInQueue(ctrl) === false) {
                modalService.addQueue(ctrl);
            }

            $element.css('z-index', modalService.getNewZIndex());
        };

        ctrl.getModalScope = function () {
            return ctrl;
        };

        ctrl.getModalElement = function () {
            return $element;
        };

        ctrl.getTransformValue = function (x, y) {
            return 'translate3d(' + x.toFixed() + 'px,' + y.toFixed() + 'px, 0px)';
        };

        ctrl.getTransformMethodString = function () {
            if (ng.isDefined(this.transform)) {
                return this.transform;
            }

            var methodsArray = ['webkitTransform', 'MozTransform', 'msTransform', 'OTransform', 'transform'],
                noopStyle = document.createElement('span').style;

            for (var i = 0, il = methodsArray.length; i < il; i += 1) {
                if (ng.isDefined(noopStyle[methodsArray[i]])) {
                    this.transform = methodsArray[i];
                    break;
                }
            }

            return this.transform;
        };
    };

    ng.module('modal')
      .controller('ModalCtrl', ModalController);

    ModalController.$inject = ['$element', '$attrs', '$location', '$scope', 'modalDefaultOptions', 'modalService'];

})(angular);