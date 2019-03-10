using System;
using AdvantShop.Core.Common.Extensions;

namespace Admin.UserControls.Catalog
{
    public partial class AddProductCopy : System.Web.UI.UserControl
    {
        public string ProductName { get; set; }
        public string ProductArtNo { get; set; }
        public string ProductUrl { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var defButtonClick = string.Format("return defaultButtonClick('{0}', event);", lnkAddProductCopy.ClientID);
            txtProductName.Attributes.Add("onkeypress", defButtonClick);
            txtArtNo.Attributes.Add("onkeypress", defButtonClick);
            txtUrl.Attributes.Add("onkeypress", defButtonClick);

            lnkAddProductCopy.Attributes.Add("data-add-productcopy-sourceproduct", Request["productid"].TryParseInt().ToString());
            
            if (string.IsNullOrEmpty(ProductName))
                return;

            txtProductName.Text = ProductName + @" - " + Resources.Resource.Admin_CopyOfProduct;
            txtUrl.Text = ProductUrl;
            txtArtNo.Text = ProductArtNo;
        }
    }
}