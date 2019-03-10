using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Helpers;
using AdvantShop.Module.AbandonedCarts.Domain;


namespace Advantshop.Modules.UserControls
{
    public partial class AbandonedCartsTemplates : UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            lvTemplates.DataSource = AbandonedCartsService.GetTemplates();
            lvTemplates.DataBind();
        }

        protected void lvTemplates_ItemCommand(object source, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                AbandonedCartsService.DeleteTemplate(SQLDataHelper.GetInt(e.CommandArgument));
            }
        }
    }
}