; (function (ng) {
    'use strict';

    ng.module('modal')
    .service('modalService', ['$compile', '$rootScope', '$q', 'modalDefaultOptions', function ($compile, $rootScope, $q, modalDefaultOptions) {

        var modals = {},
            promises = {},
            queue = [],
            working = true,
            modalService = this;

        modalService.stopWorking = function () {
            working = false;
        };

        modalService.startWorking = function () {
            working = true;

            if (queue.length > 0) {
                modalService.open(queue[0].id);
            }
        };

        modalService.isWorking = function () {
            return working === true;
        };

        modalService.checkQueue = function (modal) {
            return queue.length === 0 || modalService.existInQueue(modal);
        };

        modalService.existInQueue = function (modal) {
            return queue.indexOf(modal) !== -1;
        };

        modalService.addQueue = function (modal) {
            queue.push(modal);
        };

        modalService.removeItemQueue = function (modal) {
            var index = queue.indexOf(modal);

            if (index !== -1) {
                queue.splice(index, 1);
            }

            if (queue.length > 0) {
                modalService.open(queue[queue.length - 1].id);
            }
        };

        modalService.open = function (modalId, skipQueue) {

            if (ng.isDefined(modals[modalId])) {
                modals[modalId].modalScope.open(skipQueue);
            }
        };

        modalService.close = function (modalId) {
            if (ng.isDefined(modals[modalId])) {
                modals[modalId].modalScope.close();
            }
        };

        modalService.destroy = function (modalId) {
            if (ng.isDefined(modals[modalId])) {
                modals[modalId].modalScope.destroy();
                modalService.removeFromStorage(modalId);
            }
        };

        modalService.setVisibleFooter = function (modalId, visible) {
            if (ng.isDefined(modals[modalId])) {
                modals[modalId].modalScope.setVisibleFooter(visible);
            }
        };

        modalService.addStorage = function (modalId, modalElement, modalScope) {
            modals[modalId] = {
                modalElement: modalElement,
                modalScope: modalScope
            };

            if (promises[modalId] != null) {
                promises[modalId].resolve(modals[modalId]);
                delete promises[modalId];
            }

            return modals[modalId];
        };

        modalService.hasModal = function (modalId) {
            return modals[modalId] != null;
        };

        modalService.getModal = function (modalId) {
            var defer = $q.defer();

            if (modals[modalId] != null) {
                defer.resolve(modals[modalId]);
            } else {
                promises[modalId] = defer;
            }

            return defer.promise;
        };

        modalService.renderAttrubutes = function (attrubutes) {
            var arrStrings = [],
                tempString, keyFormatted;

            for (var key in attrubutes) {
                if (attrubutes.hasOwnProperty(key)) {


                    keyFormatted = key.replace(/[A-Z]/g, function (str) {
                        return "-" + str.toLowerCase();
                    });

                    tempString = [keyFormatted, '=', '"', attrubutes[key], '"'].join('');
                    arrStrings.push(tempString);
                }
            }

            return arrStrings.join(' ');
        };

        modalService.renderModal = function (modalId, modalHeader, modalContent, modalFooter, options, parentScope) {

            if (ng.isUndefined(modalId) || modalId.length === 0) {
                throw Error('Modal "id" is required');
            }

            if (angular.isDefined(modals[modalId])) {
                return modals[modalId].modalElement;
            }

            options = options || {};

            options = ng.extend({}, modalDefaultOptions, options, { id: modalId });

            var scope = $rootScope.$new(),
                blockStart = ['<modal-control ', modalService.renderAttrubutes(options), '>'],
                header = modalHeader != null ? ['<div class="modal-header" data-modal-header>', modalHeader, '</div>'] : [' '],
                content = modalContent != null ? ['<div class="modal-content">', modalContent, '</div>'] : [' '],
                footer = modalFooter != null ? ['<div class="modal-footer" data-modal-footer>', modalFooter, '</div>'] : [' '],
                blockEnd = ['</modal-control>'],
                compileString = blockStart.join('') + header.join('') + content.join('') + footer.join('') + blockEnd.join(''),
                modalElement;

            modalElement = ng.element(compileString);

            ng.element(document.body).append(modalElement);

            if (parentScope != null) {
                ng.extend(scope, parentScope);
            }

            return $compile(modalElement)(scope);
        };

        modalService.getNewZIndex = function () {
            return modalDefaultOptions.zIndex * (queue.length + 1);
        };

        modalService.removeFromStorage = function (modalId) {
            delete modals[modalId];
        };

    }]);

})(angular);