; (function (ng) {
    'use strict';

    //TO DO: remove attribute data-e2e-col-index

    var UiGridCustomCtrl = function ($element, $interpolate, $location, $scope, $timeout, $q, $window, uiGridConstants, uiGridEditService, uiGridCustomService, uiGridCustomConfig, uiGridCustomParamsConfig, toaster, domService) {
        var ctrl = this,
            isFirstPageLoad = true,
            gridApiReady = $q.defer(),
            locationWatch,
            historyItems = [],
            paramsOnFirstInit = {};

        ctrl.firstLoading = true;
        ctrl.isProcessing = false;

        ctrl.$onInit = function () {
            if (uiGridCustomService.validateId(ctrl.gridUniqueId) === false) {
                throw new Error('Invalid value "gridUniqueId"' + (ctrl.gridUniqueId ? ': ' + ctrl.gridUniqueId : ''));
            }

            uiGridCustomService.addInStorage(ctrl.gridUniqueId);

            ng.extend(ctrl.gridOptions, ctrl.getDataItemFromStorage());

            ctrl._params = ng.extend({}, uiGridCustomParamsConfig, ctrl.gridParams, ctrl.getDataItemFromStorage());

            ctrl.gridFilterEnabled = ctrl.gridFilterEnabled != null ? ctrl.gridFilterEnabled : true;
            ctrl.gridPaginationEnabled = ctrl.gridPaginationEnabled != null ? ctrl.gridPaginationEnabled : true;
            ctrl.gridSelectionEnabled = ctrl.gridSelectionEnabled != null ? ctrl.gridSelectionEnabled : true;

            ctrl.gridEmptyText = ctrl.gridEmptyText != null ? ctrl.gridEmptyText : 'Ни одной записи не найдено';

            var oltherOnRegisterApi = ctrl.gridOptions.onRegisterApi || function () { };

            ctrl.gridOptions.onRegisterApi = function (gridApi) {
                oltherOnRegisterApi(gridApi);

                ctrl.bindGridApi(gridApi);
            }

            ctrl.optionsFromUrl();


            if (ctrl.gridOptions != null && ctrl.gridOptions.rowEntitySave == null) {
                ctrl.gridOptions.saveRowIdentity = function (rowEntity) {
                    var result = null;

                    if (ctrl.gridRowIdentificator != null) {
                        result = rowEntity[ctrl.gridRowIdentificator];
                    } else {
                        result = ctrl.defaultRowEntitySave(rowEntity);
                    }

                    return result;
                };
            }

            return $q.when(ctrl.gridOnPreinit != null ? ctrl.gridOnPreinit({ grid: ctrl }) : null).then(function () {

                ctrl.addHistoryItem();

                return ctrl.fetchData().then(function () {
                    isFirstPageLoad = false;

                    if (ctrl.gridOnInit != null) {
                        ctrl.gridOnInit({ grid: ctrl });
                    }

                    paramsOnFirstInit.gridOptions = ng.copy(ctrl.gridOptions);
                    paramsOnFirstInit._params = ng.copy(ctrl._params);

                    return ctrl;
                });
            })
        };

        $element.on('$destroy', function () {
            uiGridCustomService.removeFromStorage(ctrl.gridUniqueId);
        });

        //$scope.$on('$destroy', function () {
        //    uiGridCustomService.removeFromStorage(ctrl.gridUniqueId);
        //});

        //ctrl.$onDestroy = function () {
        //    uiGridCustomService.removeFromStorage(ctrl.gridUniqueId);
        //};

        ctrl.update = function () {
            ctrl.optionsFromUrl();

            return $q.when(ctrl.gridOnPreinit != null ? ctrl.gridOnPreinit({ grid: ctrl }) : null).then(function () {
                return ctrl.fetchData(true).then(function () {
                    isFirstPageLoad = false;

                    if (ctrl.gridOnInit != null) {
                        ctrl.gridOnInit({ grid: ctrl });
                    }

                    return ctrl;
                });
            })
        }

        ctrl.locationWatch = function () {

            if (locationWatch != null) {
                locationWatch();
            }

            locationWatch = $scope.$on('$locationChangeSuccess', function () {

                var newParams = JSON.stringify($location.search()[ctrl.gridUniqueId]);

                if (ctrl.getLastHistoryItem() !== newParams) {
                    //$timeout(function () {

                    if (newParams == null) {
                        ctrl.gridOptions = paramsOnFirstInit.gridOptions;
                        ctrl._params = paramsOnFirstInit._params;
                    }

                    ctrl.backHistory();

                    ctrl.update();
                    //}, 100);
                }
            });
        };

        ctrl.addHistoryItem = function (item) {

            if (historyItems.indexOf(item) === -1) {
                historyItems.push(item)
            }

            return item;
        }

        ctrl.backHistory = function () {
            historyItems.splice(-1, 1);
        }

        ctrl.getLastHistoryItem = function () {
            return historyItems.length > 0 ? historyItems.slice(-1)[0] : historyItems[0];
        }

        ctrl.locationWatchUnreg = function () {
            locationWatch != null && locationWatch();
        }

        ctrl.getTableHeight = function () {
            var headerHeight = 31,
                rowsLength = ctrl.gridOptions != null && ctrl.gridOptions.data != null && ctrl.gridOptions.data.length > 0 ? ctrl.gridOptions.data.length : 1;

            return {
                height: (rowsLength * ctrl.gridOptions.rowHeight + headerHeight) + 'px'
            };
        };

        ctrl.setParamsByUrl = function () {
            if (isFirstPageLoad === false) {
                uiGridCustomService.setParamsByUrl(ctrl.gridUniqueId, ctrl._params);
            }
        }

        ctrl.fetchData = function (ignoreHistory) {

            ctrl.locationWatchUnreg();

            ctrl.setStateProcess(true);

            var defer = $q.defer();

            if (ctrl.gridUrl != null && ctrl.gridUrl.length > 0) {
                uiGridCustomService.getData(ctrl.gridUrl, uiGridCustomService.convertToServerParams(ctrl._params))
                    .then(function (result) {
                        defer.resolve(result);
                    })
                    .catch(function (response) {
                        defer.reject(response);
                    });
            } else {
                defer.resolve(ctrl.gridOptions);
            }

            return defer.promise.then(function (result) {


                ng.extend(ctrl.gridOptions, uiGridCustomService.convertToClientParams(ctrl._params, true), result);


                if (ctrl.gridOptions.data != null && ctrl.gridOptions.data.length > 0) {
                    gridApiReady.promise.then(ctrl.checkSelection).then(function () {
                        $timeout(function () {
                            ctrl.restoreState(true);
                        }, 0);
                    })
                } else if (ctrl.selectionCustom != null) {
                    ctrl.selectionCustom.clearSelectedRows();
                }

                //uiGridCustomService.calcOptions(ctrl.gridOptions);

                ctrl.setStateProcess(false);

                ctrl.firstLoading = false;

                if (ctrl.gridOnFetch != null) {
                    ctrl.gridOnFetch({ grid: ctrl });
                }

                return result;
            })
            .then(function () {
                return ctrl.selectionSelectItemsFromOutside();
            })
            .catch(function (response) {
                ctrl.gridOptions.data = [];
                toaster.error('Ошибка при загрузке данных');
                ctrl.error = 'Ошибка при загрузке данных';
                //ctrl.firstLoading = false;
                //ctrl.setStateProcess(false);
                ctrl.setStateProcess(false);
                return response;
            })
            .finally(function (result) {

                if (ignoreHistory !== true && ctrl.getLastHistoryItem() !== JSON.stringify($location.search()[ctrl.gridUniqueId])) {
                    ctrl.addHistoryItem(JSON.stringify($location.search()[ctrl.gridUniqueId]));
                }

                setTimeout(function () {
                    ctrl.locationWatch();
                }, 0);

                return result;
            });
        };

        //#region filter
        ctrl.filterInit = function (filter) {
            ctrl.filter = filter;
            if (ctrl.gridOnFilterInit != null) {
                ctrl.gridOnFilterInit({ filter: filter });
            }
        }

        ctrl.filterApply = function (params, item) {

            if (ng.isArray(params) === false) {
                throw new Error('Parameter "params" should be array')
            }

            for (var i = 0, len = params.length; i < len; i++) {
                ctrl._params[params[i].name] = params[i].value;
            }

            ctrl.gridOptions.paginationCurrentPage = 1;
            ctrl._params.paginationCurrentPage = 1;

            ctrl.fetchData()
                .then(function () {
                    ctrl.setParamsByUrl();
                });
        };

        ctrl.filterRemove = function (name, item) {

            if (item.filter.type === 'range') {
                delete ctrl._params[item.filter.rangeOptions.from.name];
                delete ctrl._params[item.filter.rangeOptions.to.name];
            } if (item.filter.type === 'datetime') {
                delete ctrl._params[item.filter.datetimeOptions.from.name];
                delete ctrl._params[item.filter.datetimeOptions.to.name];
            } else if (item.filter.type === 'date') {
                delete ctrl._params[item.filter.dateOptions.from.name];
                delete ctrl._params[item.filter.dateOptions.to.name];
            } else {
                delete ctrl._params[name];
            }

            ctrl.gridOptions.paginationCurrentPage = 1;
            ctrl._params.paginationCurrentPage = 1;

            ctrl.fetchData()
                .then(function () {
                    ctrl.setParamsByUrl();
                });
        };
        //#endregion

        //#region selection

        ctrl.selectionSelectItemsFromOutside = function () {
            return $timeout(function () {
                var selectionItemsSelected = ctrl.gridOptions.data.filter(function (rowEntity) {
                    return ctrl.gridSelectionItemsSelectedFn({ rowEntity: rowEntity });
                });

                selectionItemsSelected.forEach(function (item) {
                    ctrl.gridApi.selection.selectRow(item);
                });
            }, 100);
        }

        ctrl.selectionOnInit = function (selectionCustom) {
            ctrl.selectionCustom = selectionCustom;

            if (ctrl.gridSelectionOnInit != null) {
                ctrl.gridSelectionOnInit({ selectionCustom: selectionCustom });
            }

            ctrl.selectionSelectItemsFromOutside();
        };

        ctrl.selectionUpdate = function (response) {

            //ctrl.resetState();
            //ctrl.selectionCustom.clearSelectedRows();

            ctrl.fetchData()
                 .then(function () {
                     ctrl.setParamsByUrl();
                 });

            if (response != null && response.data.result === false) {
                if (response.data.errors != null && response.data.errors.length > 0) {
                    response.data.errors.forEach(function (item) {
                        toaster.error(item);
                    });
                }
            }

            if (ctrl.gridSelectionMassApply != null) {
                ctrl.gridSelectionMassApply();
            }
        };

        ctrl.checkSelection = function () {

            var defer = $q.defer();

            if (ctrl.selectionCustom != null && ctrl.selectionCustom.getIsSelectedAll() === true) {
                $timeout(function () {
                    //ctrl.gridApi.selection.selectAllRows();

                    var rows = ctrl.gridApi.core.getVisibleRows(ctrl.gridApi.grid);

                    rows.forEach(function (item) {
                        item.isSelected = ctrl.selectionCustom.unselectedRows.length > 0 ? ctrl.selectionCustom.indexOfUnselected(item.entity) === -1 : true;
                    });

                    ctrl.saveInStorageRows(rows);

                    defer.resolve(true);
                });
            } else {
                defer.resolve(false);
            }

            return defer.promise;
        };

        ctrl.saveInStorageRows = function (rows) {
            var row,
                rowIdentity;

            for (var i = 0, len = rows.length; i < len; i++) {

                row = rows[i];
                rowIdentity = ctrl.gridOptions.saveRowIdentity(row.entity);

                if ((ctrl.selectionCustom.getIsSelectedAll() === false || ctrl.selectionCustom.unselectedRows.length > 0) && ctrl.storageStates != null && ctrl.storageStates.selection != null && ctrl.storageStates.selection.length > 0) {
                    for (var j = 0, lenj = ctrl.storageStates.selection.length; j < lenj; j++) {
                        if (rowIdentity === ctrl.storageStates.selection[j].row) {
                            if ((ctrl.selectionCustom.getIsSelectedAll() === true && ctrl.selectionCustom.indexOfUnselected(row.entity) !== -1) || (ctrl.selectionCustom.getIsSelectedAll() === false && row.isSelected === false)) {
                                ctrl.storageStates.selection[j] = null;
                            }
                        }
                    }

                    for (var k = 0, lenk = ctrl.storageStates.selection.length; k < lenk; k++) {
                        if (ctrl.storageStates.selection[k] == null) {
                            ctrl.storageStates.selection.splice(k, 1);
                        }
                    }
                }
            }

            ctrl.saveState();
        }

        ctrl.selectionOnChange = function (rows) {

            ctrl.saveInStorageRows(rows);

            if (ctrl.gridSelectionOnChange != null) {
                ctrl.gridSelectionOnChange(rows);
            }
        };

        //#endregion

        ctrl.setSwitchEnabled = function (rowEntity, state, fieldName) {
            rowEntity[fieldName] = state;

            uiGridCustomService.applyInplaceEditing(ctrl.gridInplaceUrl, ng.extend({}, ctrl._params, rowEntity)).then(function (data) {
                if (data.result === true) {
                    toaster.pop('success', '', 'Изменения сохранены');

                    if (ctrl.gridOnInplaceApply != null) {
                        ctrl.gridOnInplaceApply({});
                    }
                } else {
                    toaster.pop('error', 'Ошибка', data.error != null ? data.error : 'Ошибка при сохранении');
                }
            });
        };

        ctrl.paginationChange = function (paginationCurrentPage, paginationPageSize, paginationPageSizes) {

            ctrl.gridOptions.paginationCurrentPage = paginationCurrentPage;
            ctrl.gridOptions.paginationPageSize = paginationPageSize;

            ctrl._params.paginationCurrentPage = paginationCurrentPage;
            ctrl._params.paginationPageSize = paginationPageSize;

            ctrl.fetchData()
                .then(function () {
                    ctrl.setParamsByUrl();
                    ctrl.saveDataInStorage({
                        paginationPageSize: paginationPageSize
                    })
                });
        };

        ctrl.clickRow = function ($event, row, fn, url) {

            if (['a', 'input', 'textarea'].indexOf($event.target.tagName.toLowerCase()) !== -1 || domService.closest($event.target, '.js-grid-not-clicked') != null || domService.closest($event.target, '.ui-select-container') != null)
                return;

            if (fn != null) {
                fn($event, row, ctrl);
            }

            if (url != null && url.length > 0) {
                $window.location.assign($interpolate(url)({ row: row }));
            }
        };

        ctrl.setParams = function (params) {
            ng.extend(ctrl._params, uiGridCustomService.convertToClientParams(params));
            uiGridCustomService.setParamsByUrl(ctrl.gridUniqueId, ctrl._params);
        };

        ctrl.clearParams = function () {
            ctrl._params = null;
            uiGridCustomService.clearParams(ctrl.gridUniqueId);
        };

        ctrl.setStateProcess = function (value) {
            ctrl.isProcessing = value;
        };

        ctrl.selectionOnRequestBefore = function () {
            ctrl.setStateProcess(true);
        };

        ctrl.clearSelectionInStorage = function () {
            if (ctrl.storageStates != null && ctrl.storageStates.selection != null) {
                ctrl.storageStates.selection.length = 0;
            }

        };

        //#region state
        ctrl.saveState = function () {

            var saveData = ctrl.gridApi.saveState.save(),
                prop,
                itemInProp,
                index;

            ctrl.storageStates = ctrl.storageStates || {};

            for (var key in saveData) {
                if (saveData.hasOwnProperty(key) === true) {

                    prop = saveData[key];

                    if (ng.isArray(prop) === true) {
                        for (var i = 0, len = prop.length; i < len; i++) {

                            itemInProp = prop[i];

                            if (ctrl.storageStates[key] != null) {
                                for (var j = 0, lenj = ctrl.storageStates[key].length; j < lenj; j++) {
                                    if ((ctrl.compareState[key] != null && ctrl.compareState[key](ctrl.storageStates[key][j], itemInProp)) || ng.equals(ctrl.storageStates[key][j], itemInProp) === true) {
                                        index = j;
                                        break;
                                    }
                                }
                            } else {
                                ctrl.storageStates[key] = [];
                            }

                            if (index != null && index !== -1) {
                                ctrl.storageStates[key][index] = itemInProp;
                            } else {
                                ctrl.storageStates[key].push(itemInProp);
                            }

                            index = null;
                        }
                    } else {
                        ctrl.storageStates[key] = saveData[key];
                    }
                }
            }

            return ctrl.storageStates;
        };

        ctrl.restoreState = function (onlySelection) {
            var result;

            if (ctrl.storageStates != null) {
                result = ctrl.gridApi.saveState.restore(null, onlySelection ? { selection: ctrl.storageStates.selection } : ctrl.storageStates)
            }

            return result;
        };

        ctrl.resetState = function () {
            ctrl.storageStates = {};
        };

        ctrl.compareState = {
            'selection': function (obj, otherObj) {
                return obj.row === otherObj.row;
            }
        }
        //#endregion

        ctrl.export = function () {
            uiGridCustomService.export(ctrl.gridUrl, uiGridCustomService.convertToServerParams(ctrl._params));
        }

        ctrl.bindGridApi = function (gridApi) {
            var destroyEditingAfter, destroySort, destroyRowRender, destroyScrollEnd;

            ctrl.gridApi = gridApi;

            destroyEditingAfter = gridApi.uiGridEditCustom.on.change(null, function (rowEntity, colDef, newValue, oldValue, callback) {
                var resultBefore;

                if (ctrl.gridOnInplaceBeforeApply != null) {
                    resultBefore = ctrl.gridOnInplaceBeforeApply({ rowEntity: rowEntity, colDef: colDef, newValue: newValue, oldValue: oldValue });

                    if (resultBefore === false) {
                        rowEntity[colDef.name] = oldValue;
                        return;
                    }
                }

                //#region remove duplicate properties
                var rowEntityToLowers = [],
                    paramsNew = {};

                for (var key in rowEntity) {
                    if (rowEntity.hasOwnProperty(key)) {
                        rowEntityToLowers.push(key.toLowerCase());
                    }
                }

                for (var keyParam in ctrl._params) {
                    if (ctrl._params.hasOwnProperty(keyParam) && rowEntityToLowers.indexOf(keyParam.toLowerCase()) === -1) {
                        paramsNew[keyParam] = ctrl._params[keyParam];
                    }
                }
                //#endregion

                uiGridCustomService.applyInplaceEditing(ctrl.gridInplaceUrl, ng.extend({}, paramsNew, rowEntity))
                    .then(function (data) {

                        if (data.result === true) {

                            toaster.pop('success', '', 'Изменения сохранены');

                            if (data.entity != null) {
                                ng.extend(rowEntity, data.entity);
                            }

                            if (ctrl.gridOnInplaceApply != null) {
                                ctrl.gridOnInplaceApply();
                            }

                            if (callback != null) {
                                callback(rowEntity, colDef, newValue, oldValue);
                            }

                        } else {
                            toaster.pop('error', 'Ошибка', data.error != null ? data.error : 'Ошибка при сохранении');
                        }
                    })
                    .catch(function (response) {
                        toaster.error('Ошибка при обновлении данных');
                        return response;
                    });
            });

            destroySort = gridApi.core.on.sortChanged(null, function (grid, sortColumns) {
                if (sortColumns.length > 0) {
                    ctrl._params.sorting = sortColumns[0].name;
                    ctrl._params.sortingType = sortColumns[0].sort.direction;
                } else {
                    delete ctrl._params.sorting;
                    delete ctrl._params.sortingType;
                }

                ctrl.fetchData()
                    .then(function () {
                        ctrl.setParamsByUrl();
                    });
            });


            // ROWS RENDER
            destroyRowRender = gridApi.core.on.rowsRendered(null, function () {
                // each rows rendered event (init, filter, pagination, tree expand)
                // Timeout needed : multi rowsRendered are fired, we want only the last one
                if (rowsRenderedTimeout) {
                    $timeout.cancel(rowsRenderedTimeout)
                }
                rowsRenderedTimeout = $timeout(function () {
                    alignContainers($element, gridApi.grid);
                });
            });

            // SCROLL END
            destroyScrollEnd = gridApi.core.on.scrollEnd(null, function () {
                alignContainers($element, gridApi.grid);
            });

            $element.on('$destroy', function () {
                destroyEditingAfter();
                destroySort();
                destroyRowRender();
                destroyScrollEnd();
            });

            gridApiReady.resolve(gridApi);
        }

        ctrl.optionsFromUrl = function () {
            var gridParamsByUrl = uiGridCustomService.getParamsByUrl(ctrl.gridUniqueId);

            if (gridParamsByUrl != null) {

                //#region set sorting on page load from url
                if (gridParamsByUrl.sorting != null) {
                    for (var i = 0, len = ctrl.gridOptions.columnDefs.length; i < len; i += 1) {
                        if (ctrl.gridOptions.columnDefs[i].name === gridParamsByUrl.sorting) {
                            ctrl.gridOptions.columnDefs[i].sort = {
                                direction: uiGridConstants[gridParamsByUrl.sortingType.toUpperCase()]
                            };
                            break;
                        }
                    }
                }
                //#endregion

                ng.extend(ctrl.gridOptions, uiGridCustomService.convertToClientParams(gridParamsByUrl));
                ng.extend(ctrl._params, uiGridCustomService.convertToClientParams(gridParamsByUrl));
            }
        }

        ctrl.defaultRowEntitySave = function (rowEntity) {
            //эта функция отвечает за генерацию уникального id для строки
            //нам нужно удалить поле $$hashKey в rowEntity, которое добавляет сам ангуляр
            //так как хэш будет каждый раз разный

            //для того чтобы не изменять искомый объект и не ломать логику ангуляра клонируем объект
            var clone = JSON.parse(JSON.stringify(rowEntity))

            //удаляем уникальный хэш
            delete clone.$$hashKey;

            return JSON.stringify(clone);
        };

        ctrl.getKey = function () {
            return $window.location.pathname + '::' + ctrl.gridUniqueId;
        }

        ctrl.getDataItemFromStorage = function () {
            return uiGridCustomService.getDataItimFromStorageByKey(ctrl.getKey());
        };

        ctrl.saveDataInStorage = function (data) {
            uiGridCustomService.saveDataInStorage(ctrl.getKey(), data);
        };
    };

    UiGridCustomCtrl.$inject = ['$element', '$interpolate', '$location', '$scope', '$timeout', '$q', '$window', 'uiGridConstants', 'uiGridEditService', 'uiGridCustomService', 'uiGridCustomConfig', 'uiGridCustomParamsConfig', 'toaster', 'domService'];

    ng.module('uiGridCustom', [
        'ui.grid',
        'ui.grid.edit',
        'ui.grid.selection',
        'ui.grid.cellNav',
        'ui.grid.autoResize',
        'ui.grid.grouping',
        'ui.grid.treeView',
        'ui.grid.saveState',
        'uiGridCustomFilter',
        'uiGridCustomPagination',
        'uiGridCustomSelection',
        'uiGridCustomEdit',
        'switchOnOff',
        'toaster',
        'dom'])
      .controller('UiGridCustomCtrl', UiGridCustomCtrl);

    var rowsRenderedTimeout;
    // auto-dimension of cells (css) need to force align rows in all containers (left and right pinning)
    var alignContainers = function alignContainers(gridContainer, grid) {
        var rows = gridContainer.find('.ui-grid-render-container-body .ui-grid-row:not(.ui-grid-custom-group-header)');
        var pinnedRowsLeft = gridContainer.find('.ui-grid-pinned-container-left .ui-grid-row:not(.ui-grid-custom-group-header)');
        var gridHasRightContainer = grid.hasRightContainer();

        if (gridHasRightContainer) {
            var pinnedRowsRight = gridContainer.find('.ui-grid-pinned-container-right .ui-grid-row:not(.ui-grid-custom-group-header)');
        }

        var bodyContainer = grid.renderContainers.body;

        // get count columns pinned on left
        var columnsPinnedOnLeft = grid.renderContainers.left != null && grid.renderContainers.left.renderedColumns.length;


        var elementParentBody,
            elementBody,
            elementParentLeft,
            elementLeft,
            elementParentRight,
            elementRight,
            rowHeight,
            pinnedRowLeftHeight,
            pinnedRowRightHeight,
            largest;

        for (var r = 0, len = rows.length; r < len; r++) {
            // Remove height CSS property to get new height if container resized (slidePanel)
            elementParentBody = angular.element(rows[r]);
            elementParentBody.css('height', 'auto');

            elementBody = elementParentBody.children('div');
            elementBody.css('height', 'auto');

            elementParentLeft = angular.element(pinnedRowsLeft[r]);
            elementParentLeft.css('height', 'auto');

            elementLeft = elementParentLeft.children('div');
            elementLeft.css('height', 'auto');

            if (gridHasRightContainer) {
                elementParentRight = angular.element(pinnedRowsRight[r]);
                elementParentRight.css('height', 'auto');

                elementRight = elementParentRight.children('div');
                elementRight.css('height', 'auto');
            }

            // GET Height when set in auto for each container
            // BODY CONTAINER
            rowHeight = rows[r].offsetHeight;
            // LEFT CONTAINER
            pinnedRowLeftHeight = 0;
            if (columnsPinnedOnLeft) {
                pinnedRowLeftHeight = pinnedRowsLeft[r].offsetHeight;
            }
            // RIGHT CONTAINER
            pinnedRowRightHeight = 0;
            if (gridHasRightContainer) {
                pinnedRowRightHeight = pinnedRowsRight[r].offsetHeight;
            }
            // LARGEST
            largest = Math.max(rowHeight, pinnedRowLeftHeight, pinnedRowRightHeight, grid.options.rowHeight);

            // Apply new row height in each container
            elementBody.css('height', largest);
            elementLeft.css('height', largest);

            if (gridHasRightContainer) {
                elementRight.css('height', largest);
            }

            // Apply new height in gridRow definition (used by scroll)
            bodyContainer.renderedRows[r].height = largest;
        }
        // NEED TO REFRESH CANVAS
        bodyContainer.canvasHeightShouldUpdate = true;
    };
    // END alignContainers()

})(window.angular);