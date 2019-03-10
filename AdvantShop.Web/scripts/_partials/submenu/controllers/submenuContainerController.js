; (function (ng) {
    'use strict';

    var SubmenuContainerCtrl = function ($element, $window) {

        var ctrl = this,
            blockOrientation;

        ctrl.items = [];

        ctrl.active = [];

        ctrl.options = {};

        ctrl.addContainerForOrientation = function (element) {
            blockOrientation = element;
        };

        ctrl.getBlockOrientation = function () {
            return blockOrientation;
        }

        ctrl.getContainerRect = function () {
            var rect;

            if (ctrl.rect == null) {
                rect = blockOrientation.getBoundingClientRect();
                ctrl.rect = {
                    top: rect.top + $window.pageYOffset,
                    right: rect.right + $window.pageXOffset,
                    bottom: rect.bottom,
                    left: rect.left,
                    height: rect.height,
                    width: rect.width
                };
            }
            return ctrl.rect;
        };

        ctrl.addItem = function (item) {
            ctrl.items.push(item);
        };

        ctrl.deactiveAll = function () {

            for (var i = 0, len = ctrl.active.length; i < len; i++) {
                if (ctrl.active[i] != null && ctrl.active[i].submenu != null) {
                    ctrl.active[i].submenu.close();
                }
            }
        }

        ctrl.deactive = function (submenuParent) {

            ctrl.memoryActive(submenuParent);

        };

        ctrl.showOneOnly = function (submenuParent, event) {
            var method, rect = submenuParent.element.getBoundingClientRect();
            for (var i = 0, len = ctrl.items.length; i < len; i++) {
                if (ctrl.items[i] != null && ctrl.items[i].submenu != null) {

                    method = ctrl.items[i] === submenuParent && submenuParent.submenu.isSubmenuVisible != true ? 'open' : 'close';

                    ctrl.items[i].submenu[method]();
                }
            }

            if (event != null) {
                setTimeout(function () {

                    var currentRect = submenuParent.element.getBoundingClientRect();

                    if (currentRect.top != rect.top && (currentRect.bottom > $window.innerHeight || currentRect.top < 0)) {
                        //event.target.scrollIntoView(true);
                        var scrollValue;

                        //if (currentRect.bottom > $window.innerHeight) {
                        //    scrollValue = rect.top - currentRect.top;
                        //}

                        if (currentRect.top < 0) {
                            scrollValue = currentRect.top - rect.top;
                        }

                        $window.scrollBy(0, scrollValue);
                    }

                }, 0);
            }
        };

        ctrl.getOptions = function () {
            return ctrl.options;
        };

        ctrl.excludeDeactivateItems = function (parent) {

            if (parent == null) {

                return;
            }

            var isFind = false,
                newActive = [];

            for (var i = 0, len = ctrl.active.length; i < len; i++) {
                //if (ctrl.active[i] != null && ctrl.active[i].submenu != null) {
                //    ctrl.active[i].submenu.close();
                //}

                isFind = ctrl.findParent(ctrl.active[i], parent.parent);

                if (isFind) {
                    newActive.push(ctrl.active[i]);
                } else {
                    if (ctrl.active[i].submenu != null) {
                        ctrl.active[i].submenu.close();
                    }

                }
            }

            ctrl.active = newActive;
        };

        ctrl.findParent = function (item, currentParent) {
            var result = false;

            if (currentParent != null) {

                if (item === currentParent) {

                    result = true;
                } else {

                    result = ctrl.findParent(item, currentParent.parent);
                }
            }

            return result;
        };

        ctrl.memoryActive = function (activeParent) {

            ctrl.excludeDeactivateItems(activeParent);
            ctrl.active.push(activeParent);
        };

    };

    ng.module('submenu')
      .controller('SubmenuContainerCtrl', SubmenuContainerCtrl);

    SubmenuContainerCtrl.$inject = ['$element', '$window'];


})(window.angular);