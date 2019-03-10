; (function (ng) {
    'use strict';

    var HelpTriggerCtrl = function ($scope, $document, domService) {

        var ctrl = this,
            triggerHover,
            timerCheckDocumentMouseOver,
            timerCheckDocumentMouseOverFromPopover;

        ctrl.mouseenter = function () {
            triggerHover = true;
            setTimeout(function () {
                if (triggerHover === true) {
                    ctrl.isOpen = true;
                    $scope.$digest();
                }
            }, 350);
        };

        ctrl.bindPopover = function (target) {
            var popover = domService.closest(target, '.popover-inner'),
                result;
            if (popover != null) {

                popover.addEventListener('mouseleave', function popoverMouseleave(event) {
                    popover.removeEventListener('mouseleave', popoverMouseleave);

                    $document[0].addEventListener('mouseover', function documentMouseoverFromPopover(event) {
                        var target = domService.closest(event.target, 'help-trigger');

                        if (target !== null && timerCheckDocumentMouseOverFromPopover != null) {
                            //clearTimeout(timerCheckDocumentMouseOverFromPopover);
                            $document[0].removeEventListener('mouseover', documentMouseoverFromPopover);
                        } else if (timerCheckDocumentMouseOverFromPopover == null) {
                            timerCheckDocumentMouseOverFromPopover = setTimeout(function () {
                                ctrl.close();
                                $scope.$digest();
                                timerCheckDocumentMouseOverFromPopover = null;
                                $document[0].removeEventListener('mouseover', documentMouseoverFromPopover);
                            }, 250);
                        }

                    });
                });

                result = true;
            } else {
                result = false;
            }

            return result;
        }

        ctrl.mouseleave = function () {
            //ctrl.isOpen = false;

            $document[0].addEventListener('mouseover', function documentMouseover(event) {

                var target = event.target;

                if (ctrl.bindPopover(target) === false) {
                    if (timerCheckDocumentMouseOver == null) {
                        timerCheckDocumentMouseOver = setTimeout(function () {
                            ctrl.close();
                            $scope.$digest();
                            $document[0].removeEventListener('mouseover', documentMouseover);
                            timerCheckDocumentMouseOver = null;
                            triggerHover = false;
                        }, 500);
                    }
                } else {
                    clearTimeout(timerCheckDocumentMouseOver);
                    $document[0].removeEventListener('mouseover', documentMouseover);
                    timerCheckDocumentMouseOver = null;
                    triggerHover = false;
                }
            });
        };

        ctrl.close = function () {
            ctrl.isOpen = false;
        };
    };

    HelpTriggerCtrl.$inject = ['$scope', '$document', 'domService'];

    ng.module('helpTrigger', ['ui.bootstrap'])
      .controller('HelpTriggerCtrl', HelpTriggerCtrl);

})(window.angular);