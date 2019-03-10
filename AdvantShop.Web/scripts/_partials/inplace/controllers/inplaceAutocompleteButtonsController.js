; (function (ng) {
    'use strict';

    var InplaceAutocompleteButtonsCtrl = function (inplaceService) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.inplaceAutocomplete = inplaceService.getInplaceAutocomplete(ctrl.inplaceAutocompleteButtons);
        };

        ctrl.btnSave = function () {
            ctrl.inplaceAutocomplete.clickedButtons = true;
            ctrl.inplaceAutocomplete.save();
        };

        ctrl.btnCancel = function () {
            ctrl.inplaceAutocomplete.clickedButtons = true;
            ctrl.inplaceAutocomplete.cancel();
        };
    };

    ng.module('inplace')
      .controller('InplaceAutocompleteButtonsCtrl', InplaceAutocompleteButtonsCtrl);

    InplaceAutocompleteButtonsCtrl.$inject = ['inplaceService'];
})(window.angular);