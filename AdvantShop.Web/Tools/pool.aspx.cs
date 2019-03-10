//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace Tools
{
    public partial class pool : System.Web.UI.Page
    {

        private void msgErr(string strMessageText, bool isSucces)
        {

            string strSuccesFormat = "<div class=\"label-box good\">{0} // at {1}</div>";
            string strFailFormat = "<div class=\"label-box error\">{0} // at {1}</div>";

            Message.Visible = true;

            if (isSucces)
            {
                Message.Text = string.Format(strSuccesFormat, strMessageText, DateTime.Now.ToString());
            }
            else
            {
                Message.Text = string.Format(strFailFormat, strMessageText, DateTime.Now.ToString());
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.Web.HttpRuntime.UsingIntegratedPipeline)
                {
                    msgErr(string.Format("Using Integrated Pipeline mode: True"), true);
                }
                else
                {
                    msgErr(string.Format("Using Integrated Pipeline mode: False"), false);
                }

            }
            catch (Exception ex)
            {
                msgErr(string.Format("Err: {0}", ex.Message), false);
            }
        }
    }
}