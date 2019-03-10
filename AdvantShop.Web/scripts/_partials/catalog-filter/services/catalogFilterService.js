; (function (ng) {

    'use strict';

    var catalogFilterService = function () {

        var service = this,
            filterStorage;



        service.buildUrl = function (filterSelectedItems) {
            var result = [], obj;

            for (var item in filterSelectedItems) {

                if (ng.isArray(filterSelectedItems[item]) === true) {
                    obj = item + '=' + filterSelectedItems[item].map(function (val) {
                        return ng.isArray(val) ? val.join(',') : val;
                    }).join('-');
                } else {
                    obj = item + '=' + filterSelectedItems[item];
                }

                result.push(obj);
            }

            return result.join('&');
        };

        service.getSelectedData = function (filterData) {

            if (filterData == null) {
                return null;
            }

            var arraySelected,
                selectedItems,
                item,
                nameRangeMin, nameRangeMax;

            for (var i = filterData.length - 1; i >= 0; i--) {

                item = filterData[i];

                if (item == null)
                    continue;

                //ищем выбранные значения
                if (item.Control == 'select' || item.Control == 'selectSearch') {
                    if (item.Selected != null && item.Selected.Id !== '0') {
                        selectedItems = item.Selected.Id;
                    }
                }
                else {
                    selectedItems = item.Values.filter(function (item) { return item.Selected }).map(function (item) { return item.Id; });
                }

                //добавляем эти значения в массив
                if (selectedItems != null && selectedItems.length > 0) {

                    arraySelected = arraySelected || {};

                    arraySelected[item.Type] = arraySelected[item.Type] || [];

                    arraySelected[item.Type].push(selectedItems);
                }

                //добавляем текст для поиска
                if (item.Control == 'input' && item.Text.length > 0) {
                    arraySelected = arraySelected || {};
                    arraySelected["q"] = [item.Text];
                }

                //добавляем значения из ползунков 
                if (item.Control == 'range') {
                    arraySelected = arraySelected || {};

                    if (item.Type == 'price') {
                        nameRangeMin = 'pricefrom';
                        nameRangeMax = 'priceto';
                    } else {
                        nameRangeMin = item.Type + '_' + item.Values[0].Id + '_min';
                        nameRangeMax = item.Type + '_' + item.Values[0].Id + '_max';
                    }

                    if (item.Values[0].Min !== item.Values[0].CurrentMin || item.Values[0].Max !== item.Values[0].CurrentMax) {
                        arraySelected[nameRangeMin] = [item.Values[0].CurrentMin];
                        arraySelected[nameRangeMax] = [item.Values[0].CurrentMax];
                    }
                }
            }

            return arraySelected;
        };

        service.parseSearchString = function (str) {
            var index = str.indexOf('?'),
                strNormalize = str,
                parameters = {},
                arrayKeyValues,
                temp;

            if (index > -1) {
                strNormalize = strNormalize.substring(index + 1);
            }

            arrayKeyValues = strNormalize.split('&');

            for (var i = arrayKeyValues.length - 1; i >= 0; i--) {
                temp = arrayKeyValues[i].split('=');

                if (temp.length === 2) {
                    parameters[decodeURIComponent(temp[0])] = decodeURIComponent(temp[1]);
                }
            }

            return parameters;
        };

        service.saveFilterData = function (filter) {
            filterStorage = filter;
        };

        service.getFilterData = function () {
            return filterStorage;
        };
    };

    ng.module('catalogFilter')
      .service('catalogFilterService', catalogFilterService);

    catalogFilterService.$inject = [];

})(window.angular);