using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;

namespace Admin.UserControls.Statistic
{
    public partial class TopStatistics : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadTopCustomers();
            LoadTopProducts();
            LoadTopProductsBySum();
        }

        private void LoadTopCustomers()
        {
            lvCustomers.DataSource = OrderStatisticsService.GetTopCustomersBySumPrice();
            lvCustomers.DataBind();
        }

        private void LoadTopProducts()
        {
            lvProducts.DataSource = OrderStatisticsService.GetTopProductsByCount();
            lvProducts.DataBind();
        }

        private void LoadTopProductsBySum()
        {
            lvProductsBySum.DataSource = OrderStatisticsService.GetTopProductsBySum();
            lvProductsBySum.DataBind();
        }

        protected string RenderLink(int productId, string urlPath, string name, string artno)
        {
            if (urlPath.IsNullOrEmpty() || productId == 0)
                return string.Format("{0} [{1}]", name, artno);

            return string.Format("<a href=\"../{0}\">{1} [{2}]</a>",
                UrlService.GetLink(ParamType.Product, urlPath, productId), name, artno);
        }
    }
}