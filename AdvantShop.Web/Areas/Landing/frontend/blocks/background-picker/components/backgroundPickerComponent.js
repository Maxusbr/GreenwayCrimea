; (function (ng) {
    'use strict';

    ng.module('backgroundPicker')
        .component('backgroundPicker', {
            templateUrl: 'areas/landing/frontend/blocks/background-picker/templates/backgroundPicker.html',
            controller: 'BackgroundPickerCtrl',
            bindings: {
                onUpdate: '&',
                colors: '<'
            }
        });
})(window.angular);