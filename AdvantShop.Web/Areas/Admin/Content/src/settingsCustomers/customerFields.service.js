; (function (ng) {
    'use strict';

    var customerFieldsService = function ($http, uiGridConstants) {
        var service = this;

        service.getFormData = function () {
            return $http.get('customerFields/getCustomerFieldFormData').then(function (response) {
                return response.data;
            });
        }

        service.getCustomerField = function (id) {
            return $http.post('customerFields/get', { id: id, rnd: Math.random() }).then(function (response) {
                return response.data;
            });
        }

        service.deleteCustomerField = function (id) {
            return $http.post('customerFields/delete', { id: id }).then(function (response) {
                return response.data;
            });
        }

        service.addOrUpdateCustomerField = function (add, params) {
            var url = add === true ? 'customerFields/add' : 'customerFields/update';
            return $http.post(url, params).then(function (response) {
                return response.data;
            });
        }

        service.getFilterColumns = function () {
            return $http.get('customers/getCustomerFields').then(function (response) {
                var data = response.data,
                    columns = [];

                if (data != null) {
                    for (var i = 0; i < data.length; i++) {
                        var column = {
                            name: '_noopColumnCustomerField_' + data[i].Id,
                            visible: false,
                            filter: {
                                placeholder: data[i].Name,
                                name: 'CustomerFields[' + data[i].Id + '].Value',
                            }
                        };
                        switch (data[i].FieldType) {
                            case 0: // select
                                column.filter.type = uiGridConstants.filter.SELECT;
                                column.filter.fetch = 'customers/getCustomerFieldValues?id=' + data[i].Id;
                                column.filter.name = 'CustomerFields[' + data[i].Id + '].ValueExact';
                                break;
                            case 2: // number
                                column.filter.type = 'range';
                                column.filter.rangeOptions = {
                                    from: { name: 'CustomerFields[' + data[i].Id + '].From' },
                                    to: { name: 'CustomerFields[' + data[i].Id + '].To' }
                                }
                                break;
                            case 4: // date
                                column.filter.type = 'date';
                                column.filter.term = {
                                    from: (new Date()).setMonth((new Date()).getMonth() - 1),
                                    to: new Date()
                                };
                                column.filter.dateOptions = {
                                    from: { name: 'CustomerFields[' + data[i].Id + '].DateFrom' },
                                    to: { name: 'CustomerFields[' + data[i].Id + '].DateTo' }
                                }
                                break;
                            default:
                                column.filter.type = uiGridConstants.filter.INPUT;
                                break;
                        }
                        columns.push(column);
                    }
                }

                return columns;
            });
        }
    };

    customerFieldsService.$inject = ['$http', 'uiGridConstants'];

    ng.module('settingsCustomers')
        .service('customerFieldsService', customerFieldsService);

})(window.angular);