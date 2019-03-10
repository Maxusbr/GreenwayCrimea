;(function ($) {

    var advantshop = Advantshop, scriptManager = advantshop.ScriptsManager, utilities = advantshop.Utilities, timerRefresh, storage = {}, period = 30000;

    var noticeStatistic = function (obj, options) {

        if (options.type == null) {
            throw Error('Type of statistics not found');
        }

        this.$obj = advantshop.GetJQueryObject(obj);
        this.options = $.extend({}, this.defaultOptions, options);

        return this;
    };
    scriptManager.NoticeStatistic = noticeStatistic;

    noticeStatistic.prototype.Init = function (obj, options) {
        var noticeStatisticObj = new noticeStatistic(obj, options);
        noticeStatisticObj.GenerateHtml();
        noticeStatisticObj.BindEvent();
    };

    noticeStatistic.prototype.GenerateHtml = function () {
        var noticeStatisticObj = this,
            options = noticeStatisticObj.options,
            div = $('<div />', { 'class': options.cssClass != null && options.cssClass.length > 0 ? options.cssClass : null });

        noticeStatisticObj.$obj.append(div);
        noticeStatisticObj.statisticBlock = div;

        if (storage[options.type] == null) {
            storage[options.type] = [];
        }

        storage[options.type].push(div);

    };

    noticeStatistic.prototype.BindEvent = function () {
        var noticeStatisticObj = this;

        noticeStatistic.prototype.Update();
    };

    noticeStatistic.prototype.Update = function () {

        var noticeStatisticObj = this;

        if (noticeStatisticObj.requestActive === true) {
            return;
        }

        noticeStatisticObj.requestActive = true;

        if (timerRefresh != null) {
            clearTimeout(timerRefresh);
        }

        $.ajax({
            url: 'httphandlers/statistic/getnoticestatistic.ashx',
            dataType: 'json',
            cache: false,
            success: function (data) {

                var orders = storage[noticeStatistic.prototype.Type.Orders];
                if (orders != null) {
                    for (var i = 0, l = orders.length; i < l; i += 1) {
                        if (data['LastOrdersCount'] != 0) {
                            orders[i].html('+' + data['LastOrdersCount']).show();
                        } else {
                            orders[i].html('').hide();
                        }
                    }
                }

                var leads = storage[noticeStatistic.prototype.Type.CRM];
                if (leads != null) {
                    for (var i = 0, l = leads.length; i < l; i += 1) {
                        if (data['LastLeadsCount'] != 0) {
                            leads[i].html('+' + data['LastLeadsCount']).show();
                        } else {
                            leads[i].html('').hide();
                        }
                    }
                }

                timerRefresh = setTimeout(function () {
                    noticeStatistic.prototype.Update();
                }, period);
            },
            error: function (data) {
                //throw Error('Error get data statistics');
                if (console != null) {
                    console.log('Error get data statistics');
                }
            },
            complete: function () {
                noticeStatisticObj.requestActive = false;
            }
        });
    };

    noticeStatistic.prototype.Type = {
        Orders: 'orders',
        CRM: 'CRM'
    };

    noticeStatistic.prototype.defaultOptions = {
        type: null,
        cssClass: ''
    };
})(jQuery);