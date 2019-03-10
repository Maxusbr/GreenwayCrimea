; (function (ng) {
    'use strict';

    var ModalProductsSelectvizrCtrl = function ($uibModalInstance, uiGridCustomConfig, uiGridConstants, $http, $q) {
        var ctrl = this;

        ctrl.$onInit = function () {
            ctrl.selectvizrTreeUrl = 'catalog/categoriestree';
            ctrl.selectvizrGridUrl = 'catalog/getcatalog';
            ctrl.data = [];
            ctrl.itemsSelected = ctrl.$resolve != null && ctrl.$resolve.value != null ?  ng.copy(ctrl.$resolve.value.itemsSelected) : null;

            ctrl.selectvizrGridOptions = ng.extend({}, uiGridCustomConfig, {
                columnDefs: [
                  {
                      name: 'ProductArtNo',
                      displayName: 'Артикул',
                      width: 100,
                      filter: {
                          placeholder: 'Артикул',
                          type: uiGridConstants.filter.INPUT,
                          name: 'ArtNo',
                      }
                  },
                  {
                      name: 'Name',
                      displayName: 'Название',
                      filter: {
                          placeholder: 'Название',
                          type: uiGridConstants.filter.INPUT,
                          name: 'Name',
                      }
                  },
                  //{
                  //    visible: false,
                  //    name: 'PhotoSrc',
                  //    filter: {
                  //        placeholder: 'Изображение',
                  //        type: uiGridConstants.filter.SELECT,
                  //        name: 'HasPhoto',
                  //        selectOptions: [{ label: 'С фотографией', value: true }, { label: 'Без фотографии', value: false }]
                  //    }
                  //},
                  {
                      visible: false,
                      name: 'BrandId',
                      filter: {
                          placeholder: 'Производитель',
                          type: uiGridConstants.filter.SELECT,
                          name: 'BrandId',
                          fetch: 'catalog/getBrandList'
                      }
                  },
                  {
                      visible: false,
                      name: 'Price',
                      filter: {
                          placeholder: 'Цена',
                          type: 'range',
                          rangeOptions: {
                              from: {
                                  name: 'PriceFrom'
                              },
                              to: {
                                  name: 'PriceTo'
                              },
                          },
                          fetch: 'catalog/getpricerangeforpaging'
                      }
                  },
                  {
                      visible: false,
                      name: 'Amount',
                      filter: {
                          placeholder: 'Количество',
                          type: 'range',
                          rangeOptions: {
                              from: {
                                  name: 'AmountFrom'
                              },
                              to: {
                                  name: 'AmountTo'
                              }
                          },
                          fetch: 'catalog/getamountrangeforpaging'
                      },
                  },
                  {
                      visible: false,
                      name: 'Enabled',
                      filter: {
                          placeholder: 'Активность',
                          type: uiGridConstants.filter.SELECT,
                          selectOptions: [{ label: 'Активные', value: true }, { label: 'Неактивные', value: false }]
                      }
                  }
                ],
                enableFullRowSelection: true
            });

            if (ctrl.$resolve.multiSelect === false) {
                ng.extend(ctrl.selectvizrGridOptions, {
                    multiSelect: false,
                    modifierKeysToMultiSelect: false,
                    enableRowSelection: true,
                    enableRowHeaderSelection: false
                });
            }
        };

        ctrl.onChange = function (categoryId, ids, selectMode) {

            var itemIndex;

            for (var i = 0, len = ctrl.data.length; i < len; i++) {
                if (ctrl.data[i].categoryId === categoryId) {
                    itemIndex = i;
                    break;
                }
            }

            if (itemIndex != null) {
                ctrl.data[itemIndex].ids = ids;
                ctrl.data[itemIndex].selectMode = selectMode;
            } else {
                ctrl.data.push({
                    categoryId: categoryId,
                    ids: ids,
                    selectMode: selectMode
                })
            }
        };

        ctrl.select = function () {

            var promiseArray;

            ctrl.data.forEach(function (dataItem) {
                if (dataItem.selectMode == "all") {
                    var promise = $http.get('catalog/getCatalogIds', { params: dataItem }).then(function (response) {
                        if (response.data != null) {
                            dataItem.selectMode = 'none';
                            dataItem.ids = response.data.ids.filter(function (item) {
                                return dataItem.ids.indexOf(item) === -1;
                            });
                        }

                        return dataItem;
                    });

                    promiseArray = promiseArray || [];

                    promiseArray.push(promise);
                }
            });


            $q.all(promiseArray || ctrl.data).then(function (data) {
                var allIds = data.reduce(function (previousValue, currentValue) {
                    return previousValue.concat(currentValue.ids);
                }, [])

                var uniqueItems = [];

                allIds.concat(ctrl.itemsSelected || []).forEach(function (item) {
                    uniqueItems.indexOf(item) === -1 ? uniqueItems.push(item) : null;
                });


                $uibModalInstance.close({ ids: uniqueItems });
            })
        };

        ctrl.close = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };

    ModalProductsSelectvizrCtrl.$inject = ['$uibModalInstance', 'uiGridCustomConfig', 'uiGridConstants', '$http', '$q'];

    ng.module('uiModal')
        .controller('ModalProductsSelectvizrCtrl', ModalProductsSelectvizrCtrl);

})(window.angular);