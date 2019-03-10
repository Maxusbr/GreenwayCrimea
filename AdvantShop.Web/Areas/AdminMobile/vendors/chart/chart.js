(function ($, $win) {

    var eventsClass = function () { };

    eventsClass.list = function ($element) {
        return $element.data('events');
        return null;
    };

    function isExistEvent ($element, eventName) {
        var events, isExist = false;
        events = eventsClass.list($element);

        if (!events) {
            isExist = false;
        } else if (eventName.indexOf('.') == -1) {
            isExist = eventName in events;
        } else {
            var eventType = /^(\w+)\.?/g.exec(eventName)[1];
            var namespace = eventName.replace(eventType + ".", "");


            namespace = namespace.split(".").reverse().join(".");

            if (!events[eventType]) {
                isExist = false;
            } else {
                for (var i = 0; i < events[eventType].length; i++) {
                    if (events[eventType][i].namespace == namespace) {
                        isExist = true;
                        break;
                    }
                }
            }
        }

        return isExist;
    };

    function evalObj (code) {
        if (typeof (code) === 'string' && code.length === 0) {
            return null;
        }
        return (new Function('return ' + code))();
    };

    var chart = function (selector, data, options) {
        this.$obj = (function (selector) {

            var obj;

            if ((selector instanceof $) === false) {
                obj = $(selector);
            } else {
                obj = selector;
            }

            return obj;
        })(selector);
        this.options = $.extend({}, this.defaultOptions, options);
        this.data = data;
        return this;
    }

    window.chart = chart;

    chart.prototype.InitTotal = function () {

        var objs = $('[data-plugin="chart"]:visible'),
            obj, data, opts;

        for (var i = 0, arrLength = objs.length; i < arrLength; i += 1) {
            obj = objs.eq(i);
            data = evalObj(obj.attr('data-chart'));
            opts = evalObj(obj.attr('data-chart-options')) || {};

            if (data == null) {
                continue;
            }

            chart.prototype.Init(obj, data, opts);
        }
    };

    $(chart.prototype.InitTotal);

    chart.prototype.Init = function (selector, data, options) {

        if ($.plot == null) {
            return;
        }

        var chartObj = new chart(selector, data, options);

        $.plot(chartObj.$obj, chartObj.data, chartObj.options);

        chartObj.BindEvent();
    }

    chart.prototype.BindEvent = function () {
        var chartObj = this, previousPoint;

        if (isExistEvent($win, 'resize.chartResize') !== true) {
            $win.on('resize.chartResize', function () {
                if (this.resizeTO != null) {
                    clearTimeout(this.resizeTO);
                }

                this.resizeTO = setTimeout(function () {
                    chart.prototype.InitTotal();
                }, 100);
            });
        }

        chartObj.$obj.on('plothover', function (event, pos, item) {

            if (item != null) {
                if (previousPoint != item.dataIndex) {

                    previousPoint = item.dataIndex;

                    chartObj.showTooltip(item);
                }
            } else {
                $('#chartTooltip').hide();
                previousPoint = null;
            }
        });

    };

    chart.prototype.showTooltip = function (item) {
        var x = item.pageX,
        y = item.pageY,
        contents = item.datapoint[1].toFixed(2),
        tooltip = $('#chartTooltip');

        if (tooltip.length === 0) {
            tooltip = $('<div />', { id: 'chartTooltip', 'class': 'chart-tooltip', css: { zIndex: '9999' } });
            tooltip.appendTo('body');
        } else {
            tooltip.hide();
        }

        tooltip.html(contents);

        tooltip.css({
            top: y + 5,
            left: x + 5,
            backgroundColor: item.series.color
        });

        tooltip.stop(true, true).fadeIn(200);
    }

    chart.prototype.defaultOptions = {
        canvas: true,
        series: {
            lines: { show: true },
            points: { show: true }
        },
        grid: {
            hoverable: true
        }
    }

})(jQuery, $(window))