//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Module.ProductTabs.Domain;

namespace AdvantShop.Module.ProductTabs
{
    public partial class Admin_DetailsCommonTabsSettings : UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvTabs.DataSource = ProductTabsRepository.GetCommonTabs();
            lvTabs.DataBind();
        }

        protected void lvTabs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "deleteTab":
                    int tabBodyId;
                    if (Int32.TryParse(e.CommandArgument.ToString(), out tabBodyId))
                    {
                        var tabBody = ProductTabsRepository.GetProductTabBody(tabBodyId);
                        if (tabBody != null)
                        {
                            ProductTabsRepository.DeleteProductTabBody(tabBodyId);
                            ProductTabsRepository.DeleteProductTabTitle(tabBody.TabTitleId);
                        }
                        Response.Redirect(Request.Url.ToString());
                    }
                    break;
            }
        }
    }
}