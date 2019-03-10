; (function (ng) {
    'use strict';

    var AutocompleterCtrl = function ($scope, autocompleterService, domService) {

        var ctrl = this,
            listScrollable,
            listCtrl,
            showEmptyResultMessageDirty = ctrl.showEmptyResultMessage != null &&  ctrl.showEmptyResultMessage();

        ctrl.result = null;
        ctrl.activeItem = null;
        ctrl.isVisibleAutocomplete = false;
        ctrl.viewMode = 'default';
        ctrl.itemFromObjects = [];
        ctrl.items = [];
        ctrl.isClickedItem = false;

        ctrl.showEmptyResultMessage = showEmptyResultMessageDirty != null ? showEmptyResultMessageDirty : true;

        ctrl.toggleVisible = function (visible) {

            if (visible === true) {
                ctrl.activeItem = null;
            }

            if (ctrl.isVisibleAutocomplete !== visible) {
                ctrl.isVisibleAutocomplete = visible;
            }
        };

        ctrl.addList = function (listDOM, listController) {
            listScrollable = listDOM.querySelector('.js-autocompleter-list');
            ctrl.listCtrl = listController;
        };

        ctrl.addItem = function (item) {

            item.groupIndex = item.groupIndex != null ? item.groupIndex : 0;

            ctrl.items[item.groupIndex] = ctrl.items[item.groupIndex] || [];

            if (item.index == null) {
                ctrl.items[item.groupIndex].push(item);
                item.index = ctrl.items[item.groupIndex].length - 1;
            } else {
                ctrl.items[item.groupIndex][item.index] = item;
            }

            return ctrl.items;
        };

        ctrl.setListPosition = function (pos) {
            ctrl.listPositional = pos;
        };

        ctrl.request = function (val) {
            if (ng.isDefined(val) && val.length >= ctrl.minLength) {
                autocompleterService.getData(ctrl.requestUrl, val, ctrl.params).then(function(response) {

                    ctrl.result = response;
                    //ctrl.items.length = 0;

                    if (ctrl.result == null) {
                        return;
                    }

                    if (ng.isArray(ctrl.result)) {
                        ctrl.viewMode = 'default';
                        ctrl.emptyResult = ctrl.result.length === 0;
                    } else {
                        ctrl.viewMode = 'additional';
                        ctrl.emptyResult = ctrl.result.Empty === true;
                    }
                    ctrl.toggleVisible(true);
                });
            } else if (ng.isDefined(val) && val.length < ctrl.minLength) {
                ctrl.toggleVisible(false);
            }
        };

        ctrl.navigate = function (isDown) {

            var indexGroup, indexCurrent, newIndexGroup, newIndex, newActiveItem;

            var currentItem, currentGroup,
                navVal = isDown === true ? 1 : -1;

            if (ctrl.items.length === 0) {
                return;
            }

            if (ctrl.activeItem == null) {
                currentGroup = ctrl.getIndexFirstOrDefaultGroup();
                currentItem = ctrl.getIndexFirstOrDefaultItem(currentGroup);
                newActiveItem = currentItem;
            }
            else {
                currentItem = ctrl.activeItem;
                newActiveItem = ctrl.items[currentItem.groupIndex][currentItem.index + navVal];
            }

            //пытаемся найти элемент в след./пред. группе
            if (newActiveItem == null && ctrl.items[currentItem.groupIndex + navVal] != null && ctrl.items[currentItem.groupIndex + navVal].length > 0) {
                newActiveItem = ctrl.getIndexFirstOrDefaultItem(ctrl.items[currentItem.groupIndex + navVal]);
            }

            if (newActiveItem != null) {

                ctrl.processItems(function (group, item) {
                    if (item != null) {
                        item.isActive = false;
                    }
                });

                newActiveItem.isActive = true;
                ctrl.activeItem = newActiveItem;

                ctrl.checkScroll(ctrl.activeItem.itemDOM);

            } else {
                ctrl.activeItem = null;
            }
        };

        ctrl.checkScroll = function (element) {
            var topContainer = listScrollable.scrollTop,
                bottomContainer = topContainer + listScrollable.clientHeight,
                topItem = element.offsetTop,
                heightItem = element.clientHeight,
                bottomItem = topItem + heightItem;

            if (bottomContainer < bottomItem) {
                listScrollable.scrollTop += heightItem;
            } else if (topContainer > topItem) {
                listScrollable.scrollTop = topItem;
            }
        };

        ctrl.apply = function (val) {

            ctrl.model.$setViewValue(val);
            ctrl.model.$render();

            ctrl.applyFn({ value: val, obj: ctrl.activeItem != null ? ctrl.activeItem.item : null });

            ctrl.toggleVisible(false);

            ctrl.isDirty = false;
        };

        ctrl.autocompleteKeyup = function ($event, val) {

            $event.stopPropagation();

            var keyCode = $event.keyCode;

            switch (keyCode) {
                case 38: //arrow up
                    $event.preventDefault();
                    ctrl.navigate(false);
                    break;
                case 40: //arrow down
                    $event.preventDefault();
                    ctrl.navigate(true);
                    break;
                case 13: //enter
                    if (ctrl.activeItem != null) {
                        ctrl.apply(ctrl.activeItem.item[ctrl.field]);
                    } else {
                        ctrl.apply(val);
                    }
                    break;
                default:
                    ctrl.isDirty = true;
                    ctrl.request(val);
                    break;
            }
        };

        ctrl.crossClick = function ($event) {
            ctrl.toggleVisible(false);

            $event.stopPropagation();
        };

        ctrl.itemClick = function ($event, item) {

            ctrl.isClickedItem = true;

            var selectedValue = item.item[ctrl.field];

            ctrl.apply(selectedValue);

            $event.stopPropagation();
        };

        ctrl.itemActive = function (item) {
            item.isActive = true;
            ctrl.activeItem = item;
        };

        ctrl.itemDeactive = function (item) {
            item.isActive = false;
            ctrl.activeItem = null;
        };

        ctrl.clickOut = function (event) {
            if (ctrl.isVisibleAutocomplete === true && domService.closest(event.target, '.js-autocompleter-sub') == null) {
                $scope.$apply(function () {
                    ctrl.toggleVisible(false);
                });
            }
        };

        ctrl.getIndexFirstOrDefaultGroup = function () {

            var group;

            for (var i = 0, len = ctrl.items.length; i < len; i++) {
                if (ctrl.items[i] != null && ctrl.items[i].length > 0) {
                    group = ctrl.items[i];
                    break;
                }
            }

            return group;
        };

        ctrl.getIndexFirstOrDefaultItem = function (group) {

            var item;

            for (var i = 0, len = group.length; i < len; i++) {
                if (group[i] != null) {
                    item = group[i];
                    break;
                }
            }

            return item;
        };

        ctrl.processItems = function (func) {
            for (var i = 0, l = ctrl.items.length; i < l; i++) {
                if (ctrl.items[i] != null) {
                    for (var k = 0, l2 = ctrl.items[i].length; k < l2; k++) {
                        if (func(ctrl.items[i], ctrl.items[i][k]) === false) {
                            break;
                        };
                    }
                }
            }
        };
    };

    ng.module('autocompleter')
      .controller('AutocompleterCtrl', AutocompleterCtrl);

    AutocompleterCtrl.$inject = ['$scope', 'autocompleterService', 'domService'];

})(window.angular);