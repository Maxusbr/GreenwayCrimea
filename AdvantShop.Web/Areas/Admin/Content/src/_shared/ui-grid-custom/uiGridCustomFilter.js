; (function (ng) {
    'use strict';

    var UiGridCustomFilterCtrl = function ($http, $q, $timeout, uiGridConstants) {
        var ctrl = this;

        ctrl.blocks = [];
        ctrl.menuItems = [];
        ctrl.columns = [];
        ctrl.searchTimer = null;

        ctrl.$onInit = function () {
            ctrl.gridColumnDefs.forEach(function (value) {
                if (value.filter != null) {

                    ctrl.columns.push(value);

                    ctrl.addFilterBlockOnLoad(value, ctrl.gridParams);
                }
            });

            ctrl.gridSearchText = ctrl.gridParams.search || ctrl.gridSearchText;

            ctrl.gridSearchPlaceholder = ctrl.gridSearchPlaceholder || 'Введите текст для поиска';

            if (ctrl.onInit != null) {
                ctrl.onInit({ filter: ctrl });
            }
        };

        ctrl.updateColumns = function () {
            ctrl.blocks.length = 0;
            ctrl.columns.length = 0;

            ctrl.gridColumnDefs.forEach(function (value) {
                if (value.filter != null) {
                    ctrl.columns.push(value);
                    ctrl.addFilterBlockOnLoad(value, ctrl.gridParams);
                }
            });
        };

        ctrl.addFilterBlock = function (item, selectedValue, itemPropName) {
            item.filterMaster = {};

            ng.copy(item.filter, item.filterMaster);

            ctrl.fill(item.filter.type, item, selectedValue, itemPropName);

            if (ctrl.blocks.indexOf(item) === -1) {
                ctrl.blocks.push(item);
            }
        };

        ctrl.getDataForFilter = function (item) {
            var defer = $q.defer(),
                promise;

            if (item.filter.fetch != null) {
                if (ng.isString(item.filter.fetch) === true) {
                    promise = $http.get(item.filter.fetch, { params: ctrl.gridParams }).then(function (response) {
                        return response.data;
                    });
                } else {
                    promise = item.filter.fetch;
                }
            } else {
                defer.resolve();
                promise = defer.promise;
            }

            return promise;
        };

        ctrl.checkVisibleMenuItem = function (items) {
            var result = items.filter(function (value) {
                return ctrl.blocks.indexOf(value) === -1
            });

            return result;
        };

        ctrl.onApplySearch = function (params, item, event) {

            if (ctrl.searchTimer != null) {
                $timeout.cancel(ctrl.searchTimer);
            }

            ctrl.searchTimer = $timeout(function () {
                ctrl.onApplyBlock(params, item);
            }, 300);
        };

        ctrl.onApplyBlock = function (params, item) {
            ctrl.onChange({ params: params, item: item });
        };

        ctrl.onClose = function (name, item) {
            var index = ctrl.blocks.indexOf(item);

            if (index !== -1) {
                ctrl.blocks.splice(index, 1);
            }

            item.filter = item.filterMaster;

            ctrl.onRemove({ name: name, item: item });
        };

        ctrl.fill = function (type, item, selectedValue, itemPropName) {

            return ctrl.getDataForFilter(item).then(function (data) {

                if (ctrl.handlerTypes[type] != null) {
                    ctrl.handlerTypes[type].fill(item, data, selectedValue, itemPropName);
                } else {
                    new Error('Not found type ' + item.filter.type + ' for filter in grid');
                }
            });
        };

        ctrl.addFilterBlockOnLoad = function (item, params) {
            var nameFilter, urlValue;

            for (var key in params) {
                if (params.hasOwnProperty(key)) {

                    urlValue = params[key];

                    item.showOnPageLoad = ctrl.handlerTypes[item.filter.type].check(key, item);

                    if (item.showOnPageLoad === true) {
                        ctrl.addFilterBlock(item, urlValue, key);
                    }
                }
            };
        };

        ctrl.handlerTypes = {
            'range': {
                check: function (key, item) {
                    return key === item.filter.rangeOptions.to.name || key === item.filter.rangeOptions.from.name;
                },
                fill: function (item, data, selectedValue, itemPropName) {

                    if (itemPropName === item.filter.rangeOptions.to.name) {
                        item.filter.term = item.filter.term || {};
                        item.filter.term.to = selectedValue;
                    }

                    if (itemPropName === item.filter.rangeOptions.from.name) {
                        item.filter.term = item.filter.term || {};
                        item.filter.term.from = selectedValue;
                    }

                    if (data != null) {
                        item.filter.term = data;
                    }

                    return item;
                }
            },
            'select': {
                check: function (key, item) {
                    return key === item.name || key === item.filter.name;
                },
                fill: function (item, data, selectedValue, itemPropName) {
                    if (data != null) {
                        item.filter.selectOptions = data;
                    }
                    if (selectedValue != null) {
                        item.filter.term = selectedValue.toString();
                    }
                }
            },
            'datetime': {
                check: function (key, item) {
                    return key === item.filter.datetimeOptions.to.name || key === item.filter.datetimeOptions.from.name;
                },
                fill: function (item, data, selectedValue, itemPropName) {

                    if (itemPropName === item.filter.datetimeOptions.to.name) {
                        item.filter.term = item.filter.term || {};
                        item.filter.term.to = selectedValue;
                        item.showOnPageLoad = true;
                    }

                    if (itemPropName === item.filter.datetimeOptions.from.name) {
                        item.filter.term = item.filter.term || {};
                        item.filter.term.from = selectedValue;
                        item.showOnPageLoad = true;
                    }

                    if (data != null) {
                        item.filter.term = data;
                    }
                }
            },
            'date': {
                check: function (key, item) {
                    return key === item.filter.dateOptions.to.name || key === item.filter.dateOptions.from.name;
                },
                fill: function (item, data, selectedValue, itemPropName) {

                    if (itemPropName === item.filter.dateOptions.to.name) {
                        item.filter.term = item.filter.term || {};
                        item.filter.term.to = selectedValue;
                        item.showOnPageLoad = true;
                    }

                    if (itemPropName === item.filter.dateOptions.from.name) {
                        item.filter.term = item.filter.term || {};
                        item.filter.term.from = selectedValue;
                        item.showOnPageLoad = true;
                    }

                    if (data != null) {
                        item.filter.term = data;
                    }
                }
            },
            'input': {
                check: function (key, item) {
                    return key === item.filter.name;
                },
                fill: function (item, data, selectedValue, itemPropName) {
                    item.filter.term = selectedValue;

                    if (data != null) {
                        item.filter.term = data;
                    }
                }
            },
            'number': {
                check: function (key, item) {
                    return key === item.filter.name;
                },
                fill: function (item, data, selectedValue, itemPropName) {
                    item.filter.term = selectedValue;

                    if (data != null) {
                        item.filter.term = data;
                    }
                }
            }
        }
    };

    UiGridCustomFilterCtrl.$inject = ['$http', '$q', '$timeout', 'uiGridConstants'];

    ng.module('uiGridCustomFilter', ['ui.bootstrap.datetimepicker', 'ui.dateTimeInput'])
      .controller('UiGridCustomFilterCtrl', UiGridCustomFilterCtrl);

})(window.angular);