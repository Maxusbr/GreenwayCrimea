using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Module.FindCheaper;

namespace Advantshop.Modules.UserControls.FindCheaper
{
    public partial class Admin_FindCheaperManager : UserControl
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            lvItems.DataSource = FindCheaperService.GetRequests();
            lvItems.DataBind();
        }

        protected void lvItemsItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int itemId;
            if (!int.TryParse(e.CommandArgument.ToString(), out itemId))
            {
                return;
            }

            switch (e.CommandName)
            {
                case "deleteItem":
                    FindCheaperService.DeleteRequest(itemId);

                    break;
                case "updateItem":

                    var request = FindCheaperService.GetRequest(itemId);
                    if (request == null)
                    {
                        return;
                    }

                    var wishPrice = 0f;
                    if (!float.TryParse(((TextBox)lvItems.EditItem.FindControl("txtWishPrice")).Text, out wishPrice))
                    {
                        return;
                    }

                    request.ClientName = ((TextBox)lvItems.EditItem.FindControl("txtClientName")).Text;
                    request.ClientPhone = ((TextBox)lvItems.EditItem.FindControl("txtClientPhone")).Text;
                    request.WishPrice = wishPrice;
                    request.WhereCheaper = ((TextBox)lvItems.EditItem.FindControl("txtWhereCheaper")).Text;
                    request.ManagerComment = ((TextBox)lvItems.EditItem.FindControl("txtManagerComment")).Text;
                    request.IsProcessed = ((CheckBox)lvItems.EditItem.FindControl("ckbIsProcessed")).Checked;

                    FindCheaperService.UpdateRequest(request);

                    lvItems.EditIndex = -1;
                    break;
            }
        }


        protected void lvTabs_ItemEditing(object sender, ListViewEditEventArgs e)
        {
        }

        protected void lvTabs_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvItems.EditIndex = -1;
        }
    }
}