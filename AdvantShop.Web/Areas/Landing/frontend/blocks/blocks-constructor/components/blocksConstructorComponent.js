; (function (ng) {
    'use strict';

    ng.module('blocksConstructor')
    .directive('blocksConstructorContainer', function () {
        return {
            restrict: 'EA',
            scope: true,
            controller: 'BlocksConstructorContainerCtrl',
            controllerAs: 'blocksConstructorContainer',
            bindToController: true
        }
    });

    ng.module('blocksConstructor')
        .component('blocksConstructor', {
            templateUrl: 'areas/landing/frontend/blocks/blocks-constructor/templates/blocksConstructor.html',
            controller: 'BlocksConstructorCtrl',
            require: {
                blocksConstructorContainerCtrl: '^blocksConstructorContainer'
            },
            bindings: {
                landingpageId: '@',
                blockId: '@',
                name: '@',
                type: '@',
                sortOrder: '<',
                settings: '<',
                isShowAddSubblock: '<'
            }
        });

    ng.module('blocksConstructor')
    .component('blocksConstructorModalNewBlock', {
        templateUrl: 'areas/landing/frontend/blocks/blocks-constructor/templates/blocks/new.html',
        controller: 'BlocksConstructorNewBlockCtrl',
        bindings: {
            modalData: '<',
            onApply: '&'
        }
    });

    ng.module('blocksConstructor')
    .component('blocksConstructorModalSettingsBlock', {
        templateUrl: 'areas/landing/frontend/blocks/blocks-constructor/templates/modalSettings.html',
        controller: 'BlocksConstructorSettingsBlockCtrl',
        bindings: {
            modalData: '<',
            onApply: '&',
            onUploadFileBackground: '&',
            onUpdateFileBackground: '&',
            onDeleteFileBackground: '&',
            onCancel: '&'
        }
    });

    ng.module('blocksConstructor')
    .component('blocksConstructorModalAddSubblock', {
        templateUrl: 'areas/landing/frontend/blocks/blocks-constructor/templates/modalAddSubblock.html',
        controller: 'BlocksConstructorAddSubblockCtrl',
        bindings: {
            modalData: '<',
            onApply: '&',
            onCancel: '&'
        }
    });

})(window.angular);