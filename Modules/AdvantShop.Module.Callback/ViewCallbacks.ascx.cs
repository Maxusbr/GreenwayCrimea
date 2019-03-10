using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Module.Callback.Services;

namespace AdvantShop.Module.Callback
{
    public partial class Admin_ViewCallbacks : UserControl
    {
        private const string ModuleName = "Callback";

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            lvCallbacks.DataSource = CallbackRepository.GetCallbackCustomers();
            lvCallbacks.DataBind();
        }

        protected void lvCallbacksItemCommand(object sender, ListViewCommandEventArgs e)
        {
            int callbackID;
            if (Int32.TryParse(e.CommandArgument.ToString(), out callbackID))
            {
                switch (e.CommandName)
                {
                    case "processCallBack":
                        CallbackRepository.SetCallbackProcessed(callbackID, true);
                        break;
                    case "deleteCallBack":
                        CallbackRepository.DeleteCallbackById(callbackID);
                        break;
                }
            }
        }
    }
}