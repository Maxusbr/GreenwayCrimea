using System;

namespace Tools.core
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if ((Session["YouCanUserCore"] == null) || ((bool)Session["YouCanUserCore"] != true))
            {
                Page.Response.Redirect("~/tools/core/default.aspx");
            }
        }
    

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void lbnExitCoreAuth_Click(object sender, EventArgs e)
        {
            Session.Remove("YouCanUserCore");
            Page.Response.Redirect("~/tools/core/default.aspx");
        }
    }
}
