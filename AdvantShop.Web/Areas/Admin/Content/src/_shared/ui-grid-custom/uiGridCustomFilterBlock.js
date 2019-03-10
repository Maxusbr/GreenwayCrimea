; (function (ng) {
    'use strict';

    var MODEL_TYPE = 'YYYY-MM-DDTHH:mm:ss',
        DATETIME_TYPE_INPUT = 'DD.MM.YYYY HH:mm',
        DATE_TYPE_INPUT = 'DD.MM.YYYY',
        INPUT_FORMATS = [DATETIME_TYPE_INPUT, DATE_TYPE_INPUT, MODEL_TYPE].concat([moment.ISO_8601]).filter(uniqueDatetime);

    function uniqueDatetime(value, index, self) {
        return ['Date', 'moment', 'milliseconds', undefined].indexOf(value) === -1 &&
            self.indexOf(value) === index;
    };

    function toStringDatetime(modelValue, displayFormat, inputFormats) {
        if (angular.isUndefined(modelValue) || modelValue === '' || modelValue === null) {
            return null;
        }

        if (angular.isDate(modelValue)) {
            return moment(modelValue).format(displayFormat);
        } else if (angular.isNumber(modelValue)) {
            return moment.utc(modelValue).format(displayFormat);
        }
        return moment(modelValue, inputFormats, moment.locale(), true).format(displayFormat);
    }

    var UiGridCustomFilterBlockCtrl = function ($timeout, dateTimeParserFactory) {
        var ctrl = this,
            timer;

        ctrl.$onInit = function () {
            var datetimeFormat;

            if (ctrl.blockType === 'datetime') {

                datetimeFormat = dateTimeParserFactory(MODEL_TYPE, INPUT_FORMATS, true);

                ctrl.item.filter.term.to = datetimeFormat(toStringDatetime(ctrl.item.filter.term.to, DATETIME_TYPE_INPUT, INPUT_FORMATS));
                ctrl.item.filter.term.from = datetimeFormat(toStringDatetime(ctrl.item.filter.term.from, DATETIME_TYPE_INPUT, INPUT_FORMATS));

                ctrl.apply([{ name: ctrl.item.filter.datetimeOptions.from.name, value: ctrl.item.filter.term.from }, { name: ctrl.item.filter.datetimeOptions.to.name, value: ctrl.item.filter.term.to }], ctrl.item);

            } else if (ctrl.blockType === 'date') {
                datetimeFormat = dateTimeParserFactory(MODEL_TYPE, INPUT_FORMATS, true);

                ctrl.item.filter.term.to = datetimeFormat(toStringDatetime(ctrl.item.filter.term.to, DATE_TYPE_INPUT, INPUT_FORMATS));
                ctrl.item.filter.term.from = datetimeFormat(toStringDatetime(ctrl.item.filter.term.from, DATE_TYPE_INPUT, INPUT_FORMATS));

                ctrl.apply([{ name: ctrl.item.filter.dateOptions.from.name, value: ctrl.item.filter.term.from }, { name: ctrl.item.filter.dateOptions.to.name, value: ctrl.item.filter.term.to }], ctrl.item);
            } else if (ctrl.blockType === 'range' && ctrl.item.filter.rangeOptions && ctrl.item.filter.term != null) {
                ctrl.apply([{ name: ctrl.item.filter.rangeOptions.from.name, value: ctrl.item.filter.term.from }, { name: ctrl.item.filter.rangeOptions.to.name, value: ctrl.item.filter.term.to }], ctrl.item, true);
            } else if ((ctrl.blockType === 'input' || ctrl.blockType === 'number') && ctrl.item.filter.term != null) {
                ctrl.apply([{ name: ctrl.item.filter.name, value: ctrl.item.filter.term }], ctrl.item, true);
            }

            ctrl.blockUrl = '../areas/admin/content/src/_shared/ui-grid-custom/templates/filter-types/' + ctrl.blockType + '.html';
        };

        ctrl.close = function (name, item) {
            ctrl.onClose({ name: name, item: item });
        };

        ctrl.apply = function (params, item, debounce) {

            if (debounce === true) {

                if (timer != null) {
                    $timeout.cancel(timer);
                }

                timer = $timeout(function () {
                    ctrl.onApply({ params: params, item: item });
                }, 300)
            } else {
                ctrl.onApply({ params: params, item: item });
            }
        };

        
    };

    UiGridCustomFilterBlockCtrl.$inject = ['$timeout','dateTimeParserFactory'];

    ng.module('uiGridCustomFilter')
      .controller('UiGridCustomFilterBlockCtrl', UiGridCustomFilterBlockCtrl);

})(window.angular);