; (function (ng) {
    'use strict';

    ng.module('switcherState')
        .component('switcherState', {
            templateUrl: '../areas/admin/content/src/_shared/switcher-state/templates/switcherState.html',
            controller: 'SwitcherStateCtrl',
            bindings: {
                checked: '<?',
                onChange: '&',
                textOn: '@',
                textOff: '@',
                name: '@',
                invert: '<?'
            }
        });
})(window.angular);