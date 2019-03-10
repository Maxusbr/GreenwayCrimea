//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace Tools
{
    public partial class Sqlexec : System.Web.UI.Page
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
            msgErr(strMessageText, isSucces, false);
        }

        private void msgErr(string strMessageText, bool isSucces, bool boolCollectStrings)
        {
            string strSuccesFormat = "<div class=\"label-box good\">{0} // at {1}</div>";
            string strFailFormat = "<div class=\"label-box error\">{0} // at {1}</div>";
            string strFormat = "";

            Message.Visible = true;

            if (isSucces)
            {
                strFormat = strSuccesFormat;
            }
            else
            {
                strFormat = strFailFormat;
            }

            if (boolCollectStrings)
            {
                Message.Text += string.Format(strFormat, strMessageText, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss.FFFFFFF"));
            }
            else
            {
                Message.Text = string.Format(strFormat, strMessageText, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss.FFFFFFF"));
            }
        }

        protected void btnGoExec_Click(object sender, System.EventArgs e)
        {

            if (!PingDb())
            {
                return;
            }

            msgErr(true);

            GridView1.Visible = chkShowResult.Checked;

            if (chkShowResult.Checked)
            {

                try
                {
                    SqlDataSource1.ConnectionString = txtCNtext.Text;
                    SqlDataSource1.SelectCommandType = SqlDataSourceCommandType.Text;
                    SqlDataSource1.SelectCommand = txtSqlText.Text;                    

                    GridView1.DataSource = SqlDataSource1;
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    GridView1.Visible = false;
                    msgErr(System.Web.HttpUtility.HtmlEncode(ex.Message), false);
                }

            }
            else
            {

                //string str = txtSqlText.Text; // "asdasdasd GO 11112 123 12 31 23 123";
                //var colec = str.Split('a');

                string source = txtSqlText.Text; // "[stop]ONE[stop][stop]TWO[stop][stop][stop]THREE[stop][stop]";

                source = source.Replace("Go--", "GO--");
                source = source.Replace("go--", "GO--");
                source = source.Replace("gO--", "GO--");

                string[] stringSeparators = new string[] { "GO--" };

                var result = source.Split(stringSeparators, StringSplitOptions.None);

                int lengLimit = 32;
                string shortSqlCommand = "";
                string strSqlCommand = "";
                int intSqlCount = 0;

                // -------------------------------

                using (var cn = new SqlConnection())
                {
                    var cmd = new SqlCommand();
                    cn.ConnectionString = txtCNtext.Text;
                    cmd.Connection = cn;

                    foreach (string str1 in result)
                    {
                        intSqlCount++;
                        strSqlCommand = str1.TrimStart();

                        if (strSqlCommand == "") { continue; }
                        if (strSqlCommand.Length > lengLimit) { shortSqlCommand = strSqlCommand.Substring(0, lengLimit) + "..."; } else { shortSqlCommand = strSqlCommand; }

                        // Exec! -----------------

                        try
                        {
                            if (chkIsStoreProcedure.Checked) { cmd.CommandType = CommandType.StoredProcedure; } else { cmd.CommandType = CommandType.Text; }
                            cmd.CommandText = strSqlCommand;

                            cn.Open();
                            cmd.ExecuteNonQuery();
                            cn.Close();

                            // msgErr(string.Format("strSqlCommand = '{0}'; shortSqlCommand = '{1}'", strSqlCommand, shortSqlCommand), true, true);
                            msgErr(string.Format("#{1} SqlCommand = '{0}' is success", shortSqlCommand, intSqlCount.ToString()), true, true);

                        }
                        catch (Exception ex)
                        {
                            cn.Close();
                            msgErr(System.Web.HttpUtility.HtmlEncode(string.Format("#{1} SqlCommand = '{0}' is failed - Err: '{2}'",
                                                                                   shortSqlCommand,
                                                                                   intSqlCount.ToString(),
                                                                                   ex.Message)), false, true);
                        }

                        // Exec! -----------------

                    } // foreach

                } // using

            }

        }

        protected void execSQL(string strCn, string strSqlText)
        {

            using (var cn = new SqlConnection())
            {
                var cmd = new SqlCommand();

                try
                {
                    cn.ConnectionString = strCn;
                    cmd.Connection = cn;

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strSqlText;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    msgErr(string.Format("Result: Success, SqlCommand = '{1}'", "", ""), true, true);

                    // msgErr(string.Format("strSqlCommand = '{0}'; shortSqlCommand = '{1}'", strSqlCommand, shortSqlCommand), true, true);

                }
                catch (Exception ex)
                {
                    cn.Close();
                    msgErr(System.Web.HttpUtility.HtmlEncode(ex.Message), false, true);
                }
            }

        }

        protected void btnTestConnection_Click(object sender, System.EventArgs e)
        {
            PingDb();
        }

        protected bool PingDb()
        {

            bool res = true;

            if (txtCNtext.Text == "")
            {
                msgErr("Connection string is empty", false);
                res = false;
            }

            using (var cn = new SqlConnection())
            {

                try
                {

                    cn.ConnectionString = txtCNtext.Text;

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
                    res = true;

                }
                catch (Exception ex)
                {
                    cn.Close();
                    msgErr(System.Web.HttpUtility.HtmlEncode(ex.Message), false);
                    res = false;
                }
            } // using

            return res;
        }

    }
}