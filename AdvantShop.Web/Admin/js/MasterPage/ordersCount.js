;(function ($) {
    $(function () {
        var ordersCount = $('[data-value="orders"]', '#MenuAdmin');

        for (var i = 0, l = ordersCount.length; i < l; i += 1) {
            Advantshop.ScriptsManager.NoticeStatistic.prototype.Init(ordersCount.eq(i), { type: 'orders', cssClass: 'orders-count' });
        }

        var leadsCount = $('[data-value="CRM"]', '#MenuAdmin');
        for (var i = 0, l = leadsCount.length; i < l; i += 1) {
            Advantshop.ScriptsManager.NoticeStatistic.prototype.Init(leadsCount.eq(i), { type: 'CRM', cssClass: 'orders-count' });
        }

    });

})(jQuery);







