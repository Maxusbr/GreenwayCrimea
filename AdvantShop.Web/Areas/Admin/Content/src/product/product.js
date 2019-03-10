; (function (ng) {
    'use strict';
    var ProductCtrl = function ($http, uiGridCustomConfig, toaster, SweetAlert, $window, productService, $anchorScroll, $location, $document, $timeout) {

        var ctrl = this;

        ctrl.colors = null;
        ctrl.sizes = null;
        ctrl.isProcessGetTags = null;

        ctrl.goToPhotos = function () {
            var photosItem = angular.element(document.getElementById('photos'));
            $document.scrollTo(photosItem, 103, 1000);
        }

        ctrl.initProduct = function (productId) {
            ctrl.productId = productId;

            ctrl.getCategories();
            //ctrl.getOffersValidation();
        };


        //region categories 
        ctrl.getCategories = function () {
            $http.get('product/getCategories', { params: {productId : ctrl.productId}}).then(function (response) {
                ctrl.categories = response.data;
            });
        }

        ctrl.setMainCategory = function () {
            if (ctrl.category == null || ctrl.category.length == 0 || ctrl.category[0].value == null)
                return;

            if (ctrl.category.length != 1) {
                toaster.pop('error', '', 'Выберите одну категорию');
            }
            var categoryId = ctrl.category[0].value;

            $http.post('product/setMainCategory', { productId: ctrl.productId, categoryId: categoryId }).then(function (response) {
                ctrl.getCategories();
                toaster.pop('success', '', 'Изменения сохранены');
            });
        };

        ctrl.deleteCategory = function () {
            if (ctrl.category == null || ctrl.category.length == 0 || ctrl.category[0].value == null)
                return;

            if (ctrl.category.length != 1) {
                toaster.pop('error', '', 'Выберите одну категорию');
            }
            var categoryId = ctrl.category[0].value;

            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result) {
                    $http.post('product/deleteCategory', { productId: ctrl.productId, categoryId: categoryId }).then(function (response) {
                        if (response.data === true) {
                            ctrl.getCategories();
                            toaster.pop('success', '', 'Изменения сохранены');
                        } else {
                            toaster.pop('error', '', 'Ошибка при удалении');
                        }
                    });
                }
            });
        };

        ctrl.addCategory = function (result) {
            var categoryId = result.categoryId;
            $http.post('product/addCategory', { productId: ctrl.productId, categoryId: categoryId }).then(function (response) {
                if (response.data === true) {
                    ctrl.getCategories();
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        };
        //end region

        /* brand */
        ctrl.changeBrand = function (result) {
            $http.post('product/changeBrand', { productId: ctrl.productId, brandId: result.brandId }).then(function (response) {
                if (response.data.result === true) {
                    ctrl.brand = result.brandName;
                    ctrl.brandId = result.brandId;
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        };

        ctrl.deleteBrand = function () {
            $http.post('product/deleteBrand', { productId: ctrl.productId }).then(function (response) {
                if (response.data.result === true) {
                    ctrl.brand = "Не выбран";
                    ctrl.brandId = 0;
                    toaster.pop('success', '', 'Изменения сохранены');
                }
            });
        };
        /* end region */


        /* tags */

        ctrl.tagTransform = function (newTag) {
            return { value: newTag };
        };

        ctrl.getTags = function () {

            ctrl.isProcessGetTags = true;

            $http.get('product/getTags', { params: { productId: ctrl.productId } })
                .then(function (response) {
                    ctrl.tags = response.data.tags;
                    ctrl.selectedTags = response.data.selectedTags;

                    return response.data;
                })
                .then(function (data) {
                    return $timeout(function () {
                        ctrl.form.$setPristine();
                        return data;
                    }, 0);
                })
                .then(function (data) {
                    return $timeout(function () {
                        ctrl.isProcessGetTags = false;
                        return data;
                    }, 500)
                });
        };
        /* end tags */

        ctrl.deleteProduct = function () {
            SweetAlert.confirm("Вы уверены, что хотите удалить?", { title: "Удаление" }).then(function (result) {
                if (result) {
                    $http.post('product/deleteProduct', { productId: ctrl.productId }).then(function (response) {
                        if (response.data.result === true) {
                            $window.location.assign('catalog');
                        } else {
                            toaster.pop('error', '', 'Ошибка при удалении');
                        }
                    });
                }
            });
        };

        /* offers */

        ctrl.offersShow = function () {
            if (ctrl.gridOffersShowed !== true) {
                ctrl.gridOffersShowed = true;
                ctrl.getOffersValidation();
            }
        };

        ctrl.getOffersValidation = function () {
            $http.get('product/getOffersValidation', { params: { productId: ctrl.productId } }).then(function (response) {
                ctrl.offersValidation = response.data.result ? null : response.data.error;
            });
        };

        ctrl.gridOffersOptions = ng.extend({}, uiGridCustomConfig, {
            columnDefs: [
                {
                    name: 'Main',
                    displayName: 'Главная',
                    enableCellEdit: true,
                    enableSorting: false,
                    type: 'checkbox',
                    width: 70,
                },
                {
                    name: 'ArtNo',
                    displayName: 'Артикул',
                    enableCellEdit: true,
                    enableSorting: false,
                    width: 150,
                },
                {
                    name: 'Size',
                    displayName: 'Размер',
                    enableSorting: false,
                    enableCellEdit: true,
                    type: 'select',
                    uiGridCustomEdit: {
                        replaceNullable: false,
                        modelFromCol: 'SizeId',
                        editDropdownLazyLoad: {
                            label: function (col, row) {
                                return row.entity['Size'] || '––––';
                            },
                            value: function (col, row) {
                                return row.entity['SizeId']; 
                            }
                        },
                        editDropdownOptionsFunction: function () {
                            return ctrl.sizes || productService.getSizes().then(function (result) {
                                ctrl.sizes = [];
                                ctrl.sizes.push({ value: '', label: '––––' });
                                ctrl.sizes = ctrl.sizes.concat(result);
                                return ctrl.sizes;
                            });
                        }
                    }
                },
                {
                    name: 'Color',
                    displayName: 'Цвет',
                    enableSorting: false,
                    enableCellEdit: true,
                    type: 'select',
                    uiGridCustomEdit: {
                        replaceNullable: false,
                        modelFromCol: 'ColorId',
                        editDropdownLazyLoad: {
                            label: function (col, row) {
                                return row.entity['Color'] || '––––';
                            },
                            value: function (col, row) {
                                return row.entity['ColorId'];
                            }
                        },
                        editDropdownOptionsFunction: function () {
                            return ctrl.colors || productService.getColors().then(function (result) {
                                ctrl.colors = [];
                                ctrl.colors.push({ value: '', label: '––––' });
                                ctrl.colors = ctrl.colors.concat(result);
                                return ctrl.colors;
                            });
                        }
                    }
                },
                {
                    name: 'BasePrice',
                    displayName: 'Цена',
                    enableCellEdit: true,
                    enableSorting: false,
                    width: 120,
                },
                {
                    name: 'SupplyPrice',
                    displayName: 'Закупочная цена',
                    enableCellEdit: true,
                    enableSorting: false,
                    width: 120,
                },
                {
                    name: 'Amount',
                    displayName: 'Количество',
                    enableCellEdit: true,
                    enableSorting: false,
                    width: 100,
                },
                {
                    name: '_serviceColumn',
                    displayName: '',
                    width: 35,
                    cellTemplate:
                        '<div class="ui-grid-cell-contents"><div>' +
                            '<ui-grid-custom-delete url="product/deleteOffer" params="{\'offerId\': row.entity.OfferId }"></ui-grid-custom-delete>' +
                        '</div></div>'
                }
            ],
        });

        ctrl.gridOffersOnInit = function (grid) {
            ctrl.gridOffers = grid;
        };

        ctrl.gridOffersUpdate = function () {

            if (ctrl.gridOffers != null) {
                ctrl.gridOffers.fetchData();
            }
            
            if (ctrl.productPhotos != null) {
                ctrl.productPhotos.load();
            }
        };

        ctrl.updateMainPhoto = function (mainPhoto) {
            ctrl.mainPhotoSrc = mainPhoto != null ? mainPhoto.ImageSrc : '../images/nophoto_middle.jpg';
        };

        ctrl.initProductPhotos = function (productPhotos) {
            ctrl.productPhotos = productPhotos;
        };

        ctrl.setDiscountType = function (type) {
            ctrl.discountType = type;
            if (type === 0) {
                ctrl.DiscountAmount = 0;
            } else {
                ctrl.DiscountPercent = 0;
            }
        }
    }

    ProductCtrl.$inject = ['$http', 'uiGridCustomConfig', 'toaster', 'SweetAlert', '$window', 'productService', '$anchorScroll', '$location', '$document', '$timeout'];

    ng.module('product', ['angular-inview', 'uiGridCustom', 'productPhotos', 'productPhotos360', 'productVideos', 'productProperties', 'relatedProducts', 'productGifts'])
      .controller('ProductCtrl', ProductCtrl);

})(window.angular);