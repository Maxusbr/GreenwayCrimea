using System;
using AdvantShop.Core.Controls;
using AdvantShop.Module.ProductTabs.Domain;

namespace AdvantShop.Module.ProductTabs
{
    public partial class AdminProductTab : ProductAdminTabContent
    {
        public TabTitle TabTitle;
        protected int ProductId
        {
            get
            {
                int id;
                int.TryParse(Request["productid"], out id);
                return id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (TabTitle == null || ProductId == 0)
                return;

            if (!IsPostBack)
            {
                var tabBody = ProductTabsRepository.GetProductTabBody(TabTitle.TabTitleId, ProductId);
                ckTabBody.Text = tabBody != null ? tabBody.Body : string.Empty;
            }
        }

        public override void Save(int productId)
        {
            if (TabTitle == null || productId == 0)
                return;

            if (string.IsNullOrEmpty(ckTabBody.Text))
                ProductTabsRepository.DeleteProductTabBody(productId, TabTitle.TabTitleId);
            else
                ProductTabsRepository.AddUpdateProductTabBody(new TabBody
                {
                    Body = ckTabBody.Text,
                    ProductId = productId,
                    TabTitleId = TabTitle.TabTitleId
                });
        }
    }
}