(function ($, $win) {

    var advantshop = Advantshop,
        utilities = advantshop.Utilities,
        scriptManager = advantshop.ScriptsManager;

    var chart = function (selector, data, options) {
        this.$obj = advantshop.GetJQueryObject(selector);
        this.options = $.extend({}, this.defaultOptions, options);
        this.data = data;
        return this;
    };

    scriptManager.Chart = chart;

    chart.prototype.InitTotal = function () {

        var objs = $('[data-plugin="chart"]:visible'),
            obj, data, opts;

        for (var i = 0, arrLength = objs.length; i < arrLength; i += 1) {
            obj = objs.eq(i);

            if (obj.attr('data-chart-url') != null) {
                chart.prototype.getGraphData(obj);
            }

            data = utilities.Eval(obj.attr('data-chart'));
            opts = utilities.Eval(obj.attr('data-chart-options')) || {};

            if (data == null) {
                continue;
            }

            chart.prototype.Init(obj, data, opts);
        }

        ////extending grpahs by d3 graphs
        var d3graphs = $('[data-plugin="d3graphHorizontal"]:visible'),
            d3graph;

        for (var i = 0, arrLength = d3graphs.length; i < arrLength; i += 1) {
            d3graph = d3graphs.eq(i);

            if (d3graph.attr('data-chart-url') != null) {
                chart.prototype.getGraphData(d3graph);
            }

            data = utilities.Eval(d3graph.attr('data-chart'));

            d3HorizontalBarCreate(d3graph.find('svg')[0], data);
        }
    };

    $(chart.prototype.InitTotal);

    chart.prototype.Init = function (selector, data, options) {

        if ($.plot == null) {
            return;
        }

        var chartObj = new chart(selector, data, options);

        chartObj.$obj.hide();

        $.plot(chartObj.$obj, chartObj.data, chartObj.options);

        chartObj.$obj.show();

        chartObj.BindEvent();
    };

    chart.prototype.BindEvent = function () {
        var chartObj = this, previousPoint = null;

        if (utilities.Events.isExistEvent($win, 'resize.chartResize') !== true) {
            $win.on('resize.chartResize', function () {
                if (this.resizeTO != null) {
                    clearTimeout(this.resizeTO);
                }

                this.resizeTO = setTimeout(function () {
                    chart.prototype.InitTotal();
                }, 500);
            });
        }
        if (utilities.Events.isExistEvent(chartObj.$obj, 'plothover') !== true) {
            chartObj.$obj.on('plothover', function (event, pos, item) {

                if (item != null) {
                    if (previousPoint != item.dataIndex) {
                        previousPoint = item.dataIndex;
                        var content = "";
                        var format = d3.format("0,00");

                        if (!$.isArray(item.datapoint[1]) && item.datapoint.length == 2) {
                            var date = $.plot.formatDate(new Date(item.datapoint[0]), '%d %b');
                            content = date + " : <b>" + format(item.datapoint[1]) + "</b>";
                        } else if (item.datapoint.length == 3) {
                            content = item.series.yaxis.ticks[item.datapoint[1]].label + ": <b>" + format(item.datapoint[0]) + "</b>";
                        } else {
                            content = item.series.label + ": <b>" + format(item.datapoint[1][0][1]) + "</b>";
                        }

                        chartObj.showTooltip(item, pos.pageX, pos.pageY, content);
                    }
                } else {
                    $('#chartTooltip').hide();
                    previousPoint = null;
                }
            });
        }
    };

    chart.prototype.showTooltip = function (item, x, y, content) {
        var tooltip = $('#chartTooltip');

        if (tooltip.length === 0) {
            tooltip = $('<div />', { id: 'chartTooltip', 'class': 'chart-tooltip', css: { zIndex: '9999' } });
            tooltip.appendTo('body');
        } else {
            tooltip.hide();
        }

        tooltip.html(content);
        tooltip.css({
            top: y + 0,
            left: x + 15,
            backgroundColor: "#000"
        });

        tooltip.stop(true, true).fadeIn(200);
    };

    chart.prototype.defaultOptions = {
        canvas: true,
        series: {
            lines: { show: true },
            points: { show: true }
        },
        grid: {
            hoverable: true
        }
    };


    chart.prototype.getGraphData = function (graph) {
        if (graph.length == 0)
            return;

        var url = chart.prototype.getUrl(graph);

        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "GET",
            async: false,
            url: url,
            success: function (data) {
                if (data != null) {
                    if (data.chart != null) {
                        graph.attr("data-chart", data.chart);
                        graph.attr("data-chart-options", data.options);
                    } else {
                        graph.attr("data-chart", data);
                    }
                }
            }
        });
    }

    chart.prototype.getUrl = function (graph) {

        var type = graph.attr("data-statistics");

        switch (type) {
            case "sales":
                return chart.prototype.getSalesUrl(graph);
            case "leads":
                return chart.prototype.getLeadsUrl(graph);
            case "avgcheck":
                return chart.prototype.getAvgCheckUrl(graph);
            case "ordersby":
                return chart.prototype.getOrdersByUrl(graph);
            case "telephony":
                return chart.prototype.getTelephonyUrl(graph);
        }

        return chart.prototype.getDefaultUrl(graph);
    }

    chart.prototype.getSalesUrl = function (graph) {
        var filterUrl =
            chart.prototype.getDefaultUrl(graph) +
                "&datefrom=" + $(".datefrom").val() +
                "&dateto=" + $(".dateto").val() +
                "&paied=" + $(".paied").val() +
                "&statuses=" + $(".statuses").val() +
                "&useshippings=" + $(".useshippings").is(':checked') +
                "&groupby=" + $(".statistics-sales .radiolist input:checked").val();

        graph.attr("data-chart-url", filterUrl);

        return filterUrl;
    }

    chart.prototype.getLeadsUrl = function (graph) {
        var filterUrl =
            chart.prototype.getDefaultUrl(graph) +
                "&datefrom=" + $(".datefrom").val() +
                "&dateto=" + $(".dateto").val() +
                "&groupby=" + $(".statistics-leads .radiolist input:checked").val();

        graph.attr("data-chart-url", filterUrl);

        return filterUrl;
    }

    chart.prototype.getAvgCheckUrl = function (graph) {
        var filterUrl =
            chart.prototype.getDefaultUrl(graph) +
                "&datefrom=" + $(".datefrom").val() +
                "&dateto=" + $(".dateto").val() +
                "&paied=" + $(".paied").val() +
                "&statuses=" + $(".statuses").val() +
                "&groupby=" + $(".statistics-avgcheck .radiolist input:checked").val();

        graph.attr("data-chart-url", filterUrl);

        return filterUrl;
    }

    chart.prototype.getOrdersByUrl = function (graph) {
        var filterUrl =
            chart.prototype.getDefaultUrl(graph) +
                "&datefrom=" + $(".datefrom").val() +
                "&dateto=" + $(".dateto").val() +
                "&paied=" + $(".paied").val() +
                "&statuses=" + $(".statuses").val() +
                "&groupby=" + $(".statistics-ordersby .radiolist input:checked").val();

        graph.attr("data-chart-url", filterUrl);

        return filterUrl;
    }

    chart.prototype.getTelephonyUrl = function (graph) {
        var filterUrl =
            chart.prototype.getDefaultUrl(graph) +
                "&datefrom=" + $(".datefrom").val() +
                "&dateto=" + $(".dateto").val() +
                "&groupby=" + $(".statistics-telephony .radiolist input:checked").val();

        graph.attr("data-chart-url", filterUrl);

        return filterUrl;
    }

    chart.prototype.getDefaultUrl = function (graph) {
        var defaultUrl = "";

        if (graph.attr("data-chart-defurl") == null) {
            defaultUrl = graph.attr("data-chart-url");
            graph.attr("data-chart-defurl", defaultUrl);
        } else {
            defaultUrl = graph.attr("data-chart-defurl");
        }
        return defaultUrl;
    }

})(jQuery, $(window))