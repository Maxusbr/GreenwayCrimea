; (function (ng) {
    'use strict';

    ng.module('submenu')
    .directive('submenuContainer', ['domService', 'submenuConfig', 'submenuService', 'windowService', function (domService, submenuConfig, submenuService, windowService) {
        return {
            restrict: 'A',
            controller: 'SubmenuContainerCtrl',
            controllerAs: 'submenuContainer',
            bindToController: true,
            scope: true,
            link: function (scope, element, attrs, ctrl) {

                var timeoutMove, timeoutLeave, timeoutHover, possiblyActivate, submenuShow, blockOrientation;

                ctrl.options = ng.extend(ctrl.options, ng.copy(submenuConfig), (new Function('return ' + attrs.submenuContainer))() || {});

                if (ctrl.options.blockOrientation != null) {
                    blockOrientation = document.querySelector(ctrl.options.blockOrientation);
                }

                ctrl.addContainerForOrientation(blockOrientation || element[0]);

                if (ctrl.options.type !== 'accordion' && ctrl.options.type !== 'treeview') {

                    submenuShow = function (submenuParent) {

                        ctrl.getBlockOrientation().style.zIndex = 999999;

                        ctrl.deactive(submenuParent);

                        if (submenuParent.submenu != null) {

                            submenuParent.submenu.hiddenSubmenu();

                            if (submenuParent.submenu.isInit == null || submenuParent.submenu.isInit === false) {
                                setTimeout(function () {

                                    if (submenuParent.submenu.options.checkOrientation === true && (submenuParent.submenu.isCheckedOrientation == null || submenuParent.submenu.isCheckedOrientation === false)) {
                                        submenuParent.submenu.checkSubmenuOrientation(ctrl.getContainerRect(), submenuParent.submenu.options.verticalOrientation);
                                    }

                                    submenuParent.submenu.setInitilazed();
                                    submenuParent.submenu.open();
                                    submenuParent.submenu.visibleSubmenu();
                                    scope.$digest();
                                });
                            } else {
                                submenuParent.submenu.open();
                                submenuParent.submenu.visibleSubmenu();
                            }


                        }

                        scope.$digest();
                    };

                    possiblyActivate = function (submenuParent) {

                        var delay = 0;

                        if (submenuParent.submenu != null) {
                            delay = submenuParent.submenu.checkInTriangle(ctrl.getContainerRect());
                        }

                        if (timeoutMove != null) {
                            clearTimeout(timeoutMove);
                        }

                        if (delay) {
                            timeoutMove = setTimeout(function () {
                                possiblyActivate(submenuParent);
                            }, delay);
                        } else {
                            submenuShow(submenuParent);
                        }

                    };

                    element[0].addEventListener('touchstart', function (event) {

                        event.stopPropagation();

                        var submenuEl = domService.closest(event.target, '[data-submenu]', '[data-submenu-container]'),
                            submenuParentEl = domService.closest(event.target, '[data-submenu-parent]', '[data-submenu-container]'),
                            submenuParentLink = domService.closest(event.target, 'a', '[data-submenu-container]'),
                            submenuParentIndex,
                            submenuParent;

                        if (submenuParentEl != null) {

                            submenuParent = ctrl.items[submenuParentEl.getAttribute('data-submenu-parent-index')];

                            if (submenuParent == null || submenuParent.submenu == null) {
                                return;
                            }

                            if (submenuParentLink != null && submenuParent.submenu.isSubmenuVisible === false) {
                                event.preventDefault();
                            }

                            submenuShow(submenuParent);
                        }
                    });

                    windowService.addCallback('touchstart', function (eventObj) {

                        var isClickedMe = domService.closest(eventObj.event.target, '[data-submenu-container]') != null;

                        if (isClickedMe === false) {
                            ctrl.deactiveAll();
                            scope.$digest();
                        }
                    });

                    element[0].addEventListener('mouseenter', function () {
                        submenuService.startSpyMove();
                        if (timeoutLeave != null) {
                            clearTimeout(timeoutLeave);
                        }
                    });

                    element[0].addEventListener('mouseover', function (event) {

                        if (timeoutMove != null) {
                            clearTimeout(timeoutMove);
                        }

                        if (timeoutHover != null) {
                            clearTimeout(timeoutHover);
                        }

                        timeoutHover = setTimeout(function () {

                            var submenuElLimit = domService.closest(event.target, '[data-submenu-parent]', '[data-submenu]'),
                                submenuEl = domService.closest(event.target, '[data-submenu]', submenuElLimit),
                                submenuParentIndex,
                                submenuParent;

                            //если событие было вызвано элементом в data-submenu-parent, а не в data-submenu
                            if (submenuEl == null && submenuElLimit != null) {

                                submenuParent = ctrl.items[submenuElLimit.getAttribute('data-submenu-parent-index')];

                                if (submenuParent != null) {
                                    possiblyActivate(submenuParent);
                                }
                            }

                        }, ctrl.options.delayHover);
                    });

                    element[0].addEventListener('mouseleave', function () {
                        timeoutLeave = setTimeout(function () {
                            submenuService.stopSpyMove();

                            if (timeoutMove != null) {
                                clearTimeout(timeoutMove);
                            }

                            if (timeoutHover != null) {
                                clearTimeout(timeoutHover);
                            }
                            ctrl.deactiveAll(ctrl.getBlockOrientation());
                            ctrl.getBlockOrientation().style.zIndex = 0;
                            scope.$digest();
                        }, 350);
                    });
                }
            }
        }
    }]);

    ng.module('submenu')
    .directive('submenuParent', [function () {
        return {
            require: ['submenuParent', '^^submenuContainer', '?^^submenuParent'],
            restrict: 'A',
            controller: 'SubmenuParentCtrl',
            controllerAs: 'submenuParent',
            bindToController: true,
            scope: true,
            link: function (scope, element, attrs, ctrls) {

                var submenuCurrentParent = ctrls[0],
                    submenuContainer = ctrls[1],
                    submenuParent = ctrls[2],
                    index = submenuContainer.items.length;

                if (submenuParent != null) {
                    submenuCurrentParent.addParent(submenuParent);
                }

                submenuContainer.addItem(submenuCurrentParent);

                submenuCurrentParent.index = index;

                submenuCurrentParent.memoryElement(element[0])

                attrs.$set('data-submenu-parent-index', index);
            }
        };
    }]);

    ng.module('submenu')
       .directive('submenu', function () {
           return {
               require: ['submenu', '^submenuParent', '^^submenuContainer'],
               restrict: 'A',
               scope: true,
               controller: 'SubmenuCtrl',
               controllerAs: 'submenu',
               bindToController: true,
               link: function (scope, element, attrs, ctrls) {

                   var submenu = ctrls[0],
                       submenuParent = ctrls[1],
                       submenuContainer = ctrls[2];

                   submenu.options = submenuContainer.getOptions();

                   submenuParent.addSubmenu(submenu);
               }
           };
       });

})(angular);