; (function (ng) {
    'use strict';


    var customOptionsService = function ($http) {
        var service = this;

        service.getData = function (productId) {
            return $http.get('product/getcustomoptions', { params: { productId: productId } }).then(function (response) {
                return response.data;
            });
        };

        service.getXml = function (productId, selectedOptions) {
            return $http.get('product/customoptionsxml', { params: { productId: productId, selectedOptions: selectedOptions } }).then(function (response) {
                return response.data;
            });
        };

        service.convertToQuery = function (customOptions) {
            var arrayTemp = [],
                item, val;

            for (var i = customOptions.length - 1; i >= 0; i--) {

                val = null;
                item = customOptions[i];

                if (item.SelectedOptions != null) {

                    //DropDownList = 0,
                    //RadioButton = 1,
                    //CheckBox = 2,
                    //TextBoxSingleLine = 3,
                    //TextBoxMultiLine = 4

                    switch (item.InputType) {
                        case 0:
                        case 1:
                            val = item.SelectedOptions.ID;
                            break;
                        case 2:
                            val = !item.SelectedOptions ? '0' : '1'; //item.SelectedOptions может содержать объект или true
                            break;
                        case 3:
                        case 4:
                            val = item.SelectedOptions.OptionText;
                            break;
                        default:
                            throw Error('Not found InputType for custom options: ' + item.InputType);
                            break;
                    }

                    if (val != null) {
                        arrayTemp.push(i + '_' + val);
                    }
                }
            }

            return arrayTemp.join(';');
        };
    };

    ng.module('customOptions')
      .service('customOptionsService', customOptionsService);

    customOptionsService.$inject = ['$http'];

})(window.angular);