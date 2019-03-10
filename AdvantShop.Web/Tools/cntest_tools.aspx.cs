//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data.SqlClient;

namespace Tools
{
    public partial class cntest : System.Web.UI.Page
    {

        private void MsgErr(bool boolClean)
        {
            if (boolClean)
            {
                Message.Visible = false;
                Message.Text = string.Empty;
            }
            else
            {
                Message.Visible = false;
            }
        }

        private void MsgErr(string strMessageText, bool isSucces)
        {
            const string strSuccesFormat = "<div class=\"label-box good\">{0} // at {1}</div>";
            const string strFailFormat = "<div class=\"label-box error\">{0} // at {1}</div>";

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

        protected void btnOpen_Click(object sender, System.EventArgs e)
        {

            if (txtCNtext.Text == "")
            {
                MsgErr("Connection string is empty", false);
                return;
            }

            using (var cn = new SqlConnection())
            {
                try
                {
                    cn.ConnectionString = txtCNtext.Text;

                    cn.Open();

                    // Just try to open, nothing more...

                    cn.Close();

                    MsgErr("Successful opened", true);
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MsgErr(ex.Message, false);
                }
            }
        }

        protected void btnClear_Click(object sender, System.EventArgs e)
        {
            MsgErr(true);
        }

    }
}
