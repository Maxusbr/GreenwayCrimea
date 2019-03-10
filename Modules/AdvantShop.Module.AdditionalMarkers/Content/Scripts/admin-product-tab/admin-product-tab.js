; (function (ng) {
    'use strict';

    var AMAdminProductTabCtrl = function (toaster, moduleAdminService) {
        var ctrl = this;
        ctrl.markers = [];

        ctrl.$onInit = function () {
            ctrl.pID = ctrl.productId;
            ctrl.getAllMarkers();
            ctrl.getMarkers(ctrl.pID);
        }

        ctrl.getAllMarkers = function () {
            moduleAdminService.getAllMarkers().then(function (data) {
                ctrl.markers = data;
            });
        }

        ctrl.getMarkers = function (productId) {
            moduleAdminService.getMarkers(productId).then(function (data) {
                [].forEach.call(ctrl.markers, function (val) {
                    [].forEach.call(data.Markers, function (m) {
                        if (val.MarkerId === m.MarkerId)
                        {
                            val.Selected = true;
                        }
                    })
                })
            })
        }

        ctrl.link = function (productId, marker) {
            moduleAdminService.link(productId, marker.MarkerId).then(function (data) {
                if (data === 1)
                {
                    marker.Selected = true;
                    toaster.pop('success', '', 'Маркер "' + marker.Name + '" привязан к товару');
                } else {
                    marker.Selected = false;
                    toaster.pop('success', '', 'Маркер "' + marker.Name + '" удален из товара');
                }
            })
        }

    }


    AMAdminProductTabCtrl.$inject = ['toaster', 'moduleAdminService'];

    ng.module('amAdminProductTab', [])
        .controller('AMAdminProductTabCtrl', AMAdminProductTabCtrl)
        .component('amAdminProductTab', {
            templateUrl: '../modules/additionalmarkers/content/scripts/admin-product-tab/admin-product-tab.html',
            controller: 'AMAdminProductTabCtrl',
            bindings: {
                productId:'@'
            }
        });

})(window.angular)