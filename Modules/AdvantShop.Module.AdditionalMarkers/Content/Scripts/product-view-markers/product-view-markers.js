; (function (ng) {
    'use strict';

    var ProductViewMarkersCtrl = function (toaster, $http, $scope) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.mobile = ctrl.isMobile.toLowerCase() === 'true';
            if (ctrl.mobile) {
                var productViews = document.getElementsByClassName('prod-photo');

                if (productViews.length > 0) {
                    var productUrls = [];
                    [].forEach.call(productViews, function (val) {
                        var url = val.parentElement.getAttribute('href').split('/');
                        productUrls.push(url[url.length - 1]);
                    });
                    $http.post('amclient/GetPVMarkersMobile', { productUrls: productUrls }).then(function (response) {
                        ctrl.productViewMarkers = response.data;
                        $scope.$watch(ctrl.productViewMarkers, function () {
                            setTimeout(function () {
                                ReLocate(true);
                            }, 1);
                        });
                    })
                }
            } else {

                var productViews = document.getElementsByClassName('products-view-item');

                if (productViews.length > 0) {
                    var productIds = [];
                    [].forEach.call(productViews, function (val) { productIds.push(val.dataset.productId) });
                    $http.get('amclient/GetPVMarkers', { params: { productIds: productIds } }).then(function (response) {
                        ctrl.productViewMarkers = response.data;
                        $scope.$watch(ctrl.productViewMarkers, function () {
                            setTimeout(function () {
                                ReLocate(false);
                            }, 1);
                        });
                    })
                }
            }
        }
    };

    function ReLocate(isMobile) {
        if (isMobile) {
            var productViews = document.getElementsByClassName('prod-photo');
            [].forEach.call(productViews, function (pVal) {
                pVal = pVal.parentElement;
                var url = pVal.getAttribute('href').split('/');
                if (url[url.length - 2].toLowerCase() == "products") {
                    var markers = document.querySelectorAll('[data-productview-purl="' + url[url.length - 1] + '"]');
                    if (markers.length > 0) {
                        var set = false;
                        [].forEach.call(markers, function (val) {
                            if (set || val.children.length == 0) {
                                return;
                            }
                            set = true;
                            if (pVal.getElementsByClassName('products-view-labels').length == 0) {
                                var info = pVal.getElementsByClassName('prod-text');
                                var pvl = document.createElement('div');
                                pvl.className = "products-view-labels";
                                info[0].after(pvl);
                            }

                            var markersGallery = pVal.getElementsByClassName('products-view-labels');

                            while (val.children.length > 0) {
                                markersGallery[0].appendChild(val.children[0])
                            }
                        })
                    }
                }
            })
            return;
        }

        var markersWrap = document.getElementById('markers-wrapper-pv');
        [].forEach.call(markersWrap.children, function (val, index) {
            var step = index;
            var elem = document.querySelectorAll('.products-view-item[data-product-id="' + val.dataset.productviewPid + '"]');
            if (elem.length > 0) {
                [].forEach.call(elem, function (el) {
                    if (el.classList.contains('render-markers') || step > index) {
                        return false;
                    } else {
                        el.classList.add('render-markers');
                    }

                    step++;

                    if (el.getElementsByClassName('products-view-labels').length == 0) {
                        var info = el.getElementsByClassName('products-view-info');
                        var pvl = document.createElement('div');
                        pvl.className = "products-view-labels";
                        info[0].after(pvl);
                    }

                    var markersGallery = el.getElementsByClassName('products-view-labels');

                    while (val.children.length > 0) {
                        markersGallery[0].appendChild(val.children[0]);
                    }
                })  //
            }
        })

        markersWrap.parentNode.removeChild(markersWrap);
    }

    ProductViewMarkersCtrl.$inject = ['toaster', '$http', '$scope'];

    ng.module('productViewMarker', [])
        .controller('ProductViewMarkersCtrl', ProductViewMarkersCtrl)
        .component('productViewMarker', {
            templateUrl: 'modules/additionalmarkers/content/scripts/product-view-markers/product-view-markers.html',
            controller: 'ProductViewMarkersCtrl',
            bindings: {
                isMobile: '@'
            }
        });

})(window.angular);