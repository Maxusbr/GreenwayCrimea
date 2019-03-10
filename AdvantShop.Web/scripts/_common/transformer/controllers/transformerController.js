; (function (ng) {
    'use strict';

    //TODO: вынести работу с DOMом

    var isTouchDevice = 'ontouchstart' in document.documentElement;

    var TransformerCtrl = function ($window, $element, $scope) {

        var ctrl = this,
            container,
            containerStartRect,
            containerRect,
            elementRect,
            elBoxIndents,
            elWrapBoxIndents,
            elementStartRect,
            mutableItems = [];

        ctrl.isTouchDevice = isTouchDevice;


        ctrl.addContainer = function (containerElement) {
            var temp,
                tempEl;
            container = containerElement;
            temp = container.getBoundingClientRect();
            tempEl = $element[0].getBoundingClientRect();

            elBoxIndents = {
                paddingLeft:parseFloat($($element[0]).css('padding-left').slice(0, -2)),
                marginLeft: parseFloat($($element[0]).css('margin-left').slice(0, -2)),
            };

            elWrapBoxIndents = {
                paddingLeft: parseFloat($($element[0]).parent().css('padding-left').slice(0, -2)),
                marginLeft: parseFloat($($element[0]).parent().css('margin-left').slice(0, -2)),
            }

            elementStartRect = {
                height: tempEl.height
            }

            containerStartRect = {
                top: temp.top + $window.pageYOffset,
                right: temp.right + $window.pageXOffset,
            };

            ctrl.calc();
            
        };

        ctrl.alignmentX = function (scrollOverNew) {
            var containerRect = container.getBoundingClientRect(),
                tempRect = containerRect.left,
                containerBoxIndents = parseFloat($(container).css('padding-left').slice(0, -2)) + parseFloat($(container).css('margin-left').slice(0, -2)),
                elLeft = $element[0].offsetLeft;
            if (tempRect != 0 && tempRect > 0) {
                $element[0].style.left = tempRect + containerBoxIndents + elBoxIndents.paddingLeft + elBoxIndents.marginLeft + elWrapBoxIndents.marginLeft + elWrapBoxIndents.paddingLeft + 'px';
                ctrl.elementSizes.left = tempRect + elBoxIndents.paddingLeft + elBoxIndents.marginLeft + containerBoxIndents + elWrapBoxIndents.marginLeft + elWrapBoxIndents.paddingLeft;
            } else {
                ctrl.elementSizes.left = ctrl.elStartRect;
            }
            
        };

        ctrl.responsiveAlignmentX = function () {
            if ((window.matchMedia("(max-width: 767px)").matches) && ctrl.checkoutResponsive) {
                $element[0].classList.add('responsive-checkout');
            } else {
                $element[0].classList.remove('responsive-checkout');
                ctrl.calc();
            }
        }

        ctrl.calc = function () {
            var scrollOverOld = ctrl.scrollOver,
                reezeOld = ctrl.freeze,
                freezeNew,
                containerRect = container.getBoundingClientRect(),
                elementRect = $element[0].getBoundingClientRect(),
                scrollOverNew = $window.pageYOffset > containerStartRect.top;


            if (ctrl.elementSizes == null || ctrl.elementSizes.width === 0) {

                if (elementRect.width === 0) {
                    return;
                }

                ctrl.elementSizes = {
                    //height: elementRect.height,
                    width: elementRect.width
                };
            }

            if (ctrl.limitPos === true) {
                if (scrollOverNew === false) {
                    //default state
                    freezeNew = false;
                    ctrl.elementSizes.transform = 'translate3d(0, 0, 0)';
                    ctrl.elementSizes.top = 'auto';
                    ctrl.expandMutableItems();
                } else {
                    
                    if (containerRect.height + containerStartRect.top - ctrl.offsetTop <= elementRect.height + window.pageYOffset) {
                        //bottom sticky
                        freezeNew = true;
                        ctrl.elementSizes.transform = 'translate3d(0, ' + (containerRect.height - elementRect.height) + 'px, 0)';
                        ctrl.elementSizes.top = 'auto';
                    } else {
                        //fixed
                        freezeNew = false;
                        //if (ctrl.isTouchDevice === true) {
                        //    //ctrl.elementSizes.transform = 'translate3d(0, ' + ($window.pageYOffset - containerStartRect.top + ctrl.offsetTop) + 'px, 0)'
                        //    ctrl.elementSizes.transform = 'translate3d(0, ' + 0 + 'px, 0)';
                        //    ctrl.elementSizes.top = ctrl.offsetTop + 'px';
                        //} else {
                            ctrl.elementSizes.transform = 'translate3d(0, 0, 0)';
                            ctrl.elementSizes.top = ctrl.offsetTop + 'px';
                        //}
                            ctrl.alignmentX(scrollOverNew);

                        if ((elementRect.height + ctrl.offsetTop) > $window.innerHeight) {
                            ctrl.collapseMutableItems((elementRect.height + containerStartRect.top) - $window.innerHeight);
                        } else if ((ctrl.offsetTop + elementStartRect.height) < $window.innerHeight) {
                            ctrl.expandMutableItems();
                        }
                    }
                
                }
            }



            if (freezeNew === true) {
                ctrl.scrollOver = false
            } else {
                ctrl.scrollOver = scrollOverNew;
            }

            ctrl.freeze = freezeNew;

            if (ctrl.scrollOver === true) {
                $element[0].classList.remove('transformer-scroll-default');
                $element[0].classList.add('transformer-scroll-over');
            } else if (ctrl.scrollOver === false) {
                $element[0].classList.remove('transformer-scroll-over');
                $element[0].classList.add('transformer-scroll-default');
            }

            if (ctrl.freeze) {
                $element[0].classList.add('transformer-freeze');
            } else {
                $element[0].classList.remove('transformer-freeze');
            }


            $element.css(ctrl.elementSizes);

        };

        ctrl.windowScroll = function () {

            if (ctrl.initialize !== true) {
                ctrl.wait === true;
                return;
            }

            ctrl.calc();

            $scope.$digest();
        };

        ctrl.addMutableItem = function (element) {
            mutableItems.push({
                el: element,
                height: element.offsetHeight
            });
        };

        ctrl.collapseMutableItems = function (dim) {
            var sumMutable = 0;

            for (var i = 0, len = mutableItems.length; i < len; i++) {
                if (sumMutable < dim) {
                    sumMutable += mutableItems[i].height;
                    mutableItems[i].el.classList.add('transformer-hidden');
                } else {
                    break;
                }
            }
        };

        ctrl.expandMutableItems = function () {
            for (var i = 0, len = mutableItems.length; i < len; i++) {
                mutableItems[i].el.classList.remove('transformer-hidden');
            }
        };
    };

    ng.module('transformer')
      .controller('TransformerCtrl', TransformerCtrl);

    TransformerCtrl.$inject = ['$window', '$element', '$scope'];

})(window.angular);