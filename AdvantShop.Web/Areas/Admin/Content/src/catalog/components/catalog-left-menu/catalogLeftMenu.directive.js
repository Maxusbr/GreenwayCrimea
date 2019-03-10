; (function (ng) {
    'use strict';

    ng.module('catalog')
        .directive('catalogLeftMenu', ['$parse', function ($parse) {
            return{
                scope: true,
                restrict: 'A',
                controller: 'CatalogLeftMenuCtrl',
                controllerAs: 'catalogLeftMenu',
                bindToController: true,
                link: function (scope, element, attrs, ctrl) {
                    if (attrs.onInit != null && attrs.onInit.length > 0) {
                        $parse(attrs.onInit)(scope, {
                            catalogLeftMenu: ctrl
                        })
                    }
                }
            }
        }]);

})(window.angular);
