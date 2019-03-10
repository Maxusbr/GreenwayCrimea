using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Orders;
using Resources;

namespace Admin.UserControls.Statistic
{
    public partial class PaymentStatistics : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadTopPayments();
            LoadTopShippings();
            LoadTopCities();
        }

        private void LoadTopPayments()
        {
            paymentsPie.Attributes["data-chart"] = RenderTopPayments();
            paymentsPie.Attributes["data-chart-options"] =
                "{series: { pie: { show: true,radius: 1,innerRadius: 0.5,label: {show: true,radius: 3/4,formatter: function(label, series) {return '<div style=\"font-size:14px;text-align:center;color:white;\">' + Math.round(series.percent) + '%</div>';},background: { opacity: 0 }}, offset:{top: 0,left: -70}}}}";
        }

        private void LoadTopShippings()
        {
            shippingsPie.Attributes["data-chart"] = RenderTopShippings();
            shippingsPie.Attributes["data-chart-options"] =
                "{series: { pie: { show: true,radius: 1,innerRadius: 0.5,label: {show: true,radius: 3/4,formatter: function(label, series) {return '<div style=\"font-size:14px;text-align:center;color:white;\">' + Math.round(series.percent) + '%</div>';},background: { opacity: 0 }}, offset:{top: 0,left: -70}}}}";
        }

        private void LoadTopCities()
        {
            orderCitiesPie.Attributes["data-chart"] = RenderTopCities();
            orderCitiesPie.Attributes["data-chart-options"] =
                "{series: { pie: { show: true,radius: 1,innerRadius: 0.5,label: {show: true,radius: 3/4,formatter: function(label, series) {return '<div style=\"font-size:14px;text-align:center;color:white;\">' + Math.round(series.percent) + '%</div>';},background: { opacity: 0 }}, offset:{top: 0,left: -70}}}}";
        }

        private string RenderTopPayments()
        {
            var payments = OrderStatisticsService.GetTopPayments();

            if (payments.Count >= 10)
            {
                var paymentStat = payments.Take(9).ToList();
                paymentStat.Add(new KeyValuePair<string, int>(Resource.Admin_Statistics_Others, payments.Skip(9).Sum(x => x.Value)));
                payments = paymentStat;
            }

            return String.Format("[{0}]",
                payments.Aggregate("",
                    (current, payment) =>
                        current +
                        string.Format("{{ label: \"{0}\",  data:{1}}},", payment.Key, payment.Value)));
        }

        private string RenderTopShippings()
        {
            var shippings = OrderStatisticsService.GetTopShippings();

            if (shippings.Count >= 10)
            {
                var pshippingStat = shippings.Take(9).ToList();
                pshippingStat.Add(new KeyValuePair<string, int>(Resource.Admin_Statistics_Others, shippings.Skip(9).Sum(x => x.Value)));
                shippings = pshippingStat;
            }

            return String.Format("[{0}]",
                shippings.Aggregate("",
                    (current, shipping) =>
                        current + string.Format("{{ label: \"{0}\",  data:{1}}},", shipping.Key, shipping.Value)));
        }

        private string RenderTopCities()
        {
            var shippings = OrderStatisticsService.GetTopCities();

            if (shippings.Count >= 10)
            {
                var pshippingStat = shippings.Take(9).ToList();
                pshippingStat.Add(new KeyValuePair<string, int>(Resource.Admin_Statistics_Others, shippings.Skip(9).Sum(x => x.Value)));
                shippings = pshippingStat;
            }

            return String.Format("[{0}]",
                shippings.Aggregate("",
                    (current, shipping) =>
                        current + string.Format("{{ label: \"{0}\",  data:{1}}},", shipping.Key, shipping.Value)));
        }
    }
}