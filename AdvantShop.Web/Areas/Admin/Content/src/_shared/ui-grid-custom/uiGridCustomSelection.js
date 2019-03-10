; (function (ng) {
    'use strict';

    var UiGridCustomSelectionCtrl = function ($http, $q, $timeout) {
        var ctrl = this;

        ctrl.unselectedRows = [];

        ctrl.totalItemsSelected = 0;

        ctrl.isSelectedAll = false;

        ctrl.storageSelectedRows = [];

        ctrl.$onInit = function () {

            ctrl.calcTotalItemsSelected();

            //var oldFuncRowChange = ctrl.gridApi.selection.on.rowSelectionChanged || function () { },
            //    oldFuncRowChangeBatch = ctrl.gridApi.selection.on.rowSelectionChangedBatch || function () { };

            var destroyEventRowChange = ctrl.gridApi.selection == null ? function () { } : ctrl.gridApi.selection.on.rowSelectionChanged(null, function (row, event) {

                //oldFuncRowChange.apply(this, arguments);

                var index,
                    rowEntity = row.entity;

                if (ctrl.isSelectedAll === true) {
                    //ctrl.gridApi.selection.clearSelectedRows();
                    if (row.isSelected === false) {
                        ctrl.unselectedRows.push(rowEntity);
                    } else {
                        index = ctrl.indexOfUnselected(rowEntity);

                        if (index !== -1) {
                            ctrl.unselectedRows.splice(index, 1);
                        }
                    }
                }

                if (ctrl.unselectedRows.length === ctrl.gridOptions.totalItems) {
                    ctrl.isSelectedAll = false;
                }

                ctrl.toggleInStorage(row, row.isSelected);

                ctrl.calcTotalItemsSelected();

                if (ctrl.gridSelectionOnChange != null) {
                    ctrl.gridSelectionOnChange({
                        rows: [row]
                    });
                }
            });

            var destroyEventRowChangeBatch = ctrl.gridApi.selection == null ? function () { } : ctrl.gridApi.selection.on.rowSelectionChangedBatch(null, function (rows, event) {

                $timeout(function () {//$timeout для получения ctrl.gridApi.selection.getSelectAllState()
                    //oldFuncRowChangeBatch.apply(this, arguments);

                    //ctrl.unselectedRows.length = 0;

                    var isSelectVisibleAll = ctrl.gridApi.selection.getSelectAllState();

                    //if (isSelectVisibleAll === false ) {
                    //    ctrl.clearSelectedRows();
                    var indexInUnselected = -1;

                    rows.forEach(function (item) {
                        if (item.isSelected === false) {

                            ctrl.gridApi.selection.unSelectRow(item.entity);

                            if (ctrl.isSelectedAll === true) {

                                if (item.isSelected === false) {
                                    ctrl.unselectedRows.push(item.entity);
                                } else {
                                    index = ctrl.indexOfUnselected(item.entity);

                                    if (index !== -1) {
                                        ctrl.unselectedRows.splice(index, 1);
                                    }
                                }
                            }
                        } else if ((indexInUnselected = ctrl.indexOfUnselected(item.entity)) !== -1) {
                            ctrl.unselectedRows.splice(indexInUnselected, 1);
                            indexInUnselected = -1;
                        }
                    })
                    //}

                    if (ctrl.unselectedRows.length === ctrl.gridOptions.totalItems) {
                        ctrl.isSelectedAll = false;
                    }

                    ctrl.toggleInStorage(rows, ctrl.isSelectedAll || isSelectVisibleAll || rows[0].isSelected); //rows[0].isSelected - у всех строк при этом событии одинаковое значение

                    ctrl.calcTotalItemsSelected();

                    if (ctrl.gridSelectionOnChange != null) {
                        ctrl.gridSelectionOnChange({
                            rows: rows
                        });
                    }
                });
            });

            ctrl.$onDestroy = function () {
                destroyEventRowChange();
                destroyEventRowChangeBatch();
            };


            if (ctrl.gridSelectionOnInit != null) {
                ctrl.gridSelectionOnInit({ selectionCustom: ctrl });
            }

            //if (ctrl.gridSelectionOnChange != null) {
            //    ctrl.gridSelectionOnChange();
            //}
        };

        ctrl.selectAllRows = function ($event) {

            ctrl.unselectedRows.length = 0;

            ctrl.isSelectedAll = true;

            ctrl.gridApi.selection.selectAllRows();

            ctrl.addItemInStorage(ctrl.gridApi.selection.getSelectedGridRows());

            ctrl.calcTotalItemsSelected();

            if (ctrl.gridApi.selection.getSelectAllState() === true) {
                ctrl.gridApi.selection.raise.rowSelectionChangedBatch(ctrl.gridApi.selection.getSelectedGridRows(), $event);
            }
        };

        ctrl.clearSelectedRows = function () {

            ctrl.gridApi.selection.clearSelectedRows();

            ctrl.unselectedRows.length = 0;

            ctrl.isSelectedAll = false;

            ctrl.clearStorage();

            ctrl.grid.clearSelectionInStorage();

            ctrl.calcTotalItemsSelected();

            //ctrl.gridApi.selection.clearSelectedRows();
        };

        ctrl.select = function (action) {

            var defer = $q.defer(),
                promise = defer.promise;

            if (action.before != null) {
                promise = action.before();
            }
            else {
                defer.resolve();
            }

            return promise.then(function (result) {
                if (action.url != null) {

                    if (ctrl.gridOnRequestBefore != null) {
                        ctrl.gridOnRequestBefore();
                    }

                    return $http.post(action.url, ng.extend({}, ctrl.gridParams, ctrl.getSelectedParams(action.field))).then(function (response) {

                        if (action.after != null) {
                            action.after();
                        }

                        ctrl.clearStorage();

                        if (ctrl.gridOnAction != null) {
                            ctrl.gridOnAction({ response: response });
                        }
                    });
                } else if (action.preset != null) {
                    ctrl.presets[action.preset]();

                    if (ctrl.gridOnAction != null) {
                        ctrl.gridOnAction();
                    }
                }
            });
        };

        ctrl.calcTotalItemsSelected = function () {
            ctrl.totalItemsSelected = ctrl.gridOptions.totalItems - ctrl.unselectedRows.length;
        };

        ctrl.getSelectedParams = function (actionField) {
            //ctrl.gridApi.selection.getSelectedRows()
            var items = ctrl.isSelectedAll ? ctrl.unselectedRows : ctrl.getRowsFromStorage(),
                ids = items.map(function (item) { return item[actionField]; }),
                selectMode = ctrl.isSelectedAll ? 'all' : 'none';
            return ng.extend({}, ctrl.gridParams, { ids: ids, selectMode: selectMode });
        };

        ctrl.selectionCallGrid = function () {
            if (ctrl.gridOnCallUpdate != null) {
                ctrl.gridOnCallUpdate();
            }
        };

        ctrl.getCountSelectedRows = function () {
            return ctrl.gridApi.selection.getSelectedRows().length;
        }

        ctrl.getIsSelectedAll = function () {
            return ctrl.isSelectedAll;
        }

        ctrl.indexOfUnselected = function (rowEntity) {

            var result = -1;

            for (var i = 0, len = ctrl.unselectedRows.length; i < len; i++) {
                if (ctrl.gridOptions.saveRowIdentity(ctrl.unselectedRows[i]) === ctrl.gridOptions.saveRowIdentity(rowEntity)) {
                    result = i;
                    break;
                }
            }

            return result;
        };

        ctrl.gridActionWithCallback = function (callbak) {
            if (callbak != null) {
                callbak();
            }

            if (ctrl.gridOnAction != null) {
                ctrl.gridOnAction();
            }
        }

        ctrl.presets = {
            'unselectVisible': function () {
                ctrl.gridApi.selection.clearSelectedRows();
            },
            'unselectAll': function () {
                ctrl.clearSelectedRows();
            },
        }



        //#region storage
        ctrl.addItemInStorage = function (rows) {

            var rows = ng.isArray(rows) ? rows : (rows != null ? [rows] : []),
                index;

            for (var i = 0, len = rows.length; i < len; i++) {

                index = ctrl.getIndexRowInStorage(rows[i]);

                if (index !== -1) {
                    ctrl.storageSelectedRows[index] = rows[i];
                } else {
                    ctrl.storageSelectedRows.push(rows[i]);
                }
            }
        };

        ctrl.removeItemFromStorage = function (rows) {

            var rows = ng.isArray(rows) ? rows : (rows != null ? [rows] : []),
                index;

            for (var i = 0, len = rows.length; i < len; i++) {

                index = ctrl.getIndexRowInStorage(rows[i]);

                if (index !== -1) {
                    ctrl.storageSelectedRows.splice(index, 1);
                }
            }
        };

        ctrl.toggleInStorage = function (rows, needAdd) {

            if (needAdd === true) {
                ctrl.addItemInStorage(rows);
            } else {
                ctrl.removeItemFromStorage(rows);
            }
        };

        ctrl.clearStorage = function () {
            ctrl.storageSelectedRows.length = 0;
            //ctrl.gridApi.selection.clearSelectedRows();
        };

        ctrl.getIndexRowInStorage = function (row) {
            var index = -1;

            for (var i = 0, len = ctrl.storageSelectedRows.length; i < len; i++) {
                if (ctrl.gridOptions.saveRowIdentity(ctrl.storageSelectedRows[i].entity) === ctrl.gridOptions.saveRowIdentity(row.entity)) {
                    index = i;
                    break;
                }
            }

            return index;
        };

        ctrl.getRowsFromStorage = function () {
            return ctrl.storageSelectedRows.map(function (row) { return row.entity });
        };
        //#endregion
    };

    UiGridCustomSelectionCtrl.$inject = ['$http', '$q', '$timeout'];

    ng.module('uiGridCustomSelection', ['uiModal', 'jsTree.directive'])
        .controller('UiGridCustomSelectionCtrl', UiGridCustomSelectionCtrl);

})(window.angular);