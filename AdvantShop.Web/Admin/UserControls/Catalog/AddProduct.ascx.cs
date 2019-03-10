using System;
using AdvantShop.Core.Common.Extensions;

namespace Admin.UserControls.Catalog
{
    public partial class AddProduct : System.Web.UI.UserControl
    {
        public int? CategoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            var defButtonClick = string.Format("return defaultButtonClick('{0}', event);", lnkAddProduct.ClientID);
            txtProductName.Attributes.Add("onkeypress", defButtonClick);
            txtArtNo.Attributes.Add("onkeypress", defButtonClick);
            txtUrl.Attributes.Add("onkeypress", defButtonClick);

            lnkAddProduct.Attributes.Add("data-add-product-category", (CategoryId.HasValue ? CategoryId.Value : Request["categoryid"].TryParseInt()).ToString());
        }
    }
}