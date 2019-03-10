//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data.SqlClient;
using System.Data;

namespace Tools
{
    public partial class cnhelper : System.Web.UI.Page
    {

        private bool msgErr()
        {
            return Message.Visible;
        }

        private void msgErr(bool boolClean)
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

        protected void btnGoExec_Click(object sender, System.EventArgs e)
        {

            // Data Source='XXXXXXXX'; Connect Timeout='5'; Initial Catalog='XXXXXXX'; Persist Security Info='True'; User ID='XXXXX'; Password='XXXXXXXX';
            // Data Source='XXXXXXXX'; Initial Catalog='XXXXXXX'; Integrated Security='True';

            var sb = new System.Text.StringBuilder();

            if (!string.IsNullOrEmpty(txtDataSource.Text)) { sb.Append("Data Source='" + txtDataSource.Text + "'; "); } else { sb.Append("Data Source=''; "); }

            if (!string.IsNullOrEmpty(txtInitialCatalog.Text)) { sb.Append("Initial Catalog='" + txtInitialCatalog.Text + "'; "); } else { sb.Append("Initial Catalog=''; "); }

            if (!string.IsNullOrEmpty(txtUserID.Text)) { sb.Append("User ID='" + txtUserID.Text + "'; "); } else { sb.Append("User ID=''; "); }

            if (!string.IsNullOrEmpty(txtPassword.Text)) { sb.Append("Password='" + txtPassword.Text + "'; "); } else { sb.Append("Password=''; "); }

            if (!string.IsNullOrEmpty(txtConnectTimeout.Text)) { sb.Append("Connect Timeout='" + txtConnectTimeout.Text + "'; "); } else { sb.Append("Connect Timeout=''; "); }

            if (chkPersistSecurityInfo.Checked)
            {
                if (chkPersistSecurityInfoValue.Checked) { sb.Append("Persist Security Info='True'; "); } else { sb.Append("Persist Security Info='False'; "); }
            }

            if (chkIntegratedSecurity.Checked)
            {
                if (chkIntegratedSecurityValue.Checked) { sb.Append("Integrated Security='True'; "); } else { sb.Append("Integrated Security='False'; "); }
            }

            txtCnResultText.Text = sb.ToString();

        }

        protected void btnTestConnection_Click(object sender, System.EventArgs e)
        {

            if (txtCnResultText.Text == "")
            {
                msgErr("Connection string is empty", false);
                return;
            }

            using (var cn = new SqlConnection())
            {

                try
                {

                    cn.ConnectionString = txtCnResultText.Text;

                    var cmd = new SqlCommand
                        {
                            CommandType = CommandType.Text,
                            CommandText = "SELECT nowdate=GETDATE()",
                            Connection = cn
                        };


                    cn.Open();

                    cmd.ExecuteNonQuery();

                    cn.Close();

                    msgErr("Successful opened", true);
                }
                catch (Exception ex)
                {
                    cn.Close();
                    msgErr(System.Web.HttpUtility.HtmlEncode(ex.Message), false);
                }
            }

        }
    }
}
