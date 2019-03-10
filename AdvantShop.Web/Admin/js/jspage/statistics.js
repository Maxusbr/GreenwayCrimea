$(function () {

    var timer,
        manualGroupBy = false,
        prevFrom = '',
        prevTo = '';

    $("#tabs-headers li").on("click", function () {
        setTimeout(function () {
            updateGraphics();
        }, 100);
    });

    $(".datefrom, .dateto").on("change", function (e) {
        
        if (prevFrom == $(".datefrom").val() && prevTo == $(".dateto").val()) {
            return;
        }

        if (!manualGroupBy) {
            var from = $(".datefrom").val().split('.');
            var to = $(".dateto").val().split('.');

            var dateFrom = new Date(parseInt(from[2]), parseInt(from[1]) - 1, parseInt(from[0]), 0, 0, 0, 0);
            var dateTo = new Date(parseInt(to[2]), parseInt(to[1]) - 1, parseInt(to[0]), 0, 0, 0, 0);

            var days = (dateTo - dateFrom) / (1000 * 60 * 60 * 24);

            if (days > 35 && days < 90) {
                $(".radiolist input[value='wk']").click();
            } else if (days >= 90) {
                $(".radiolist input[value='mm']").click();
            }
        }

        updateGraphics();
    });


    $("body").on("click", ".radiolist:visible input", function (e) {
        updateGraphics();
        manualGroupBy = true;
    });

    $(".useshippings").on("click", function (e) {
        updateGraphics();
    });

    $(".paied, .statuses").on("change", function (e) {
        updateGraphics();
    });
    
    $("body").on("hover", "#abcxyz .xyz-cell", function (e) {

        var type = $(this).find("a").attr("data-prefix");
        var msg = $(".abc-hint-msg").find("div[data-hint-type=" + type + "]").html();

        if (msg != null) {
            $(".abc-hint").html(msg);
        }
    });

    $("body").on("click", "#rfm .rfm-cell", function (e) {
        var url = "StatisticsFilter.aspx?type=rfm&group=" + $(this).attr("data-prefix");

        var win = window.open(url, '_blank');
        win.focus();

        e.preventDefault();
    });



    updateGraphics();


    function updateGraphics() {

        if (timer != null) {
            clearTimeout(timer);
        }

        var currentTab = $("#tabs-headers li.selected").attr("id");
        if (currentTab == "profit" || currentTab == "check" || currentTab == "payments") {
            $(".statistic-order-option").show();
        } else {
            $(".statistic-order-option").hide();
        }

        prevFrom = $(".datefrom").val();
        prevTo = $(".dateto").val();

        timer = setTimeout(function () {
            getAvgValue();
            getVortex();
            getAbcXyz();
            getRfm();
            getManagers();
            Advantshop.ScriptsManager.Chart.prototype.InitTotal();

        }, 300);
    }

    function getAvgValue() {

        if ($(".avg-value:visible").length == 0)
            return;

        var url = "httphandlers/statistic/avgcheckgraph.ashx?type=avgvalue" +
                    "&datefrom=" + $(".datefrom").val() +
                    "&dateto=" + $(".dateto").val() +
                    "&paied=" + $(".paied").val() +
                    "&statuses=" + $(".statuses").val() +
                    "&groupby=" + $(".statistics-avgcheck .radiolist input:checked").val();

        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "GET",
            async: false,
            url: url,
            success: function (data) {
                if (data == null) {
                    $(".avg-value").html("");
                    return;
                }

                var html = "За выбранный период: <span>" + data + "</span>";

                $(".avg-value").html(html);
            }
        });
    }

    function getVortex() {

        if ($("#vortex:visible").length == 0)
            return;

        var url = "httphandlers/statistic/vortexgraph.ashx?" + "datefrom=" + $(".datefrom").val() + "&dateto=" + $(".dateto").val();

        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "GET",
            async: false,
            url: url,
            success: function (data) {
                if (data == null) {
                    $("#vortex").html("");
                    return;
                }

                var html =
                    "<h2>Воронка продаж</h2> " +
                    "<div class='vortex-line'><div class='vortex-title'>Все пользователи</div><div class='vortex-count-percent'>" + data.TotalUsersCount + " (" + data.TotalUsersCountPercent + "%)" + "</div><div class='vortex-bar-outer'><div class='vortex-bar-inner' style='width: " + data.TotalUsersCountPercent + "%;'></div></div></div>" +
                    "<div class='vortex-line'><div class='vortex-title'>Добавившие товар в корзину</div><div class='vortex-count-percent'>" + data.AddtoCartEvents + " (" + data.AddtoCartEventsPercent + "%)" + "</div><div class='vortex-bar-outer'><div class='vortex-bar-inner' style='width: " + data.AddtoCartEventsPercent + "%;'></div></div></div>" +
                    "<div class='vortex-line'><div class='vortex-title'>Оформившие заказ</div><div class='vortex-count-percent'>" + data.TotalOrdersCount + " (" + data.TotalOrdersCountPercent + "%)" + "</div><div class='vortex-bar-outer'><div class='vortex-bar-inner' style='width: " + data.TotalOrdersCountPercent + "%;'></div></div></div>" +
                    "<div class='vortex-line'><div class='vortex-title'>Оплатившие заказ</div><div class='vortex-count-percent'>" + data.PaiedOrdersCount + " (" + data.PaiedOrdersCountPercent + "%)" + "</div><div class='vortex-bar-outer'><div class='vortex-bar-inner' style='width: " + data.PaiedOrdersCountPercent + "%;'></div></div></div>" +
                    "<div class='vortex-line'><div class='vortex-title'>Заказы со стаусом выполен</div><div class='vortex-count-percent'>" + data.ComletedOrdersCount + " (" + data.ComletedOrdersCountPercent + "%)" + "</div><div class='vortex-bar-outer'><div class='vortex-bar-inner' style='width: " + data.ComletedOrdersCountPercent + "%;'></div></div></div>";

                var sources = data.Sources;
                if (sources != null && sources.length > 0) {

                    html += "<div class='vortex-orders-sources-title'>Источники заказов</div>";
                    html += "<table class='tableview vortex-orders-sources'> " +
                            "<tr class='header'> <th>Источник</th> <th>Кампания</th> <th>Кол-во заказов</th> </tr>";

                    for (var i = 0; i < sources.length; i++) {
                        html += "<tr> <td>" + sources[i].Source + "</td> <td>" + sources[i].Medium + "</td> <td>" + sources[i].Transactions + "</td> </tr> ";
                    }
                    html += "</table> ";
                }

                $("#vortex").html(html);
            }
        });
    }


    function getAbcXyz() {

        if ($("#abcxyz:visible").length == 0)
            return;

        var url = "httphandlers/statistic/AbcxyzAnalysis.ashx?" + "datefrom=" + $(".datefrom").val() + "&dateto=" + $(".dateto").val();

        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "GET",
            async: false,
            url: url,
            success: function (data) {
                if (data == null) {
                    $("#abcxyz").html("Нет данных");
                    return;
                }

                var html =
                    '<div class="abc-group">' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">AZ</div> <div>' + data.azPercent + '% </div> <a href="javascript:void(0)" data-prefix="az" onclick="abcClick(this);">' + data.az + '</a></div>' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">AY</div> <div>' + data.ayPercent + '% </div> <a href="javascript:void(0)" data-prefix="ay" onclick="abcClick(this);">' + data.ay + '</a></div>' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">AX</div> <div>' + data.axPercent + '% </div> <a href="javascript:void(0)" data-prefix="ax" onclick="abcClick(this);"">' + data.ax + '</a></div>' +
                    '</div>' +

                    '<div class="abc-group">' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">BZ</div> <div>' + data.bzPercent + '% </div> <a href="javascript:void(0)" data-prefix="bz" onclick="abcClick(this);">' + data.bz + '</a></div>' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">BY</div> <div>' + data.byPercent + '% </div> <a href="javascript:void(0)" data-prefix="by" onclick="abcClick(this);">' + data.by + '</a></div>' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">BX</div> <div>' + data.bxPercent + '% </div> <a href="javascript:void(0)" data-prefix="bx" onclick="abcClick(this);">' + data.bx + '</a></div>' +
                    '</div>' +

                    '<div class="abc-group">' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">CZ</div> <div>' + data.czPercent + '% </div> <a href="javascript:void(0)" data-prefix="cz" onclick="abcClick(this);">' + data.cz + '</a></div>' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">CY</div> <div>' + data.cyPercent + '% </div> <a href="javascript:void(0)" data-prefix="cy" onclick="abcClick(this);">' + data.cy + '</a></div>' +
                        '<div class="xyz-cell"><div class="xyz-cell-title">CX</div> <div>' + data.cxPercent + '% </div> <a href="javascript:void(0)" data-prefix="cx" onclick="abcClick(this);">' + data.cx + '</a></div>' +
                    '</div>';

                html += '<div class="hor-line"></div>';
                html += '<div class="vert-line"></div>';
                html += '<div class="hor-line-gr"><span>Z</span><span>Y</span><span>X</span></div>';
                html += '<div class="vert-line-gr"><span>A</span><span>B</span><span>C</span></div>';
                html += '<div class="vert-line-title">Вклад товара в оборот/прибыль</div>';
                html += '<div class="hor-line-title">Стабильность продаж</div>';

                $("#abcxyz").html(html);
            }
        });
    }


    function getRfm() {

        if ($("#rfm:visible").length == 0)
            return;

        var url = "httphandlers/statistic/RfmAnalysis.ashx?" + "datefrom=" + $(".datefrom").val() + "&dateto=" + $(".dateto").val();

        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "GET",
            async: false,
            url: url,
            success: function (data) {
                if (data == null) {
                    $("#rfm").html("Нет данных");
                    return;
                }

                var html, rmHtml = '', rfHtml = '', rm = data.Rm, rf = data.Rf;

                rmHtml = '<div class="clearfix"><h2 class="chart-orders-title">RM-анализ</h2></div> ' +
                         '<div class="rfm-tb">';
                for (var i = 4; i >= 0; i--) {

                    rmHtml += '<div class="rfm-row">';
                    for (var j = 0; j < 5; j++) {
                        rmHtml += '<div class="rfm-cell" data-prefix="r_m_' + (i+1) + "_" + (j+1) + '"><div>' + rm[i][j].Percent + '% </div> ' + rm[i][j].Count + '</div>';
                    }
                    rmHtml += '</div>';
                }

                rmHtml += '<div class="hor-line"></div>';
                rmHtml += '<div class="vert-line"></div>';
                rmHtml += '<div class="hor-line-gr"><span>Низкая важность</span><span>Высокая важность</span></div>';
                rmHtml += '<div class="vert-line-gr"><span>Давно</span><span>Недавно</span></div>';
                rmHtml += '<div class="vert-line-title">Давность покупки (R)</div>';
                rmHtml += '<div class="hor-line-title">Важность в обороте (M)</div>';
                rmHtml += "</div>";

                rfHtml = '<div class="clearfix"><h2 class="chart-orders-title">RF-анализ</h2></div> ' +
                         '<div class="rfm-tb"> ';
                for (var k = 4; k >= 0; k--) {

                    rfHtml += '<div class="rfm-row">';
                    for (var m = 0; m < 5; m++) {
                        rfHtml += '<div class="rfm-cell" data-prefix="r_f_' + (k+1) + "_" + (m+1) + '"><div>' + rf[k][m].Percent + '% </div> ' + rf[k][m].Count + '</div>';
                    }
                    rfHtml += '</div>';
                }

                rfHtml += '<div class="hor-line"></div>';
                rfHtml += '<div class="vert-line"></div>';
                rfHtml += '<div class="hor-line-gr"><span>Мало</span><span class="rf-sp">Много</span></div>';
                rfHtml += '<div class="vert-line-gr"><span>Давно</span><span>Недавно</span></div>';
                rfHtml += '<div class="vert-line-title">Давность покупки (R)</div>';
                rfHtml += '<div class="hor-line-title">Регулярность продаж (F)</div>';
                rfHtml += "</div>";

                html = rmHtml + rfHtml;

                $("#rfm").html(html);
            }
        });
    }

    function getManagers() {

        if ($(".managers:visible").length == 0)
            return;

        var url = "httphandlers/statistic/ManagersStatistics.ashx?" + "datefrom=" + $(".datefrom").val() + "&dateto=" + $(".dateto").val();

        $.ajax({
            dataType: "json",
            traditional: true,
            cache: false,
            type: "GET",
            async: false,
            url: url,
            success: function (data) {
                if (data == null) {
                    $(".managers").html("Нет данных");
                    return;
                }

                var html =
                    '<table class="statistics-managers-tb"> ' +
                    '<tr> <th>Менеджер</th> <th>E-mail</th> <th>Число заказов</th> <th>Число обработанных заказов</th> <th>Сумма заказов</th> </tr> ';
                
                for (var i = 0; i < data.length; i++) {
                    html +=
                        '<tr> ' +
                            '<td> <a href="ViewCustomer.aspx?CustomerId=' + data[i].CustomerId + '" target="_blank">' + data[i].Name + '</a> </td>' +
                            '<td>' + data[i].Email + '</td>' +
                            '<td>' + data[i].OrdersCountAssign + '</td>' +
                            '<td>' + data[i].OrdersCount + '</td>' +
                            '<td>' + data[i].OrdersSum + '</td>' +
                        '</tr> ';
                }

                html += '</table>';

                $(".managers").html(html);
            }
        });
    }

});

var locale = d3.locale({
    "decimal": ",",
    "thousands": "\xa0",
    "grouping": [3],
    "currency": ["", " руб."],
    "dateTime": "%A, %e %B %Y г. %X",
    "date": "%d.%m.%Y",
    "time": "%H:%M:%S",
    "periods": ["AM", "PM"],
    "days": ["воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота"],
    "shortDays": ["вс", "пн", "вт", "ср", "чт", "пт", "сб"],
    "months": ["января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря"],
    "shortMonths": ["янв", "фев", "мар", "апр", "май", "июн", "июл", "авг", "сен", "окт", "ноя", "дек"]
});

d3.format = locale.numberFormat;

function d3HorizontalBarCreate(container, data) {

    //more inform about multiBarHorizontalChart nvd3 api in http://nvd3-community.github.io/nvd3/examples/documentation.html#multiBarHorizontalChart
    nv.addGraph(function () {

        var format = data.length > 0 && typeof(data[0].format) != 'undefined'
                        ? (new Function('return ' + data[0].format))()
                        : null;

        var chart = nv.models.multiBarHorizontalChart()
            .x(function (d) { return d.label; })
            .y(function (d) { return d.value; })
            .margin({ top: 20, right: 110, bottom: 20, left: 190 })
            .groupSpacing(0.4)

            .showValues(true)
            .valueFormat(function(d, i) {
                return format != null ? format(d, i) : d3.format("0,00")(d);
            })
            .showLegend(false)
            .barColor(["#8ed346", "#6e9fe3", "#b60275", "#b294d1", "#ffda4e", "#cd3d08", "#ffb035", "#618b9d", "#b68b68", "#36a766", "#3156be", "#00b3ff", "#646464", "#a946e8", "#9d9d9d"])
            .showControls(false)
            .noData("Нет данных")


            .tooltips(true)
            .tooltipContent(function (key, y, e, graph) {
                return '<div class="chart-tooltip-d3">' + key.data.label + ': ' + d3.format("0,00")(key.data.value) + '</div>';
            });

        if (data != null && data.length > 0 && data[0].values != null && data[0].values.length > 0) {

            var count = data[0].values.length;
            var height = count == 1 ? 120 : count * 65;

            chart.height(height);
            $(container).parent().height(height + 20);

            if (count > 15) {
                chart.barColor(d3.scale.category20().range());
            }
        }
        
        chart.yAxis
            .tickFormat(function (d) { return d; })


        d3.select(container)
            .datum(data)
            .call(chart);

        //nv.utils.windowResize(chart.update);

        return chart;
    });
}

function abcClick(el) {
    var url =
            "StatisticsFilter.aspx?type=abcxyz&group=" + $(el).attr("data-prefix") +
            "&from=" + $(".datefrom").val() +
            "&to=" + $(".dateto").val();

    var win = window.open(url, '_blank');
    win.focus();
}