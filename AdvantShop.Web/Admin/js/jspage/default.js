
(function ($) {
    var mpCurrentSaasData;

    $(function () {
        if ($("#modalCurrentSaasData").length) {
            mpCurrentSaasData = $.advModal({
                title: 'Сообщение',
                htmlContent: $("#modalCurrentSaasData"),
                control: $(".battery"),
                afterClose: function () { initValidation($("form")); },
                clickOut: false
            });
        }

        var radioChart = $('[name="gr-chart"]');

        if (radioChart.length > 0) {
            radioChart.on('click', function () {
                for (var i = 0, arrLength = radioChart.length; i < arrLength; i += 1) {
                    $(radioChart[i].value).hide();
                }

                $(this.value).show();

                Advantshop.ScriptsManager.Chart.prototype.InitTotal();
            });
        }

        var radioList = $('.last-orders-list label'); //$('[name="gr-lastOrders"]');

        if (radioList.length > 0) {
            radioList.on('click', function () {
                for (var i = 0, arrLength = radioList.length; i < arrLength; i += 1) {
                    var id = $(radioList[i]).find('input').val();
                    $(id).hide();
                }

                var elId = $(this).find('input').val();
                $(elId).show();
            });
        }
    });

    //function showChartOrderWeek() {
    //    $('#chartMounth').hide();
    //    $('#chartWeek').show();
    //}

    //function showChartOrderMounth() {
    //    $('#chartMounth').show();
    //    $('#chartWeek').hide();
    //}
})(jQuery)




